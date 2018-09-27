using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CPL.ViewComponents
{
    public class DepositWithdrawViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ISysUserService _sysUserService;
        private readonly ISettingService _settingService;

        public DepositWithdrawViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._sysUserService = sysUserService;
            this._settingService = settingService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<DepositAndWithdrawViewModel>(user);
            viewModel.BTCAvailable = viewModel.TokenAmount / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value);
            return View(viewModel);
        }
    }
}
