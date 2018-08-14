using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumCoinTransactionType
    {
        [Display(Name = "DEPOSIT BTC")]
        DEPOSIT_BTC = 1,
        [Display(Name = "DEPOSIT ETH")]
        DEPOSIT_ETH = 2,
        [Display(Name = "WITHDRAW BTC")]
        WITHDRAW_BTC = 3,
        [Display(Name = "WITHDRAW ETH")]
        WITHDRAW_ETH = 4,
        [Display(Name = "EXCHANGE BTC TO CPL")]
        EXCHANGE_BTC_TO_CPL = 5,
        [Display(Name = "EXCHANGE CPL TO BTC")]
        EXCHANGE_CPL_TO_BTC = 6,
        [Display(Name = "EXCHANGE ETH TO CPL")]
        EXCHANGE_ETH_TO_CPL = 7,
        [Display(Name = "EXCHANGE CPL TO ETH")]
        EXCHANGE_CPL_TO_ETH = 8,
    }
}
