using CPL.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public double? Rate { get; set; }
        public string TxHashId { get; set; }
        public string TxHashReference { get; set; }

        public string CoinAmountInString { get; set; }
        public string CreatedDateInString { get; set; }
        public string CurrencyInString { get; set; }
        public string TokenAmountInString { get; set; }
        public string RateInString { get; set; }
        public string TypeInString { get; set; }
        public EnumCoinTransactionStatus StatusInEnum { get; set; }
        public EnumCurrency CurrencyInEnum { get; set; }
        public string StatusInString { get; set; }
    }
}
