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

namespace DataSerializr.Json
{
    /// <summary>
    /// A utility to derive Clr types from JSON types and vice versa.
    /// </summary>
    public static class JsonTypeConverter
    {
        private const string NUMBER = "number";
        private const string STRING = "string";
        private const string DATETIME = "datetime";
        private const string BOOLEAN = "boolean";

        /// <summary>
        /// Determines the equivalent Json type from the Clr type.
        /// </summary>
        /// <param name="type">The Clr type</param>
        /// <returns>Returns the Json type.</returns>
        public static string GetJsonTypeFromClrType(Type type)
        {
            if (IsNumeric(type))
                return NUMBER;

            if (type == typeof(DateTime))
                return DATETIME;
            if (type == typeof(bool))
                return BOOLEAN;

            return STRING;
        }

        /// <summary>
        /// Determines the equivalent Clr type from the Json type.
        /// </summary>
        /// <param name="jsonType"></param>
        /// <returns>Returns the Clr type.</returns>
        public static Type GetClrTypeFromJsonType(string jsonType)
        {
            if (jsonType == NUMBER)
                return typeof(double);

            if (jsonType == DATETIME)
                return typeof(DateTime);

            if (jsonType == BOOLEAN)
                return typeof(bool);

            return typeof(string);
        }

        /// <summary>
        /// Determines whether the Clr type is a numeric type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNumeric(Type type)
        {
            return type == typeof(int) 
                || type == typeof(long) 
                || type == typeof(double) 
                || type == typeof(decimal) 
                || type == typeof(short) 
                || type == typeof(float);
        }
    }
}
