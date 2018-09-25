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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPL.Controllers
{
    public class LotteryController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
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
            ITemplateService templateService,
            ISysUserService sysUserService,
            ILotteryService lotteryService,
            ILotteryPrizeService lotteryPrizeService,
            ILotteryHistoryService lotteryHistoryService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._lotteryService = lotteryService;
            this._lotteryPrizeService = lotteryPrizeService;
            this._lotteryHistoryService = lotteryHistoryService;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                var lottery = _lotteryService.Query()
                                .Include(x => x.LotteryDetails)
                                .Include(x => x.LotteryHistories)
                                //.Include(x => x.LotteryPrizes)
                                .FirstOrDefault(x => x.Id == id && !x.IsDeleted && (x.Status == (int)EnumLotteryGameStatus.ACTIVE || x.Status == (int)EnumLotteryGameStatus.DEACTIVATED));
                if (lottery == null)
                    return RedirectToAction("Index", "Home");

                var viewModel = Mapper.Map<LotteryIndexViewModel>(lottery);
                var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
                if (user != null)
                    viewModel.SysUserId = user.Id;

                // DO NOT DELETE! THIS BLOCK OF CODE TO CALCULATE THE WINNING POSSIBILITY
                //foreach (var lottery in viewModel.Lotteries)
                //{
                //    var numberOfGroup = lottery.Volume / CPLConstant.LotteryGroupSize;
                //    var groups = Enumerable.Repeat(0, numberOfGroup).ToArray();
                //    var groupSize = CPLConstant.LotteryGroupSize;

                //    for (var i = lottery.LotteryPrizes.Count - 1; i >= 0; i--)
                //    {
                //        lottery.LotteryPrizes[i].Probability = Math.Round(((decimal)lottery.LotteryPrizes[i].Volume / (decimal)numberOfGroup) * (1m / (decimal)groupSize) * 100m, 4);
                //        ProbabilityCalculate(ref groups, ref numberOfGroup, ref groupSize, lottery.LotteryPrizes[i].Volume);
                //    }
                //}

                if (viewModel.Volume > 0)
                    viewModel.PrecentOfPerchasedTickets = ((decimal)viewModel.LotteryHistories.Count() / (decimal)viewModel.Volume * 100m).ToString();

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

        [Permission(EnumRole.Guest)]
        public IActionResult GetConfirmPurchaseTicket(int amount, int lotteryId)
        {
            var viewModel = new LotteryTicketPurchaseViewModel();

            viewModel.TicketPrice = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == lotteryId).UnitPrice;
            viewModel.TotalTickets = amount;
            viewModel.TotalPriceOfTickets = viewModel.TotalTickets * viewModel.TicketPrice;

            return new JsonResult(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult ConfirmPurchaseTicket(LotteryTicketPurchaseViewModel viewModel, MobileModel mobileModel)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            if (user == null && !mobileModel.IsMobile)
            {
                var loginViewModel = new AccountLoginModel();

                var gcaptchaKey = _settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.GCaptchaKey)?.Value;
                loginViewModel.GCaptchaKey = gcaptchaKey;
                return PartialView("_Login", loginViewModel);
            }
            else
            {
                try
                {
                    int userId;
                    if (mobileModel.IsMobile)
                    {
                        if (mobileModel.MobileUserId.HasValue)
                        {
                            userId = mobileModel.MobileUserId.Value;
                        }
                        else
                        {
                            return new JsonResult(new
                            {
                                code = EnumResponseStatus.ERROR,
                                error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs
                            });
                        }
                    }
                    else
                    {
                        userId = user.Id;
                    }

                    var currentUser = _sysUserService.Queryable().Where(x => x.Id == userId).FirstOrDefault();
                    
                    var lotteryId = viewModel.LotteryId;

                    var lotteryRecordList = _lotteryHistoryService.Queryable().Where(x => x.LotteryId == lotteryId.Value).ToList();
                    var lastTicketIndex = 0;

                    if (lotteryRecordList.Count > 0)
                        lastTicketIndex = _lotteryHistoryService.Queryable().Where(x => x.LotteryId == lotteryId.Value).Max(x => x.TicketIndex);

                    var unitPrice = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == lotteryId.Value).UnitPrice;

                    var totalPriceOfTickets = viewModel.TotalTickets * unitPrice;

                    var currentLottery = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == lotteryId);

                    if (viewModel.TotalTickets <= currentLottery.Volume - lotteryRecordList.Count())
                    {
                        if (totalPriceOfTickets <= currentUser.TokenAmount)
                        {
                            /// Example paramsInJson: {"1":{"uint32":"4"},"2":{"address":"0xB43eA1802458754A122d02418Fe71326030C6412"}, "3": {"uint32[]":"[1, 2, 3]"}}
                            var userAddress = user.ETHHDWalletAddress;
                            var ticketIndexList = new List<int>[viewModel.TotalTickets / 10 + 1];
                            var lotteryPhase = _lotteryService.Queryable().FirstOrDefault(x => !x.IsDeleted && x.Id == lotteryId).Phase;

                            var listIndex = 0;
                            ticketIndexList[listIndex] = new List<int>();

                            for (int i = 0; i < viewModel.TotalTickets; i++)
                            {
                                if (i % 10 == 0 && i != 0)
                                {
                                    listIndex++;
                                    ticketIndexList[listIndex] = new List<int>();
                                }
                                lastTicketIndex += 1;
                                ticketIndexList[listIndex].Add(lastTicketIndex);
                            }

                            var totalOfTicketSuccessful = 0;

                            var buyTime = DateTime.Now;
                            foreach (var ticket in ticketIndexList)
                            {
                                if (ticket == null) continue;

                                var paramJson = string.Format(CPLConstant.RandomParamInJson, lotteryPhase, userAddress, string.Join(",", ticket.ToArray()));

                                var ticketGenResult = ServiceClient.ETokenClient.CallTransactionAsync(Authentication.Token, CPLConstant.OwnerAddress, CPLConstant.OwnerPassword, "random", CPLConstant.GasPriceMultiplicator, CPLConstant.DurationInSecond, paramJson);
                                ticketGenResult.Wait();

                                if (ticketGenResult.Result.Status.Code == 0)
                                {
                                    for (int i = 0; i < ticket.Count; i++)
                                    {
                                        var lotteryRecord = new LotteryHistory
                                        {
                                            CreatedDate = buyTime,
                                            LotteryId = lotteryId.Value,
                                            SysUserId = user.Id,
                                            TicketIndex = ticket[i],
                                            TxHashId = ticketGenResult.Result.TxId
                                        };

                                        _lotteryHistoryService.Insert(lotteryRecord);
                                        totalOfTicketSuccessful++;
                                    }
                                }
                            }
                            currentUser.TokenAmount -= totalOfTicketSuccessful * unitPrice;
                            _sysUserService.Update(currentUser);

                            _unitOfWork.SaveChanges();

                            if (mobileModel.IsMobile)
                            {
                                return new JsonResult(new
                                {
                                    code = EnumResponseStatus.SUCCESS,
                                    token = currentUser.TokenAmount.ToString("N0"),
                                    hintThankyou = string.Format(LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "HintThankYouLottery1"), totalOfTicketSuccessful),
                                    message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PurchaseSuccessfully")
                                });
                            }

                            return new JsonResult(new { success = true,
                                                        token = currentUser.TokenAmount.ToString("N0"),
                                                        hintThankyou = string.Format(LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "HintThankYouLottery1"), totalOfTicketSuccessful),
                                                        message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PurchaseSuccessfully") });
                        }
                        else
                        {
                            if (mobileModel.IsMobile)
                            {
                                return new JsonResult(new
                                {
                                    code = EnumResponseStatus.WARNING,
                                    error_message_key = CPLConstant.MobileAppConstant.LotteryDetailNotEnoughCPL
                                });
                            }

                            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NotEnoughCPL") });
                        }
                            
                    }
                    else
                    {
                        if (mobileModel.IsMobile)
                        {
                            return new JsonResult(new
                            {
                                code = EnumResponseStatus.WARNING,
                                error_message_key = CPLConstant.MobileAppConstant.LotteryDetailNoTicketsLeft
                            });
                        }

                        return new JsonResult(new { success = false, message = string.Format(LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "NoTicketsLeft"), currentLottery.Volume - lotteryRecordList.Count()) });
                    }
                }
                catch (Exception ex)
                {
                    if (mobileModel.IsMobile)
                    {
                        return new JsonResult(new
                        {
                            code = EnumResponseStatus.ERROR,
                            error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs,
                            error_message = ex.Message
                        });
                    }
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }
            }
        }
    }
}