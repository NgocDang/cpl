﻿using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CPL.ViewComponents
{
    public class LotteryResultViewComponent: ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ISettingService _settingService;
        private readonly ISysUserService _sysUserService;

        public LotteryResultViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._settingService = settingService;
            this._sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            if (HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser") != null)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
                var viewModel = Mapper.Map<LotteryResultViewModel>(user);
                var ethToBTCRate = CoinExchangeExtension.CoinExchanging();
                viewModel.ETHToTokenRate = (1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value)) / ethToBTCRate;
                viewModel.BTCToTokenRate = 1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value);
                return View(viewModel);
            } else
            {
                return Content(string.Empty);
            }
        }
    }
}