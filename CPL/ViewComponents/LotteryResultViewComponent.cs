using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CPL.ViewComponents
{
    public class LotteryResultViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ISettingService _settingService;
        private readonly ISysUserService _sysUserService;
        private readonly ILotteryHistoryService _lotteryHistoryService;

        public LotteryResultViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService,
            ILotteryHistoryService lotteryHistoryService)
        {
            this._mapper = mapper;
            this._settingService = settingService;
            this._sysUserService = sysUserService;
            this._lotteryHistoryService = lotteryHistoryService;
        }

        public IViewComponentResult Invoke(int? lotteryId)
        {
            if (HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser") != null)
            {
                var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
                var viewModel = Mapper.Map<LotteryResultViewModel>(user);
                var ethToBTCRate = CoinExchangeExtension.CoinExchanging();
                viewModel.ETHToTokenRate = (1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value)) / ethToBTCRate;
                viewModel.BTCToTokenRate = 1 / decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value);

                // Get lottery result
                var status = _lotteryHistoryService
                    .Queryable()
                    .Where(x => x.SysUserId == user.Id)
                    .Any();

                if (status)
                {
                    var lotteryHistory = _lotteryHistoryService
                        .Query()
                        .Include(x => x.LotteryPrize)
                        .Select()
                        .Where(x => x.SysUserId == user.Id && x.LotteryPrizeId.HasValue)
                        .OrderByDescending(x => x.LotteryPrize.Value)
                        .FirstOrDefault();

                    viewModel.Status = lotteryHistory != null ? true : false;
                    viewModel.Result = lotteryHistory?.LotteryPrize.Index.ToString();
                }

                return View(viewModel);
            }
            else
            {
                return Content(string.Empty);
            }
        }
    }
}
