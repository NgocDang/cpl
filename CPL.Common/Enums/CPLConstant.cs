using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Common.Enums
{
    public class CPLConstant
    {
        //Change this environment setting will affect CPL globally including Web & Services!!!
        public static EnumEnvironment Environment = EnumEnvironment.TESTNET;

        //Authentication
        public static string ActivateExpiredInDays = "ActivateExpiredInDays";
        public static string ResetPasswordExpiredInDays = "ResetPasswordExpiredInDays";
        public static string IsAccountActivationEnable = "IsAccountActivationEnable";

        //Setting
        public static string BTCToTokenRate = "BTCToTokenRate";
        public static string IsKYCVerificationActivated = "IsKYCVerificationActivated";
        public static string CookieExpirations = "CookieExpirations";

        //Affiliate
        public struct StandardAffiliate
        {
            public static string Tier1DirectRate = "StandardAffiliate.Tier1DirectRate";
            public static string Tier2SaleToTier1Rate = "StandardAffiliate.Tier2SaleToTier1Rate";
            public static string Tier3SaleToTier1Rate = "StandardAffiliate.Tier3SaleToTier1Rate";
        }

        public struct AgencyAffiliate
        {
            public static string Tier1DirectRate = "AgencyAffiliate.Tier1DirectRate";
            public static string Tier2DirectRate = "AgencyAffiliate.Tier2DirectRate";
            public static string Tier3DirectRate = "AgencyAffiliate.Tier3DirectRate";
            public static string Tier2SaleToTier1Rate = "AgencyAffiliate.Tier2SaleToTier1Rate";
            public static string Tier3SaleToTier1Rate = "AgencyAffiliate.Tier3SaleToTier1Rate";
            public static string Tier3SaleToTier2Rate = "AgencyAffiliate.Tier3SaleToTier2Rate";
        }

        //Two-factor authentication
        public static string AppName = "CPL";
        public static string TwoFactorAuthenticationSecretKey = "CPLFamHopper";

        //SMTP
        public struct SMTP
        {
            public static string Email { get { if (Environment == EnumEnvironment.MAINNET) return "info.cplchain@gmail.com"; else return "info.cplcoin@gmail.com"; } }
            public static string Contact { get { if (Environment == EnumEnvironment.MAINNET) return "info.cplchain@gmail.com"; else return "info.cplcoin@gmail.com"; } }
            public static string From { get { if (Environment == EnumEnvironment.MAINNET) return "info.cplchain@gmail.com"; else return "info.cplcoin@gmail.com"; } }
        }

        // Maintenance
        public struct Maintenance
        {
            public static bool IsOnMaintenance { get; set; }
            public static char MaintenanceAllowedIpDelimiter { get; set; } = ';';
            public static string MaintenanceAllowedIp { get; set; } = "14.161.32.23" + MaintenanceAllowedIpDelimiter + "::1";
            public static string IsOnMaintenanceSetting = "IsOnMaintenance";
        }

        // Google Analytic 
        public struct Analytic
        {
            public static string Credential
            {
                get
                {
                    if (Environment == EnumEnvironment.MAINNET) return "{\"type\":\"service_account\",\"project_id\":\"cpo-prod\",\"private_key_id\":\"471c7d766a430c43eb6b4b77b922e2a5a9926f88\",\"private_key\":\"-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQC2OzRbQlfKFQUc\nLmvmerAzpWNiNTlIwp1D/ror7YZnkwDVw9Oi5gMCfRb8pSelPSg1xhby9GlMCvN2\nUE2z0utRMTWZ8yBBK9qH4bT25w4+RRfjUMymXsn1y0SO8t46o9WCQuFPcNC3cei4\nmC7V1yWnYTOTjRbRG+HHfT0rPFKZePBHiF8pAIYVAuLMOObAhgT7WTkFJvWFOCES\nMq3RVQgJbAOaPzsLDfzzhfCmZ3XmQMiVCyfZvVBHHmeBACWEYl2UztY0iKEtlDMr\n9zdAKTpafL6QB23ENHiFGZm9D1UN7+scN3SYXZpscZfBYtk/EUNopDtWL4U/u3Ni\n+3aL+8DDAgMBAAECggEALEoHV4awP3UMYOUHZGS9HlA770pwObbS8guew75exB1+\nPnXxoc21PY2ZYJ3jHM3YT/ls6m9siAbBVSf/Oo0wk/MZy+jDFhPKcOvyR2+wjoHM\nZHrmwiiXfR2B9Bwi1q2q3pXSnLQWhf8xSGv0NhSsGhoNpvbLbNcR8KIHk9J9YL8S\nhTaYyJwdsPPIRsYBsWXVIgSWEd05GDBEB73YgaGQV1eaG/b1gFDgLTdNI//uJIkl\nWG3LIomLaHqp/3Vs/uJg4BSoit4b8fQPNcWuV1j1YUDsFXeQjzO95sJTSChqDG34\nqfmf65S+eLL2lxBrJ8aC6CYsCygxYeyMzY3yKFwPAQKBgQDrfYvvKFZXyTCumkvq\n79vqXSgfyaDLbv3cbDDmAvhzkSOWbpa/lJtuJ/RigKrDvSkI+E4WUP4gdKQSqW17\nGEHBjGff/Y8EXbrK0Up7WIwA9kCum6fv0hxp6r0IWRTyeXnqGHNQtmse9egRe1+H\nbR5x7mgVPNIxqjkkHYlNqZe7EQKBgQDGGjOAWEbRH7HpMU93pVdSrjyKaBaCuBYR\n3Bu/17bwY5UqH67jPL3lnc2QR7qar/uwKFMP/GRxfko0BTpCt/+yfpsGrjBn9xDO\naoo1dZOBOz7OJfIachbRW/9Qzd4P9GFZekdjcgJDs3XdCA3Tz1mjVtvGrCIbfV6V\n07z/QIn2kwKBgDvBcd7QB4jjHp1Pve/xyYYg+WuHiGvW0Eg8k/Pp0Q3SSIuFqG8Y\n0JAVR2gvaiWdPGpwR7BWAQZPUssBfyx7LURl0n3+I2piNmH81b+S6oPb+PHVwbmT\nQW6HCEyz/iLBPHRwm+JhR7FliOvWuJju0YjrjDHHazb7EODfNdYlMMFhAoGBAIXS\nBz5I47W60mTzcXoiT+XDW5QnkiVFk2sHJBuo7aRfsNDezDYmvyTQP5+aRLzf6oZq\nBwpCdduQ8zFG4r9VKj56deDCZGKmrEyz/Z71SOpAikGZsn0Y2r8chVEhcEOY0vYt\nuvG7fzoajaD9ItIVPLkxII+JkhS90NonSYGoKEpPAoGBAMoUarQUi4AOS67oNRkD\nVSktLFKYaj1IpRgFNVPsLvroJV0WhIo4DRFh2cj4uAW/kx6QZImSZvXQ1ObOO36J\naFeYiFeyLKac+Hge+ck2rJuuN3U0Uwfruj7S3tiHueTyuSlhbNOjuh7XteBpfHQZ\n+jpXEY3c+VTs2nPnGTeRqI7T\n-----END PRIVATE KEY-----\n\",\"client_email\":\"infocpl@cpo-prod.iam.gserviceaccount.com\",\"client_id\":\"103069886009076742291\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\":\"https://www.googleapis.com/robot/v1/metadata/x509/infocpl%40cpo-prod.iam.gserviceaccount.com\"}";
                    else return "{\"type\":\"service_account\",\"project_id\":\"cpo-test-216107\",\"private_key_id\":\"4895f13cee002d644cd004cdacdaa48f4818ac7a\",\"private_key\":\"-----BEGIN PRIVATE KEY-----\nMIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQCfuwkEUcvSWXaz\npgL2ivkziSaISd3+QP6KkEuMJRc6s3nC8yfl5Q3jUM7t0uxYHQ9tMKDzr4s5yPzU\npx5RLRG3zJU1LK7ZjsbeIHxUsvwwgrUSoBreGaGmFs2SVDH7oDUAloGIvDwg0IhK\nHgPc+ho48QdaN1aCeQn2+9AGsggI8QJNgAzlaO0pf4P4sYmOauacVQeM+i4ErAfo\nDbwtZSxAOkMkBh30EfObR9kGOLxLWmQajFMyLuYnKxG+j7q9/UFF5noNrj0/n2yD\nGGmep1079nVTinO2s1ytbxAhaWu4gW7ngbfUv5BGzDCm5M31s5q2iTTLVwgttJn6\n6aLhwccrAgMBAAECggEAA7aR3ZdXMSMiRsfXrsqJQ4DvM+Dft77kC+2RzAa6tQMi\nE/2aElaSTZP903qBQwUd7DBitXeBuYyqD/l4k3OEQ20OlOdegzE1/0jrCv8ELM6F\nDhNJvJsE7nr+OWYaqVpOkR5ukXKqxSqD2JE9DsCDpoo33QmHSc4thJ/S9Kzj0e1a\n7npp2BEQT4IhCKhFj2N46IRPXgfqzdz1GaMLT2chUocBxroY6GX9318RYDV1vAMc\nGJNi22eZNd56E0Cay4XKtIx3F0WjV2UCeF/JEtvenuplnQJFfQE73vnEk6bqcYd6\naXWtd3uKJJ9Yhp6wslPg5XuLWg5rAkl8vObUfHnEkQKBgQDZrNY3fpt6AENqepIV\naiWYblD0tiLAqsBYvWDL58F7ZTEjMCjBFt81LIvdxb2/590QdJT3Wj5rH3Bhah/2\n2jS3fSwpiwP/UuhfE3meLbbO3jAoULNqcOzrguwG6LkkmhzKhI9y/QlN1FCvmPNm\neut5OfbsJOruLAuvuBsKS1VMmwKBgQC72n5CNP703w7Oq1tSQYDS2DKjdZYBbR3Z\nPk+wQopyqBDjK0DQ+fLt7NqbLIMPohvDx6j7bVRGBVhl76t+TWDSp1gouwCCJBt5\nwOfl92Bi4a2t0Ih7Ebs0YvKvlrR2bfhwI2oW8UCH0WxwrUl8JRJdPv8ynuZAXinL\nBniqWQxwsQKBgGdJLKhOagW8xEBtAo4ovMgUaFOFbP4Qx44T5i86u8MKZAN3YY8l\nhCoeBgvoPT4nDe019E1eQ8netBt1GM+wAF2khmT78pyfQWWDvyyAMoL8SMuIZ2m1\nRaYGjGpG2Tr16X1q4wpD6LtGaDa6E5xn7P2rtG+eoN78hVsOt73tuv7LAoGBAKKR\nt+p8CpjH4Pyh9un8ngvZ5hjnzeV2FPrInhYfv8jyAYtXB0TSypV1AMqBfvqsg/Ku\ne2L7fo8KWTH+dB9dv4kOIZE+JHZ74PtBIm814kgwnilp6nL3KVdwui2S8qH7EaGa\nRFmOdxYS+xGYJZw2SOpBdoDMWt8hnnGBLDPf1JBBAoGBAMAtD1lny/cEtnu9pavN\nw7dPnXXe/jJWQBgbNK4/R04r5xyhkdVESiK/K91j/p++nao40Em0OHqJmb+ffi1U\nX4iMQdBMYlgx8JzQfoefFpBaPn8JZIxUC/YgsFhXWFoOOiKOyIT+CFZTyKUIOm/3\nRHCUajgZ3TQ9ECt6Noa+5SiJ\n-----END PRIVATE KEY-----\n\",\"client_email\":\"infocpl@cpo-test-216107.iam.gserviceaccount.com\",\"client_id\":\"103739034404838109792\",\"auth_uri\":\"https://accounts.google.com/o/oauth2/auth\",\"token_uri\":\"https://oauth2.googleapis.com/token\",\"auth_provider_x509_cert_url\":\"https://www.googleapis.com/oauth2/v1/certs\",\"client_x509_cert_url\":\"https://www.googleapis.com/robot/v1/metadata/x509/infocpl%40cpo-test-216107.iam.gserviceaccount.com\"}";
                }
            }  // TEST

            public static string AccountId = "125563668";
            public static string PropertyId { get { if (Environment == EnumEnvironment.MAINNET) return "UA-125563668-1"; else return "UA-125563668-2"; } }
            public static string WebsiteUrl { get { if (Environment == EnumEnvironment.MAINNET) return @"https://cplchain.net"; else return @"https://cpl.famhopperchain.com"; } }

            public static string Timezone = "Japan";

            public static string HomeViewId = "HomeViewId";
            public static string LotteryViewId = "LotteryViewId";
            public static string PricePredictionViewId = "PricePredictionViewId";

            public static string LotteryCategoryViewName = "LotteryCategory_";
            public static string LotteryCategoryFilterExpression = @"/Lottery/Detail/[0-9]+\?lottery-category-id=";
        }

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
        public static string ProjectEmail { get { if (Environment == EnumEnvironment.MAINNET) return "info.cplchain@gmail.com"; else return "info.cplcoin@gmail.com"; } }
        public static string ProjectName = "CPL";

        //Lottery game
        public static int LotteryGroupSize = 10;
        public static int MaximumNumberOfWinnerPerGroup = 2;
        public static string LotteryGameDrawingInHourOfDay = "LotteryGameDrawingTimeInHour";
        public static int LotteryGamePerRow = 4;

        // High Chart
        public static int HourBeforeInChart = 1; // hour
        public static int LowestRateBTCInterval = 100;

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
        public static string BTCDepositAddress { get { if (Environment == EnumEnvironment.MAINNET) return "14AUtFRakncVdqy5b1qWmcFctjtRENgeDU"; else return "mzobLqk3DwwhLnirjjjm7Xmzqxh2xtU6ew"; } }
        public static string BTCWithdrawAddress { get { if (Environment == EnumEnvironment.MAINNET) return "14AUtFRakncVdqy5b1qWmcFctjtRENgeDU"; else return "mzobLqk3DwwhLnirjjjm7Xmzqxh2xtU6ew"; } }
        public static string BTCWithdrawPrivateKey { get { if (Environment == EnumEnvironment.MAINNET) return "Kza4W2kD8cqvpzD1ERMWQKGWjbDxRoWUXZEFa11fxVUtNwjXgfmi"; else return "cVB6Q5EZoWRTNfNnVpCuwLaZ8DeSxPJNPXJc5uM9gMbBM9WkUckY"; } }

        // ETH
        public static string ETHDepositAddress { get { if (Environment == EnumEnvironment.MAINNET) return "0x1D6a318516d6a87699B6Bcc307506961589258fB"; else return "0xB43eA1802458754A122d02418Fe71326030C6412"; } }
        public static string ETHWithdrawAddress { get { if (Environment == EnumEnvironment.MAINNET) return "0x1D6a318516d6a87699B6Bcc307506961589258fB"; else return "0xB43eA1802458754A122d02418Fe71326030C6412"; } }
        public static string ETHWithdrawPrivateKey { get { if (Environment == EnumEnvironment.MAINNET) return "203e5543b0017638431bd334e91f5b7a39200596cd2e56799791c6bb3ffc4509"; else return "118dface57cbfc63874c0158964d7f810154feb3ddd08277636cd591e3638e60"; } }

        public static string Etherscan { get { if (Environment == EnumEnvironment.MAINNET) return "https://etherscan.io/tx/{0}"; else return "https://rinkeby.etherscan.io/tx/{0}"; } }
        public static string BlockInfo { get { if (Environment == EnumEnvironment.MAINNET) return "https://www.blockchain.com/btc/tx/{0}"; else return "https://testnet.blockchain.info/tx/{0}"; } }

        //Request to get BTC and ETH address count limit
        public static int RequestCountLimit = 5;
        public static int RequestCountIntervalInMiliseconds = 3000;

        public static string RandomParamInJson = "{{'1':{{'uint32':'{0}'}},'2':{{'address':'{1}'}}, '3': {{'uint32[]':'[{2}]'}}}}"; // Replace { by {{ to fix bug invalid input string when using string.Format
        public static string GetTicketParamInJson = "{{'1':{{'uint32':'{0}'}},'2':{{'address':'{1}'}}, '3': {{'uint32':'{2}'}}}}";

        public struct Format
        {
            public static string Amount = "#,##0.########";
            public static string Number = "N0";
            public static string DateTime = "yyyy/MM/dd HH:mm:ss";
            public static string Date = "yyyy/MM/dd";
            public static string Time = @"hh\:mm\:ss";
        }

        public static string NumberOfAgencyAffiliateExpiredDays = "NumberOfAgencyAffiliateExpiredDays";
        public static decimal LotteryTotalRevenuePercentage = 0.2m;
        public static string PricePredictionTotalAwardPercentage = "PricePredictionTotalAwardPercentage";
        public static DateTime FirstDeploymentDate = new DateTime(2018, 8, 28);

        public struct MobileAppConstant
        {
            public static string LoginScreenInactivatingAccount = "LoginScreen_Inactivating_Account";
            public static string LoginScreenInvalidEmailPassword = "LoginScreen_Invalid_Email_Password";
            public static string LoginScreenInvalidPIN = "LoginScreen_Invalid_PIN";

            public static string RegisterScreenInputEmailExist = "RegisterScreen_Input_Email_Exist";

            public static string LotteryDetailNoTicketsLeft = "LotteryDetail_No_Tickets_Left";
            public static string LotteryDetailNotEnoughCPL = "LotteryDetail_Not_Enough_CPL";


            public static string ContactScreenEmailSentSuccessfully = "ContactScreen_Email_Sent_Successfully";

            public static string EditPasswordScreenPasswordUpdatedSuccessfully = "EditPasswordScreen_Password_Updated_Successfully";
            public static string EditPasswordScreenInvalidCurrentPassword = "EditPasswordScreen_Invalid_Current_Password";
            public static string EditPasswordScreenNonExistingAccount = "EditPasswordScreen_NonExisting_Account";

            public static string EditEmailScreenEmailUpdatedSuccessfully = "EditEmailScreen_Email_Updated_Successfully";
            public static string EditEmailScreenNonExistingAccount = "EditEmailScreen_NonExisting_Account";
            public static string EditEmailScreenExistingEmail = "EditEmailScreen_Existing_Email";

            public static string DepositAndWithdrawScreenInvalidWithdrawAmount = "DepositAndWithdrawScreen_Invalid_Withdraw_Amount";
            public static string DepositAndWithdrawScreenInsufficientFunds = "DepositAndWithdrawScreen_Insufficient_Funds";
            public static string DepositAndWithdrawScreenInvalidBTCAddress = "DepositAndWithdrawScreen_Invalid_BTCAddress";
            public static string DepositAndWithdrawScreenWithdrawedSuccessfully = "DepositAndWithdrawScreen_Withdrawed_Successfully";

            public static string ProfileEditUserScreenUpdatedSuccessfully = "ProfileEditUserScreen_Updated_Successfully";
            public static string ProfileEditUserScreenNonExistingAccount = "ProfileEditUserScreen_NonExisting_Account";

            public static string CommonErrorOccurs = "Common_ErrorOccurs";
        }
    }
}
