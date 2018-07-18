using CPL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPL.WCF.Misc
{
    public static class BTCCurrentPrice
    {
        public static decimal Price { get; set; }
        public static DateTime Time { get; set; }
    }
}