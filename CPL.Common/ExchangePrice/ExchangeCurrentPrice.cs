using CPL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.ExchangePrice
{
    public static class ExchangeCurrentPrice
    {
        public static BTCPrice BTCCurrentPrice()
        {
            return new BTCPrice()
            {
                Price = ExchangeExtension.GetBTCPrice(),
                Time = DateTime.UtcNow
            };
        }
    }
}
