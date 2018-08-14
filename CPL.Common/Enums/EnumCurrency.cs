using CPL.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumCurrency
    {
        [CSS("ETH-alt")]
        ETH = 1,
        [CSS("BTC-alt")]
        BTC = 2,
        CPL = 3,
        USDT = 4
    }
}
