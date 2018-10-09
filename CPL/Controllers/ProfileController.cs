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
using CPL.Domain;
using Newtonsoft.Json;

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
        private readonly IAgencyService _agencyService;
        private readonly IAffiliateService _affiliateService;
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
            IAffiliateService affiliateService,
            ICoinTransactionService coinTransactionService,
            ILotteryHistoryService lotteryHistoryService,
            IDataContextAsync dataContextAsync,
            ITemplateService templateService,
            IAgencyService agencyService)
        {
            this._hostingEnvironment = hostingEnvironment;
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._coinTransactionService = coinTransactionService;
            this._sysUserService = sysUserService;
            this._affiliateService = affiliateService;
            this._unitOfWork = unitOfWork;
            this._templateService = templateService;
            this._dataContextAsync = dataContextAsync;
            this._lotteryHistoryService = lotteryHistoryService;
            this._agencyService = agencyService;
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
            var tfa = new TwoFactorAuthenticator()
            {
                DefaultClockDriftTolerance = TimeSpan.FromSeconds(30)
            };
            var setupInfo = tfa.GenerateSetupCode(CPLConstant.AppName, user.Email, $"{CPLConstant.TwoFactorAuthenticationSecretKey}{user.Id}", 300, 300);
            viewModel.QrCodeSetupImageUrl = setupInfo.QrCodeSetupImageUrl;
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditEmail(EditEmailViewModel viewModel, MobileModel mobileModel)
        {
            var isEmailExisting = _sysUserService.Queryable().Any(x => x.Email == viewModel.NewEmail && x.IsDeleted == false);
            if (isEmailExisting)
            {
                if (mobileModel.IsMobile)
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.WARNING,
                        name = "new-email",
                        error_message_key = CPLConstant.MobileAppConstant.EditEmailScreenExistingEmail
                    });

                return new JsonResult(new { success = false, name = "new-email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExistingEmail") });
            }

            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                user.Email = viewModel.NewEmail;
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                if (mobileModel.IsMobile)
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        success_message_key = CPLConstant.MobileAppConstant.EditEmailScreenEmailUpdatedSuccessfully
                    });
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "EmailUpdated") });
            }

            if (mobileModel.IsMobile)
                return new JsonResult(new
                {
                    code = EnumResponseStatus.WARNING,
                    error_message_key = CPLConstant.MobileAppConstant.EditEmailScreenNonExistingAccount
                });
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditPassword(EditPasswordViewModel viewModel, MobileModel mobileModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(viewModel.CurrentPassword, user.Password))
                {
                    if (mobileModel.IsMobile)
                        return new JsonResult(new
                        {
                            code = EnumResponseStatus.WARNING,
                            error_message_key = CPLConstant.MobileAppConstant.EditPasswordScreenInvalidCurrentPassword
                        });
                    return new JsonResult(new { success = false, name = "current-password", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidCurrentPassword") });
                }
                    

                user.Password = viewModel.NewPassword.ToBCrypt();
                HttpContext.Session.SetObjectAsJson("CurrentUser", Mapper.Map<SysUserViewModel>(user));
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                if (mobileModel.IsMobile)
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        success_message_key = CPLConstant.MobileAppConstant.EditPasswordScreenPasswordUpdatedSuccessfully
                    });
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PasswordUpdated") });
            }

            if (mobileModel.IsMobile)
                return new JsonResult(new
                {
                    code = EnumResponseStatus.WARNING,
                    error_message_key = CPLConstant.MobileAppConstant.EditPasswordScreenNonExistingAccount
                });
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
            var isKYCVerificationActivated = bool.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.IsKYCVerificationActivated).Value);

            if((isKYCVerificationActivated && (!user.KYCVerified.HasValue || !user.KYCVerified.Value))
                || !user.AffiliateId.HasValue // Not joined yet
                || user.AffiliateId.Value == (int)EnumAffiliateApplicationStatus.PENDING) //Pending
            {
                var viewModel = Mapper.Map<SubmitAffiliateViewModel>(user);
                viewModel.IsKYCVerificationActivated = isKYCVerificationActivated;
                return View("SubmitAffiliate", viewModel);
            } else if (user.AgencyId.HasValue && !user.IsIntroducedById.HasValue) // TopAgency
            {
                var viewModel = TopAgencyAffiliate(user);
                return View("TopAgencyAffiliate", viewModel);
            } else //if (!user.AgencyId.HasValue || (user.AgencyId.HasValue && user.IsIntroducedById.HasValue)) // Standard Affiliate
            {
                var viewModel = StandardAffiliate(user);
                return View("StandardAffiliate", viewModel);
            }
        }

        private TopAgencyAffiliateViewModel TopAgencyAffiliate(SysUser user)
        {
            var viewModel = Mapper.Map<TopAgencyAffiliateViewModel>(user);
            var agency = _agencyService.Queryable().FirstOrDefault(x => x.Id == user.AgencyId);

            // Affiliate url
            viewModel.AffiliateUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Url.Action("Register", "Authentication", new { id = viewModel.Id });
            viewModel.IsTier2TabVisible = agency.IsTier2TabVisible;
            viewModel.IsTier3TabVisible = agency.IsTier3TabVisible;

            // Total sale
            SqlParameter TotalAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@TotalAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter TodayAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@TodayAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter YesterdayAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@YesterdayAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter[] parameters = {
                    new SqlParameter() {
                        ParameterName = "@SysUserId",
                        SqlDbType = SqlDbType.Int,
                        Value = user.Id,
                        Direction = ParameterDirection.Input
                    },
                    TotalAffiliateSaleParam, TodayAffiliateSaleParam, YesterdayAffiliateSaleParam
                };

            _dataContextAsync.ExecuteSqlCommand("exec dbo.usp_GetAffiliateSale @SysUserId, @TotalAffiliateSale OUTPUT, @TodayAffiliateSale OUTPUT, @YesterdayAffiliateSale OUTPUT", parameters);

            viewModel.TotalAffiliateSale = Convert.ToInt32((TotalAffiliateSaleParam.Value as int?).GetValueOrDefault(0));
            viewModel.TodayAffiliateSale = Convert.ToInt32((TodayAffiliateSaleParam.Value as int?).GetValueOrDefault(0));
            viewModel.YesterdayAffiliateSale = Convert.ToInt32((YesterdayAffiliateSaleParam.Value as int?).GetValueOrDefault(0));

            // Total user register
            viewModel.TotalIntroducedUsers = _sysUserService.Queryable()
                                           .Count(x => x.IsIntroducedById != null && x.IsIntroducedById == user.Id);
            viewModel.TotalIntroducedUsersToday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.Date).Count();
            viewModel.TotalIntroducedUsersYesterday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.AddDays(-1).Date).Count();

            return viewModel;
        }

        private StandardAffiliateViewModel StandardAffiliate(SysUser user)
        {
            var viewModel = Mapper.Map<StandardAffiliateViewModel>(user);
            var affiliate = _affiliateService.Queryable().FirstOrDefault(x => x.Id == user.AffiliateId);

            // Affiliate url
            viewModel.AffiliateUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + Url.Action("Register", "Authentication", new { id = viewModel.Id });
            viewModel.IsTier2TabVisible = affiliate.IsTier2TabVisible;
            viewModel.IsTier3TabVisible = affiliate.IsTier3TabVisible;

            // Total sale
            SqlParameter TotalAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@TotalAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter TodayAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@TodayAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter YesterdayAffiliateSaleParam = new SqlParameter()
            {
                ParameterName = "@YesterdayAffiliateSale",
                SqlDbType = SqlDbType.Money,
                Direction = ParameterDirection.Output,
                IsNullable = true
            };
            SqlParameter[] parameters = {
                    new SqlParameter() {
                        ParameterName = "@SysUserId",
                        SqlDbType = SqlDbType.Int,
                        Value = user.Id,
                        Direction = ParameterDirection.Input
                    },
                    TotalAffiliateSaleParam, TodayAffiliateSaleParam, YesterdayAffiliateSaleParam
                };

            _dataContextAsync.ExecuteSqlCommand("exec dbo.usp_GetAffiliateSale @SysUserId, @TotalAffiliateSale OUTPUT, @TodayAffiliateSale OUTPUT, @YesterdayAffiliateSale OUTPUT", parameters);

            viewModel.TotalAffiliateSale = Convert.ToInt32((TotalAffiliateSaleParam.Value as int?).GetValueOrDefault(0));
            viewModel.TodayAffiliateSale = Convert.ToInt32((TodayAffiliateSaleParam.Value as int?).GetValueOrDefault(0));
            viewModel.YesterdayAffiliateSale = Convert.ToInt32((YesterdayAffiliateSaleParam.Value as int?).GetValueOrDefault(0));

            // Total user register
            viewModel.TotalIntroducedUsers = _sysUserService.Queryable()
                                           .Count(x => x.IsIntroducedById != null && x.IsIntroducedById == user.Id);
            viewModel.TotalIntroducedUsersToday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.Date).Count();
            viewModel.TotalIntroducedUsersYesterday = _sysUserService.Queryable()
                                            .Where(x => x.IsIntroducedById.HasValue && x.IsIntroducedById.Value == user.Id
                                            && x.AffiliateCreatedDate.HasValue && x.AffiliateCreatedDate.Value.Date == DateTime.Now.AddDays(-1).Date).Count();

            
            return viewModel;
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoEditTwoFactorAuthentication(bool value, string pin)
        {
            if (value)
            {
                var userId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
                var tfa = new TwoFactorAuthenticator()
                {
                    DefaultClockDriftTolerance = TimeSpan.FromSeconds(30)
                };
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

        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetTopAgencyStatistics(int sysUserId, int periodInDay, int pageSize = 10, int pageIndex = 1,
                                                    string orderColumn = "UsedCPL", string orderDirection = "desc", string searchValue = "")
        {
            List<SqlParameter> storeParams = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value= sysUserId},
                new SqlParameter() {ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
            };

            var dataSet = _dataContextAsync.ExecuteStoredProcedure("usp_GetAffiliateInfo_Tier1_Statistics", storeParams);

            var totalAffiliateSaleChanges = new List<SummaryChange>();
            var directAffiliateSaleChanges = new List<SummaryChange>();
            var totalIntroducedUsersChanges = new List<SummaryChange>();
            var directIntroducedUsersChanges = new List<SummaryChange>();

            for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
            {
                totalAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalAffiliateSale"])) });
                directAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["DirectAffiliateSale"])) });
                totalIntroducedUsersChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalIntroducedUsers"])) });
                directIntroducedUsersChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["DirectIntroducedUsers"])) });
            }

            //Table[0]//////////////////////////////////////////////////////
            //TotalSale|DirectSale|TotalIntroducedUsers|DirectIntroducedUsers
            //123//////456/////////789//////////////////10//////////////////

            var viewModel = new TopAgencyAffiliateInfoViewModel
            {
                TotalAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]),
                DirectAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[1]),
                TotalIntroducedUsers = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[2]),
                DirectIntroducedUsers = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[3]),

                TotalAffiliateSaleChangesInJson = JsonConvert.SerializeObject(totalAffiliateSaleChanges),
                DirectAffiliateSaleChangesInJson = JsonConvert.SerializeObject(directAffiliateSaleChanges),
                TotalIntroducedUsersChangesInJson = JsonConvert.SerializeObject(totalIntroducedUsersChanges),
                DirectIntroducedUsersChangesInJson = JsonConvert.SerializeObject(directIntroducedUsersChanges),
            };

            return PartialView("_TopAgencyAffiliateStatistics", viewModel);
        }

        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetNonTopAgencyStatistics(int sysUserId, int periodInDay, string kindOfTier, int pageSize = 10, int pageIndex = 1,
                                                    string orderColumn = "UsedCPL", string orderDirection = "desc", string searchValue = "")
        {
            List<SqlParameter> storeParams = new List<SqlParameter>() {
                    new SqlParameter() {ParameterName = "@Tier", SqlDbType = SqlDbType.Int, Value= int.Parse(kindOfTier)},
                    new SqlParameter() {ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value= sysUserId},
                    new SqlParameter() {ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                };

            var dataSet = _dataContextAsync.ExecuteStoredProcedure("usp_GetAffiliateInfo_NonTier1_Statistics", storeParams);

            var totalAffiliateSaleChanges = new List<SummaryChange>();

            for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
            {
                totalAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalAffiliateSale"])) });
            }

            //Table[0]//////////////////////////////////////////////////////
            //TotalSale
            //123//////
            var viewModel = new NonTopAgencyAffiliateInfoViewModel
            {
                TotalAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]),

                TotalAffiliateSaleChangesInJson = JsonConvert.SerializeObject(totalAffiliateSaleChanges),
            };

            return PartialView("_NonTopAgencyAffiliateStatistics", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public JsonResult SearchTopAgencyAffiliate(DataTableAjaxPostModel viewModel, int sysUserId, string kindOfTier, int periodInDay)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchTopAgencyAffiliateFunc(viewModel, out filteredResultsCount, out totalResultsCount, sysUserId, kindOfTier, periodInDay);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.User)]
        public IList<TopAgencyAffiliateIntroducedUsersViewModel> SearchTopAgencyAffiliateFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int sysUserId, string kindOfTier, int periodInDay)
        {
            var searchBy = (model.search.value != null) ? model.search.value : string.Empty;
            var pageSize = model.length;
            var pageIndex = model.start + 1;

            string sortBy = string.Empty;
            string sortDir = "KindOfTier";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }

            List<SqlParameter> storeParams = new List<SqlParameter>()
            {
                new SqlParameter() {ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value= sysUserId},
                new SqlParameter() {ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                new SqlParameter() {ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = pageSize},
                new SqlParameter() {ParameterName = "@PageIndex", SqlDbType = SqlDbType.Int, Value = pageIndex},
                new SqlParameter() {ParameterName = "@OrderColumn", SqlDbType = SqlDbType.NVarChar, Value = sortBy},
                new SqlParameter() {ParameterName = "@OrderDirection", SqlDbType = SqlDbType.NVarChar, Value = sortDir},
                new SqlParameter() {ParameterName = "@SearchValue", SqlDbType = SqlDbType.NVarChar, Value = searchBy},
            };

            var uspName = string.Empty;
            if (kindOfTier == ((int)EnumKindOfTier.TIER1).ToString())
            {
                uspName = "[usp_GetAffiliateInfo_Tier1_IntroducedUsers]";
            }
            else if (kindOfTier == ((int)EnumKindOfTier.TIER2).ToString())
            {
                uspName = "[usp_GetAffiliateInfo_NonTier1_IntroducedUsers]";
                storeParams.Add(new SqlParameter() { ParameterName = "@Tier", SqlDbType = SqlDbType.Int, Value = (int)EnumKindOfTier.TIER2 });
            }
            else if (kindOfTier == ((int)EnumKindOfTier.TIER3).ToString())
            {
                uspName = "[usp_GetAffiliateInfo_NonTier1_IntroducedUsers]";
                storeParams.Add(new SqlParameter() { ParameterName = "@Tier", SqlDbType = SqlDbType.Int, Value = (int)EnumKindOfTier.TIER3 });
            }

            var dataSet = _dataContextAsync.ExecuteStoredProcedure(uspName, storeParams);

            DataTable table = dataSet.Tables[0];
            var rows = new List<DataRow>(table.Rows.OfType<DataRow>()); //  the Rows property of the DataTable object is a collection that implements IEnumerable but not IEnumerable<T>
            var viewModels = Mapper.Map<List<DataRow>, List<TopAgencyAffiliateIntroducedUsersViewModel>>(rows);

            totalResultsCount = Convert.ToInt32((dataSet.Tables[1].Rows[0])["TotalCount"]);
            filteredResultsCount = Convert.ToInt32((dataSet.Tables[2].Rows[0])["FilteredCount"]);

            return viewModels;
        }


        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetTier1StandardAffiliateStatistics(int sysUserId, int periodInDay, int pageSize = 10, int pageIndex = 1,
                                                   string orderColumn = "UsedCPL", string orderDirection = "desc", string searchValue = "")
        {
            List<SqlParameter> storeParams = new List<SqlParameter>() {
                    new SqlParameter() {ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value= sysUserId},
                    new SqlParameter() {ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                };

            var dataSet = _dataContextAsync.ExecuteStoredProcedure("usp_GetAffiliateInfo_Tier1_Statistics", storeParams);

            var totalAffiliateSaleChanges = new List<SummaryChange>();
            var directAffiliateSaleChanges = new List<SummaryChange>();
            var totalIntroducedUsersChanges = new List<SummaryChange>();
            var directIntroducedUsersChanges = new List<SummaryChange>();

            for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
            {
                totalAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalAffiliateSale"])) });
                directAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["DirectAffiliateSale"])) });
                totalIntroducedUsersChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalIntroducedUsers"])) });
                directIntroducedUsersChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["DirectIntroducedUsers"])) });
            }

            //Table[0]//////////////////////////////////////////////////////
            //TotalSale|DirectSale|TotalIntroducedUsers|DirectIntroducedUsers
            //123//////456/////////789//////////////////10//////////////////
            var viewModel = new Tier1StandardAffiliateInfoViewModel
            {
                TotalAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]),
                DirectAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[1]),
                TotalIntroducedUsers = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[2]),
                DirectIntroducedUsers = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[3]),

                TotalAffiliateSaleChangesInJson = JsonConvert.SerializeObject(totalAffiliateSaleChanges),
                DirectAffiliateSaleChangesInJson = JsonConvert.SerializeObject(directAffiliateSaleChanges),
                TotalIntroducedUsersChangesInJson = JsonConvert.SerializeObject(totalIntroducedUsersChanges),
                DirectIntroducedUsersChangesInJson = JsonConvert.SerializeObject(directIntroducedUsersChanges),
            };

            return PartialView("_Tier1StandardAffiliateStatistics", viewModel);
        }

        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetNonTier1StandardAffiliateStatistics(int sysUserId, int periodInDay, string kindOfTier, int pageSize = 10, int pageIndex = 1,
                                                    string orderColumn = "UsedCPL", string orderDirection = "desc", string searchValue = "")
        {
            List<SqlParameter> storeParams = new List<SqlParameter>() {
                    new SqlParameter() {ParameterName = "@Tier", SqlDbType = SqlDbType.Int, Value= int.Parse(kindOfTier)},
                    new SqlParameter() {ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value= sysUserId},
                    new SqlParameter() {ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                };

            var dataSet = _dataContextAsync.ExecuteStoredProcedure("usp_GetAffiliateInfo_NonTier1_Statistics", storeParams);

            var totalAffiliateSaleChanges = new List<SummaryChange>();

            for (int i = 0; i < dataSet.Tables[1].Rows.Count; i++)
            {
                totalAffiliateSaleChanges.Add(new SummaryChange { Date = DateTime.Parse(((dataSet.Tables[1].Rows[i])["Date"]).ToString()), Value = Convert.ToInt32(((dataSet.Tables[1].Rows[i])["TotalAffiliateSale"])) });
            }

            //Table[0]//////////////////////////////////////////////////////
            //TotalSale
            //123//////
            var viewModel = new NonTier1StandardAffiliateInfoViewModel
            {
                TotalAffiliateSale = Convert.ToInt32(dataSet.Tables[0].Rows[0].ItemArray[0]),

                TotalAffiliateSaleChangesInJson = JsonConvert.SerializeObject(totalAffiliateSaleChanges),
            };

            return PartialView("_NonTier1StandardAffiliateStatistics", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public JsonResult SearchStandardAffiliateIntroducedUsers(DataTableAjaxPostModel viewModel, int sysUserId, string kindOfTier, int periodInDay)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchStandardAffiliateIntroducedUsersFunc(viewModel, out filteredResultsCount, out totalResultsCount, sysUserId, kindOfTier, periodInDay);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.User)]
        public IList<StandardAffiliateIntroducedUsersViewModel> SearchStandardAffiliateIntroducedUsersFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int sysUserId, string kindOfTier, int periodInDay)
        {
            var searchBy = (model.search.value != null) ? model.search.value : string.Empty;
            var pageSize = model.length;
            var pageIndex = model.start + 1;

            string sortBy = string.Empty;
            string sortDir = "UsedCPL";

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower();
            }

            var uspName = string.Empty;

            List<StandardAffiliateIntroducedUsersViewModel> viewModel = new List<StandardAffiliateIntroducedUsersViewModel>();
            List<SqlParameter> storeParams = new List<SqlParameter>();

            if (kindOfTier == ((int)EnumKindOfTier.TIER1).ToString())
            {
                storeParams = new List<SqlParameter>()            {
                    new SqlParameter() { ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value = sysUserId},
                    new SqlParameter() { ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                    new SqlParameter() { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = pageSize},
                    new SqlParameter() { ParameterName = "@PageIndex", SqlDbType = SqlDbType.Int, Value = pageIndex},
                    new SqlParameter() { ParameterName = "@OrderColumn", SqlDbType = SqlDbType.NVarChar, Value = sortBy},
                    new SqlParameter() { ParameterName = "@OrderDirection", SqlDbType = SqlDbType.NVarChar, Value = sortDir},
                    new SqlParameter() { ParameterName = "@SearchValue", SqlDbType = SqlDbType.NVarChar, Value = searchBy},
                };

                uspName = "usp_GetAffiliateInfo_Tier1_IntroducedUsers";

            }
            else
            {
                storeParams = new List<SqlParameter>()            {
                    new SqlParameter() { ParameterName = "@Tier", SqlDbType = SqlDbType.Int, Value = int.Parse(kindOfTier)},
                    new SqlParameter() { ParameterName = "@SysUserId", SqlDbType = SqlDbType.Int, Value = sysUserId},
                    new SqlParameter() { ParameterName = "@PeriodInDay", SqlDbType = SqlDbType.Int, Value = periodInDay},
                    new SqlParameter() { ParameterName = "@PageSize", SqlDbType = SqlDbType.Int, Value = pageSize},
                    new SqlParameter() { ParameterName = "@PageIndex", SqlDbType = SqlDbType.Int, Value = pageIndex},
                    new SqlParameter() { ParameterName = "@OrderColumn", SqlDbType = SqlDbType.NVarChar, Value = sortBy},
                    new SqlParameter() { ParameterName = "@OrderDirection", SqlDbType = SqlDbType.NVarChar, Value = sortDir},
                    new SqlParameter() { ParameterName = "@SearchValue", SqlDbType = SqlDbType.NVarChar, Value = searchBy},
                };

                uspName = "usp_GetAffiliateInfo_NonTier1_IntroducedUsers";
            }

            var dataSet = _dataContextAsync.ExecuteStoredProcedure(uspName, storeParams);
            DataTable table = dataSet.Tables[0]; 
            var rows = new List<DataRow>(table.Rows.OfType<DataRow>()); //  the Rows property of the DataTable object is a collection that implements IEnumerable but not IEnumerable<T>
            var viewModels = Mapper.Map<List<DataRow>, List<StandardAffiliateIntroducedUsersViewModel>>(rows);

            totalResultsCount = Convert.ToInt32((dataSet.Tables[1].Rows[0])["TotalCount"]);
            filteredResultsCount = Convert.ToInt32((dataSet.Tables[2].Rows[0])["FilteredCount"]);

            return viewModels;
        }
    }
}
