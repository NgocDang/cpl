using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPL.Common.Misc
{
    public static class DateTimeExtension
    {
        /// <summary>
        /// Convert date time to unix time
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static long ToUnixTimeInSeconds(this DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}