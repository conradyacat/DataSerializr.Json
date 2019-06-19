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

using System.Collections.Generic;
using System.Data;
using DataSerializr.Json.Test.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace DataSerializr.Json.Test
{
    public class DataViewJsonConverterTest
    {
        [Fact]
        public void Should_Serialize_DataView_Without_Keys()
        {
            var testData = TestDataHelper.GetTestDataWithoutKeys().DefaultView;
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataViewJsonConverter() }
            };
            string actual = JsonConvert.SerializeObject(testData, Formatting.None, settings);
            string expected = "{\"name\":\"Table1\"," + 
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"number\"},{\"name\":\"Int16\",\"type\":\"number\"}," + 
                    "{\"name\":\"Int64\",\"type\":\"number\"},{\"name\":\"Decimal\",\"type\":\"number\"}," +
                    "{\"name\":\"Float\",\"type\":\"number\"},{\"name\":\"Double\",\"type\":\"number\"}," + 
                    "{\"name\":\"String\",\"type\":\"string\"},{\"name\":\"DateTime\",\"type\":\"datetime\"}," + 
                    "{\"name\":\"Boolean\",\"type\":\"boolean\"}]," + 
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," + 
                    "[23,1,null,null,null,null,null,null,false]]}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Serialize_DataView_Using_Json_Types()
        {
            var testData = TestDataHelper.GetTestData().DefaultView;
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataViewJsonConverter() }
            };
            string actual = JsonConvert.SerializeObject(testData, Formatting.None, settings);
            string expected = "{\"name\":\"Table1\"," + 
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"number\"},{\"name\":\"Int16\",\"type\":\"number\"}," + 
                    "{\"name\":\"Int64\",\"type\":\"number\"},{\"name\":\"Decimal\",\"type\":\"number\"}," +
                    "{\"name\":\"Float\",\"type\":\"number\"},{\"name\":\"Double\",\"type\":\"number\"}," + 
                    "{\"name\":\"String\",\"type\":\"string\"},{\"name\":\"DateTime\",\"type\":\"datetime\"}," + 
                    "{\"name\":\"Boolean\",\"type\":\"boolean\"}]," + 
                "\"keys\":[\"Int32\",\"Int16\"]," +
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," + 
                    "[23,1,null,null,null,null,null,null,false]]}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Serialize_DataView_Using_Clr_Types()
        {
            var testData = TestDataHelper.GetTestData().DefaultView;
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataViewJsonConverter(useClrTypes: true) }
            };
            string actual = JsonConvert.SerializeObject(testData, Formatting.None, settings);
            string expected = "{\"name\":\"Table1\"," +
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"System.Int32\"},{\"name\":\"Int16\",\"type\":\"System.Int16\"}," +
                    "{\"name\":\"Int64\",\"type\":\"System.Int64\"},{\"name\":\"Decimal\",\"type\":\"System.Decimal\"}," +
                    "{\"name\":\"Float\",\"type\":\"System.Single\"},{\"name\":\"Double\",\"type\":\"System.Double\"}," +
                    "{\"name\":\"String\",\"type\":\"System.String\"},{\"name\":\"DateTime\",\"type\":\"System.DateTime\"}," +
                    "{\"name\":\"Boolean\",\"type\":\"System.Boolean\"}]," +
                "\"keys\":[\"Int32\",\"Int16\"]," +
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," +
                    "[23,1,null,null,null,null,null,null,false]]}";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Should_Deserialize_DataView_WithoutKeys()
        {
            string testData = "{\"name\":\"Table1\"," + 
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"number\"},{\"name\":\"Int16\",\"type\":\"number\"}," + 
                    "{\"name\":\"Int64\",\"type\":\"number\"},{\"name\":\"Decimal\",\"type\":\"number\"}," +
                    "{\"name\":\"Float\",\"type\":\"number\"},{\"name\":\"Double\",\"type\":\"number\"}," + 
                    "{\"name\":\"String\",\"type\":\"string\"},{\"name\":\"DateTime\",\"type\":\"datetime\"}," + 
                    "{\"name\":\"Boolean\",\"type\":\"boolean\"}]," + 
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," + 
                    "[23,1,null,null,null,null,null,null,false]]}";

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataViewJsonConverter() }
            };

            DataView actual = JsonConvert.DeserializeObject<DataView>(testData, settings);
            DataTable expected = TestDataHelper.GetTestDataWithoutKeys();

            Assert.Equal(expected.TableName, actual.Table.TableName);
            Assert.Equal(expected.Columns.Count, actual.Table.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.Table.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Table.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Table.Columns);
            AssertHelper.AssertRows(expected, actual.Table);
        }

        [Fact]
        public void Should_Deserialize_DataView_Using_Json_Types()
        {
            string testData = "{\"name\":\"Table1\"," + 
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"number\"},{\"name\":\"Int16\",\"type\":\"number\"}," + 
                    "{\"name\":\"Int64\",\"type\":\"number\"},{\"name\":\"Decimal\",\"type\":\"number\"}," +
                    "{\"name\":\"Float\",\"type\":\"number\"},{\"name\":\"Double\",\"type\":\"number\"}," + 
                    "{\"name\":\"String\",\"type\":\"string\"},{\"name\":\"DateTime\",\"type\":\"datetime\"}," + 
                    "{\"name\":\"Boolean\",\"type\":\"boolean\"}]," + 
                "\"keys\":[\"Int32\",\"Int16\"]," +
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," + 
                    "[23,1,null,null,null,null,null,null,false]]}";

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataViewJsonConverter() }
            };

            DataView actual = JsonConvert.DeserializeObject<DataView>(testData, settings);
            DataTable expected = TestDataHelper.GetTestData();

            Assert.Equal(expected.TableName, actual.Table.TableName);
            Assert.Equal(expected.Columns.Count, actual.Table.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.Table.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Table.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Table.Columns);
            AssertHelper.AssertPrimaryKeys(expected.PrimaryKey, actual.Table.PrimaryKey);
            AssertHelper.AssertRows(expected, actual.Table);
        }

        [Fact]
        public void Should_Deserialize_DataView_Using_Clr_Types()
        {
            string testData = "{\"name\":\"Table1\"," +
                "\"columns\":[{\"name\":\"Int32\",\"type\":\"System.Int32\"},{\"name\":\"Int16\",\"type\":\"System.Int16\"}," +
                    "{\"name\":\"Int64\",\"type\":\"System.Int64\"},{\"name\":\"Decimal\",\"type\":\"System.Decimal\"}," +
                    "{\"name\":\"Float\",\"type\":\"System.Single\"},{\"name\":\"Double\",\"type\":\"System.Double\"}," +
                    "{\"name\":\"String\",\"type\":\"System.String\"},{\"name\":\"DateTime\",\"type\":\"System.DateTime\"}," +
                    "{\"name\":\"Boolean\",\"type\":\"System.Boolean\"}]," +
                "\"keys\":[\"Int32\",\"Int16\"]," +
                "\"rows\":[[12,2,3,123.4,543.21,321.98,\"Hello!\",1416873600000,true]," +
                    "[23,1,null,null,null,null,null,null,false]]}";

            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new DataViewJsonConverter(useClrTypes: true) }
            };

            DataView actual = JsonConvert.DeserializeObject<DataView>(testData, settings);
            DataTable expected = TestDataHelper.GetTestData();

            Assert.Equal(expected.TableName, actual.Table.TableName);
            Assert.Equal(expected.Columns.Count, actual.Table.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.Table.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Table.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Table.Columns, true);
            AssertHelper.AssertPrimaryKeys(expected.PrimaryKey, actual.Table.PrimaryKey);
            AssertHelper.AssertRows(expected, actual.Table, true);
        }
    }
}
