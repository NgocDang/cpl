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

            //TODO: Should load data from current lottery game in Lottery and LotteryPrize tables
            viewModel.Lotteries = _lotteryService
                .Query()
                .Include(x => x.LotteryHistories)
                .Include(x => x.LotteryPrizes)
                .Select()
                .Where(x => x.LotteryHistories.Count() < x.Volume)
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

            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PurchaseSuccessfully") });
        }
    }
}