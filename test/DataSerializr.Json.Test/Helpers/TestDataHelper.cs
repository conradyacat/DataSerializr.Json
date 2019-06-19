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
using System.Data;

namespace DataSerializr.Json.Test.Helpers
{
    public static class TestDataHelper
    {
        public static DataTable GetTestDataWithoutKeys()
        {
            var dt = new DataTable("Table1");
            dt.Columns.Add("Int32", typeof(int));
            dt.Columns.Add("Int16", typeof(short));
            dt.Columns.Add("Int64", typeof(long));
            dt.Columns.Add("Decimal", typeof(decimal));
            dt.Columns.Add("Float", typeof(float));
            dt.Columns.Add("Double", typeof(double));
            dt.Columns.Add("String", typeof(string));
            dt.Columns.Add("DateTime", typeof(DateTime));
            dt.Columns.Add("Boolean", typeof(bool));

            dt.Rows.Add(12, (short)2, 3L, 123.4m, 543.21, 321.98, "Hello!", new DateTime(2014, 11, 25), true);
            dt.Rows.Add(23, (short)1, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, false);

            return dt;
        }

        public static DataTable GetTestData()
        {
            var dt = GetTestDataWithoutKeys();
            dt.PrimaryKey = new [] 
            {
                dt.Columns["Int32"],
                dt.Columns["Int16"]
            };

            return dt;
        }
    }
}
