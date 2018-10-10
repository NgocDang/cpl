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

        /// <summary>
        /// Convert date time to UTC unix time
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static long ToUTCUnixTimeInSeconds(this DateTime time)
        {
            return (long)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// Convert date time to unix time
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static long ToUnixTimeInMilliseconds(this DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// Convert date time to UTC unix time
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static long ToUTCUnixTimeInMilliseconds(this DateTime time)
        {
            return (long)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }
    }
}