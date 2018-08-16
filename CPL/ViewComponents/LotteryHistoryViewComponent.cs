using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CPL.ViewComponents
{
    public class LotteryHistoryViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;

        public LotteryHistoryViewComponent(ISysUserService sysUserService) {
            this._sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke(LotteryHistoryViewComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
