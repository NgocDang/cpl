using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly IGameHistoryService _gameHistoryService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;

        public DashboardController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            IGameHistoryService gameHistoryService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._gameHistoryService = gameHistoryService;
        }

        public IActionResult Index()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;
            return View(viewModel);
        }

        public IActionResult DepositeAndWithdrawal()
        {
            var viewModel = new DepositAndWithdrawViewModel();
            return View(viewModel);
        }

        public IActionResult WithdrawBTC()
        {
            return PartialView("_BtcOut");
        }

        [HttpPost]
        public IActionResult GetDataPieChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);
            decimal coinRate = CoinExchangeExtension.CoinExchanging();
            var tokenRate = _settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value;
            viewModel.TotalBalance = user.ETHAmount * coinRate + user.TokenAmount / decimal.Parse(tokenRate) + user.BTCAmount;

            // Holding Percentage
            viewModel.HoldingPercentage = new HoldingPercentageViewModel();

            if (user.TokenAmount > 0)
            {
                viewModel.HoldingPercentage.CPLPercentage = user.TokenAmount / decimal.Parse(tokenRate) / viewModel.TotalBalance * 100;
            }
            if (user.TokenAmount > 0)
            {
                viewModel.HoldingPercentage.ETHPercentage = user.ETHAmount * coinRate / viewModel.TotalBalance * 100;
            }
            if (user.BTCAmount > 0)
            {
                viewModel.HoldingPercentage.BTCPercentage = user.BTCAmount / viewModel.TotalBalance * 100;
            }

            var mess = JsonConvert.SerializeObject(viewModel.HoldingPercentage, Formatting.Indented);
            return new JsonResult(new { success = true, message = mess });
        }

        [HttpPost]
        public IActionResult GetDataLineChart()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            var viewModel = Mapper.Map<DashboardViewModel>(user);

            viewModel.MonthlyInvest = _gameHistoryService.Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => x.Amount) })
                        .ToList();
            viewModel.MonthlyInvest.Reverse();

            viewModel.AssetChange = _gameHistoryService.Queryable()
                        .Where(x => x.SysUserId == user.Id && x.Result.HasValue)
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => (x.Award.Value - x.Amount)) })
                        .ToList();
            viewModel.AssetChange.Reverse();

            viewModel.BonusChange = _gameHistoryService.Queryable()
                        .Where(x => x.SysUserId == user.Id && x.Result.HasValue)
                        .GroupBy(x => x.CreatedDate.Day)
                        .Select(y => new WalletChangeViewModel { Date = y.Select(x => x.CreatedDate.ToString("yyyy-MM-dd")).FirstOrDefault(), Amount = y.Sum(x => x.Award.Value) })
                        .ToList();
            viewModel.BonusChange.Reverse();


            var mess = JsonConvert.SerializeObject(viewModel, Formatting.Indented);
            return new JsonResult(new { success = true, message = mess });
        }

    }
}
