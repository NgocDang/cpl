using CPL.Common.ExchangePrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public class CPLConstant
    {
        //Authentication
        public static string ActivateExpiredInDays = "ActivateExpiredInDays";
        public static string ResetPasswordExpiredInDays = "ResetPasswordExpiredInDays";
        public static string IsAccountActivationEnable = "IsAccountActivationEnable";

        //Setting
        public static string BTCToTokenRate = "BTCToTokenRate";

        //Two-factor authentication
        public static string AppName = "CPL";
        public static string TwoFactorAuthenticationSecretKey = "CPLFamHopper";

        //Notification
        public static string AdminEmail = "info.cplcoin@gmail.com";

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
        //public static string BTCMnemonic = "violin cute bulb grunt hybrid uniform father chef antique lawsuit camp inherit"; // PROD
        public static string BTCMnemonic = "cute violin bulb grunt hybrid uniform father chef antique lawsuit camp inherit"; // TEST

        //EWallet
        //public static string ETHMnemonic = "stock fatigue leopard despair boat brother asset dizzy marriage drop fragile add"; // PROD
        public static string ETHMnemonic = "fatigue stock leopard despair boat brother asset dizzy marriage drop fragile add"; // TEST

        //ETransaction
        public static string ETransactionAPIKey = "3FMSCKJX5HUXF5D2HTZ5QVUFXRMJAJ4WSA";

        //ETN duration in second
        public static int DurationInSecond = 1200;

        //FHCore service endpoint
        public static string ServiceEndpoint = "ServiceEndPointUrl";
        public static string AuthenticationServiceEndpoint = "Security/Authentication.svc";
        public static string EmailServiceEndpoint = "Message/Email.svc";
        public static string ETokenServiceEndpoint = "Token/EToken.svc";

        // FHCore Authentication
        public static string ProjectEmail = "info.cplcoin@gmail.com";
        public static string ProjectName = "CPL";

        //Lottery game
        public static int LotteryTicketPrice = 500;
        public static int LotteryGroupSize = 10;
        public static int MaximumNumberOfWinnerPerGroup = 2;

        //Game name
        public static string LotteryName = "Lottery";
        public static string PricePredictionName = "Price Prediction";
        public static string LotteryGameDrawingInHourOfDay = "LotteryGameDrawingTimeInHour";

        // High Chart
        public static int HourBeforeInChart = 12; // hour
        public static int LowestRateBTCNumber = 1000; // milisecond

        // GCapcha Key
        public static string GCaptchaKey = "GCaptchaKey";

        // Page size of News
        public static int PageSize = 5;

        // Smart Contract
        //public static string SmartContractAddress = "0xFF7C0b51bc57DA70A484CEBE6a6258d77Fe38699"; // PROD
        public static string SmartContractAddress = "0x6f47E11F5B9ec93BCC6947C13b9Fd1e5fcdD0FD0"; // TEST
        public static string Environment = "Environment";
        public static string OwnerAddress = "0xAc4bAc4CE2DCA3A8b1C9f2b81C25da04875134D0";
        public static string OwnerPassword = "ngocdang";
        public static string Platform = "Platform";
        public static string Abi = "[{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"previousOwner\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"OwnershipTransferred\",\"type\":\"event\"},{\"constant\":false,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_listTicketIndex\",\"type\":\"uint32[]\"}],\"name\":\"random\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_listTicketIndex\",\"type\":\"uint32[]\"}],\"name\":\"reRandom\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"phaseNumber\",\"type\":\"uint32\"},{\"indexed\":true,\"name\":\"playerAddress\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"ticketIndex\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"ticketNumber\",\"type\":\"uint32\"}],\"name\":\"TicketNumberGenerated\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"phaseNumber\",\"type\":\"uint32\"},{\"indexed\":true,\"name\":\"playerAddress\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"ticketIndex\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"oldTicketNumber\",\"type\":\"uint32\"},{\"indexed\":false,\"name\":\"newTicketNumber\",\"type\":\"uint32\"}],\"name\":\"TicketNumberReGenerated\",\"type\":\"event\"},{\"constant\":false,\"inputs\":[{\"name\":\"newOwner\",\"type\":\"address\"}],\"name\":\"transferOwnership\",\"outputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"_phase\",\"type\":\"uint32\"},{\"name\":\"_playerAddress\",\"type\":\"address\"},{\"name\":\"_ticketIndex\",\"type\":\"uint32\"}],\"name\":\"getTicket\",\"outputs\":[{\"name\":\"\",\"type\":\"uint32\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"owner\",\"outputs\":[{\"name\":\"\",\"type\":\"address\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"}]";
        //public static int GasPriceMultiplicator = 1; // PROD
        public static int GasPriceMultiplicator = 3; // TEST
        public static string paramJson = "{\"1\":{\"uint32\":\"lotteryphase\"},\"2\":{\"address\":\"useraddress\"}, \"3\": {\"uint32[]\":\"[ticketindexlist]\"}}";
    }
}
