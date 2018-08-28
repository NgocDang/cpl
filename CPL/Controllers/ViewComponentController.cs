using CPL.Misc;
using CPL.Misc.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class ViewComponentController : Controller
    {
        [Permission(EnumRole.User)]
        public IActionResult GetExchangeViewComponent()
        {
            return ViewComponent("Exchange");
        }

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
    }
}
