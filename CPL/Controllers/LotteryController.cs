using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Domain;
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
    [Permission(EnumRole.Guest)]
    public class LotteryController : Controller
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
        private readonly ILotteryService _lotteryService;
        private readonly ILotteryPrizeService _lotteryPrizeService;
        private readonly ILotteryHistoryService _lotteryHistoryService;

        public LotteryController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            ILotteryService lotteryService,
            ILotteryPrizeService lotteryPrizeService,
            ILotteryHistoryService lotteryHistoryService,
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
            this._lotteryService = lotteryService;
            this._lotteryPrizeService = lotteryPrizeService;
            this._lotteryHistoryService = lotteryHistoryService;
            this._gameHistoryService = gameHistoryService;
        }

        public IActionResult Index()
        {
            var viewModel = new LotteryGameViewModel();
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            viewModel.SysUserId = user?.Id;

            //TODO: Should load data from current lottery game in Lottery and LotteryPrize tables
            viewModel.Lotteries = _lotteryService
                .Query()
                .Include(x => x.LotteryHistories)
                .Include(x => x.LotteryPrizes)
                .Select()
                .Where(x => x.LotteryHistories.Count() < x.Volume && x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE))
                .Select(x => Mapper.Map<LotteryViewModel>(x))
                .ToList();

            foreach (var lottery in viewModel.Lotteries)
            {
                var numberOfGroup = lottery.Volume / CPLConstant.LotteryGroupSize;
                var groups = Enumerable.Repeat(0, numberOfGroup).ToArray();
                var groupSize = CPLConstant.LotteryGroupSize;

                for (var i = lottery.LotteryPrizes.Count - 1; i >= 0; i--)
                {
                    lottery.LotteryPrizes[i].Probability = Math.Round(((decimal)lottery.LotteryPrizes[i].Volume / (decimal)numberOfGroup) * (1m / (decimal)groupSize) * 100m, 4);
                    ProbabilityCalculate(ref groups, ref numberOfGroup, ref groupSize, lottery.LotteryPrizes[i].Volume);
                }

            }

            if (viewModel.Lotteries != null && viewModel.Lotteries[0] != null && viewModel.Lotteries[0].Volume > 0)
                viewModel.PrecentOfPerchasedTickets = ((decimal)viewModel.Lotteries[0].LotteryHistories.Count() / (decimal)viewModel.Lotteries[0].Volume * 100).ToString();

            return View(viewModel);
        }

        private void ProbabilityCalculate(ref int[] groups, ref int numberOfGroup, ref int groupSize, int numberOfGroupWasRemoved)
        {
            if (groups[0] == 0)
            {
                groups = Enumerable.Repeat(1, numberOfGroup).ToArray();
                groupSize--;
            }
            else
            {
                var counterMaximumExist = groups.Count(x => x == CPLConstant.MaximumNumberOfWinnerPerGroup);
                Enumerable.Repeat(CPLConstant.MaximumNumberOfWinnerPerGroup, numberOfGroupWasRemoved).ToArray().CopyTo(groups, counterMaximumExist);
                numberOfGroup -= numberOfGroupWasRemoved;
            }
        }

        public IActionResult GetConfirmPurchaseTicket(int amount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (user == null)
            {
                return new JsonResult(new
                {
                    success = true,
                    url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("LogIn", "Authentication")}?returnUrl={Url.Action("Index", "Lottery")}"
                });
            }

            var viewModel = new LotteryTicketPurchaseViewModel();

            viewModel.TicketPrice = 500;
            viewModel.TotalTickets = amount;
            viewModel.TotalPriceOfTickets = viewModel.TotalTickets * viewModel.TicketPrice;

            return PartialView("_PurchaseTicketConfirm", viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmPurchaseTicket(LotteryTicketPurchaseViewModel viewModel)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (user == null)
            {
                return new JsonResult(new
                {
                    success = true,
                    url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("LogIn", "Authentication")}?returnUrl={Url.Action("Index", "Lottery")}"
                });
            }
            else
            {
                var currentUser = _sysUserService.Query().Select().Where(x => x.Id == user.Id).FirstOrDefault();
                var lotteryId = _lotteryService
                                    .Query()
                                    .Include(x => x.LotteryHistories)
                                    .Include(x => x.LotteryPrizes)
                                    .Select()
                                    .Where(x => x.LotteryHistories.Count() < x.Volume && x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE))
                                    .Select(x => Mapper.Map<LotteryViewModel>(x))
                                    .LastOrDefault()?.Id;
                var lotteryRecordList = _lotteryHistoryService.Queryable().Where(x => x.LotteryId == lotteryId.Value).ToList();
                var lastTicketIndex = 0;

                if (lotteryRecordList.Count > 0)
                    lastTicketIndex = _lotteryHistoryService.Queryable().Where(x => x.LotteryId == lotteryId.Value).Max(x => x.TicketIndex);

                var totalPriceOfTickets = viewModel.TotalTickets * CPLConstant.LotteryTicketPrice;

                if(totalPriceOfTickets <= currentUser.TokenAmount)
                {
                    /// Example paramsInJson: {"1":{"uint32":"4"},"2":{"address":"0xB43eA1802458754A122d02418Fe71326030C6412"}, "3": {"uint32[]":"[1, 2, 3]"}}
                    var userAddress = user.ETHHDWalletAddress;
                    var ticketIndexList = new List<int>();
                    var lotteryPhase = _lotteryService.Queryable().Where(x => x.Id == lotteryId).FirstOrDefault().Phase;

                    for (int i = 0; i < viewModel.TotalTickets; i++)
                    {
                        lastTicketIndex += 1;
                        ticketIndexList.Add(lastTicketIndex);
                    }
                    var paramJson = string.Format(CPLConstant.RandomParamInJson, lotteryPhase, userAddress, string.Join(",", ticketIndexList.ToArray()));

                    var buyTime = DateTime.Now;
                    var ticketGenResult = ServiceClient.ETokenClient.CallTransactionAsync(Authentication.Token, CPLConstant.OwnerAddress, CPLConstant.OwnerPassword, "random", CPLConstant.GasPriceMultiplicator, CPLConstant.DurationInSecond, paramJson);
                    ticketGenResult.Wait();

                    if(ticketGenResult.Result.Status.Code == 0)
                    {
                        for (int i = 0; i < viewModel.TotalTickets; i++)
                        {
                            var lotteryRecord = new LotteryHistory
                            {
                                CreatedDate = buyTime,
                                LotteryId = lotteryId.Value,
                                SysUserId = user.Id,
                                TicketIndex = ticketIndexList[i],
                                TxHashId = ticketGenResult.Result.TxId
                            };

                            _lotteryHistoryService.Insert(lotteryRecord);
                        }

                        currentUser.TokenAmount -= totalPriceOfTickets;
                        _sysUserService.Update(currentUser);

                        _unitOfWork.SaveChanges();
                        return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PurchaseSuccessfully") });
                    }
                    else
                        return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PurchaseFailed") });

                }
                else
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });
            }
        }

        public JsonResult SearchLotteryHistory(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchLotteryHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<LotteryHistoryViewModel> SearchLotteryHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
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

            totalResultsCount = _lotteryHistoryService
                                 .Query()
                                 .Include(x => x.Lottery)
                                 .Include(x => x.LotteryPrize)
                                 .Select()
                                 .Where(x => x.SysUserId == user.Id)
                                 .Count();

            // search the dbase taking into consideration table sorting and paging
            var lotteryHistory = _lotteryHistoryService
                                          .Query()
                                          .Include(x => x.Lottery)
                                          .Include(x => x.LotteryPrize)
                                          .Select()
                                          .Where(x => x.SysUserId == user.Id)
                                          .Select(x => new LotteryHistoryViewModel
                                          {
                                              CreatedDate = x.CreatedDate,
                                              CreatedDateInString = x.CreatedDate.ToString("yyyy/MM/dd hh:mm:ss"),
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
}