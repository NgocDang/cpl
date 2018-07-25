using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumGameResult
    {
        WIN = 1,
        LOSE = 2,
        KYC_PENDING = 3
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

    public enum EnumGameStatus
    {
        NOW = 1,
        END = 2
    }
}
