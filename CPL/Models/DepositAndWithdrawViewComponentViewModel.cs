using System.ComponentModel.DataAnnotations;

namespace CPL.Models
{
    public class DepositAndWithdrawViewComponentViewModel
    {
        public decimal BTCAmount { get; set; }
        public decimal ETHAmount { get; set; }
        public decimal TokenAmount { get; set; }
        public string BTCHDWalletAddress { get; set; }
        public string ETHHDWalletAddress { get; set; }
        public string BTCQrCodeImage { get; set; }
        public string ETHQrCodeImage { get; set; }

        public decimal BTCAvailable { get; set; }
    }
}
