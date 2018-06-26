using CPL.Misc.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CPL.Models
{
    public class AccountLoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //public bool TwoFactorAuthenticationEnable { get; set; }

        //public string PIN { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }

    public class AccountForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public EnumAccountStatus Status { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }

    public class AccountResetPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public int Id { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public EnumAccountStatus Status { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }

    public class AccountRegistrationModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }

        public int? IsIntroducedById { get; set; }
        public bool IsAgencyAffiliate { get; set; }
        public string AgencyToken { get; set; }
        public EnumAccountStatus Status { get; set; }
        public string Message { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }

    }

    public class AccountActivateModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public EnumAccountStatus Status { get; set; }
        public string Message { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }

    public class AccountResendModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public EnumAccountStatus Status { get; set; }

        public IList<LangViewModel> Langs { get; set; }
        public LangViewModel Lang { get; set; }
    }
}
