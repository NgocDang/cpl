using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Google.Authenticator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CPL.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly ISysUserService _sysUserService;
        private readonly ITemplateService _templateService;
        private readonly ISettingService _settingService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IViewRenderService _viewRenderService;

        public AuthenticationController(ILangService langService, IMapper mapper, ISettingService settingService,
        ISysUserService sysUserService, IUnitOfWorkAsync unitOfWork, ITemplateService templateService, IViewRenderService viewRenderService)
        {
            _langService = langService;
            _mapper = mapper;
            _sysUserService = sysUserService;
            _settingService = settingService;
            _templateService = templateService;
            _unitOfWork = unitOfWork;
            _viewRenderService = viewRenderService;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Login(string returnUrl)
        {
            ClearSession();
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            var gcaptchaKey = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.GCaptchaKey)?.Value;
            viewModel.GCaptchaKey = gcaptchaKey;

            if (HttpContext.Session.GetInt32("LangId").HasValue)
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
            else
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);

            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult Login(AccountLoginModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email && x.IsDeleted == false);

                if (user != null && BCrypt.Net.BCrypt.Verify(viewModel.Password, user.Password))
                {
                    if (!string.IsNullOrEmpty(user.ActivateToken))
                        return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "EmailInactivatingAccount") });
                    else
                    {
                        if (user.TwoFactorAuthenticationEnable)
                        {
                            return new JsonResult(new { success = true, twofactor = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "WaitingPIN") });
                        }
                        else
                        {
                            HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                            return viewModel.ReturnUrl == null ? RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Home")}") : RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{viewModel.ReturnUrl}");
                        }
                    }
                }
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidEmailPassword") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult VerifyPIN(AccountLoginModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email);
            var tfa = new TwoFactorAuthenticator();
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN($"{CPLConstant.TwoFactorAuthenticationSecretKey}{user.Id}", viewModel.PIN);

            if (isCorrectPIN)
            {
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                return RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Home")}");
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidPIN") });
        }

        [Permission(EnumRole.Guest)]
        public ActionResult Register(int? id, string token)
        {
            ClearSession();

            var viewModel = new AccountRegistrationModel();
            //viewModel.Status = EnumAccountStatus.UNREGISTERED;
            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();
				
            var gcaptchaKey = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.GCaptchaKey)?.Value;
            viewModel.GCaptchaKey = gcaptchaKey;
			
            if (HttpContext.Session.GetInt32("LangId").HasValue)
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
            else
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);

            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult DoRegister(AccountRegistrationModel viewModel)
        {
            //// Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                if (_sysUserService.Queryable().Any(x => x.Email == viewModel.Email && x.IsDeleted == false))
                {
                    return new JsonResult(new { success = false, name = "email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExistingEmail") });
                }

                var isAccountActivationEnable = bool.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.IsAccountActivationEnable).Value);
                var latestAddressIndex = _sysUserService.Queryable().LastOrDefault()?.ETHHDWalletAddressIndex ?? 0;
                // Try to create a user with the given identity
                var user = new SysUser
                {
                    Email = viewModel.Email,
                    Password = viewModel.Password.ToBCrypt(),
                    CreatedDate = DateTime.Now,
                    IsAdmin = false,
                    ActivateToken = isAccountActivationEnable ? Guid.NewGuid().ToString() : null,
                    BTCAmount = 0,
                    ETHAmount = 0,
                    TokenAmount = 0
                };

                try
                {
                    var requestCount = 0;
                    var isETHHDWalletAddressGenerated = false;
                    var isBTCHDWalletAddressGenerated = false;
                    while (requestCount < CPLConstant.RequestCountLimit)
                    {
                        // Populate ETH HD Wallet Address
                        if (!isETHHDWalletAddressGenerated)
                        {
                            var eWallet = new EWalletService.EWalletClient().GetAccountAsync(Authentication.Token, CPLConstant.ETHMnemonic, latestAddressIndex + 1);
                            eWallet.Wait();

                            if (eWallet.Result.Status.Code == 0) //OK
                            {
                                user.ETHHDWalletAddress = eWallet.Result.Address;
                                user.ETHHDWalletAddressIndex = latestAddressIndex + 1;
                                isETHHDWalletAddressGenerated = true;
                            }
                        }


                        // Populate BTC HD Wallet Address
                        if (!isBTCHDWalletAddressGenerated)
                        {
                            var bWallet = new BWalletService.BWalletClient().GetAccountAsync(Authentication.Token, CPLConstant.BTCMnemonic, latestAddressIndex + 1);
                            bWallet.Wait();

                            if (bWallet.Result.Status.Code == 0) //OK
                            {
                                user.BTCHDWalletAddress = bWallet.Result.Address;
                                user.BTCHDWalletAddressIndex = latestAddressIndex + 1;
                                isBTCHDWalletAddressGenerated = true;
                            }
                        }


                        if (isETHHDWalletAddressGenerated && isBTCHDWalletAddressGenerated)
                            break;
                        else
                        {
                            requestCount++;
                            Thread.Sleep(CPLConstant.RequestCountIntervalInMiliseconds);
                        }
                    }

                    if (requestCount == CPLConstant.RequestCountLimit)
                        return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }


                _sysUserService.Insert(user);
                _unitOfWork.SaveChanges();

                if (isAccountActivationEnable)
                {
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Activate.ToString());
                    var activateEmailTemplateViewModel = Mapper.Map<ActivateEmailTemplateViewModel>(user);
                    activateEmailTemplateViewModel.ActivateUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Activate", "Authentication", new { token = activateEmailTemplateViewModel.ActivateToken, id = activateEmailTemplateViewModel.Id })}";
                    activateEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    //Populate language
                    activateEmailTemplateViewModel.RegistrationSuccessfulText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "RegistrationSuccessful");
                    activateEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
                    activateEmailTemplateViewModel.RegisterActivateText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "RegisterActivate");
                    activateEmailTemplateViewModel.NotWorkUrlText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotWorkUrl");
                    activateEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
                    activateEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
                    activateEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
                    activateEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
                    activateEmailTemplateViewModel.ExpiredEmail24hText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExpiredEmail24h");
                    activateEmailTemplateViewModel.ActivateText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Activate");

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Authentication/_ActivateEmailTemplate.cshtml", activateEmailTemplateViewModel).Result;
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ActivateEmailSent") });
                }
                else
                {
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Member.ToString());
                    var memberEmailTemplateViewModel = Mapper.Map<MemberEmailTemplateViewModel>(user);
                    memberEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    // Populate language
                    memberEmailTemplateViewModel.ActivationSuccessfulText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ActivationSuccessful");
                    memberEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
                    memberEmailTemplateViewModel.TeamMemberNowText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "TeamMemberNow");
                    memberEmailTemplateViewModel.PlayGameNowText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PlayGameNow");
                    memberEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
                    memberEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
                    memberEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
                    memberEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
                    memberEmailTemplateViewModel.CPLTeamText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CPLTeam");

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Authentication/_MemberEmailTemplate.cshtml", memberEmailTemplateViewModel).Result;
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);

                    // Log in
                    HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                    return new JsonResult(new { success = true, activated = true, url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Home")}" });
                }
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [Permission(EnumRole.User)]
        public IActionResult LogOut()
        {
            ClearSession();
            return RedirectToAction("Index", "Home");
        }

        [Permission(EnumRole.User)]
        public IActionResult GetConfirm()
        {
            return PartialView("_LogoutConfirm");
        }

        private IActionResult RedirectToLocal(string returnUrl = "")
        {
            if (!string.IsNullOrWhiteSpace(returnUrl))
                return new JsonResult(new
                {
                    success = true,
                    url = returnUrl
                });

            return new JsonResult(new
            {
                success = true,
                url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("LogIn", "Authentication")}"
            });
        }

        private void ClearSession()
        {
            // Keep language setting
            var langId = (int)EnumLang.ENGLISH;
            if (HttpContext.Session.GetInt32("LangId").HasValue)
                langId = HttpContext.Session.GetInt32("LangId").Value;

            // Clear session
            HttpContext.Session.Clear();

            // Reset language setting
            HttpContext.Session.SetInt32("LangId", langId);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public ActionResult DoForgotPassword(AccountForgotPasswordModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email && x.IsDeleted == false);
                if (user == null)
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });

                user.ResetPasswordDate = DateTime.Now;
                user.ResetPasswordToken = Guid.NewGuid().ToString();
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.ForgotPassword.ToString());

                var forgotPasswordEmailTemplateViewModel = Mapper.Map<ForgotPasswordEmailTemplateViewModel>(user);
                forgotPasswordEmailTemplateViewModel.ResetPasswordUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("ResetPassword", "Authentication", new { token = forgotPasswordEmailTemplateViewModel.ResetPasswordToken, id = forgotPasswordEmailTemplateViewModel.Id })}";
                forgotPasswordEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                // Populate language
                forgotPasswordEmailTemplateViewModel.ResetYourPasswordText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ResetYourPassword");
                forgotPasswordEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
                forgotPasswordEmailTemplateViewModel.ResetPasswordRequestText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ResetPasswordRequest");
                forgotPasswordEmailTemplateViewModel.ButtonClickBelowText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ButtonClickBelow");
                forgotPasswordEmailTemplateViewModel.NotWorkUrlText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotWorkUrl");
                forgotPasswordEmailTemplateViewModel.NotYourRequestText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotYourRequest");
                forgotPasswordEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
                forgotPasswordEmailTemplateViewModel.ConnectWithUsText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ConnectWithUs");
                forgotPasswordEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
                forgotPasswordEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
                forgotPasswordEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
                forgotPasswordEmailTemplateViewModel.ExpiredEmail24hText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExpiredEmail24h");

                template.Body = _viewRenderService.RenderToStringAsync("/Views/Authentication/_ForgotPasswordEmailTemplate.cshtml", forgotPasswordEmailTemplateViewModel).Result;
                EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ResetPasswordEmailSent") });
            }

            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [Permission(EnumRole.Guest)]
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            ClearSession();

            var viewModel = new AccountForgotPasswordModel();
            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            if (HttpContext.Session.GetInt32("LangId").HasValue)
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
            else
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public ActionResult DoResetPassword(AccountResetPasswordModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == viewModel.Id);
                if (user == null)
                {
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
                }
                else if (user.ResetPasswordToken != viewModel.Token)
                {
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidToken") });
                }
                else
                {
                    user.ResetPasswordDate = null;
                    user.ResetPasswordToken = null;
                    user.Password = viewModel.Password.ToBCrypt();
                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = $"{LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PasswordResetSuccessfully")} {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Click")} <a href='{$"{ HttpContext.Request.Scheme }://{HttpContext.Request.Host}{Url.Action("LogIn", "Authentication")}"}'>{LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Here")}</a> {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ToLogIn")}" });
                }
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [Permission(EnumRole.Guest)]
        public ActionResult ResetPassword(int id, string token)
        {
            var viewmodel = new AccountResetPasswordModel();
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                viewmodel.Status = EnumAccountStatus.REQUEST_NOT_EXIST;
                viewmodel.Message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount");
                viewmodel.Langs = _langService.Queryable()
                    .Select(x => Mapper.Map<LangViewModel>(x))
                    .ToList();

                if (HttpContext.Session.GetInt32("LangId").HasValue)
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
                else
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
                return View(viewmodel);
            }
            else if (user.ResetPasswordDate == null)
            {
                viewmodel.Status = EnumAccountStatus.REQUEST_NOT_EXIST;
                viewmodel.Message = $"Request does not exist. Please try again.";
                viewmodel.Langs = _langService.Queryable()
                    .Select(x => Mapper.Map<LangViewModel>(x))
                    .ToList();

                if (HttpContext.Session.GetInt32("LangId").HasValue)
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
                else
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
                return View(viewmodel);
            }
            else if (user.ResetPasswordDate.Value.AddDays(int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.ResetPasswordExpiredInDays).Value)) > DateTime.Now)
            {
                if (user.ResetPasswordToken == token)
                {
                    viewmodel.Id = id;
                    viewmodel.Token = token;
                    viewmodel.Langs = _langService.Queryable()
                        .Select(x => Mapper.Map<LangViewModel>(x))
                        .ToList();

                    if (HttpContext.Session.GetInt32("LangId").HasValue)
                        viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
                    else
                        viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
                    return View(viewmodel);
                }
                else
                {
                    viewmodel.Status = EnumAccountStatus.INVALID_TOKEN;
                    viewmodel.Message = $"Invalid token. Please try again.";
                    viewmodel.Langs = _langService.Queryable()
                        .Select(x => Mapper.Map<LangViewModel>(x))
                        .ToList();

                    if (HttpContext.Session.GetInt32("LangId").HasValue)
                        viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
                    else
                        viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
                    return View(viewmodel);
                }
            }
            else
            {
                viewmodel.Status = EnumAccountStatus.EXPIRED;
                viewmodel.Message = $"{LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExpiredResetPasswordToken")} {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Click")} <a href='{$"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("ForgotPassword", "Authentication")}"}'>{LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Here")}</a> {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ToAskResetPassword")}";
                viewmodel.Langs = _langService.Queryable()
                    .Select(x => Mapper.Map<LangViewModel>(x))
                    .ToList();

                if (HttpContext.Session.GetInt32("LangId").HasValue)
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
                else
                    viewmodel.Lang = viewmodel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);
                return View(viewmodel);
            }
        }
    }
}