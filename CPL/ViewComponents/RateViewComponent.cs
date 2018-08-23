using AutoMapper;
using CPL.Common.BTCRateHelper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class RateViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;
        private readonly ISettingService _settingService;

        public RateViewComponent(ISysUserService sysUserService, 
            ISettingService settingService
            )
        {
            this._sysUserService = sysUserService;
            this._settingService = settingService;
        }

        public IViewComponentResult Invoke(int? sysUserId)
        {
            int userId = sysUserId ?? HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == userId);
            var viewModel = Mapper.Map<RateViewModel>(user);

            var ethToBTCRate = BTCRateHelper.GetBTCRate(EnumCurrenciesPair.ETHBTC.ToString()).Value;
            viewModel.ETHToTokenRate = (1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value)) / ethToBTCRate;
            viewModel.BTCToTokenRate = 1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value);

            viewModel.SysUserId = user.Id;

            return View(viewModel);
        }
    }
}
