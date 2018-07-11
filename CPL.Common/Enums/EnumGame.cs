using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public enum EnumGameResult
    {
        WIN = 1,
        LOSE = 2,
        PENDING = 3
    }

    public enum EnumPricePredictionStatus
    {
        UP,     // true
        DOWN    // false
    }

    public enum EnumGameId
    {
        LOTTERY = 1,
        WORLDCUP = 2,
        PRICE_PREDICTION = 3
    }
}
