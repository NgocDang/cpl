using CPL.Common.Enums;
using CPL.Misc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPL.Common.Misc;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Models
{
    public class CoinTransactionViewModel
    {
        public int Id { get; set; }
        public string FromWalletAddress { get; set; }
        public string ToWalletAddress { get; set; }
        public decimal CoinAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CurrencyId { get; set; }
        public decimal? TokenAmount { get; set; }
        public int Type { get; set; }
        public bool? Status { get; set; }
        public double? Rate { get; set; }
        public string TxHashId { get; set; }

        public string CurrencyInString { get; set; }
        public string TxHashReference { get { return CurrencyId == 1 ? string.Format(CPLConstant.Etherscan, TxHashId) : CurrencyId == 2 ? string.Format(CPLConstant.BlockInfo, TxHashId) : "#"; } }
        public string CreatedDateInString { get { return CreatedDate.ToString(Format.DateTime); } }
        public string CoinAmountInString { get { return CoinAmount.ToString(Format.Amount); } }
        public string TokenAmountInString { get { return TokenAmount.GetValueOrDefault(0).ToString(Format.Amount); } }
        public string RateInString { get; set; }
        public string TypeInString { get { return EnumHelper<EnumCoinTransactionType>.GetDisplayValue((EnumCoinTransactionType)Type); } }
        public EnumCoinTransactionStatus StatusInEnum { get { return Status.ToEnumCoinstransactionStatus(); } }
        public EnumCurrency CurrencyInEnum { get { return (EnumCurrency)CurrencyId; } }
        public string StatusInString { get { return Status.ToEnumCoinstransactionStatus().ToString(); } }
    }
}
