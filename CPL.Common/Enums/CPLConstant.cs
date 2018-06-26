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

        //Two-factor authentication
        public static string AppName = "CPL";
        public static string TwoFactorAuthenticationSecretKey = "FamHopper";

        // Maintenance
        public struct Maintenance
        {
            public static bool IsOnMaintenance { get; set; }
            public static char MaintenanceAllowedIpDelimiter { get; set; } = ';';
            public static string MaintenanceAllowedIp { get; set; } = "14.161.32.23" + MaintenanceAllowedIpDelimiter + "::1";
            public static string IsOnMaintenanceSetting = "IsOnMaintenance";
        }

        //BWallet
        public static string BTCMnemonic = "clinic length diary episode fan hope initial broccoli anger happy exile aunt"; // Already changed diff with NAM

        //EWallet
        public static string ETHMnemonic = "cover cake supply collect army resist street ring angry record solar axis"; // Already changed diff with NAM

        //ETransaction
        public static string ETransactionAPIKey = "3FMSCKJX5HUXF5D2HTZ5QVUFXRMJAJ4WSA";

        //ETN duration in second
        public static int DurationInSecond = 1200;
    }
}
