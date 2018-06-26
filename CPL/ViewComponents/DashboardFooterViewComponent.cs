using CPL.Core.Interfaces;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class DashboardFooterViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;

        public DashboardFooterViewComponent(ISysUserService sysUserService)
        {
            _sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new DashboardFooterViewModel();
            return View(viewModel);
        }
    }
}
