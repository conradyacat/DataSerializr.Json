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
    public class DataTableJsonConverterTest
    {
        [Fact]
        public void Should_Serialize_DataTable_Without_Keys()
        {
            var testData = TestDataHelper.GetTestDataWithoutKeys();
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataTableJsonConverter() }
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
        public void Should_Serialize_DataTable_Using_Json_Types()
        {
            var testData = TestDataHelper.GetTestData();
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataTableJsonConverter() }
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
        public void Should_Serialize_DataTable_Using_Clr_Types()
        {
            var testData = TestDataHelper.GetTestData();
            var settings = new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> {  new DataTableJsonConverter(useClrTypes: true) }
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
        public void Should_Deserialize_DataTable_WithoutKeys()
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
                Converters = new List<JsonConverter> {  new DataTableJsonConverter() }
            };

            DataTable actual = JsonConvert.DeserializeObject<DataTable>(testData, settings);
            DataTable expected = TestDataHelper.GetTestDataWithoutKeys();

            Assert.Equal(expected.TableName, actual.TableName);
            Assert.Equal(expected.Columns.Count, actual.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Columns);
            AssertHelper.AssertRows(expected, actual);
        }

        [Fact]
        public void Should_Deserialize_DataTable_Using_Json_Types()
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
                Converters = new List<JsonConverter> {  new DataTableJsonConverter() }
            };

            DataTable actual = JsonConvert.DeserializeObject<DataTable>(testData, settings);
            DataTable expected = TestDataHelper.GetTestData();

            Assert.Equal(expected.TableName, actual.TableName);
            Assert.Equal(expected.Columns.Count, actual.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Columns);
            AssertHelper.AssertPrimaryKeys(expected.PrimaryKey, actual.PrimaryKey);
            AssertHelper.AssertRows(expected, actual);
        }

        [Fact]
        public void Should_Deserialize_DataTable_Using_Clr_Types()
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
                Converters = new List<JsonConverter> { new DataTableJsonConverter(useClrTypes: true) }
            };

            DataTable actual = JsonConvert.DeserializeObject<DataTable>(testData, settings);
            DataTable expected = TestDataHelper.GetTestData();

            Assert.Equal(expected.TableName, actual.TableName);
            Assert.Equal(expected.Columns.Count, actual.Columns.Count);
            Assert.Equal(expected.PrimaryKey.Length, actual.PrimaryKey.Length);
            Assert.Equal(expected.Rows.Count, actual.Rows.Count);
            AssertHelper.AssertColumns(expected.Columns, actual.Columns, true);
            AssertHelper.AssertPrimaryKeys(expected.PrimaryKey, actual.PrimaryKey);
            AssertHelper.AssertRows(expected, actual, true);
        }
    }
}
