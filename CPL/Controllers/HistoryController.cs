using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CPL.Common.CurrenciesPairRateHelper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using CPL.ViewComponents;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ILotteryHistoryService _lotteryHistoryService;
        private readonly ISysUserService _sysUserService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly ISettingService _settingService;
        private readonly ICoinTransactionService _coinTransactionService;

        public HistoryController(
            ILotteryHistoryService lotteryHistoryService,
            ISysUserService sysUserService,
            ISettingService settingService,
            ICoinTransactionService coinTransactionService,
            IPricePredictionHistoryService pricePredictionHistoryService)
        {
            this._lotteryHistoryService = lotteryHistoryService;
            this._sysUserService = sysUserService;
            this._pricePredictionHistoryService = pricePredictionHistoryService;
            this._settingService = settingService;
            this._coinTransactionService = coinTransactionService;
        }

        [Permission(EnumRole.User)]
        public IActionResult Game()
        {
            var viewModel = new GameHistoryIndexViewModel();
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
            return View(viewModel);
        }

        #region Lottery History
        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public IActionResult Lottery(DateTime? createdDate, int? lotteryId, int sysUserId)
        {
            var viewModel = new LotteryHistoryIndexViewModel
            {
                CreatedDate = createdDate,
                LotteryId = lotteryId,
                SysUserId = sysUserId
            };
            return View(viewModel);
        }

        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public JsonResult SearchLotteryHistory(DataTableAjaxPostModel viewModel, DateTime? createdDate, int? lotteryId, int sysUserId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchLotteryHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount, createdDate, lotteryId, sysUserId);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public IList<LotteryHistoryViewModel> SearchLotteryHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, 
            DateTime? createdDate, int? lotteryId, int sysUserId)
        {
            if (sysUserId == 0)
            {
                sysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
            }
            var searchBy = (model.search != null) ? model.search.value?.ToLower() : null;
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

            if (createdDate.HasValue || lotteryId.HasValue)
            {
                totalResultsCount = _lotteryHistoryService
                                 .Query()
                                 .Include(x => x.Lottery)
                                 .Include(x => x.LotteryPrize)
                                 .Select()
                                 .Where(x => x.SysUserId == sysUserId && x.LotteryId == lotteryId.Value && (createdDate.HasValue ? x.CreatedDate.Date == createdDate.Value.Date : true))
                                 .Count();

                // search the dbase taking into consideration table sorting and paging
                var lotteryHistory = _lotteryHistoryService
                                              .Query()
                                              .Include(x => x.Lottery)
                                              .Include(x => x.LotteryPrize)
                                              .Select()
                                              .Where(x => x.SysUserId == sysUserId && x.LotteryId == lotteryId.Value && (createdDate.HasValue ? x.CreatedDate.Date == createdDate.Value.Date : true))
                                              .Select(x => new LotteryHistoryViewModel
                                              {
                                                  CreatedDate = x.CreatedDate,
                                                  CreatedDateInString = x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
                                                  LotteryStartDate = x.Lottery.CreatedDate,
                                                  LotteryStartDateInString = x.Lottery.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
                                                  LotteryPhase = x.Lottery.Phase,
                                                  LotteryPhaseInString = x.Lottery.Phase.ToString("D3"),
                                                  Result = x.Result == EnumGameResult.WIN.ToString() ? "Win" : (x.Result == EnumGameResult.LOSE.ToString() ? "Lose" : (x.Result == EnumGameResult.KYC_PENDING.ToString() ? "KYC Pending" : string.Empty)),
                                                  Award = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value : 0,
                                                  AwardInString = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value.ToString("#,##0.##") : 0.ToString("#,##0.##"),
                                                  TicketNumber = !string.IsNullOrEmpty(x.TicketNumber) ? $"{x.Lottery.Phase.ToString("D3")}{CPLConstant.ProjectName}{x.TicketNumber}" : string.Empty,
                                                  UpdatedDate = x.UpdatedDate,
                                                  UpdatedDateInString = x.UpdatedDate.HasValue ? x.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") : string.Empty,
                                              });

                if (string.IsNullOrEmpty(searchBy))
                {
                    filteredResultsCount = totalResultsCount;
                }
                else
                {
                    lotteryHistory = lotteryHistory
                                            .Where(x => x.CreatedDateInString.ToLower().Contains(searchBy)
                                                        || x.LotteryPhaseInString.ToLower().Contains(searchBy)
                                                        || x.Result.ToLower().Contains(searchBy)
                                                        || x.AwardInString.ToLower().Contains(searchBy)
                                                        || x.TicketNumber.ToLower().Contains(searchBy)
                                                        || x.UpdatedDateInString.ToLower().Contains(searchBy));

                    filteredResultsCount = lotteryHistory.Count();
                }

                return lotteryHistory.AsQueryable().OrderBy(sortBy, sortDir).Skip(skip).Take(take).ToList();
            }
            else
            {
                totalResultsCount = _lotteryHistoryService
                                 .Query()
                                 .Include(x => x.Lottery)
                                 .Include(x => x.LotteryPrize)
                                 .Select()
                                 .Where(x => x.SysUserId == sysUserId)
                                 .Count();

                // search the dbase taking into consideration table sorting and paging
                var lotteryHistory = _lotteryHistoryService
                                              .Query()
                                              .Include(x => x.Lottery)
                                              .Include(x => x.LotteryPrize)
                                              .Select()
                                              .Where(x => x.SysUserId == sysUserId)
                                              .Select(x => new LotteryHistoryViewModel
                                              {
                                                  CreatedDate = x.CreatedDate,
                                                  CreatedDateInString = x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
                                                  LotteryStartDate = x.Lottery.CreatedDate,
                                                  LotteryStartDateInString = x.Lottery.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
                                                  LotteryPhase = x.Lottery.Phase,
                                                  LotteryPhaseInString = x.Lottery.Phase.ToString("D3"),
                                                  Result = x.Result == EnumGameResult.WIN.ToString() ? "Win" : (x.Result == EnumGameResult.LOSE.ToString() ? "Lose" : (x.Result == EnumGameResult.KYC_PENDING.ToString() ? "KYC Pending" : string.Empty)),
                                                  Award = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value : 0,
                                                  AwardInString = x.LotteryPrizeId.HasValue ? x.LotteryPrize.Value.ToString("#,##0.##") : 0.ToString("#,##0.##"),
                                                  TicketNumber = !string.IsNullOrEmpty(x.TicketNumber) ? $"{x.Lottery.Phase.ToString("D3")}{CPLConstant.ProjectName}{x.TicketNumber}" : string.Empty,
                                                  UpdatedDate = x.UpdatedDate,
                                                  UpdatedDateInString = x.UpdatedDate.HasValue ? x.UpdatedDate.Value.ToString("yyyy/MM/dd hh:mm:ss") : string.Empty,
                                              });

                if (string.IsNullOrEmpty(searchBy))
                {
                    filteredResultsCount = totalResultsCount;
                }
                else
                {
                    lotteryHistory = lotteryHistory
                                            .Where(x => x.CreatedDateInString.ToLower().Contains(searchBy)
                                                        || x.LotteryPhaseInString.ToLower().Contains(searchBy)
                                                        || x.Result.ToLower().Contains(searchBy)
                                                        || x.AwardInString.ToLower().Contains(searchBy)
                                                        || x.TicketNumber.ToLower().Contains(searchBy)
                                                        || x.UpdatedDateInString.ToLower().Contains(searchBy));

                    filteredResultsCount = lotteryHistory.Count();
                }

                return lotteryHistory.AsQueryable().OrderBy(sortBy, sortDir).Skip(skip).Take(take).ToList();
            }
        }
        #endregion

        #region Game History
        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public JsonResult SearchGameHistory(DataTableAjaxPostModel viewModel, int sysUserId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchGameHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount, sysUserId);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public IList<GameHistoryViewModel> SearchGameHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int sysUserId)
        {
            var user = new SysUserViewModel();
            if (sysUserId > 0)
                user = Mapper.Map<SysUserViewModel>(_sysUserService.Queryable().Where(x => x.Id == sysUserId).FirstOrDefault());
            else
                user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
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

                var lotteryHistory = _lotteryHistoryService.Query()
                        .Include(x => x.Lottery)
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.Lottery.UnitPrice, x.LotteryPrize?.Value})
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameType.LOTTERY.ToString(),
                            GameId = y.Select(x=>x.LotteryId).FirstOrDefault(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : (y.Any(x => x.Result == EnumGameResult.LOSE.ToString()) ? EnumGameResult.LOSE.ToString() : string.Empty)),
                            AmountInString = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()) : 0,
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
                        .Include(x => x.Lottery)
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.Lottery.UnitPrice, x.LotteryPrize?.Value})
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameType.LOTTERY.ToString(),
                            GameId = y.Select(x => x.LotteryId).FirstOrDefault(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : EnumGameResult.LOSE.ToString()),
                            AmountInString = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()) : 0,
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
                        .Include(x => x.Lottery)
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.Lottery.UnitPrice, x.LotteryPrize?.Value})
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            CreatedDateInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("yyyy/MM/dd"),
                            CreatedTimeInString = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault().ToString("HH:mm:ss"),
                            GameType = EnumGameType.LOTTERY.ToString(),
                            GameId = y.Select(x => x.LotteryId).FirstOrDefault(),
                            Result = y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true ? EnumGameResult.WIN.ToString() : (y.Any(x => x.Result == EnumGameResult.KYC_PENDING.ToString()) == true ? EnumGameResult.KYC_PENDING.ToString() : EnumGameResult.LOSE.ToString()),
                            AmountInString = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("#,##0"),
                            Amount = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()),
                            AwardInString = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))).ToString("#,##0"),
                            Award = (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString()).Sum(x => x.Value.GetValueOrDefault(0))),
                            BalanceInString = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()).ToString("+#,##0;-#,##0") : string.Empty,
                            Balance = (y.Any(x => x.Result == EnumGameResult.WIN.ToString()) == true || y.Any(x => x.Result == EnumGameResult.LOSE.ToString())) == true ?
                                                    (y.Select(x => x).Where(x => x.Result == EnumGameResult.WIN.ToString() || x.Result == EnumGameResult.LOSE.ToString()).Sum(x => x.Value.GetValueOrDefault(0)) - y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()) : 0,
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
        #endregion

        #region Transaction History
        [Permission(EnumRole.User, EnumEntity.CoinTransaction, EnumAction.Read)]
        public IActionResult Transaction(TransactionHistoryIndexViewModel viewModel)
        {
            return View(viewModel);
        }

        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public JsonResult SearchTransactionHistory(DataTableAjaxPostModel viewModel, int? sysUserId, int? currencyId)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchTransactionHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount, sysUserId, currencyId);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        [Permission(EnumRole.User, EnumEntity.LotteryHistory, EnumAction.Read)]
        public IList<CoinTransactionViewModel> SearchTransactionHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount, int? sysUserId, int? currencyId)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == (sysUserId ?? HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id));
            var searchBy = (model.search != null) ? model.search.value?.ToLower() : null;
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

            // NULLABLE INT CANNOT BE USED IN LINQ, SO VALUE CAST IS A MUST
            var _currencyId = 0;
            if (currencyId.HasValue)
                _currencyId = currencyId.Value;

            totalResultsCount = _coinTransactionService
                                 .Queryable()
                                 .Where(x => x.SysUserId == user.Id && (_currencyId == 0 ? true : x.CurrencyId == _currencyId))
                                 .Count();

            // search the dbase taking into consideration table sorting and paging
            var transactionHistory = _coinTransactionService
                                          .Query()
                                          .Include(x => x.Currency)
                                          .Select()
                                          .Where(x => x.SysUserId == user.Id && (_currencyId == 0 ? true : x.CurrencyId == _currencyId))
                                          .Select(x => new CoinTransactionViewModel
                                          {
                                              Id = x.Id,
                                              CreatedDate = x.CreatedDate,
                                              CreatedDateInString = x.CreatedDate.ToString(Format.DateTime),
                                              FromWalletAddress = x.FromWalletAddress,
                                              ToWalletAddress = x.ToWalletAddress,
                                              CoinAmount = x.CoinAmount,
                                              CoinAmountInString = x.CoinAmount.ToString(Format.Amount),
                                              CurrencyId = x.CurrencyId,
                                              CurrencyInString = x.Currency.Name,
                                              TypeInString = EnumHelper<EnumCoinTransactionType>.GetDisplayValue((EnumCoinTransactionType)x.Type),
                                              StatusInEnum = x.Status.ToEnumCoinstransactionStatus(),
                                              StatusInString = x.Status.ToEnumCoinstransactionStatus().ToString(),
                                              Rate = x.Rate,
                                              TxHashId = x.TxHashId,
                                              TokenAmountInString = x.TokenAmount.GetValueOrDefault(0).ToString(Format.Amount)
                                          });

            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = totalResultsCount;
            }
            else
            {
                transactionHistory = transactionHistory
                                        .Where(x => x.CreatedDateInString.ToLower().Contains(searchBy)
                                                    || x.ToWalletAddress.ToLower().Contains(searchBy)
                                                    || x.CoinAmountInString.ToLower().Contains(searchBy)
                                                    || x.TypeInString.ToLower().Contains(searchBy)
                                                    || x.CurrencyInString.ToLower().Contains(searchBy)
                                                    || x.StatusInEnum.ToString().ToLower().Contains(searchBy));

                filteredResultsCount = transactionHistory.Count();
            }

            return transactionHistory.AsQueryable().OrderBy(sortBy, sortDir).Skip(skip).Take(take).ToList();
        }

        [Permission(EnumRole.User, EnumEntity.CoinTransaction, EnumAction.Read)]
        public IActionResult TransactionDetail(int id)
        {
            //var sysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
            var viewModel = _coinTransactionService
                                    .Query()
                                    .Include(x => x.Currency)
                                    .Select()
                                    .Where(x => /*x.SysUserId == sysUserId &&*/ x.Id == id)
                                    .Select(x => new CoinTransactionViewModel
                                    {
                                        Id = x.Id,
                                        CreatedDate = x.CreatedDate,
                                        CreatedDateInString = x.CreatedDate.ToString(Format.DateTime),
                                        FromWalletAddress = x.FromWalletAddress,
                                        ToWalletAddress = x.ToWalletAddress,
                                        CoinAmount = x.CoinAmount,
                                        CoinAmountInString = x.CoinAmount.ToString(Format.Amount),
                                        CurrencyId = x.CurrencyId,
                                        CurrencyInEnum = (EnumCurrency)x.CurrencyId,
                                        CurrencyInString = x.Currency.Name,
                                        TypeInString = EnumHelper<EnumCoinTransactionType>.GetDisplayValue((EnumCoinTransactionType)x.Type),
                                        StatusInEnum = x.Status.ToEnumCoinstransactionStatus(),
                                        StatusInString = x.Status.ToEnumCoinstransactionStatus().ToString(),
                                        Rate = x.Rate,
                                        TxHashId = x.TxHashId,
                                        TxHashReference = x.CurrencyId == 1 ? string.Format(CPLConstant.Etherscan, x.TxHashId) : x.CurrencyId == 2 ? string.Format(CPLConstant.BlockInfo, x.TxHashId) : "#",
                                        TokenAmount = x.TokenAmount,
                                        TokenAmountInString = x.TokenAmount.GetValueOrDefault(0).ToString(Format.Amount)
                                    })
                                    .FirstOrDefault();
            return View(viewModel);
        }
        #endregion


        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult GetDataPieChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<GameHistoryViewModel>(user);
            decimal coinRate = CurrenciesPairRateHelper.GetCurrenciesPairRate(EnumCurrenciesPair.ETHBTC.ToString()).Value;
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value;
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
        [Permission(EnumRole.User)]
        public IActionResult GetDataLineChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<GameHistoryViewModel>(user);

            var lotteryHistory = _lotteryHistoryService.Query()
                        .Include(x => x.Lottery)
                        .Include(x => x.LotteryPrize).Select()
                        .Where(x => x.SysUserId == user.Id)
                        .Select(x => new { x.LotteryId, x.CreatedDate, x.Result, x.Lottery.UnitPrice, x.LotteryPrize?.Value })
                        .GroupBy(x => x.LotteryId)
                        .Select(y => new GameHistoryViewModel
                        {
                            CreatedDate = y.Select(x => x.CreatedDate).OrderByDescending(x => x).FirstOrDefault(),
                            Amount = (y.Select(x => x).Count() * y.Select(x => x.UnitPrice).FirstOrDefault()),
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
    }
}