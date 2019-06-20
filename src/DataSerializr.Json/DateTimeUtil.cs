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
    /// A utility for converting <see cref="DateTime"/> to and from a numeric value.
    /// </summary>
    public static class DateTimeUtil
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);

        /// <summary>
        /// Converts a <see cref="DateTime"/> instance to an equivalent numeric date value.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> instance.</param>
        /// <param name="timeOffset">The time offset</param>
        /// <returns>Returns a numeric date value.</returns>
        public static long ToNumericDateTime(DateTime date, int timeOffset = 0)
        {
            return (long)(date.AddMinutes(timeOffset) - UnixEpoch).TotalMilliseconds;
        }

        /// <summary>
        /// Converts a numeric date value to an equivalent <see cref="DateTime"/> instance.
        /// </summary>
        /// <param name="numericDate">The numeric date value</param>
        /// <param name="timeOffset">The time offset</param>
        /// <returns>Return the <see cref="DateTime"/> instance.</returns>
        public static DateTime FromNumericDatetime(long numericDate, int timeOffset = 0)
        {
            if (numericDate == -1)
                return DateTime.MinValue;

            int seconds = (int)(numericDate / 1000);
            int milliseconds = (int)(numericDate % 1000);
            DateTime epoch = UnixEpoch.AddMinutes(timeOffset * -1);
            return epoch + new TimeSpan(0, 0, 0, seconds, milliseconds);
        }
    }
}
