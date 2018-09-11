using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Common.Enums
{
    public enum EnumCurrencyPair
    {
        [Display(Name = "ETH/BTC")]
        ETHBTC,
        [Display(Name = "BTC/USDT")]
        BTCUSDT
    }
}
