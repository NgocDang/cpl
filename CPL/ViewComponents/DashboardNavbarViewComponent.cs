using CPL.Common.Enums;
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
    public class DashboardNavbarViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;
        private readonly IPricePredictionHistoryService _pricePredictionHistoryService;
        private readonly ILotteryHistoryService _lotteryHistoryService;

        public DashboardNavbarViewComponent(ISysUserService sysUserService, IPricePredictionHistoryService pricePredictionHistoryService, ILotteryHistoryService lotteryHistoryService)
        {
            _sysUserService = sysUserService;
            _pricePredictionHistoryService = pricePredictionHistoryService;
            _lotteryHistoryService = lotteryHistoryService;
        }

        public IViewComponentResult Invoke()
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var viewmodel = AutoMapper.Mapper.Map<DashboardNavbarViewModel>(user);
            if (user is null)
                return View(viewmodel);
            var lotteryRecords = _lotteryHistoryService.Queryable().Where(x => x.SysUserId == user.Id && x.Result == EnumGameResult.WIN.ToString() && x.UpdatedDate.HasValue && x.UpdatedDate.Value.AddDays(7) >= DateTime.Now).ToList();
            var pricePredictionRecords = _pricePredictionHistoryService.Queryable().Where(x => x.SysUserId == user.Id && x.Result == EnumGameResult.WIN.ToString() && x.UpdatedDate.HasValue && x.UpdatedDate.Value.AddDays(7) >= DateTime.Now).ToList();
            if (lotteryRecords.Count > 0 || pricePredictionRecords.Count > 0)
                viewmodel.NotificationStatus = true;
            if (user.KYCVerified.HasValue)
                viewmodel.KYCStatus = user.KYCVerified.HasValue ? (user.KYCVerified.Value == true ? true : false) : false; 
            return View(viewmodel);
        }
    }
}
