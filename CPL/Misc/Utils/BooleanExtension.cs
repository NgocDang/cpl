using CPL.Common.Enums;

namespace CPL.Misc.Utils
{
    public static class BooleanExtension
    {
        public static bool ToBoolean(this EnumPricePredictionStatus value)
        {
            return value == EnumPricePredictionStatus.UP;
        }
    }
}
