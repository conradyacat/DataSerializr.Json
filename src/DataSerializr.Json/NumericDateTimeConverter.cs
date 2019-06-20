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
using Newtonsoft.Json;

namespace DataSerializr.Json
{
    /// <summary>
    /// Converts <see cref="DateTime"/> to and from numeric date value.
    /// </summary>
    public class NumericDateTimeConverter : JsonConverter
    {
        private readonly int _timeOffset;

        /// <summary>
        /// Initializes an instance of NumericDateTimeConverter.
        /// </summary>
        /// <param name="timeOffset">The time offset to use to calculate the time.</param>
        public NumericDateTimeConverter(int timeOffset)
        {
            _timeOffset = timeOffset;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified value type.
        /// </summary>
        /// <param name="objectType">The type of the object.</param>
        /// <returns>Returns a boolean value whether the object instance can be converted.</returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
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
            var timestamp = long.Parse(reader.Value.ToString());
            return DateTimeUtil.FromNumericDateTime(timestamp, _timeOffset);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dt = (DateTime)value;
            var timestamp = DateTimeUtil.ToNumericDateTime(dt, _timeOffset);
            writer.WriteValue(timestamp);
        }
    }
}
