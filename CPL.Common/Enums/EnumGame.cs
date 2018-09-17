using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumGameResult
    {
        WIN = 1,
        LOSE = 2,
        KYC_PENDING = 3,
        REFUNDED = 4
    }

    public enum EnumPricePredictionStatus
    {
        UP,     // true
        DOWN    // false
    }

    public enum EnumGameType
    {
        LOTTERY = 1,
        WORLDCUP = 2,
        PRICE_PREDICTION = 3
    }

    public enum EnumLotteryGameStatus
    {
        PENDING = 1,
        ACTIVE = 2,
        COMPLETED = 3,
        DEACTIVATED = 4,
    }

    public enum EnumPricePredictionGameStatus
    {
        ACTIVE = 1,
        COMPLETED = 2
    }
}
