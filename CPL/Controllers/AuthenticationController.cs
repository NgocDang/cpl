using System;
using System.Collections.Generic;
using System.Linq;
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
    [Permission(EnumRole.Guest)]
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

        public IActionResult Login(string returnUrl)
        {
            ClearSession();
            var viewModel = new AccountLoginModel { ReturnUrl = returnUrl };
            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            if (HttpContext.Session.GetInt32("LangId").HasValue)
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
            else
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);

            HttpContext.Session.SetInt32("LangId", 1);
            return View(viewModel);
        }

        [HttpPost]
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
                            return viewModel.ReturnUrl == null ? RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Dashboard")}") : RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{viewModel.ReturnUrl}");
                        }
                    }
                }
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidEmailPassword") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        public IActionResult VerifyPIN(AccountLoginModel viewModel)
        {
            var tfa = new TwoFactorAuthenticator();
            bool isCorrectPIN = tfa.ValidateTwoFactorPIN(CPLConstant.TwoFactorAuthenticationSecretKey, viewModel.PIN);

            if (isCorrectPIN)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email);
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                return RedirectToLocal($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Dashboard")}");
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidPIN") });
        }


        public ActionResult Register(int? id, string token)
        {
            ClearSession();

            var viewModel = new AccountRegistrationModel();
            //viewModel.Status = EnumAccountStatus.UNREGISTERED;
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
        public IActionResult Register(AccountRegistrationModel viewModel)
        {
            //// Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                if (_sysUserService.Queryable().Any(x => x.Email == viewModel.Email && x.IsDeleted == false))
                {
                    return new JsonResult(new { success = false, name = "email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExistingEmail") });
                }

                var isAccountActivationEnable = bool.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.IsAccountActivationEnable).Value);
                // Try to create a user with the given identity
                var user = new SysUser
                {
                    Email = viewModel.Email,
                    Password = viewModel.Password.ToBCrypt(),
                    CreatedDate = DateTime.Now,
                    IsAdmin = false,
                    ActivateToken = isAccountActivationEnable ? Guid.NewGuid().ToString() : null,
                    BTCHDWalletAddress = "1P8qwzdXgXBTyZPPXxsZPxTDmuimtTGj4d",
                    BTCAmount = 10,
                    ETHHDWalletAddress = "0xEbf7aEa65944A953C27643160797a90176e6f03f",
                    ETHAmount = 100,
                    TokenAmount = 10000000
                };

                _sysUserService.Insert(user);
                _unitOfWork.SaveChanges();

                if (isAccountActivationEnable)
                {
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Activate.ToString());
                    var activateViewModel = Mapper.Map<ActivateEmailTemplateViewModel>(user);
                    activateViewModel.ActivateUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Activate", "Authentication", new { token = activateViewModel.ActivateToken, id = activateViewModel.Id })}";
                    activateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    //Populate language
                    activateViewModel.RegistrationSuccessfulText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "RegistrationSuccessful");

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Authentication/_ActivateEmailTemplate.cshtml", activateViewModel).Result;
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ActivateEmailSent") });
                }
                else
                {
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Member.ToString());
                    var memberViewModel = Mapper.Map<MemberEmailTemplateViewModel>(user);
                    memberViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    // Populate language
                    memberViewModel.ActivationSuccessfulText = @LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ActivationSuccessful");

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Authentication/_MemberEmailTemplate.cshtml", memberViewModel).Result;
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);

                    // Log in
                    HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                    return new JsonResult(new { success = true, activated = true, url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Index", "Dashboard")}" });
                }
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        public IActionResult LogOut()
        {

            ClearSession();
            return RedirectToAction("Index", "Home");
        }

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
    }
}