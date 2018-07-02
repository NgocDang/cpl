using System.ComponentModel.DataAnnotations;

namespace CPL.Models
{
    public class DepositAndWithdrawViewModel
    {
        public decimal BtcAmount { get; set; }
        public decimal EthAmount { get; set; }
        public string BtcAddress { get; set; }
        public string EthAddress { get; set; }
    }
}
