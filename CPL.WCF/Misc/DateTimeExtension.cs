using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPL.WCF.Misc
{
    public static class DateTimeExtension
    {
        public static long ToUnixTimeInSeconds(this DateTime time)
        {
            return (long)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }
}