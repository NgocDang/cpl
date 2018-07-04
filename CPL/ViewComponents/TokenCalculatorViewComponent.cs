using AutoMapper;
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
    public class TokenCalculatorViewComponent: ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ISettingService _settingService;
        private readonly ISysUserService _sysUserService;

        public TokenCalculatorViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._settingService = settingService;
            this._sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = new TokenCalculatorViewModel(); ;
            var ethToBTCRate = CoinExchangeExtension.CoinExchanging();
            viewModel.ETHToTokenRate = (decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value)) * ethToBTCRate;
            viewModel.BTCToTokenRate = 1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value);
            return View(viewModel);
        }
    }
}
