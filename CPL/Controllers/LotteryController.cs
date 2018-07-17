﻿using AutoMapper;
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

        public LotteryController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
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
            this._gameHistoryService = gameHistoryService;
        }

        public IActionResult Index()
        {
            var viewModel = new LotteryViewModel();

            viewModel.TotalTicket = 5000;
            viewModel.TicketCollected = 2543;

            viewModel.FirstPrizeProbability = 0.1m;
            viewModel.SecondPrizeProbability = 5m;
            viewModel.ThirdPrizeProbability = 10m;
            viewModel.FourthPrizeProbability = 50m;

            viewModel.NumberOfTicketWinFirstPrize = 1;
            viewModel.NumberOfTicketWinSecondPrize = 5;
            viewModel.NumberOfTicketWinThirdPrize = 25;
            viewModel.NumberOfTicketWinFourthPrize = 500;
            
            return View(viewModel);
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