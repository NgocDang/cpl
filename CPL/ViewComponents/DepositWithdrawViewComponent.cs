using AutoMapper;
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

        public DepositWithdrawViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var model = Mapper.Map<DepositAndWithdrawViewModel>(user);
            return View(model);
        }
    }
}
