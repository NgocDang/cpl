using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public static class Authentication
    {
        public static string Token { get; set; }
    }

    public static class ServiceClient
    {
        public static AuthenticationService.AuthenticationClient AuthenticationClient { get; set; }
        public static EmailService.EmailClient EmailClient { get; set; }
        public static BTCCurrentPriceService.BTCCurrentPriceClient BTCCurrentPriceClient { get; set; }
        public static ETokenService.ETokenClient ETokenClient { get; set; }
        public static EWalletService.EWalletClient EWalletClient { get; set; }
        public static BWalletService.BWalletClient BWalletClient { get; set; }
        public static EAccountService.EAccountClient EAccountClient { get; set; }
        public static BAccountService.BAccountClient BAccountClient { get; set; }
    }
}
