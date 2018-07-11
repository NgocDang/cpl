using CPL.Common.ExchangePrice;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public class CPLConstant
    {
        //Realtime exchange price
        public struct ExchangePrice
        {
            public static BTCPrice BTCPrice;
        }
        
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
        public static string BTCMnemonic = "clinic length diary episode fan hope initial broccoli anger happy exile aunt"; // Already changed diff with NAM

        //EWallet
        public static string ETHMnemonic = "cover cake supply collect army resist street ring angry record solar axis"; // Already changed diff with NAM

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
    }
}
