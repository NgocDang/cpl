using CPL.Common.Enums;

namespace CPL.Misc.Utils
{
    public static class EnumBooleanExtension
    {
        public static bool ToBoolean(this EnumPricePredictionStatus value)
        {
            return value == EnumPricePredictionStatus.UP;
        }

        public static bool? ToBoolean(this EnumCoinstransactionStatus value)
        {
            if (value == EnumCoinstransactionStatus.PENDING)
                return null;
            else return value == EnumCoinstransactionStatus.SUCCESS;
        }
    }
}
