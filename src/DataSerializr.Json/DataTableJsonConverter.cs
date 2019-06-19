// Copyright (c) 2014 Conrad Yacat
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace DataSerializr.Json
{
    /// <summary>
    /// Converts <see cref="DataTable"/> to and from JSON.
    /// </summary>
    public class DataTableJsonConverter : JsonConverter
    {
        private const string NAME = "name";
        private const string COLUMNS = "columns";
        private const string KEYS = "keys";
        private const string ROWS = "rows";
        private const string TYPE = "type";

        private readonly int _timeOffset;
        private readonly Func<string, Type> _getClrTypeFunc;
        private readonly Func<Type, string> _getJsonTypeFunc;

        /// <summary>
        /// Initializes an instance of DataTableJsonConverter.
        /// </summary>
        /// <param name="timeOffset">The time offset to use to calculate the time.</param>
        /// <param name="useClrTypes">A flag to determine whether to use Clr types during serialization/deserialization. The default if false.</param>
        public DataTableJsonConverter(int timeOffset = 0, bool useClrTypes = false)
        {
            _timeOffset = timeOffset;

            if (useClrTypes)
            {
                _getClrTypeFunc = Type.GetType;
                _getJsonTypeFunc = t => t.FullName;
            }
            else
            {
                _getClrTypeFunc = JsonTypeConverter.GetClrTypeFromJsonType;
                _getJsonTypeFunc = JsonTypeConverter.GetJsonTypeFromClrType;
            }
        }

        /// <summary>
        /// Determines whether this instance can convert the specified value type.
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataTable);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!reader.Read())
                return null;

            var dt = new DataTable();

            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == NAME)
            {
                dt.TableName = reader.ReadAsString();
                reader.Read();
            }

            // read the columns
            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == COLUMNS)
            {
                while (reader.TokenType != JsonToken.StartArray)
                    reader.Read();

                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    reader.Read();
                    string columnName = reader.ReadAsString();
                    reader.Read();
                    string typeName = reader.ReadAsString();
                    dt.Columns.Add(columnName, _getClrTypeFunc(typeName));
                    reader.Read();
                }

                reader.Read();
            }

            // read the keys
            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == KEYS)
            {
                while (reader.TokenType != JsonToken.StartArray)
                    reader.Read();

                var keys = new List<DataColumn>();
                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    string columnName = reader.Value.ToString();
                    keys.Add(dt.Columns[columnName]);
                }

                dt.PrimaryKey = keys.ToArray();

                reader.Read();
            }

            // read the rows
            if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == ROWS)
            {
                while (reader.TokenType != JsonToken.StartArray)
                    reader.Read();

                while (reader.Read() && reader.TokenType != JsonToken.EndArray)
                {
                    DataRow newRow = dt.NewRow();
                    foreach (DataColumn column in dt.Columns)
                    {
                        reader.Read();
                        if (reader.Value == null)
                            continue;

                        if (column.DataType == typeof(DateTime))
                            newRow[column.ColumnName] = DateTimeUtil.FromNumericDatetime((long)reader.Value, _timeOffset);
                        else
                            newRow[column.ColumnName] = reader.Value;
                    }

                    dt.Rows.Add(newRow);
                    reader.Read();
                }

                reader.Read();
            }

            return dt;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is DataTable dt))
                throw new ArgumentException("The object is not a DataTable type", nameof(value));

            writer.WriteStartObject();

            writer.WritePropertyName(NAME);
            writer.WriteValue(dt.TableName);

            // write the columns
            writer.WritePropertyName(COLUMNS);
            writer.WriteStartArray();

            foreach (DataColumn column in dt.Columns)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(NAME);
                writer.WriteValue(column.ColumnName);
                writer.WritePropertyName(TYPE);
                writer.WriteValue(_getJsonTypeFunc(column.DataType));
                writer.WriteEndObject();
            }

            writer.WriteEndArray();

            // write the keys
            if (dt.PrimaryKey.Length > 0)
            {
                writer.WritePropertyName(KEYS);
                writer.WriteStartArray();

                foreach (DataColumn column in dt.PrimaryKey)
                {
                    writer.WriteValue(column.ColumnName);
                }

                writer.WriteEndArray();
            }

            // write the rows
            writer.WritePropertyName(ROWS);
            writer.WriteStartArray();

            foreach (DataRow row in dt.Rows)
            {
                writer.WriteStartArray();

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    var columnValue = row[i];
                    if (columnValue is DateTime)
                        columnValue = DateTimeUtil.ToNumericDateTime((DateTime)columnValue, _timeOffset);

                    if (columnValue == DBNull.Value)
                        writer.WriteNull();
                    else
                        writer.WriteValue(columnValue);
                }

                writer.WriteEndArray();
            }

            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
}
