using AutoMapper;
using CPL.Common.Enums;
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
        private readonly IDictionary<string, string> countryDict = new Dictionary<string, string>();
        private readonly ILotteryService _lotteryService;
        private readonly ILotteryPrizeService _lotteryPrizeService;

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
            ILotteryPrizeService lotteryPrizeService)
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
            this._hostingEnvironment = hostingEnvironment;
        }

        [Permission(EnumRole.Admin)]
        public IActionResult Index()
        {
            var viewModel = new AdminViewModel();

            // User management
            viewModel.TotalKYCPending = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && !x.KYCVerified.Value);
            viewModel.TotalKYCVerified = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && x.KYCVerified.Value);
            viewModel.TotalUser = _sysUserService.Queryable().Count();

            // Game management
            var lotteryGames = _lotteryService.Queryable();
            viewModel.TotalLotteryGame = lotteryGames.Count();
            viewModel.TotalLotteryGamePending = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.PENDING).Count();
            viewModel.TotalLotteryGameActive = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.ACTIVE).Count();
            viewModel.TotalLotteryGameCompleted = lotteryGames.Where(x => x.Status == (int)EnumLotteryGameStatus.COMPLETED).Count();

            viewModel.TotalNews = _newsService.Queryable().Count();
            return View(viewModel);
        }

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
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
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
        public IActionResult UpdateUser(SysUserViewModel viewModel)
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
        public IActionResult DeleteUser(int id)
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
        public IActionResult KYCVerify()
        {
            var viewModel = new KYCVerifyViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult UpdateKYCVerify(int id)
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
        public IActionResult CancelKYCVerify(int id)
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
        public IActionResult News()
        {
            var viewModel = new NewsViewModel();
            return View(viewModel);
        }

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
        public JsonResult SaveEditNews(NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var news = _newsService.Queryable()
                .FirstOrDefault(x => x.Id == viewModel.Id);
                if (viewModel.FileImage != null)
                {
                    var newsPath = Path.Combine(_hostingEnvironment.WebRootPath, @"images\news");
                    var image = $"{viewModel.FileImage.FileName}";
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
        public JsonResult AddNews(NewsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (viewModel.FileImage != null)
                    _newsService.Insert(new Domain.News { Title = viewModel.Title, CreatedDate = DateTime.Now, Description = viewModel.Description, ShortDescription = viewModel.ShortDescription, Image = viewModel.FileImage.FileName });
                else
                    _newsService.Insert(new Domain.News { Title = viewModel.Title, CreatedDate = DateTime.Now, Description = viewModel.Description, ShortDescription = viewModel.ShortDescription });
                _unitOfWork.SaveChanges();

                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "AddSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [HttpPost]
        public JsonResult DeleteNews(int id)
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
        public IActionResult Lottery()
        {
            return View();
        }

        [HttpPost]
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

        public IActionResult ViewLottery(int id)
        {
            var lottery = new LotteryViewModel();

            lottery = Mapper.Map<LotteryViewModel>(_lotteryService.Query()
                                                        .Include(x => x.LotteryPrizes)
                                                        .Select()
                                                        .FirstOrDefault(x => x.Id == id));

            return PartialView("_ViewLottery", lottery);
        }

        public IActionResult ViewLotteryPrize(UserLotteryPrizeViewModel viewModel)
        {
            return PartialView("_ViewLotteryPrize", viewModel);
        }

        [HttpPost]
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

        public IActionResult EditLottery(int id)
        {
            var lottery = new LotteryViewModel();

            lottery = Mapper.Map<LotteryViewModel>(_lotteryService.Query()
                                                        .Include(x => x.LotteryPrizes)
                                                        .Select()
                                                        .FirstOrDefault(x => x.Id == id));

            return PartialView("_EditLottery", lottery);
        }

        public IActionResult AddLottery()
        {
            var lottery = new LotteryViewModel();
            return PartialView("_EditLottery", lottery);
        }

        [HttpPost]
        public JsonResult UpdateLottery(LotteryViewModel viewModel)
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

                // slider image
                if (viewModel.DesktopListingImageFile != null)
                {
                    var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    var sliderImage = $"{lottery.Phase.ToString()}_Slider_{timestamp}_{viewModel.SlideImageFile.FileName}";
                    var sliderImagePath = Path.Combine(pathLottery, sliderImage);
                    viewModel.SlideImageFile.CopyTo(new FileStream(sliderImagePath, FileMode.Create));
                    lottery.SlideImage = sliderImage;
                }

                // desktop image
                if (viewModel.MobileListingImageFile != null)
                {
                    var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    var desktopImage = $"{lottery.Phase.ToString()}_Desktop_{timestamp}_{viewModel.DesktopListingImageFile.FileName}";
                    var desktopImagePath = Path.Combine(pathLottery, desktopImage);
                    viewModel.DesktopListingImageFile.CopyTo(new FileStream(desktopImagePath, FileMode.Create));
                    lottery.DesktopListingImage = desktopImage;
                }

                // mobile image
                if (viewModel.SlideImageFile != null)
                {
                    var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    var mobileImage = $"{lottery.Phase.ToString()}_Mobile_{timestamp}_{viewModel.MobileListingImageFile.FileName}";
                    var mobileImagePath = Path.Combine(pathLottery, mobileImage);
                    viewModel.MobileListingImageFile.CopyTo(new FileStream(mobileImagePath, FileMode.Create));
                    lottery.MobileListingImage = mobileImage;
                }

                _lotteryService.Update(lottery);

                // lottery prize
                var lotteryPrize = _lotteryPrizeService.Queryable().Where(x => x.LotteryId == viewModel.Id).ToList();
                var numberOfOldPrize = lotteryPrize.Count();
                var numberOfNewPrize = viewModel.LotteryPrizes.Count - 1;

                if (numberOfOldPrize >= numberOfNewPrize)
                {
                    for (var i = 0; i < numberOfOldPrize; i++)
                    {
                        if (i < numberOfNewPrize)
                        {
                            var newPrize = lotteryPrize[i];
                            newPrize.Value = viewModel.LotteryPrizes[i].Value;
                            newPrize.Volume = viewModel.LotteryPrizes[i].Volume;
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
                            newPrize.Value = viewModel.LotteryPrizes[i].Value;
                            newPrize.Volume = viewModel.LotteryPrizes[i].Volume;
                            _lotteryPrizeService.Update(newPrize);
                        }
                        else
                        {
                            var prize = viewModel.LotteryPrizes[i];
                            if (prize.Volume == 0 && prize.Value == 0) continue;
                            var color = "";
                            switch (i)
                            {
                                case 0:
                                    color = "warning";
                                    break;
                                case 1:
                                    color = "primary";
                                    break;
                                case 2:
                                    color = "danger";
                                    break;
                                default:
                                    color = "success";
                                    break;
                            }

                            _lotteryPrizeService.Insert(new LotteryPrize()
                            {
                                Name = Utils.AddOrdinal(i + 1),
                                Value = prize.Value,
                                Color = color,
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
        public JsonResult AddLottery(LotteryViewModel viewModel)
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

                if (viewModel.DesktopListingImageFile != null && viewModel.MobileListingImageFile != null && viewModel.SlideImageFile != null)
                {
                    var pathLottery = Path.Combine(_hostingEnvironment.WebRootPath, @"images\lottery");
                    string timestamp = DateTime.Now.ToString("yyyyMMddhhmmss");

                    // slider image
                    var sliderImage = $"{lottery.Phase.ToString()}_Slider_{timestamp}_{viewModel.SlideImageFile.FileName}";
                    var sliderImagePath = Path.Combine(pathLottery, sliderImage);
                    viewModel.SlideImageFile.CopyTo(new FileStream(sliderImagePath, FileMode.Create));
                    lottery.SlideImage = sliderImage;

                    // desktop image
                    var desktopImage = $"{lottery.Phase.ToString()}_Desktop_{timestamp}_{viewModel.DesktopListingImageFile.FileName}";
                    var desktopImagePath = Path.Combine(pathLottery, desktopImage);
                    viewModel.DesktopListingImageFile.CopyTo(new FileStream(desktopImagePath, FileMode.Create));
                    lottery.DesktopListingImage = desktopImage;

                    // mobile image
                    var mobileImage = $"{lottery.Phase.ToString()}_Mobile_{timestamp}_{viewModel.MobileListingImageFile.FileName}";
                    var mobileImagePath = Path.Combine(pathLottery, mobileImage);
                    viewModel.MobileListingImageFile.CopyTo(new FileStream(mobileImagePath, FileMode.Create));
                    lottery.MobileListingImage = mobileImage;
                }

                _lotteryService.Insert(lottery);

                // Lottery prize
                foreach (var prize in viewModel.LotteryPrizes)
                {
                    if (prize.Volume == 0 && prize.Value == 0) continue;
                    var index = viewModel.LotteryPrizes.IndexOf(prize);
                    var color = "";
                    switch (index)
                    {
                        case 0:
                            color = "warning";
                            break;
                        case 1:
                            color = "primary";
                            break;
                        case 2:
                            color = "danger";
                            break;
                        default:
                            color = "success";
                            break;
                    }

                    _lotteryPrizeService.Insert(new LotteryPrize()
                    {
                        Name = Utils.AddOrdinal(index + 1),
                        Value = prize.Value,
                        Color = color,
                        Volume = prize.Volume,
                        LotteryId = currentId + 1
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

        public IActionResult ConfirmDeleteLottery(int id)
        {
            ViewData["gameId"] = id;
            return PartialView("_ConfirmDeleteLottery");
        }

        [HttpPost]
        public JsonResult DeleteLottery(int id)
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
        public JsonResult ActivateLottery(int id)
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