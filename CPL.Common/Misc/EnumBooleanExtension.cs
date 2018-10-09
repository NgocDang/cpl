using CPL.Common.Enums;

namespace CPL.Common.Misc
{
    public static class EnumBooleanExtension
    {
        public static bool ToBoolean(this EnumPricePredictionStatus value)
        {
            return value == EnumPricePredictionStatus.UP;
        }

        public static bool? ToBoolean(this EnumCoinTransactionStatus value)
        {
            if (value == EnumCoinTransactionStatus.PENDING)
                return null;
            else return value == EnumCoinTransactionStatus.SUCCESS;
        }

        public static EnumCoinTransactionStatus ToEnumCoinstransactionStatus(this bool? value)
        {
            if (!value.HasValue)
            {
                return EnumCoinTransactionStatus.PENDING;
            }
            else if (value == false)
            {
                return EnumCoinTransactionStatus.FAIL;
            }
            else // value = true
            {
                return EnumCoinTransactionStatus.SUCCESS;
            }
        }
    }
}
