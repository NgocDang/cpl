using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class ViewComponentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISysUserService _sysUserService;

        public ViewComponentController(
            IMapper mapper,
            IUnitOfWorkAsync unitOfWork,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._sysUserService = sysUserService;
            this._unitOfWork = unitOfWork;
        }

        //[Permission(EnumRole.User)]
        //public IActionResult GetExchangeViewComponent()
        //{
        //    return ViewComponent("Exchange");
        //}

        [Permission(EnumRole.User)]
        public IActionResult GetRateViewComponent()
        {
            return ViewComponent("Rate");
        }

        [Permission(EnumRole.User)]
        public IActionResult GetDepositWithdrawViewComponent()
        {
            return ViewComponent("DepositWithdraw");
        }

        [Permission(EnumRole.Guest)]
        public IActionResult GetPricePredictionViewComponent(int id)
        {
            var viewModel = new PricePredictionViewComponentViewModel();
            viewModel.Id = id;
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser")?.Id;
            if (viewModel.SysUserId.HasValue)
            {
                viewModel.TokenAmount = _sysUserService.Queryable().FirstOrDefault(x => x.Id == viewModel.SysUserId).TokenAmount;
            }
            return ViewComponent("PricePrediction", viewModel);
        }

        [Permission(EnumRole.Admin)]
        public IActionResult GetGameSummaryStatisticViewComponent(double periodInDay)
        {
            return ViewComponent("GameSummaryStatistic", periodInDay);
        }
    }
}
