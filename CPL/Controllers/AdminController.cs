using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPL.Controllers
{
    [Permission(EnumRole.Admin)]
    public class AdminController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IGameHistoryService _gameHistoryService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;

        public AdminController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            IGameHistoryService gameHistoryService,
            ILotteryHistoryService lotteryHistoryService,
            IPricePredictionHistoryService pricePredictionHistoryService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._gameHistoryService = gameHistoryService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
        }

        public IActionResult Index()
        {
            var viewModel = new AdminViewModel();
            viewModel.TotalKYCPending = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && !x.KYCVerified.Value);
            viewModel.TotalKYCVerified = _sysUserService.Queryable().Count(x => x.KYCVerified.HasValue && x.KYCVerified.Value);
            viewModel.TotalUser = _sysUserService.Queryable().Count();
            return View(viewModel);
        }

        public IActionResult AllUser()
        {
            var viewModel = new AllUserViewModel();
            return View(viewModel);
        }

        public IActionResult UserDashboard(int id)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == id);
            var viewModel = Mapper.Map<UserDashboardAdminViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;
            
            return View(viewModel);
        }

        public IActionResult KYCVerify()
        {
            var viewModel = new KYCVerifyViewModel();
            return View(viewModel);
        }

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

        public IActionResult EditUser(int id)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == id);

            return PartialView("_Edit", Mapper.Map<SysUserViewModel>(user));
        }

        [HttpPost]
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
                user.StreetAddress = viewModel.StreetAddress.ToLower();
                user.TwoFactorAuthenticationEnable = viewModel.TwoFactorAuthenticationEnable;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "UpdateSuccessfully") });
            }

            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        [HttpPost]
        public IActionResult DeleteUser(SysUserViewModel viewModel)
        {
            var user = _sysUserService.Queryable()
                .FirstOrDefault(x => x.Id == viewModel.Id);

            if (user != null)
            {
                user.IsDeleted = true;

                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
                return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "DeleteSuccessfully") });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NonExistingAccount") });
        }

        public JsonResult SearchUserGameHistory(DataTableAjaxPostModel viewModel, int userId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchUserGameHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount, userId);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<GameHistoryViewModel> SearchUserGameHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int userId)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == userId);
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "desc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _lotteryHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .GroupBy(x => x.LotteryId)
                        .Count()
                        +
                        _pricePredictionHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();


                totalResultsCount = _lotteryHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .GroupBy(x => x.LotteryId)
                        .Count()
                        +
                        _pricePredictionHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();

                var lotteryHistory = _lotteryHistoryService
                        .Query().Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.LotteryPrize?.Value })
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameId.LOTTERY.ToString(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : EnumGameResult.LOSE.ToString()),
                            AmountInString = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice) : 0,
                        })
                        .AsQueryable()
                        .ToList();

                var pricePredictionHistory = _pricePredictionHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .ToList();

                return lotteryHistory.Concat(pricePredictionHistory).AsQueryable().OrderBy("CreatedDate", sortDir)
                    .AsQueryable()
                    .OrderBy(sortBy, sortDir)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
            else
            {
                filteredResultsCount = _lotteryHistoryService.Query()
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.LotteryPrize?.Value })
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameId.LOTTERY.ToString(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : EnumGameResult.LOSE.ToString()),
                            AmountInString = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice) : 0,
                        })
                        .Where(x => x.AmountInString.Contains(searchBy) || x.AwardInString.Contains(searchBy) || x.GameType.ToLower().Contains(searchBy) || x.CreatedDateInString.Contains(searchBy) || x.Result.ToLower().Contains(searchBy) || x.Result.ToLower().Contains(searchBy))
                        .Count()
                        +
                        _pricePredictionHistoryService.Query()
                        .Select()
                        .AsQueryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .Where(x => x.AmountInString.Contains(searchBy) || x.AwardInString.Contains(searchBy) || x.GameType.ToLower().Contains(searchBy) || x.CreatedDateInString.Contains(searchBy) || x.Result.ToLower().Contains(searchBy) || x.Result.ToLower().Contains(searchBy))
                        .Count();

                totalResultsCount = _lotteryHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .GroupBy(x => x.LotteryId)
                        .Count()
                        +
                        _pricePredictionHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();

                var lotteryHistory = _lotteryHistoryService.Query()
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.LotteryPrize?.Value })
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameId.LOTTERY.ToString(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : EnumGameResult.LOSE.ToString()),
                            AmountInString = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice) : 0,
                        })
                        .Where(x => x.AmountInString.Contains(searchBy) || x.AwardInString.Contains(searchBy) || x.GameType.ToLower().Contains(searchBy) || x.CreatedDateInString.Contains(searchBy) || x.Result.ToLower().Contains(searchBy) || x.Result.ToLower().Contains(searchBy))
                        .AsQueryable()
                        .ToList();

                var pricePredictionHistory = _pricePredictionHistoryService
                        .Query()
                        .Select()
                        .AsQueryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .Where(x => x.AmountInString.Contains(searchBy) || x.AwardInString.Contains(searchBy) || x.GameType.ToLower().Contains(searchBy) || x.CreatedDateInString.Contains(searchBy) || x.Result.ToLower().Contains(searchBy) || x.Result.ToLower().Contains(searchBy))
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .ToList();

                return lotteryHistory.Concat(pricePredictionHistory)
                    .AsQueryable().OrderBy(sortBy, sortDir)
                    .Skip(skip)
                    .Take(take)
                    .ToList();
            }
        }
    }
}