using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    [Permission(EnumRole.Guest)]
    public class DashboardSidebarViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;

        public DashboardSidebarViewComponent(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            return View(AutoMapper.Mapper.Map<DashboardSidebarViewModel>(user));
        }
    }
}
