using AutoMapper;
using CPL.Common.CurrencyPairRateHelper;
using CPL.Common.Enums;
using CPL.Common.Misc;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly IPricePredictionService _pricePredictionService;
        private readonly INewsService _newsService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILotteryService _lotteryService;
        private readonly ILotteryPrizeService _lotteryPrizeService;
        private readonly IAgencyTokenService _agencyTokenService;
        private readonly IAffiliateService _affiliateService;
        private readonly ILotteryCategoryService _lotteryCategoryService;
        private readonly ILotteryDetailService _lotteryDetailService;
        private readonly IAnalyticService _analyticService;


        public AdminController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService,
            IPricePredictionService pricePredictionService,
            INewsService newsService,
            IHostingEnvironment hostingEnvironment,
            IAnalyticService analyticService,
            ILotteryService lotteryService,
            IAffiliateService affiliateService,
            ILotteryPrizeService lotteryPrizeService,
            ILotteryCategoryService lotteryCategoryService,
            ILotteryDetailService lotteryDetailService,
            IAgencyTokenService agencyTokenService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._lotteryService = lotteryService;
            this._lotteryPrizeService = lotteryPrizeService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._pricePredictionService = pricePredictionService;
            this._newsService = newsService;
            this._analyticService = analyticService;
            this._affiliateService = affiliateService;
            this._hostingEnvironment = hostingEnvironment;
            this._agencyTokenService = agencyTokenService;
            this._lotteryDetailService = lotteryDetailService;
            this._lotteryCategoryService = lotteryCategoryService;
        }

        [Permission(EnumRole.Admin)]
        public IActionResult Index()
        {
            //Example of using Analytic Service
            //var deviceCategories = _analyticService.GetDeviceCategory(CPLConstant.Analytic.HomeViewId, DateTime.Now.AddDays(-7), DateTime.Now);
            //var bounceRates = _analyticService.GetBounceRate(CPLConstant.Analytic.HomeViewId, DateTime.Now.AddDays(-7), DateTime.Now);
            //var pageViews = _analyticService.GetPageViews(CPLConstant.Analytic.HomeViewId, DateTime.Now.AddDays(-7), DateTime.Now);

            var viewModel = new AdminViewModel();

            // User management
            viewModel.TotalKYCPending = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && !x.KYCVerified.Value);
            viewModel.TotalKYCVerified = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && x.KYCVerified.Value);
            viewModel.TotalUser = _sysUserService.Queryable().Count();
            viewModel.TotalUserToday = _sysUserService.Queryable().Count(x => x.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"));
            viewModel.TotalUserYesterday = _sysUserService.Queryable().Count(x => x.CreatedDate.ToString("dd/MM/yyyy") == DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));

            // Game management
            var lotteryGames = _lotteryService.Queryable().Where(x => !x.IsDeleted);
            var pricePredictioNGames = _pricePredictionService.Queryable();
            var lotteryHistories = _lotteryHistoryService.Queryable();
            var pricePredictionHistories = _pricePredictionHistoryService.Queryable();

            // lottery game
            viewModel.TotalLotteryGame = lotteryGames.Count();
            var totalSaleInLotteryGame = _lotteryHistoryService.Query()
                                        .Include(x => x.Lottery)
                                        .Select(x => x.Lottery.UnitPrice).Sum();

            var totalSaleInLotteryGameToday = _lotteryHistoryService.Query()
                                        .Include(x => x.Lottery)
                                        .Select()
                                        .Where(x => x.CreatedDate.Date.Equals(DateTime.Now.Date))
                                        .Sum(x => x.Lottery.UnitPrice);
            var totalSaleInLotteryGameYesterday = _lotteryHistoryService.Query()
                                        .Include(x => x.Lottery)
                                        .Select()
                                        .Where(x => x.CreatedDate.Date.Equals(DateTime.Now.AddDays(-1).Date))
                                        .Sum(x => x.Lottery.UnitPrice);
            // price prediction game
            viewModel.TotalPricePredictionGame = pricePredictioNGames.Count();
            var totalSaleIPricePredictionGame = _pricePredictionHistoryService.Queryable()
                                            .Sum(x => x.Amount);
            var totalSaleIPricePredictionGameToday = _pricePredictionHistoryService.Queryable()
                                            .Where(x => x.CreatedDate.Date.Equals(DateTime.Now.Date))
                                            .Sum(x => x.Amount);
            var totalSaleIPricePredictionGameYesterday = _pricePredictionHistoryService.Queryable()
                                            .Where(x => x.CreatedDate.Date.Equals(DateTime.Now.AddDays(-1).Date))
                                            .Sum(x => x.Amount);
            // all game
            viewModel.TotalGame = viewModel.TotalLotteryGame + viewModel.TotalPricePredictionGame;
            viewModel.TotalSaleInGame = totalSaleInLotteryGame + (int)totalSaleIPricePredictionGame;
            viewModel.TotalSaleInGameToday = totalSaleInLotteryGameToday + (int)totalSaleIPricePredictionGameToday;
            viewModel.TotalSaleInGameYesterday = totalSaleInLotteryGameYesterday + (int)totalSaleIPricePredictionGameYesterday;

            // Affiliate
            viewModel.TotalAgencyAffiliate = _sysUserService.Queryable().Count(x => x.AgencyId != null && x.AgencyId > 0);
            viewModel.TotalAgencyAffiliateToday = _sysUserService.Queryable()
                                                    .Where(x => x.AffiliateCreatedDate != null && x.AffiliateCreatedDate.Value.Date == DateTime.Now.Date)
                                                    .Count(x => x.AgencyId != null && x.AgencyId > 0);
            viewModel.TotalAgencyAffiliateYesterday = _sysUserService.Queryable()
                                                    .Where(x => x.AffiliateCreatedDate != null && x.AffiliateCreatedDate.Value.Date == DateTime.Now.AddDays(-1).Date)
                                                    .Count(x => x.AgencyId != null && x.AgencyId > 0);
            viewModel.TotalStandardAffiliate = _sysUserService.Queryable().Count(x => x.AgencyId == null && x.AffiliateId != null && x.AffiliateId > 0);
            viewModel.TotalStandardAffiliateToday = _sysUserService.Queryable()
                                                    .Where(x => x.AffiliateCreatedDate != null && x.AffiliateCreatedDate.Value.Date == DateTime.Now.Date)
                                                    .Count(x => x.AgencyId == null && x.AffiliateId != null && x.AffiliateId > 0);
            viewModel.TotalStandardAffiliateYesterday = _sysUserService.Queryable()
                                                    .Where(x => x.AffiliateCreatedDate != null && x.AffiliateCreatedDate.Value.Date == DateTime.Now.AddDays(-1).Date)
                                                    .Count(x => x.AgencyId == null && x.AffiliateId != null && x.AffiliateId > 0);

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

            viewModel.TotalAffiliateApplicationApproved = _sysUserService.Queryable().Count(x => x.AffiliateId.HasValue && x.AffiliateId.Value != (int)EnumAffiliateApplicationStatus.PENDING);
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

            if (user.AffiliateId > 0)
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AffiliateHasBeenApproved") });

            var affiliate = new Affiliate
            {
                Tier1DirectRate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier1DirectRate).Value),
                Tier2SaleToTier1Rate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier2SaleToTier1Rate).Value),
                Tier3SaleToTier1Rate = int.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.StandardAffiliate.Tier3SaleToTier1Rate).Value)
            };

            _affiliateService.Insert(affiliate);
            _unitOfWork.SaveChanges();

            user.AffiliateId = affiliate.Id;
            user.AffiliateCreatedDate = DateTime.Now;
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

        [Permission(EnumRole.Admin)]
        public IActionResult StandardAffiliate()
        {
            return View();
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchStandardAffiliate(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchStandardAffiliateFunc(viewModel, out filteredResultsCount, out totalResultsCount);
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
        public IList<StandardAffliateViewModel> SearchStandardAffiliateFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
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
                        .Count(x => x.AffiliateId.HasValue && x.AffiliateId > 0 && !x.AgencyId.HasValue);

                var standardAffliate =
                            ((CPLContext)HttpContext.RequestServices.GetService(typeof(IDataContextAsync))).SysUser
                            .Include(x => x.Affiliate)
                            .Include(x => x.LotteryHistories)
                            .ThenInclude(x => x.LotteryPrize)
                            .ThenInclude(x => x.Lottery)
                            .Include(x => x.PricePredictionHistories)
                            .Include(x => x.DirectIntroducedUsers)
                            .Where(x => x.AffiliateId.HasValue && x.AffiliateId > 0 && !x.AgencyId.HasValue)
                            .AsQueryable()
                            .OrderBy("AffiliateCreatedDate", false)
                            .Select(x => new StandardAffliateViewModel
                            {
                                Id = x.Id,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.Email,
                                IsLocked = x.IsLocked,
                                TotalIntroducer = x.DirectIntroducedUsers.Count(y => y.IsIntroducedById == x.Id),
                                AffiliateId = x.AffiliateId,

                                // lottery
                                TotalDirectCPLAwardedInLottery = x.DirectIntroducedUsers.Sum(y => y.LotteryHistories.Sum(z => z.LotteryPrize.Value)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLAwardedInLottery = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.LotteryPrize.Value))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLAwardedInLottery = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.LotteryPrize.Value))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,
                                TotalDirectCPLUsedInLottery = x.DirectIntroducedUsers.Sum(y => y.LotteryHistories.Sum(z => z.Lottery.UnitPrice)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLUsedInLottery = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.Lottery.UnitPrice))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLUsedInLottery = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.Lottery.UnitPrice))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,

                                // price predciotn
                                TotalDirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.PricePredictionHistories.Sum(z => z.Award)).GetValueOrDefault(0),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Award))).GetValueOrDefault(0),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Award))).GetValueOrDefault(0),// * x.Affiliate.Tier3SaleToTier1Rate / 100,
                                TotalDirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.PricePredictionHistories.Sum(z => z.Amount)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Amount))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Amount))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,

                                AffiliateCreatedDate = x.AffiliateCreatedDate,
                                AffiliateCreatedDateInString = x.AffiliateCreatedDate.GetValueOrDefault().ToString(Format.DateTime),
                                Tier1DirectRate = x.Affiliate.Tier1DirectRate,
                                Tier2SaleToTier1Rate = x.Affiliate.Tier2SaleToTier1Rate,
                                Tier3SaleToTier1Rate = x.Affiliate.Tier3SaleToTier1Rate
                            })
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

                return standardAffliate;
            }
            else
            {
                filteredResultsCount = _sysUserService.Queryable()
                        .Where(x => x.AffiliateId.HasValue && x.AffiliateId > 0 && !x.AgencyId.HasValue)
                        .Count(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy));

                totalResultsCount = _sysUserService.Queryable()
                        .Count(x => x.AffiliateId.HasValue && x.AffiliateId > 0 && !x.AgencyId.HasValue);

                var standardAffliate =
                            ((CPLContext)HttpContext.RequestServices.GetService(typeof(IDataContextAsync))).SysUser
                            .Include(x => x.Affiliate)
                            .Include(x => x.LotteryHistories)
                            .ThenInclude(x => x.LotteryPrize)
                            .ThenInclude(x => x.Lottery)
                            .Include(x => x.PricePredictionHistories)
                            .Include(x => x.DirectIntroducedUsers)
                            .Where(x => x.AffiliateId.HasValue && x.AffiliateId > 0 && !x.AgencyId.HasValue)
                            .Select(x => new StandardAffliateViewModel
                            {
                                Id = x.Id,
                                FirstName = x.FirstName,
                                LastName = x.LastName,
                                Email = x.Email,
                                IsLocked = x.IsLocked,
                                TotalIntroducer = x.DirectIntroducedUsers.Count(y => y.IsIntroducedById == x.Id),
                                AffiliateId = x.AffiliateId,

                                // lottery
                                TotalDirectCPLAwardedInLottery = x.DirectIntroducedUsers.Sum(y => y.LotteryHistories.Sum(z => z.LotteryPrize.Value)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLAwardedInLottery = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.LotteryPrize.Value))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLAwardedInLottery = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.LotteryPrize.Value))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,
                                TotalDirectCPLUsedInLottery = x.DirectIntroducedUsers.Sum(y => y.LotteryHistories.Sum(z => z.Lottery.UnitPrice)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLUsedInLottery = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.Lottery.UnitPrice))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLUsedInLottery = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.LotteryHistories.Sum(k => k.Lottery.UnitPrice))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,

                                // price predciotn
                                TotalDirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.PricePredictionHistories.Sum(z => z.Award)).GetValueOrDefault(0),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Award))).GetValueOrDefault(0),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLAwardedInPricePrediction = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Award))).GetValueOrDefault(0),// * x.Affiliate.Tier3SaleToTier1Rate / 100,
                                TotalDirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.PricePredictionHistories.Sum(z => z.Amount)),// * x.Affiliate.Tier1DirectRate / 100,
                                TotalTier2DirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Amount))),// * x.Affiliate.Tier2SaleToTier1Rate / 100,
                                TotalTier3DirectCPLUsedInPricePrediction = x.DirectIntroducedUsers.SelectMany(y => y.DirectIntroducedUsers).Sum(y => y.DirectIntroducedUsers.Sum(z => z.PricePredictionHistories.Sum(k => k.Amount))),// * x.Affiliate.Tier3SaleToTier1Rate / 100,

                                AffiliateCreatedDate = x.AffiliateCreatedDate,
                                AffiliateCreatedDateInString = x.AffiliateCreatedDate.GetValueOrDefault().ToString(Format.DateTime),
                                Tier1DirectRate = x.Affiliate.Tier1DirectRate,
                                Tier2SaleToTier1Rate = x.Affiliate.Tier2SaleToTier1Rate,
                                Tier3SaleToTier1Rate = x.Affiliate.Tier3SaleToTier1Rate
                            })
                            .Where(x => x.FirstName.ToLower().Contains(searchBy) || x.LastName.ToLower().Contains(searchBy) || x.Email.ToLower().Contains(searchBy))
                            .AsQueryable()
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();

                return standardAffliate;
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoLockStandardAffiliate(int id)
        {
            try
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);

                user.IsLocked = !user.IsLocked;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();

                if (user.IsLocked)
                    return new JsonResult(new { success = true, isLocked = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "LockSuccessful") });
                else
                    return new JsonResult(new { success = true, isLocked = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UnLockSuccessful") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoUpdateStandardAffiliateRate(StandardAffliateDataModel model)
        {
            try
            {
                var standardAffiliate = _affiliateService.Queryable().FirstOrDefault(x => x.Id == model.Id);

                var user = _sysUserService.Queryable().FirstOrDefault(x => x.AffiliateId == model.Id);
                if (user != null && !user.IsLocked)
                {
                    if (model.Tier1DirectRate != null)
                        standardAffiliate.Tier1DirectRate = model.Tier1DirectRate.Value;
                    if (model.Tier2SaleToTier1Rate != null)
                        standardAffiliate.Tier2SaleToTier1Rate = model.Tier2SaleToTier1Rate.Value;
                    if (model.Tier3SaleToTier1Rate != null)
                        standardAffiliate.Tier3SaleToTier1Rate = model.Tier3SaleToTier1Rate.Value;

                    _affiliateService.Update(standardAffiliate);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, isLocked = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
                }
                else
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoUpdateStandardAffiliateRates(string data)
        {
            try
            {
                var _data = JsonConvert.DeserializeObject<StandardAffliateDataModel>(data);
                foreach (var id in _data.Ids)
                {
                    var standardAffiliate = _affiliateService.Queryable().FirstOrDefault(x => x.Id == id);

                    if (_data.Tier1DirectRate != null)
                        standardAffiliate.Tier1DirectRate = _data.Tier1DirectRate.Value;
                    if (_data.Tier2SaleToTier1Rate != null)
                        standardAffiliate.Tier2SaleToTier1Rate = _data.Tier2SaleToTier1Rate.Value;
                    if (_data.Tier3SaleToTier1Rate != null)
                        standardAffiliate.Tier3SaleToTier1Rate = _data.Tier3SaleToTier1Rate.Value;

                    _affiliateService.Update(standardAffiliate);
                }

                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
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
            decimal coinRate = CurrencyPairRateHelper.GetCurrencyPairRate(EnumCurrencyPair.ETHBTC.ToString()).Value;
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

                // total CPL used and total CPL awarded in lottery game 
                var lotteryHistories = _lotteryHistoryService.Query()
                                    .Include(x => x.Lottery)
                                    .Include(x => x.LotteryPrize)
                                    .Select()
                                    .AsQueryable()
                                    .GroupBy(x => x.SysUserId)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = y.Sum(x => x.Lottery.UnitPrice), TotalCPLAwarded = (int)y.Sum(x => (x.LotteryPrize != null) ? x.LotteryPrize.Value : 0) });

                // total CPL used and total CPL awarded in priceprediction game 
                var pricePredictionHistories = _pricePredictionHistoryService.Queryable()
                                    .GroupBy(x => x.SysUserId)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = (int)y.Sum(x => x.Amount), TotalCPLAwarded = (int)y.Sum(x => x.Award ?? 0) });

                // total CPL used and total CPL awarded in all game 
                var histories = lotteryHistories.Concat(pricePredictionHistories).ToList()
                                    .GroupBy(x => x.Id)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = y.Sum(x => x.TotalCPLUsed), TotalCPLAwarded = y.Sum(x => x.TotalCPLAwarded) });

                var sysUsers = _sysUserService.Queryable()
                            .Skip(skip)
                            .Take(take)
                            .ToList();

                return sysUsers.LeftOuterJoin(histories, user => user.Id,
                                                    history => history.Id,
                                                    (user, history) => new SysUserViewModel()
                                                    {
                                                        Id = user.Id,
                                                        Email = user.Email,
                                                        FirstName = user.FirstName,
                                                        LastName = user.LastName,
                                                        StreetAddress = user.StreetAddress,
                                                        Mobile = user.Mobile,
                                                        CreatedDateInString = user.CreatedDate.ToString("yyyy/MM/dd"),
                                                        Country = user.Country,
                                                        City = user.City,
                                                        IsDeleted = user.IsDeleted,
                                                        BTCAmount = user.BTCAmount,
                                                        ETHAmount = user.ETHAmount,
                                                        TokenAmount = user.TokenAmount,
                                                        TotalCPLUsed = (history != null) ? history.TotalCPLUsed : 0,
                                                        TotalCPLAwarded = (history != null) ? history.TotalCPLAwarded : 0,
                                                        TotalCPLUsedInString = (history != null) ? history.TotalCPLUsed.ToString(CPLConstant.Format.Amount) : "0",
                                                        TotalCPLAwardedInString = (history != null) ? history.TotalCPLAwarded.ToString(CPLConstant.Format.Amount) : "0"
                                                    })
                                                    .AsQueryable()
                                                    .OrderBy(sortBy, sortDir)
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

                // total CPL used and total CPL awarded in lottery game 
                var lotteryHistories = _lotteryHistoryService.Query()
                                    .Include(x => x.Lottery)
                                    .Include(x => x.LotteryPrize)
                                    .Select()
                                    .AsQueryable()
                                    .GroupBy(x => x.SysUserId)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = y.Sum(x => x.Lottery.UnitPrice), TotalCPLAwarded = (int)y.Sum(x => (x.LotteryPrize != null) ? x.LotteryPrize.Value : 0) });

                // total CPL used and total CPL awarded in priceprediction game 
                var pricePredictionHistories = _pricePredictionHistoryService.Queryable()
                                    .GroupBy(x => x.SysUserId)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = (int)y.Sum(x => x.Amount), TotalCPLAwarded = (int)y.Sum(x => x.Award ?? 0) });

                // total CPL used and total CPL awarded in all game 
                var histories = lotteryHistories.Concat(pricePredictionHistories).ToList()
                                    .GroupBy(x => x.Id)
                                    .Select(y => new SysUserViewModel { Id = y.Key, TotalCPLUsed = y.Sum(x => x.TotalCPLUsed), TotalCPLAwarded = y.Sum(x => x.TotalCPLAwarded) });

                var sysUsers = _sysUserService.Queryable()
                        .Where(x => x.FirstName.Contains(searchBy) || x.LastName.Contains(searchBy)
                        || x.Email.Contains(searchBy) || x.StreetAddress.Contains(searchBy) || x.Mobile.Contains(searchBy))
                        .Skip(skip)
                        .Take(take);

                return sysUsers.LeftOuterJoin(histories, user => user.Id,
                                                    history => history.Id,
                                                    (user, history) => new SysUserViewModel()
                                                    {
                                                        Id = user.Id,
                                                        Email = user.Email,
                                                        FirstName = user.FirstName,
                                                        LastName = user.LastName,
                                                        StreetAddress = user.StreetAddress,
                                                        Mobile = user.Mobile,
                                                        CreatedDateInString = user.CreatedDate.ToString("yyyy/MM/dd"),
                                                        Country = user.Country,
                                                        City = user.City,
                                                        IsDeleted = user.IsDeleted,
                                                        BTCAmount = user.BTCAmount,
                                                        ETHAmount = user.ETHAmount,
                                                        TokenAmount = user.TokenAmount,
                                                        TotalCPLUsed = (history != null) ? history.TotalCPLUsed : 0,
                                                        TotalCPLAwarded = (history != null) ? history.TotalCPLAwarded : 0,
                                                        TotalCPLUsedInString = (history != null) ? history.TotalCPLUsed.ToString(CPLConstant.Format.Amount) : "0",
                                                        TotalCPLAwardedInString = (history != null) ? history.TotalCPLAwarded.ToString(CPLConstant.Format.Amount) : "0"
                                                    })
                                                    .AsQueryable()
                                                    .OrderBy(sortBy, sortDir)
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

        #region Game
        [Permission(EnumRole.Admin)]
        public IActionResult Game()
        {
            return View();
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult GetDataGameSummaryStatisticChart(int periodInDay)
        {
            var viewModel = new GameManagementIndexViewModel();

            var lotterySale = _lotteryHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ?  x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date)
                        .Select(y => new SummaryChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), Value = y.Sum(x => x.Lottery.UnitPrice) });

            var pricePredictionSale = _pricePredictionHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date)
                        .Select(y => new SummaryChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), Value = y.Sum(x => (int)x.Amount) });

            viewModel.TotalSaleChanges = (lotterySale ?? Enumerable.Empty<SummaryChange>()).Concat(pricePredictionSale ?? Enumerable.Empty<SummaryChange>()).GroupBy(x => x.Date).Select(y => new SummaryChange { Date = y.Select(x => x.Date).FirstOrDefault(), Value = y.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

            var lotteryRevenue = _lotteryHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date)
                        .Select(y => new SummaryChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), Value = y.Sum(x => Convert.ToInt32(x.Lottery.UnitPrice * LotteryTotalRevenuePercentage)) });

            var pricePredictionRevenue = _pricePredictionHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date)
                        .Select(y => new SummaryChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), Value = y.Sum(x => Convert.ToInt32(x.Amount * PricePredictionTotalRevenuePercentage)) });

            viewModel.TotalRevenueChanges = (lotteryRevenue ?? Enumerable.Empty<SummaryChange>()).Concat(pricePredictionRevenue ?? Enumerable.Empty<SummaryChange>()).GroupBy(x => x.Date).Select(y => new SummaryChange { Date = y.Select(x => x.Date).FirstOrDefault(), Value = y.Sum(x => x.Value) }).OrderBy(x => x.Date).ToList();

            viewModel.PageViewChanges = _analyticService.GetPageViews(CPLConstant.Analytic.HomeViewId, periodInDay > 0 ? DateTime.Now.Date.AddDays(-periodInDay) : CPLConstant.FirstDeploymentDate, DateTime.Now).OrderBy(x => x.Date).ToList();

            var lotteryPlayers = _lotteryHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date).Select(y => new PlayersChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), SysUserIdList = y.Select(x => x.SysUserId) }).ToList();

            var pricePredictionPlayers = _pricePredictionHistoryService.Queryable()
                        .Where(x => periodInDay > 0 ? x.CreatedDate.Date >= DateTime.Now.Date.AddDays(-periodInDay) : x.CreatedDate <= DateTime.Now)
                        .GroupBy(x => x.CreatedDate.Date).Select(y => new PlayersChange { Date = y.Select(x => x.CreatedDate.Date).FirstOrDefault(), SysUserIdList = y.Select(x => x.SysUserId) }).ToList();

            viewModel.TotalPlayersChanges = (lotteryPlayers ?? Enumerable.Empty<PlayersChange>()).Concat(pricePredictionPlayers ?? Enumerable.Empty<PlayersChange>()).GroupBy(x => x.Date).Select(y => new SummaryChange { Date = y.Select(x => x.Date).FirstOrDefault(), Value = y.SelectMany(x => x.SysUserIdList).Distinct().Count() }).OrderBy(x => x.Date).ToList();

            var mess = JsonConvert.SerializeObject(viewModel, Formatting.Indented);
            return new JsonResult(new { success = true, message = mess });
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult SearchPurchasedLotteryHistory(DataTableAjaxPostModel viewModel, int? lotteryCategoryId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchPurchasedLotteryHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount, lotteryCategoryId);
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
        public IList<AdminLotteryHistoryViewComponentViewModel> SearchPurchasedLotteryHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int? lotteryCategoryId)
        {
            var searchBy = (model.search != null) ? model.search?.value : null;
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

            var purchasedLotteryHistory = _lotteryHistoryService
                    .Query()
                    .Include(x => x.Lottery)
                    .Include(x => x.SysUser)
                    .Select()
                    .Where(x => !lotteryCategoryId.HasValue || x.Lottery.LotteryCategoryId == lotteryCategoryId)
                    .GroupBy(x => new { x.CreatedDate, x.LotteryId, x.SysUserId })
                    .Select(y => new AdminLotteryHistoryViewComponentViewModel
                    {
                        SysUserId = y.Key.SysUserId,
                        UserName = y.FirstOrDefault().SysUser.Email,
                        Status = ((EnumLotteryGameStatus)(y.FirstOrDefault().Lottery.Status)).ToString(),
                        NumberOfTicket = y.Count(),
                        TotalPurchasePrice = y.Sum(x => x.Lottery.UnitPrice),
                        Title = y.FirstOrDefault().Lottery.Title,
                        PurchaseDateTime = y.Key.CreatedDate,
                    });

            filteredResultsCount = totalResultsCount = purchasedLotteryHistory.Count();

            // search the dbase taking into consideration table sorting and paging
            if (!string.IsNullOrEmpty(searchBy))
            {
                searchBy = searchBy.ToLower();
                bool condition(AdminLotteryHistoryViewComponentViewModel x) => x.UserName.ToLower().Contains(searchBy) || x.Status.ToLower().Contains(searchBy) || x.PurchaseDateTimeInString.ToLower().Contains(searchBy)
                                    || x.NumberOfTicketInString.ToLower().Contains(searchBy) || x.Title.ToLower().Contains(searchBy);
                purchasedLotteryHistory = purchasedLotteryHistory
                        .Where(condition);

                filteredResultsCount = purchasedLotteryHistory
                        .Count();
            }

            return purchasedLotteryHistory
                  .AsQueryable()
                  .OrderBy(sortBy, sortDir)
                  .Skip(skip)
                  .Take(take)
                  .ToList();
        }
        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult GetDataRevenuePercentagePieChart()
        {
            try
            {
                // lottery game
                var totalSaleInLotteryGame = _lotteryHistoryService.Query()
                                .Include(x => x.Lottery)
                                .Select(x => x.Lottery).Sum(y => y?.UnitPrice);
                var totalAwardInLotteryGame = _lotteryHistoryService.Query()
                    .Include(x => x.LotteryPrize)
                    .Select(x => x.LotteryPrize).Sum(y => y?.Value);
                var revenueInLotteryGame = (totalSaleInLotteryGame - totalAwardInLotteryGame) * CPLConstant.LotteryTotalRevenuePercentage;

                // price prediction game
                var totalSaleIPricePredictionGame = _pricePredictionHistoryService.Queryable()
                                                    .Sum(x => x.Amount);
                var totalAwardIPricePredictionGame = _pricePredictionHistoryService.Queryable()
                                                    .Sum(x => x.Award);
                var revenueInPricePredictionGame = (totalSaleIPricePredictionGame - totalAwardIPricePredictionGame) * CPLConstant.PricePredictionTotalRevenuePercentage;

                return new JsonResult(new { success = true, revenueLotteryGame = revenueInLotteryGame , revenuePricePredictionGame = revenueInPricePredictionGame });
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false });
            }
        }


        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult GetDataTeminalPercentagePieChart()
        {
            try
            {
                var deviceCategories = _analyticService.GetDeviceCategory(CPLConstant.Analytic.HomeViewId, FirstDeploymentDate, DateTime.Now);
                var totalDesktop = deviceCategories.GroupBy(x => x.DeviceCategory == EnumDeviceCategory.DESKTOP).Sum(y => y.Sum(x => x.Count));
                var totalMobile = deviceCategories.GroupBy(x => x.DeviceCategory == EnumDeviceCategory.MOBILE).Sum(y => y.Sum(x => x.Count));
                var totalTablet = deviceCategories.GroupBy(x => x.DeviceCategory == EnumDeviceCategory.TABLET).Sum(y => y.Sum(x => x.Count));

                return new JsonResult(new { success = true, desktop = totalDesktop,
                                                            mobile = totalMobile,
                                                            tablet = totalTablet
                });
            }
            catch (Exception)
            {
                return new JsonResult(new { success = false });
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
                filteredResultsCount = _lotteryService.Queryable().Where(x => !x.IsDeleted)
                        .Count();

                totalResultsCount = _lotteryService.Queryable().Where(x => !x.IsDeleted)
                        .Count();

                return _lotteryService.Query()
                            .Include(x => x.LotteryDetails)
                            .Select()
                            .AsQueryable()
                            .Where(x => !x.IsDeleted)
                            .Select(x => Mapper.Map<LotteryViewModel>(x))
                            .OrderBy(sortBy, sortDir)
                            .Skip(skip)
                            .Take(take)
                            .ToList();
            }
            else
            {
                filteredResultsCount = _lotteryService.Queryable()
                        .Where(x => !x.IsDeleted && (x.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss").Contains(searchBy) || x.Title.Contains(searchBy)))
                        .Count();

                totalResultsCount = _lotteryService.Queryable()
                        .Count(x => !x.IsDeleted);

                return _lotteryService.Query()
                        .Include(x => x.LotteryDetails)
                        .Select()
                        .AsQueryable()
                        .Where(x => !x.IsDeleted && (x.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss").Contains(searchBy) || x.Title.Contains(searchBy)))
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
                                                        .Include(x => x.LotteryDetails)
                                                        .Select()
                                                        .FirstOrDefault(x => !x.IsDeleted && x.Id == id));

            foreach (var detail in lottery.LotteryDetails)
            {
                var lang = Mapper.Map<LangViewModel>(_langService.Queryable().Where(x => x.Id == detail.LangId).FirstOrDefault());
                detail.Lang = lang;
            }

            lottery.LotteryCategory = _lotteryCategoryService.Queryable().Where(x => x.Id == lottery.LotteryCategoryId).FirstOrDefault().Name;

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
                        .Count(x => !x.IsDeleted);

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
            var lotteries = new LotteryViewModel();
            lotteries = Mapper.Map<LotteryViewModel>(_lotteryService.Query()
                                                        .Include(x => x.LotteryPrizes)
                                                        .Include(x => x.LotteryDetails)
                                                        .Select()
                                                        .FirstOrDefault(x => !x.IsDeleted && x.Id == id));

            foreach (var detail in lotteries.LotteryDetails)
            {
                var lang = Mapper.Map<LangViewModel>(_langService.Queryable().Where(x => x.Id == detail.LangId).FirstOrDefault());
                detail.Lang = lang;
            }

            lotteries.LotteryCategories = _lotteryCategoryService.Query().Select(x => Mapper.Map<LotteryCategoryViewModel>(x)).ToList();

            return PartialView("_EditLottery", lotteries);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult AddLottery()
        {
            var lottery = new LotteryViewModel();

            var langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            foreach (var lang in langs)
            {
                lottery.LotteryDetails.Add(new LotteryDetailViewModel()
                {
                    Lang = lang
                });
            }

            lottery.LotteryCategories = _lotteryCategoryService.Query().Select(x => Mapper.Map<LotteryCategoryViewModel>(x)).ToList();
            return PartialView("_EditLottery", lottery);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult AddLotteryCategory()
        {
            var lotteryCategory = new LotteryCategoryViewModel();
            return PartialView("_EditLotteryCategory", lotteryCategory);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoAddLotteryCategory(LotteryCategoryViewModel viewModel)
        {
            try
            {
                if (_lotteryCategoryService.Queryable().Any(x => x.Name == viewModel.Name))
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExistingCategory") });
                else
                {
                    _lotteryCategoryService.Insert(new LotteryCategory()
                    {
                        Name = viewModel.Name,
                        Description = viewModel.Description
                    });

                    _unitOfWork.SaveChanges();
                    var newCategory = _lotteryCategoryService.Queryable().FirstOrDefault(x => x.Name == viewModel.Name);
                    return new JsonResult(new { success = true, id = newCategory.Id, name = newCategory.Name, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AddSuccessfully") });
                }



            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoEditLottery(LotteryViewModel viewModel)
        {
            try
            {
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == viewModel.Id);

                // lottery game
                lottery.Title = viewModel.Title;
                lottery.Volume = viewModel.Volume;
                lottery.UnitPrice = viewModel.UnitPrice;
                lottery.LotteryCategoryId = viewModel.LotteryCategoryId;

                if (!viewModel.IsPublished)
                    lottery.Status = (int)EnumLotteryGameStatus.PENDING;
                else
                    lottery.Status = (int)EnumLotteryGameStatus.ACTIVE;

                var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                foreach (var detail in viewModel.LotteryDetails)
                {
                    var lotteryDetail = _lotteryDetailService.Queryable().FirstOrDefault(x => x.Id == detail.Id);
                    // Desktop slide image
                    if (detail.DesktopTopImageFile != null)
                    {
                        var desktopTopImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_dt_{timestamp}_{detail.DesktopTopImageFile.FileName}";
                        var desktopTopImagePath = Path.Combine(pathLottery, desktopTopImage);
                        detail.DesktopTopImageFile.CopyTo(new FileStream(desktopTopImagePath, FileMode.Create));
                        lotteryDetail.DesktopTopImage = desktopTopImage;
                    }

                    // Mobile slide image
                    if (detail.MobileTopImageFile != null)
                    {
                        var mobileTopImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_mt_{timestamp}_{detail.MobileTopImageFile.FileName}";
                        var mobileTopImagePath = Path.Combine(pathLottery, mobileTopImage);
                        detail.MobileTopImageFile.CopyTo(new FileStream(mobileTopImagePath, FileMode.Create));
                        lotteryDetail.MobileTopImage = mobileTopImage;
                    }

                    // desktop listing image
                    if (detail.DesktopListingImageFile != null)
                    {
                        var desktopListingImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_dl_{timestamp}_{detail.DesktopListingImageFile.FileName}";
                        var desktopListingImagePath = Path.Combine(pathLottery, desktopListingImage);
                        detail.DesktopListingImageFile.CopyTo(new FileStream(desktopListingImagePath, FileMode.Create));
                        lotteryDetail.DesktopListingImage = desktopListingImage;
                    }

                    // mobile listing image
                    if (detail.MobileListingImageFile != null)
                    {
                        var mobileListingImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_ml_{timestamp}_{detail.MobileListingImageFile.FileName}";
                        var mobileListingImagePath = Path.Combine(pathLottery, mobileListingImage);
                        detail.MobileListingImageFile.CopyTo(new FileStream(mobileListingImagePath, FileMode.Create));
                        lotteryDetail.MobileListingImage = mobileListingImage;
                    }

                    // prize image
                    if (detail.PrizeImageFile != null)
                    {
                        var prizeImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_p_{timestamp}_{detail.PrizeImageFile.FileName}";
                        var prizeImagePath = Path.Combine(pathLottery, prizeImage);
                        detail.PrizeImageFile.CopyTo(new FileStream(prizeImagePath, FileMode.Create));
                        lotteryDetail.PrizeImage = prizeImage;
                    }

                    lotteryDetail.Description = detail.Description;

                    _lotteryDetailService.Update(lotteryDetail);
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
                var latestLottery = _lotteryService.Queryable().LastOrDefault(x => !x.IsDeleted);
                var currentPhase = latestLottery == null ? 0 : latestLottery.Phase;
                var currentId = latestLottery == null ? 0 : latestLottery.Id;

                var lottery = new Lottery();

                lottery.Title = viewModel.Title;
                lottery.Volume = viewModel.Volume;
                lottery.UnitPrice = viewModel.UnitPrice;
                lottery.Phase = currentPhase + 1;
                lottery.CreatedDate = DateTime.Now;
                lottery.LotteryCategoryId = viewModel.LotteryCategoryId;

                if (!viewModel.IsPublished)
                    lottery.Status = (int)EnumLotteryGameStatus.PENDING;
                else
                    lottery.Status = (int)EnumLotteryGameStatus.ACTIVE;

                var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                _lotteryService.Insert(lottery);
                _unitOfWork.SaveChanges();

                foreach (var detail in viewModel.LotteryDetails)
                {
                    var lotteryDetail = new LotteryDetail();
                    // Desktop slide image
                    if (detail.DesktopTopImageFile != null)
                    {
                        var desktopTopImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_dt_{timestamp}_{detail.DesktopTopImageFile.FileName}";
                        var desktopTopImagePath = Path.Combine(pathLottery, desktopTopImage);
                        detail.DesktopTopImageFile.CopyTo(new FileStream(desktopTopImagePath, FileMode.Create));
                        lotteryDetail.DesktopTopImage = desktopTopImage;
                    }

                    // Mobile slide image
                    if (detail.MobileTopImageFile != null)
                    {
                        var mobileTopImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_mt_{timestamp}_{detail.MobileTopImageFile.FileName}";
                        var mobileTopImagePath = Path.Combine(pathLottery, mobileTopImage);
                        detail.MobileTopImageFile.CopyTo(new FileStream(mobileTopImagePath, FileMode.Create));
                        lotteryDetail.MobileTopImage = mobileTopImage;
                    }

                    // desktop listing image
                    if (detail.DesktopListingImageFile != null)
                    {
                        var desktopListingImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_dl_{timestamp}_{detail.DesktopListingImageFile.FileName}";
                        var desktopListingImagePath = Path.Combine(pathLottery, desktopListingImage);
                        detail.DesktopListingImageFile.CopyTo(new FileStream(desktopListingImagePath, FileMode.Create));
                        lotteryDetail.DesktopListingImage = desktopListingImage;
                    }

                    // mobile listing image
                    if (detail.MobileListingImageFile != null)
                    {
                        var mobileListingImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_ml_{timestamp}_{detail.MobileListingImageFile.FileName}";
                        var mobileListingImagePath = Path.Combine(pathLottery, mobileListingImage);
                        detail.MobileListingImageFile.CopyTo(new FileStream(mobileListingImagePath, FileMode.Create));
                        lotteryDetail.MobileListingImage = mobileListingImage;
                    }

                    // prize image
                    if (detail.PrizeImageFile != null)
                    {
                        var prizeImage = $"{lottery.Phase.ToString()}_lang_{detail.LangId}_p_{timestamp}_{detail.PrizeImageFile.FileName}";
                        var prizeImagePath = Path.Combine(pathLottery, prizeImage);
                        detail.PrizeImageFile.CopyTo(new FileStream(prizeImagePath, FileMode.Create));
                        lotteryDetail.PrizeImage = prizeImage;
                    }

                    lotteryDetail.LotteryId = lottery.Id;
                    lotteryDetail.LangId = detail.LangId;
                    lotteryDetail.Description = detail.Description;

                    _lotteryDetailService.Insert(lotteryDetail);
                }

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
                // udpate status for lottery game
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                lottery.IsDeleted = true;

                // refund money for user
                var amountToken = lottery.UnitPrice;
                var lotteryHistories = _lotteryHistoryService.Query().Include(x => x.SysUser).Select().Where(x => x.LotteryId == id).ToList();
                foreach (var lotteryHistory in lotteryHistories)
                {
                    lotteryHistory.SysUser.TokenAmount += amountToken;
                    _sysUserService.Update(lotteryHistory.SysUser);
                }

                _lotteryService.Update(lottery);
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
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == id);
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

        [Permission(EnumRole.Admin)]
        public IActionResult ConfirmDeactivateLottery(ConfirmLotteryViewModel viewModel)
        {
            return PartialView("_ConfirmDeactivateLottery", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public JsonResult DoDeactivateLottery(int id)
        {
            try
            {
                var lottery = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == id);
                lottery.Status = (int)EnumLotteryGameStatus.DEACTIVATED;
                _lotteryService.Update(lottery);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeactivateSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        #endregion

        #region PricePrediction
        #endregion

        #region Setting
        [Permission(EnumRole.Admin)]
        public IActionResult Setting()
        {
            var viewModel = new SettingViewModel();
            var settings = _settingService.Queryable();
            viewModel.IsKYCVerificationActivated = bool.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.IsKYCVerificationActivated).Value);
            viewModel.IsAccountActivationEnable = bool.Parse(settings.FirstOrDefault(x => x.Name == CPLConstant.IsAccountActivationEnable).Value);
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
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Admin)]
        public IActionResult DoUpdateSetting(string data)
        {
            try
            {
                var dataInList = JsonConvert.DeserializeObject<List<SettingDataModel>>(data);
                foreach (var _data in dataInList)
                {
                    var setting = _settingService.Queryable().FirstOrDefault(x => x.Name == _data.Name);
                    setting.Value = _data.Value;
                    _settingService.Update(setting);
                }

                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
            }
        }

        #endregion
    }
}