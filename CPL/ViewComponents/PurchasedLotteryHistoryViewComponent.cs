using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CPL.Common.Enums.CPLConstant;

namespace CPL.ViewComponents
{
    public class PurchasedLotteryHistoryViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? lotteryCategoryId)
        {
            return View();
        }
    }
}
