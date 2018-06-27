using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumCoinTransactionType
    {
        DEPOSIT_BTC = 1,
        DEPOSIT_ETH = 2,
        WITHDRAW_BTC = 3,
        WITHDRAW_ETH = 4,
        EXCHANGE_BTC_TO_CPL = 5,
        EXCHANGE_CPL_TO_BTC = 6,
        EXCHANGE_ETH_TO_CPL = 7,
        EXCHANGE_CPL_TO_ETH = 8,
    }
}
