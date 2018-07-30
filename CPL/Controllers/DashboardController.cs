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
using System.Collections.Generic;
using System.Linq;

namespace CPL.Controllers
{
    [Permission(EnumRole.User)]
    public class DashboardController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly IGameHistoryService _gameHistoryService;


        public DashboardController(
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
            IGameHistoryService gameHistoryService)
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
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._gameHistoryService = gameHistoryService;
        }

        public IActionResult Index()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;
            return View(viewModel);
        }

        public IActionResult DepositAndWithdrawal()
        {
            var viewModel = new DepositAndWithdrawViewModel();
            return View(viewModel);
        }

        public IActionResult Exchange()
        {
            var viewModel = new ExchangeViewModel();
            return View(viewModel);
        }

        public IActionResult WithdrawBTC()
        {
            return PartialView("_BtcOut");
        }

        [HttpPost]
        public IActionResult GetDataPieChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;

            // Holding Percentage
            viewModel.HoldingPercentage = new HoldingPercentageViewModel();

            if (user.TokenAmount > 0)
            {
                viewModel.HoldingPercentage.CPLPercentage = user.TokenAmount / decimal.Parse(tokenRate) / viewModel.TotalBalance * 100;
            }
            if (user.TokenAmount > 0)
            {
                viewModel.HoldingPercentage.ETHPercentage = user.ETHAmount * coinRate / viewModel.TotalBalance * 100;
            }
            if (user.BTCAmount > 0)
            {
                viewModel.HoldingPercentage.BTCPercentage = user.BTCAmount / viewModel.TotalBalance * 100;
            }

            var mess = JsonConvert.SerializeObject(viewModel.HoldingPercentage, Formatting.Indented);
            return new JsonResult(new { success = true, message = mess });
        }

        [HttpPost]
        public IActionResult GetDataLineChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);

            var lotteryHistory = _lotteryHistoryService
                        .Query().Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.LotteryPrize?.Value })
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            Amount = (y.Select(x => x).Count() * CPLConstant.LotteryTicketPrice),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                        })
                        .AsQueryable()
                        .ToList();

            var pricePredictionHistory = _pricePredictionHistoryService
                    .Queryable()
                    .Where(x => x.SysUserId == user.Id)
                    .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                    .ToList();

            var gameHistoryList = lotteryHistory.Concat(pricePredictionHistory).ToList();

            viewModel.MonthlyInvest = gameHistoryList.AsQueryable()
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => x.Amount) })
                        .ToList();
            viewModel.MonthlyInvest.Reverse();

            viewModel.AssetChange = gameHistoryList.AsQueryable()
                        .Where(x => x.Result != string.Empty)
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => (x.Award.Value - x.Amount)) })
                        .ToList();
            viewModel.AssetChange.Reverse();

            viewModel.BonusChange = gameHistoryList.AsQueryable()
                        .Where(x => x.Result != string.Empty)
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => x.Award.Value) })
                        .ToList();
            viewModel.BonusChange.Reverse();


            var mess = JsonConvert.SerializeObject(viewModel, Formatting.Indented);
            return new JsonResult(new { success = true, message = mess });
        }

        public JsonResult SearchGameHistory(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchGameHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<GameHistoryViewModel> SearchGameHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
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
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : "LOSE"),
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
                        .OrderBy(sortBy, sortDir)
                        .ToList();

                var pricePredictionHistory = _pricePredictionHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .ToList();

                return lotteryHistory.Concat(pricePredictionHistory).AsQueryable().OrderBy("CreatedDate", sortDir).AsQueryable().OrderBy(sortBy, sortDir).ToList();
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
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : "LOSE"),
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
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : "LOSE"),
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
                        .Skip(skip)
                        .Take(take)
                        .AsQueryable()
                        .OrderBy(sortBy, sortDir)
                        .ToList();

                var pricePredictionHistory = _pricePredictionHistoryService
                        .Query()
                        .Select()
                        .AsQueryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .Where(x => x.AmountInString.Contains(searchBy) || x.AwardInString.Contains(searchBy) || x.GameType.ToLower().Contains(searchBy) || x.CreatedDateInString.Contains(searchBy) || x.Result.ToLower().Contains(searchBy) || x.Result.ToLower().Contains(searchBy))
                        .Skip(skip)
                        .Take(take)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .ToList();

                return lotteryHistory.Concat(pricePredictionHistory).AsQueryable().OrderBy(sortBy, sortDir).ToList();
            }
        }
    }
}