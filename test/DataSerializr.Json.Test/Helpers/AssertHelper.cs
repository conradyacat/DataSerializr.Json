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
using Xunit;

namespace DataSerializr.Json.Test.Helpers
{
    public static class AssertHelper
    {
        public static void AssertColumns(DataColumnCollection expectedColumns, DataColumnCollection actualColumns, bool useClrTypes = false)
        {
            for (int i = 0; i < expectedColumns.Count; i++)
            {
                DataColumn expectedColumn = expectedColumns[i];
                DataColumn actualColumn = actualColumns[i];

                Assert.Equal(expectedColumn.ColumnName, actualColumn.ColumnName);

                if (!useClrTypes && JsonTypeConverter.IsNumeric(expectedColumn.DataType))
                {
                    Assert.Equal(typeof(double), actualColumn.DataType);
                }
                else
                {
                    Assert.Equal(expectedColumn.DataType, actualColumn.DataType);
                }
            }
        }

        public static void AssertPrimaryKeys(DataColumn[] expectedPrimaryKeys, DataColumn[] actualPrimaryKeys)
        {
            for (int i = 0; i < expectedPrimaryKeys.Length; i++)
            {
                Assert.Equal(expectedPrimaryKeys[i].ColumnName, actualPrimaryKeys[i].ColumnName);
            }
        }

        public static void AssertRows(DataTable expected, DataTable actual, bool useClrTypes = false)
        {
            for (int i = 0; i < expected.Rows.Count; i++)
            {
                DataRow expectedRow = expected.Rows[i];
                DataRow actualRow = actual.Rows[i];

                foreach (DataColumn expectedColumn in expected.Columns)
                {
                    var expectedColumnValue = expectedRow[expectedColumn.ColumnName];
                    var actualColumnValue = actualRow[expectedColumn.ColumnName];

                    if (!useClrTypes && JsonTypeConverter.IsNumeric(expectedColumn.DataType)
                        && expectedColumnValue != DBNull.Value && actualColumnValue != DBNull.Value)
                    {
                       Assert.Equal(Convert.ToDecimal(expectedColumnValue), Convert.ToDecimal(actualColumnValue)); 
                    }
                    else
                    {
                        Assert.Equal(expectedColumnValue, actualColumnValue);
                    }
                }
            }
        }
    }
}