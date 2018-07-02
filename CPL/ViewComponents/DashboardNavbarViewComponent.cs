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
    public class DashboardNavbarViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;

        public DashboardNavbarViewComponent(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            return View(AutoMapper.Mapper.Map< DashboardNavbarViewModel>(user));
        }
    }
}
