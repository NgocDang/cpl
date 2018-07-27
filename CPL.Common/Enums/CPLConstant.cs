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
        public static string BTCMnemonic = "violin cute bulb grunt hybrid uniform father chef antique lawsuit camp inherit"; // Will changed on production

        //EWallet
        public static string ETHMnemonic = "stock fatigue leopard despair boat brother asset dizzy marriage drop fragile add"; // Will changed on production

        //ETransaction
        public static string ETransactionAPIKey = "3FMSCKJX5HUXF5D2HTZ5QVUFXRMJAJ4WSA";

        //ETN duration in second
        public static int DurationInSecond = 1200;

        //FHCore service endpoint
        public static string ServiceEndpoint = "ServiceEndPointUrl";
        public static string AuthenticationServiceEndpoint = "Security/Authentication.svc";
        public static string EmailServiceEndpoint = "Message/Email.svc";

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

        public static string FirstPrize = "1st";
        public static string SecondPrize = "2nd";
        public static string ThirdPrize = "3rd";
        public static string FourthPrize = "4th";

        // High Chart
        public static int HourBeforeInChart = 12; // hour
        public static int LowestRateBTCNumber = 1000; // milisecond

        // GCapcha Key
        public static string GCaptchaKey = "GCaptchaKey";
    }
}
