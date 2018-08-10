using CPL.Common.ExchangePrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public class CPLConstant
    {
        public static EnumEnvironment Environment = EnumEnvironment.TESTNET;

        //Authentication
        public static string ActivateExpiredInDays = "ActivateExpiredInDays";
        public static string ResetPasswordExpiredInDays = "ResetPasswordExpiredInDays";
        public static string IsAccountActivationEnable = "IsAccountActivationEnable";

        //Setting
        public static string BTCToTokenRate = "BTCToTokenRate";

        //Two-factor authentication
        public static string AppName = "CPL";
        public static string TwoFactorAuthenticationSecretKey = "CPLFamHopper";

        //SMTP
        public struct SMTP
        {
            public static string Email = "info.cplcoin@gmail.com";
            public static string Contact = "info.cplcoin@gmail.com";
            public static string From = "info.cplcoin@gmail.com";
        }

        // Maintenance
        public struct Maintenance
        {
            public static bool IsOnMaintenance { get; set; }
            public static char MaintenanceAllowedIpDelimiter { get; set; } = ';';
            public static string MaintenanceAllowedIp { get; set; } = "14.161.32.23" + MaintenanceAllowedIpDelimiter + "::1";
            public static string IsOnMaintenanceSetting = "IsOnMaintenance";
        }

        // Social Url
        public static string FacebookUrl = "FacebookUrl";

        //BWallet
        public static string BTCMnemonic { get { if (Environment == EnumEnvironment.MAINNET) return "violin cute bulb grunt hybrid uniform father chef antique lawsuit camp inherit"; else return "cute violin bulb grunt hybrid uniform father chef antique lawsuit camp inherit"; } }  // TEST

        //EWallet
        public static string ETHMnemonic { get { if (Environment == EnumEnvironment.MAINNET) return "stock fatigue leopard despair boat brother asset dizzy marriage drop fragile add"; else return "fatigue stock leopard despair boat brother asset dizzy marriage drop fragile add"; } }

        //ETransaction
        public static string ETransactionAPIKey = "3FMSCKJX5HUXF5D2HTZ5QVUFXRMJAJ4WSA";

        //ETN duration in second
        public static int DurationInSecond = 1200;

        //CPL service endpoint
        public static string CPLServiceEndpoint = "CPLServiceEndPointUrl";
        public static string BTCCurrentPriceServiceEndpoint = "ExchangeCurrentPrice/BTCCurrentPrice.svc";

        //FHCore service endpoint
        public static string FHCoreServiceEndpoint = "FHCoreServiceEndPointUrl";
        public static string AuthenticationServiceEndpoint = "Security/Authentication.svc";
        public static string EmailServiceEndpoint = "Message/Email.svc";
        public static string ETokenServiceEndpoint = "Token/EToken.svc";
        public static string EWalletServiceEndpoint = "Wallet/EWallet.svc";
        public static string BWalletServiceEndpoint = "Wallet/BWallet.svc";
        public static string EAccountServiceEndpoint = "Account/EAccount.svc";
        public static string BAccountServiceEndpoint = "Account/BAccount.svc";

        // FHCore Authentication
        public static string ProjectEmail = "info.cplcoin@gmail.com";
        public static string ProjectName = "CPL";

        //Lottery game
        public static int LotteryTicketPrice = 500;
        public static int LotteryGroupSize = 10;
        public static int MaximumNumberOfWinnerPerGroup = 2;
        public static string LotteryGameDrawingInHourOfDay = "LotteryGameDrawingTimeInHour";

        // High Chart
        public static int HourBeforeInChart = 12; // hour
        public static int LowestRateBTCInterval = 500;

        // GCaptcha Key
        public static string GCaptchaKey = "GCaptchaKey";

        // Page size of News
        public static int PageSize = 5;

        // Smart Contract
        public static string SmartContractAddress { get { if (Environment == EnumEnvironment.MAINNET) return "0xFF7C0b51bc57DA70A484CEBE6a6258d77Fe38699"; else return "0x6f47E11F5B9ec93BCC6947C13b9Fd1e5fcdD0FD0"; } }
        
        public static string OwnerAddress = "0xAc4bAc4CE2DCA3A8b1C9f2b81C25da04875134D0";
        public static string OwnerPassword = "ngocdang";
        public static string Platform = "Platform";
        public static string Abi = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"constant\":false,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_listTicketIndex\",\"type\":\"uint32[]\"}],\"name\":\"random\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_listTicketIndex\",\"type\":\"uint32[]\"}],\"name\":\"reRandom\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"phaseNumber\",\"type\":\"uint32\"},{\"indexed\":true,\"name\":\"playerAddress\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"ticketIndex\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"ticketNumber\",\"type\":\"uint32\"}],\"name\":\"TicketNumberGenerated\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"phaseNumber\",\"type\":\"uint32\"},{\"indexed\":true,\"name\":\"playerAddress\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"ticketIndex\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"oldTicketNumber\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"newTicketNumber\",\"type\":\"uint32\"}],\"name\":\"TicketNumberReGenerated\",\"type\":\"event\"},{\"constant\":false,\"inputs\":[{\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_ticketIndex\",\"type\":\"uint32\"}],\"name\":\"getTicket\",\"outputs\":[{\"name\":\"\",\"type\":\"uint32\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"}]";

        public static int GasPriceMultiplicator { get { if (Environment == EnumEnvironment.MAINNET) return 1; else return 3; } }

        // Withdraw and deposit
        // BTC
        public static string BTCDepositAddress { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }
        public static string BTCWithdrawAddress { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }
        public static string BTCWithdrawPrivateKey { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }
        // ETH
        public static string ETHDepositAddress { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }
        public static string ETHWithdrawAddress { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }
        public static string ETHWithdrawPrivateKey { get { if (Environment == EnumEnvironment.MAINNET) return ""; else return ""; } }

        public static string Etherscan { get { if (Environment == EnumEnvironment.MAINNET) return "https://etherscan.io/tx/{0}"; else return "https://rinkeby.etherscan.io/tx/{0}"; } }
        public static string BlockInfo { get { if (Environment == EnumEnvironment.MAINNET) return "https://www.blockchain.com/btc/tx/{0}"; else return "https://testnet.blockchain.info/tx/{0}"; } }

        //Request to get BTC and ETH address count limit
        public static int RequestCountLimit = 5;
        public static int RequestCountIntervalInMiliseconds = 3000;

        public static string RandomParamInJson = "{{\"1\":{{\"uint32\":\"{0}\"}},\"2\":{{\"address\":\"{1}\"}}, \"3\": {{\"uint32[]\":\"[{2}]\"}}}}"; // Replace { by {{ to fix bug invalid input string when using string.Format
        public static string GetTicketParamInJson = "{\"1\":{\"uint32\":\"{0}\"},\"2\":{\"address\":\"{1}\"}, \"3\": {\"uint32\":\"{2}\"}}";
    }
}
