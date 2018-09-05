using AutoMapper;
using CPL.Common.CurrenciesPairRateHelper;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CPL.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly INewsService _newsService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILotteryService _lotteryService;
        private readonly ILotteryPrizeService _lotteryPrizeService;
        private readonly IAgencyTokenService _agencyTokenService;
        private readonly IAffiliateService _affiliateService;

        public AdminController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            INewsService newsService,
            IHostingEnvironment hostingEnvironment,
            ILotteryService lotteryService,
            IAffiliateService affiliateService,
            ILotteryPrizeService lotteryPrizeService,
            IAgencyTokenService agencyTokenService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._lotteryService = lotteryService;
            this._lotteryPrizeService = lotteryPrizeService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._newsService = newsService;
            this._affiliateService = affiliateService;
            this._hostingEnvironment = hostingEnvironment;
            this._agencyTokenService = agencyTokenService;
        }

        [Permission(EnumRole.Admin)]
        public IActionResult Index()
        {
            var viewModel = new AdminViewModel();

            // User management
            viewModel.TotalKYCPending = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && !x.KYCVerified.Value);
            viewModel.TotalKYCVerified = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && x.KYCVerified.Value);
            viewModel.TotalUser = _sysUserService.Queryable().Count();
            viewModel.TotalUserToday = _sysUserService.Queryable().Count(x => x.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"));
            viewModel.TotalUserYesterday = _sysUserService.Queryable().Count(x => x.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));

            // Game management
            var lotteryGames = _lotteryService.Queryable();
            viewModel.TotalLotteryGame = lotteryGames.Count();
            viewModel.TotalLotteryGamePending = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.PENDING).Count();
            viewModel.TotalLotteryGameActive = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.ACTIVE).Count();
            viewModel.TotalLotteryGameCompleted = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.COMPLETED).Count();

            // Affiliate
            // TODO: Get data from database
            viewModel.TotalAgencyAffiliate = 1000;
            viewModel.TotalAgencyAffiliateToday = 10;
            viewModel.TotalAgencyAffiliateYesterday = 10;
            viewModel.TotalStandardAffiliate = 1000;
            viewModel.TotalStandardAffiliateToday = 10;
            viewModel.TotalStandardAffiliateYesterday = 10;

            //Setting
            var settings = _settingService.Queryable();
            viewModel.KYCVerificationActivated = bool.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.IsKYCVerificationActivated).Value) ? LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "On") : LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Off");
            viewModel.AccountActivationEnable = bool.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.IsAccountActivationEnable).Value) ? LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "On") : LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Off");
            viewModel.CookieExpirations = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.CookieExpirations).Value);

            viewModel.StandardAffiliate = new StandardAffiliateRateViewModel
            {
                Tier1DirectRate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier1DirectRate).Value),
                Tier2SaleToTier1Rate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier2SaleToTier1Rate).Value),
                Tier3SaleToTier1Rate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier3SaleToTier1Rate).Value)
            };

            viewModel.AgencyAffiliate = new AgencyAffiliateRateViewModel
            {
                Tier1DirectRate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier1DirectRate).Value),
                Tier2DirectRate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier2DirectRate).Value),
                Tier3DirectRate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier3DirectRate).Value),
                Tier2SaleToTier1Rate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier2SaleToTier1Rate).Value),
                Tier3SaleToTier1Rate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier3SaleToTier1Rate).Value),
                Tier3SaleToTier2Rate = int.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.AgencyAffiliate.Tier3SaleToTier2Rate).Value)
            };

            viewModel.TotalAffiliateApplicationApproved = _sysUserService.Queryable().Count(x=>x.AffiliateId.HasValue && x.AffiliateId.Value != (int)EnumAffiliateApplicationStatus.PENDING);
            viewModel.TotalAffiliateApplicationPending = _sysUserService.Queryable().Count(x => x.AffiliateId.HasValue && x.AffiliateId == (int)EnumAffiliateApplicationStatus.PENDING);

            viewModel.NumberOfAgencyAffiliateExpiredDays = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.NumberOfAgencyAffiliateExpiredDays).Value);

            return View(viewModel);
        }

        #region Affiliate
        [Permission(EnumRole.Admin)]
        public IActionResult AffiliateApprove()
        {
            var viewModel = new AffiliateApproveViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoApproveAffiliateApplication(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);

            var affiliate = new Affiliate
            {
                Tier1DirectRate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier1DirectRate).Value),
                Tier2SaleToTier1Rate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier2SaleToTier1Rate).Value),
                Tier3SaleToTier1Rate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier3SaleToTier1Rate).Value)
            };

            _affiliateService.Insert(affiliate);
            _unitOfWork.SaveChanges();

            user.AffiliateId = affiliate.Id;
            _sysUserService.Update(user);
            _unitOfWork.SaveChanges();

            var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.AffiliateApprove.ToString());
            var affiliateApproveEmailTemplateViewModel = Mapper.Map<AffiliateApproveEmailTemplateViewModel>(user);
            affiliateApproveEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            // Populate languages
            affiliateApproveEmailTemplateViewModel.AffiliateApplicationText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AffiliateApplication");
            affiliateApproveEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
            affiliateApproveEmailTemplateViewModel.AffiliateApprovedDescriptionText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AffiliateApprovedDescription");
            affiliateApproveEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
            affiliateApproveEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
            affiliateApproveEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
            affiliateApproveEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
            affiliateApproveEmailTemplateViewModel.CPLTeamText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CPLTeam");

            template.Body = _viewRenderService.RenderToStringAsync("/Views/Admin/_AffiliateApproveEmailTemplate.cshtml", affiliateApproveEmailTemplateViewModel).Result;
            EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);

            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AffiliateIsApproved") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchAffiliateApplication(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchAffiliateApplicationFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.Admin)]
        public IList<SysUserViewModel> SearchAffiliateApplicationFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = totalResultsCount = _sysUserService.Queryable()
                        .Count(x => x.AffiliateId.HasValue);

                return _sysUserService.Queryable()
                            .Where(x => x.AffiliateId.HasValue)
                            .OrderBy("AffiliateCreatedDate", false)
                            .Select(x => Mapper.Map<SysUserViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.AffiliateId.HasValue)
                        .Count(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy));

                totalResultsCount = _sysUserService.Queryable()
                        .Count(x => x.AffiliateId.HasValue);

                return _sysUserService.Queryable()
                        .Where(x => x.AffiliateId.HasValue)
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy))
                        .Select(x => Mapper.Map<SysUserViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult GenerateAgencyAffiliateUrl(AgencyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var agencyToken = new AgencyToken();
                agencyToken.Token = Guid.NewGuid().ToString();
                agencyToken.ExpiredDate = DateTime.Now.AddDays(viewModel.NumberOfAgencyAffiliateExpiredDays);
                _agencyTokenService.Insert(agencyToken);
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("Register", "Authentication", new { token = agencyToken.Token })}", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AgencyAffiliateURLGenerated") });
            }

            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        #endregion

        #region User
        [Permission(EnumRole.Admin)]
        public IActionResult AllUser()
        {
            var viewModel = new AllUserViewModel();
            return View(viewModel);
        }

        [Permission(EnumRole.Admin)]
        public new IActionResult User(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            var viewModel = Mapper.Map<UserDashboardAdminViewModel>(user);
            decimal coinRate = CurrenciesPairRateHelper.GetCurrenciesPairRate(EnumCurrenciesPair.ETHBTC.ToString()).Value;
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;
            return View(viewModel);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult EditUser(int id)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_EditUser", Mapper.Map<SysUserViewModel>(user));
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoEditUser(SysUserViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == viewModel.Id);
            if (user != null)
            {
                var existingUser = _sysUserService.Queryable().FirstOrDefault(x => x.Email == viewModel.Email);
                if (existingUser != null && existingUser.Id != viewModel.Id)
                    return new JsonResult(new { success = false, name = "email", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidOrExistingEmail") });

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                user.Mobile = viewModel.Mobile;
                user.Email = viewModel.Email;
                if (!string.IsNullOrEmpty(viewModel.Password))
                    user.Password = viewModel.Password.ToBCrypt();
                //user.StreetAddress = viewModel.StreetAddress.ToLower();
                user.TwoFactorAuthenticationEnable = viewModel.TwoFactorAuthenticationEnable;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }

            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin, EnumEntity.SysUser, EnumAction.Delete)]
        public IActionResult DoDeleteUser(int id)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == id);

            if (user != null)
            {
                user.IsDeleted = true;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeleteSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchAllUser(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchAllUserFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.Admin)]
        public IList<SysUserViewModel> SearchAllUserFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Count();

                return _sysUserService.Queryable()
                            .Select(x => Mapper.Map<SysUserViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy) || x.StreetAddress.Contains(searchBy) || x.Mobile.Contains(searchBy))
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Count();

                return _sysUserService.Queryable()
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy) || x.StreetAddress.Contains(searchBy) || x.Mobile.Contains(searchBy))
                        .Select(x => Mapper.Map<SysUserViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion

        #region KYC
        [Permission(EnumRole.Admin)]
        public IActionResult KYCVerify()
        {
            var viewModel = new KYCVerifyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoAcceptKYCVerify(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            user.KYCVerified = true;

            // Transfer prize
            var lotteryHistorys = _lotteryHistoryService
                              .Query().Include(x => x.LotteryPrize).Select()
                              .Where(x => x.SysUserId == user.Id && x.Result == EnumGameResult.KYC_PENDING.ToString())
                              .ToList();
            foreach (var lotteryHistory in lotteryHistorys)
            {
                user.TokenAmount += lotteryHistory.LotteryPrize.Value;
                // Update status
                lotteryHistory.Result = EnumGameResult.WIN.ToString();
                _lotteryHistoryService.Update(lotteryHistory);
            }

            // Save DB
            _sysUserService.Update(user);
            _unitOfWork.SaveChanges();

            // Send email
            var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.KYCVerify.ToString());
            var kycVerifyEmailTemplateViewModel = Mapper.Map<KYCVerifyEmailTemplateViewModel>(user);
            kycVerifyEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            kycVerifyEmailTemplateViewModel.KYCVerifiedText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCVerified");
            kycVerifyEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
            kycVerifyEmailTemplateViewModel.KYCVerifiedDescriptionText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCVerifiedDescription");
            kycVerifyEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
            kycVerifyEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
            kycVerifyEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
            kycVerifyEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
            kycVerifyEmailTemplateViewModel.CPLTeamText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CPLTeam");

            template.Body = _viewRenderService.RenderToStringAsync("/Views/Admin/_KYCVerifyEmailTemplate.cshtml", kycVerifyEmailTemplateViewModel).Result;
            EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), user.Email);

            return new JsonResult(new { success = true, message = user.FirstName + $" {LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "KYCVerifiedEmailSent")}" });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoCancelKYCVerify(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            user.KYCVerified = null;
            user.KYCCreatedDate = null;
            user.FrontSide = null;
            user.BackSide = null;

            _sysUserService.Update(user);
            _unitOfWork.SaveChanges();

            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CancelSuccessfully") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchKYCVerify(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchKYCVerifyFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.Admin)]
        public IList<SysUserViewModel> SearchKYCVerifyFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                return _sysUserService.Queryable()
                            .Where(x => x.KYCVerified.HasValue)
                            .OrderBy("KYCCreatedDate", false)
                            .Select(x => Mapper.Map<SysUserViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy))
                        .Count();

                totalResultsCount = _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Count();

                return _sysUserService.Queryable()
                        .Where(x => x.KYCVerified.HasValue)
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy))
                        .Select(x => Mapper.Map<SysUserViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion

        #region News
        [Permission(EnumRole.Admin)]
        public IActionResult News()
        {
            var viewModel = new NewsViewModel();
            return View(viewModel);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult EditNews(int id)
        {
            var news = new NewsViewModel();
            if (id > 0)
            {
                news = Mapper.Map<NewsViewModel>(_newsService.Queryable().FirstOrDefault(x => x.Id == id));
            }
            return PartialView("_EditNews", news);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoEditNews(NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var news = _newsService.Queryable()
                .FirstOrDefault(x => x.Id == viewModel.Id);
                if (viewModel.FileImage != null)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
                    var newsPath = Path.Combine(_hostingEnvironment.WebRootPath, @"images\news");
                    var image = $"News_{timestamp}_{viewModel.FileImage.FileName}";
                    var frontSidePath = Path.Combine(newsPath, image);
                    viewModel.FileImage.CopyTo(new FileStream(frontSidePath, FileMode.Create));
                    news.Image = image;
                }

                news.Title = viewModel.Title;
                news.ShortDescription = viewModel.ShortDescription;
                news.Description = viewModel.Description;
                _newsService.Update(news);
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoAddNews(NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.FileImage != null)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");
                    var newsPath = Path.Combine(_hostingEnvironment.WebRootPath, @"images\news");
                    var image = $"News_{timestamp}_{viewModel.FileImage.FileName}";
                    var frontSidePath = Path.Combine(newsPath, image);
                    viewModel.FileImage.CopyTo(new FileStream(frontSidePath, FileMode.Create));

                    _newsService.Insert(new Domain.News { Title = viewModel.Title, CreatedDate = DateTime.Now, Description = viewModel.Description, ShortDescription = viewModel.ShortDescription, Image = image });
                }
                else
                    _newsService.Insert(new Domain.News { Title = viewModel.Title, CreatedDate = DateTime.Now, Description = viewModel.Description, ShortDescription = viewModel.ShortDescription });
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AddSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoDeleteNews(int id)
        {
            var news = _newsService.Queryable()
            .FirstOrDefault(x => x.Id == id);
            if (news != null)
            {
                _newsService.Delete(news);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeleteSuccessfully") });
            }
            else
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchNews(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchNewsFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.Admin)]
        public IList<NewsViewModel> SearchNewsFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _newsService.Queryable()
                        .Count();

                totalResultsCount = _newsService.Queryable()
                        .Count();

                return _newsService.Queryable()
                            .Select(x => Mapper.Map<NewsViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _newsService.Queryable()
                        .Where(x => x.Title.Contains(searchBy) || x.ShortDescription.Contains(searchBy))
                        .Count();

                totalResultsCount = _newsService.Queryable()
                        .Count();

                return _newsService.Queryable()
                        .Where(x => x.Title.Contains(searchBy) || x.ShortDescription.Contains(searchBy))
                        .Select(x => Mapper.Map<NewsViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }
        #endregion

        #region Lottery
        [Permission(EnumRole.Admin)]
        public IActionResult Lottery()
        {
            return View();
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchLottery(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchLotteryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.Admin)]
        public IList<LotteryViewModel> SearchLotteryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _lotteryService.Queryable()
                        .Count();

                totalResultsCount = _lotteryService.Queryable()
                        .Count();

                return _lotteryService.Queryable()
                            .Select(x => Mapper.Map<LotteryViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _lotteryService.Queryable()
                        .Where(x => x.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss").Contains(searchBy) || x.Title.Contains(searchBy))
                        .Count();

                totalResultsCount = _lotteryService.Queryable()
                        .Count();

                return _lotteryService.Queryable()
                        .Where(x => x.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss").Contains(searchBy) || x.Title.Contains(searchBy))
                        .Select(x => Mapper.Map<LotteryViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }

        [Permission(EnumRole.Admin)]
        public IActionResult ViewLottery(int id)
        {
            var lottery = new LotteryViewModel();

            lottery = Mapper.Map<LotteryViewModel>(_lotteryService.Query()
                                                        .Include(x => x.LotteryPrizes)
                                                        .Select()
                                                        .FirstOrDefault(x => x.Id == id));

            return PartialView("_ViewLottery", lottery);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult ViewLotteryPrize(UserLotteryPrizeViewModel viewModel)
        {
            return PartialView("_ViewLotteryPrize", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchUserLotteryPrize(DataTableAjaxPostModel viewModel, int lotteryId, int lotteryPrizeId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchUserLotteryPrizeFunc(viewModel, out filteredResultsCount, out totalResultsCount, lotteryId, lotteryPrizeId);
            var result = Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });

            return result;
        }

        [Permission(EnumRole.Admin)]
        public IList<UserLotteryPrizeViewModel> SearchUserLotteryPrizeFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int lotteryId, int lotteryPrizeId)
        {
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[1].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _lotteryHistoryService
                    .Queryable()
                    .Where(x => x.LotteryId == lotteryId && x.LotteryPrizeId == lotteryPrizeId)
                    .Count();

                totalResultsCount = filteredResultsCount;

                var result = _lotteryHistoryService.Query()
                    .Include(x => x.SysUser)
                    .Select()
                    .Where(x => x.LotteryId == lotteryId && x.LotteryPrizeId == lotteryPrizeId)
                    .Select(x => Mapper.Map<UserLotteryPrizeViewModel>(x.SysUser))
                    .AsQueryable()
                    .Skip(skip)
                    .Take(take)
                    .ToList();

                return result;
            }
            else
            {
                filteredResultsCount = _lotteryHistoryService.Query()
                    .Include(x => x.SysUser)
                    .Select()
                    .Where(x => x.LotteryId == lotteryId && x.LotteryPrizeId == lotteryPrizeId && x.SysUser.Email.Contains(searchBy))
                    .Count();

                totalResultsCount = _lotteryService.Queryable()
                        .Count();

                return _lotteryHistoryService.Query()
                        .Include(x => x.SysUser)
                        .Select()
                        .Where(x => x.LotteryId == lotteryId && x.LotteryPrizeId == lotteryPrizeId && x.SysUser.Email.Contains(searchBy))
                        .Select(x => Mapper.Map<UserLotteryPrizeViewModel>(x.SysUser))
                        .AsQueryable()
                        .Skip(skip)
                        .Take(take)
                        .ToList();
            }
        }

        [Permission(EnumRole.Admin)]
        public IActionResult EditLottery(int id)
        {
            var lottery = new LotteryViewModel();

            lottery = Mapper.Map<LotteryViewModel>(_lotteryService.Query()
                                                        .Include(x => x.LotteryPrizes)
                                                        .Select()
                                                        .FirstOrDefault(x => x.Id == id));

            return PartialView("_EditLottery", lottery);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult AddLottery()
        {
            var lottery = new LotteryViewModel();
            return PartialView("_EditLottery", lottery);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoEditLottery(LotteryViewModel viewModel)
        {
            try
            {
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => x.Id == viewModel.Id);

                // lottery game
                lottery.Title = viewModel.Title;
                lottery.Volume = viewModel.Volume;
                lottery.UnitPrice = viewModel.UnitPrice;

                if (!viewModel.IsPublished)
                    lottery.Status = (int)EnumLotteryGameStatus.PENDING;
                else
                    lottery.Status = (int)EnumLotteryGameStatus.ACTIVE;

                var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                // Desktop slide image
                if (viewModel.DesktopSlideImageFile != null)
                {
                    var desktopSlideImage = $"{lottery.Phase.ToString()}_ds_{timestamp}_{viewModel.DesktopSlideImageFile.FileName}";
                    var desktopSlideImagePath = Path.Combine(pathLottery, desktopSlideImage);
                    viewModel.DesktopSlideImageFile.CopyTo(new FileStream(desktopSlideImagePath, FileMode.Create));
                    lottery.DesktopSlideImage = desktopSlideImage;
                }

                // Mobile slide image
                if (viewModel.MobileSlideImageFile != null)
                {
                    var mobileSlideImage = $"{lottery.Phase.ToString()}_ms_{timestamp}_{viewModel.MobileSlideImageFile.FileName}";
                    var mobileSlideImagePath = Path.Combine(pathLottery, mobileSlideImage);
                    viewModel.MobileSlideImageFile.CopyTo(new FileStream(mobileSlideImagePath, FileMode.Create));
                    lottery.MobileSlideImage = mobileSlideImage;
                }

                // desktop listing image
                if (viewModel.DesktopListingImageFile != null)
                {
                    var desktopListingImage = $"{lottery.Phase.ToString()}_dl_{timestamp}_{viewModel.DesktopListingImageFile.FileName}";
                    var desktopListingImagePath = Path.Combine(pathLottery, desktopListingImage);
                    viewModel.DesktopListingImageFile.CopyTo(new FileStream(desktopListingImagePath, FileMode.Create));
                    lottery.DesktopListingImage = desktopListingImage;
                }

                // mobile listing image
                if (viewModel.MobileListingImageFile != null)
                {
                    var mobileListingImage = $"{lottery.Phase.ToString()}_ml_{timestamp}_{viewModel.MobileListingImageFile.FileName}";
                    var mobileListingImagePath = Path.Combine(pathLottery, mobileListingImage);
                    viewModel.MobileListingImageFile.CopyTo(new FileStream(mobileListingImagePath, FileMode.Create));
                    lottery.MobileListingImage = mobileListingImage;
                }

                // prize image
                if (viewModel.PrizeImageFile != null)
                {
                    var prizeImage = $"{lottery.Phase.ToString()}_p_{timestamp}_{viewModel.PrizeImageFile.FileName}";
                    var prizeImagePath = Path.Combine(pathLottery, prizeImage);
                    viewModel.PrizeImageFile.CopyTo(new FileStream(prizeImagePath, FileMode.Create));
                    lottery.PrizeImage = prizeImage;
                }

                _lotteryService.Update(lottery);

                // lottery prize
                var lotteryPrize = _lotteryPrizeService.Queryable().Where(x => x.LotteryId == viewModel.Id).ToList();
                var numberOfOldPrize = lotteryPrize.Count();

                // Order to set index of the prize
                var orderedLotteryPrizes = viewModel.LotteryPrizes.OrderByDescending(x => x.Value).ToList();
                for (int i = 0; i < orderedLotteryPrizes.Count; i++)
                {
                    orderedLotteryPrizes[i].Index = i + 1;
                }
                var numberOfNewPrize = orderedLotteryPrizes.Count - 1;

                if (numberOfOldPrize >= numberOfNewPrize)
                {
                    for (var i = 0; i < numberOfOldPrize; i++)
                    {
                        if (i < numberOfNewPrize)
                        {
                            var newPrize = lotteryPrize[i];
                            newPrize.Index = orderedLotteryPrizes[i].Index;
                            newPrize.Value = orderedLotteryPrizes[i].Value;
                            newPrize.Volume = orderedLotteryPrizes[i].Volume;
                            _lotteryPrizeService.Update(newPrize);
                        }
                        else
                        {
                            var deletedPrize = lotteryPrize[i];
                            _lotteryPrizeService.Delete(deletedPrize);
                        }
                    }
                }
                else
                {
                    for (var i = 0; i < numberOfNewPrize; i++)
                    {
                        if (i < numberOfOldPrize)
                        {
                            var newPrize = lotteryPrize[i];
                            newPrize.Index = orderedLotteryPrizes[i].Index;
                            newPrize.Value = orderedLotteryPrizes[i].Value;
                            newPrize.Volume = orderedLotteryPrizes[i].Volume;
                            _lotteryPrizeService.Update(newPrize);
                        }
                        else
                        {
                            var prize = orderedLotteryPrizes[i];
                            if (prize.Volume == 0 && prize.Value == 0) continue;
                            _lotteryPrizeService.Insert(new LotteryPrize()
                            {
                                Index = prize.Index,
                                Value = prize.Value,
                                Volume = prize.Volume,
                                LotteryId = viewModel.Id
                            });
                        }
                    }
                }
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoAddLottery(LotteryViewModel viewModel)
        {
            try
            {
                // Lottery game
                var latestLottery = _lotteryService.Queryable().LastOrDefault();
                var currentPhase = latestLottery == null ? 0 : latestLottery.Phase;
                var currentId = latestLottery == null ? 0 : latestLottery.Id;

                var lottery = new Lottery();

                lottery.Title = viewModel.Title;
                lottery.Volume = viewModel.Volume;
                lottery.UnitPrice = viewModel.UnitPrice;
                lottery.Phase = currentPhase + 1;
                lottery.CreatedDate = DateTime.Now;

                if (!viewModel.IsPublished)
                    lottery.Status = (int)EnumLotteryGameStatus.PENDING;
                else
                    lottery.Status = (int)EnumLotteryGameStatus.ACTIVE;

                var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                // Desktop slide image
                if (viewModel.DesktopSlideImageFile != null)
                {
                    var desktopSlideImage = $"{lottery.Phase.ToString()}_ds_{timestamp}_{viewModel.DesktopSlideImageFile.FileName}";
                    var desktopSlideImagePath = Path.Combine(pathLottery, desktopSlideImage);
                    viewModel.DesktopSlideImageFile.CopyTo(new FileStream(desktopSlideImagePath, FileMode.Create));
                    lottery.DesktopSlideImage = desktopSlideImage;
                }

                // Mobile slide image
                if (viewModel.MobileSlideImageFile != null)
                {
                    var mobileSlideImage = $"{lottery.Phase.ToString()}_ms_{timestamp}_{viewModel.MobileSlideImageFile.FileName}";
                    var mobileSlideImagePath = Path.Combine(pathLottery, mobileSlideImage);
                    viewModel.MobileSlideImageFile.CopyTo(new FileStream(mobileSlideImagePath, FileMode.Create));
                    lottery.MobileSlideImage = mobileSlideImage;
                }

                // desktop listing image
                if (viewModel.DesktopListingImageFile != null)
                {
                    var desktopListingImage = $"{lottery.Phase.ToString()}_dl_{timestamp}_{viewModel.DesktopListingImageFile.FileName}";
                    var desktopListingImagePath = Path.Combine(pathLottery, desktopListingImage);
                    viewModel.DesktopListingImageFile.CopyTo(new FileStream(desktopListingImagePath, FileMode.Create));
                    lottery.DesktopListingImage = desktopListingImage;
                }

                // mobile listing image
                if (viewModel.MobileListingImageFile != null)
                {
                    var mobileListingImage = $"{lottery.Phase.ToString()}_ml_{timestamp}_{viewModel.MobileListingImageFile.FileName}";
                    var mobileListingImagePath = Path.Combine(pathLottery, mobileListingImage);
                    viewModel.MobileListingImageFile.CopyTo(new FileStream(mobileListingImagePath, FileMode.Create));
                    lottery.MobileListingImage = mobileListingImage;
                }

                // prize image
                if (viewModel.PrizeImageFile != null)
                {
                    var prizeImage = $"{lottery.Phase.ToString()}_p_{timestamp}_{viewModel.PrizeImageFile.FileName}";
                    var prizeImagePath = Path.Combine(pathLottery, prizeImage);
                    viewModel.PrizeImageFile.CopyTo(new FileStream(prizeImagePath, FileMode.Create));
                    lottery.PrizeImage = prizeImage;
                }

                _lotteryService.Insert(lottery);
                _unitOfWork.SaveChanges();

                // Order to set index of the prize
                var orderedLotteryPrizes = viewModel.LotteryPrizes.OrderByDescending(x => x.Value).ToList();
                for (int i = 0; i < orderedLotteryPrizes.Count; i++)
                {
                    orderedLotteryPrizes[i].Index = i + 1;
                }

                // Lottery prize
                foreach (var prize in orderedLotteryPrizes)
                {
                    if (prize.Volume == 0 && prize.Value == 0) continue;
                    _lotteryPrizeService.Insert(new LotteryPrize()
                    {
                        Index = prize.Index,
                        Value = prize.Value,
                        Volume = prize.Volume,
                        LotteryId = lottery.Id
                    });
                }

                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AddSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [Permission(EnumRole.Admin)]
        public IActionResult ConfirmDeleteLottery(int id)
        {
            ViewData["gameId"] = id;
            return PartialView("_ConfirmDeleteLottery");
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoDeleteLottery(int id)
        {
            try
            {
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => x.Id == id);
                var lotteryPrize = _lotteryPrizeService.Queryable().Where(x => x.LotteryId == id);

                foreach (var prize in lotteryPrize)
                {
                    _lotteryPrizeService.Delete(prize);
                }

                _lotteryService.Delete(lottery);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeleteSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoActivateLottery(int id)
        {
            try
            {
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => x.Id == id);
                lottery.Status = (int)EnumLotteryGameStatus.ACTIVE;
                _lotteryService.Update(lottery);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ActivateSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }
        #endregion
    }
}