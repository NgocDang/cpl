using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPL.Models;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Core.Interfaces;
using AutoMapper;
using CPL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using CPL.Misc.Utils;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Google.Authenticator;
using CPL.Common.Enums;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace CPL.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ISysUserService _sysUserService;
        private readonly ICoinTransactionService _coinTransactionService;
        private readonly ITemplateService _templateService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IDataContextAsync _dataContextAsync;

        public ProfileController(
            IHostingEnvironment hostingEnvironment,
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ISysUserService sysUserService,
            ICoinTransactionService coinTransactionService,
            ILotteryHistoryService lotteryHistoryService,
            IDataContextAsync dataContextAsync,
            ITemplateService templateService)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._coinTransactionService = coinTransactionService;
            this._sysUserService = sysUserService;
            this._unitOfWork = unitOfWork;
            this._templateService = templateService;
            this._dataContextAsync = dataContextAsync;
            this._lotteryHistoryService = lotteryHistoryService;
        }

        [Permission(EnumRole.User)]
        public IActionResult Index()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<ProfileViewModel>(user);

            viewModel.NumberOfGameHistories = _lotteryHistoryService.Queryable().Count(x => x.SysUserId == viewModel.Id);
            viewModel.NumberOfTransactions = _coinTransactionService.Queryable().Count(x => x.SysUserId == viewModel.Id);

            // Mapping KYC status
            if (!user.KYCVerified.HasValue)
            {
                viewModel.KYCStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotVerifiedYet");
            }
            else if (user.KYCVerified == true)
            {
                viewModel.KYCStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Verified");
            }
            else // viewModel.KYCVerified == false
            {
                viewModel.KYCStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Pending");
            }

            // Mapping Affiliate status
            if (!user.AffiliateId.HasValue)
            {
                viewModel.AffiliateStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotJoinedYet");
            }
            else if (user.AffiliateId.Value != (int)EnumAffiliateApplicationStatus.PENDING)
            {
                viewModel.AffiliateStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Approved");
            }
            else // viewModel.AffiliateId.Value != (int)EnumAffiliateApplicationStatus.PENDING
            {
                viewModel.AffiliateStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Pending");
            }

            // Mapping TwoFactorAuthenticationEnable status
            if (user.TwoFactorAuthenticationEnable)
            {
                viewModel.TwoFactorAuthenticationEnableStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "IsEnabled");
            }
            else // viewModel.TwoFactorAuthenticationEnable == false
            {
                viewModel.TwoFactorAuthenticationEnableStatus = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "IsNotEnabled");
            }

            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult Update(ProfileViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Gender = viewModel.Gender;
                user.DOB = viewModel.DOB;
                user.PostalCode = viewModel.PostalCode;
                user.Country = viewModel.Country;
                user.City = viewModel.City;
                user.StreetAddress = viewModel.StreetAddress;
                user.Mobile = viewModel.Mobile;

                // KYC
                if (viewModel.FrontSideImage != null && viewModel.BackSideImage != null)
                {
                    user.KYCCreatedDate = DateTime.Now;
                    user.KYCVerified = false;
                    var kyc = Path.Combine(_hostingEnvironment.WebRootPath, @"images\kyc");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    // Front Size
                    var frontSide = $"{viewModel.Id.ToString()}_FS_{timestamp}_{viewModel.FrontSideImage.FileName}";
                    var frontSidePath = Path.Combine(kyc, frontSide);
                    viewModel.FrontSideImage.CopyTo(new FileStream(frontSidePath, FileMode.Create));
                    user.FrontSide = frontSide;

                    // Back Size
                    var backSide = $"{viewModel.Id.ToString()}_BS_{timestamp}_{viewModel.BackSideImage.FileName}";
                    var backSidePath = Path.Combine(kyc, backSide);
                    viewModel.BackSideImage.CopyTo(new FileStream(backSidePath, FileMode.Create));
                    user.BackSide = backSide;
                }

                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                if (viewModel.FrontSideImage != null && viewModel.BackSideImage != null)
                {
                    return new JsonResult(new
                    {
                        success = true,
                        message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PersonalInfoUpdated"),
                        kycconfirm = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCReceived"),
                        kycverify = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Pending")
                    });
                }

                return new JsonResult(new
                {
                    success = true,
                    message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PersonalInfoUpdated"),
                    gender = viewModel.Gender == true ? LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Male") : LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Female")
                });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [Permission(EnumRole.User)]
        public IActionResult Edit()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<ProfileViewModel>(user);
            viewModel.NumberOfGameHistories = _lotteryHistoryService.Queryable().Count(x => x.SysUserId == viewModel.Id);
            viewModel.NumberOfTransactions = _coinTransactionService.Queryable().Count(x => x.SysUserId == viewModel.Id);

            return PartialView("_Edit", viewModel);
        }

        [Permission(EnumRole.User)]
        public IActionResult Security()
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var viewModel = Mapper.Map<SecurityViewModel>(user);
            var tfa = new TwoFactorAuthenticator();
            var setupInfo = tfa.GenerateSetupCode(CPLConstant.AppName, user.Email, $"{CPLConstant.TwoFactorAuthenticationSecretKey}{user.Id}", 300, 300);
            viewModel.QrCodeSetupImageUrl = setupInfo.QrCodeSetupImageUrl;
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditEmail(EditEmailViewModel viewModel)
        {
            var isEmailExisting = _sysUserService.Queryable().Any(x => x.Email == viewModel.NewEmail && x.IsDeleted == false);
            if (isEmailExisting)
                return new JsonResult(new { success = false, name = "new-email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExistingEmail") });

            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                user.Email = viewModel.NewEmail;
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "EmailUpdated") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditPassword(EditPasswordViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(viewModel.CurrentPassword, user.Password))
                    return new JsonResult(new { success = false, name = "current-password", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidCurrentPassword") });

                user.Password = viewModel.NewPassword.ToBCrypt();
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PasswordUpdated") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [Permission(EnumRole.User)]
        public IActionResult KYC()
        {
            var user = _sysUserService.Queryable().Where(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id).FirstOrDefault();
            var viewModel = Mapper.Map<KYCViewModel>(user);
            if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName)
                && user.DOB.HasValue && !string.IsNullOrEmpty(user.Country) && !string.IsNullOrEmpty(user.City) && !string.IsNullOrEmpty(user.StreetAddress)
                && !string.IsNullOrEmpty(user.Mobile))
                viewModel.IsProfileEmpty = false;
            else
                viewModel.IsProfileEmpty = true;
            return View(viewModel);
        }

        [Permission(EnumRole.User)]
        public IActionResult Affiliate()
        {
            var user = _sysUserService.Queryable().Where(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id).FirstOrDefault();
            var viewModel = Mapper.Map<ProfileAffiliateViewModel>(user);

            viewModel.IsKYCVerificationActivated = bool.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.IsKYCVerificationActivated).Value);

            // Affiliate url
            viewModel.AffiliateUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Url.Action("Register", "Authentication", new { id = viewModel.Id });

            // Total sale
            SqlParameter TotalSaleParam = new SqlParameter()
            {
                ParameterName = "@TotalSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter TodaySaleParam = new SqlParameter()
            {
                ParameterName = "@TodaySale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter YesterdaySaleParam = new SqlParameter()
            {
                ParameterName = "@YesterdaySale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter[] parameters = {
                new SqlParameter() {
                    ParameterName = "@UserId",
                    SqlDbType = SqlDbType.Int,
                    Value = user.Id,
                    Direction = ParameterDirection.Input
                },
                TotalSaleParam, TodaySaleParam, YesterdaySaleParam
            };

            _dataContextAsync.ExecuteSqlCommand("exec dbo.usp_GetAffiliateSale @UserId, @TotalSale OUTPUT, @TodaySale OUTPUT, @YesterdaySale OUTPUT", parameters);

            viewModel.TotalSale = Convert.ToInt32(TotalSaleParam.Value);
            viewModel.TotalSaleToday = Convert.ToInt32(TodaySaleParam.Value);
            viewModel.TotalSaleYesterday = Convert.ToInt32(YesterdaySaleParam.Value);

            // Total user register
            viewModel.TotalUserRegister = _sysUserService.Queryable()
                                           .Where(x => x.IsIntroducedById != null && x.IsIntroducedById == user.Id).Count();
            viewModel.TotalUserRegisterToday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.Date).Count();
            viewModel.TotalUserRegisterYesterday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.AddDays(-1).Date).Count();

            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditTwoFactorAuthentication(bool value, string pin)
        {
            if (value)
            {
                var userId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
                var tfa = new TwoFactorAuthenticator();
                bool isCorrectPIN = tfa.ValidateTwoFactorPIN($"{CPLConstant.TwoFactorAuthenticationSecretKey}{userId}", pin);

                if (isCorrectPIN)
                {
                    var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == userId);
                    user.TwoFactorAuthenticationEnable = value;
                    HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "TwoFactorAuthenticationUpdated") });
                }
                else
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidPIN") });
            }
            else
            {
                var userId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == userId);
                user.TwoFactorAuthenticationEnable = value;
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "TwoFactorAuthenticationUpdated") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditKYC(KYCViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                if (viewModel.FrontSideImage != null && viewModel.BackSideImage != null)
                {
                    user.KYCCreatedDate = DateTime.Now;
                    user.KYCVerified = false;
                    var kyc = Path.Combine(_hostingEnvironment.WebRootPath, @"images\kyc");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    // Front Size
                    var frontSide = $"{viewModel.Id.ToString()}_FS_{timestamp}_{viewModel.FrontSideImage.FileName}";
                    var frontSidePath = Path.Combine(kyc, frontSide);
                    viewModel.FrontSideImage.CopyTo(new FileStream(frontSidePath, FileMode.Create));
                    user.FrontSide = frontSide;

                    // Back Size
                    var backSide = $"{viewModel.Id.ToString()}_BS_{timestamp}_{viewModel.BackSideImage.FileName}";
                    var backSidePath = Path.Combine(kyc, backSide);
                    viewModel.BackSideImage.CopyTo(new FileStream(backSidePath, FileMode.Create));
                    user.BackSide = backSide;
                }

                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                return new JsonResult(new
                {
                    success = true,
                    message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCSubmitted")
                });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoSubmitAffiliate(KYCViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                user.AffiliateId = (int)EnumAffiliateApplicationStatus.PENDING;
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                return new JsonResult(new
                {
                    success = true,
                    message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AffiliateApplicationSubmitted")
                });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }
    }
}
