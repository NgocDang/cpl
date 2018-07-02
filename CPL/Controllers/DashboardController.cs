using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

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

        public JsonResult SearchGameHistory(DataTableAjaxPostModel viewModel)
        {
            // action inside a standard controller
            int filteredResultsCount;
            int totalResultsCount;
            var res = SearchGameHistoryFunc(viewModel, out filteredResultsCount, out totalResultsCount);
            return Json(new
            {
                // this is what datatables wants sending back
                draw = viewModel.draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = res
            });
        }

        public IList<GameHistoryViewModel> SearchGameHistoryFunc(DataTableAjaxPostModel model, out int filteredResultsCount, out int totalResultsCount)
        {
            var user = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            var searchBy = (model.search != null) ? model.search.value : null;
            var take = model.length;
            var skip = model.start;

            string sortBy = "";
            bool sortDir = true;

            if (model.order != null)
            {
                // in this example we just default sort on the 1st column
                sortBy = model.columns[model.order[0].column].data;
                sortDir = model.order[0].dir.ToLower() == "asc";
            }

            // search the dbase taking into consideration table sorting and paging
            if (string.IsNullOrEmpty(searchBy))
            {
                filteredResultsCount = _gameHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();

                totalResultsCount = _gameHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();

                return _gameHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .OrderBy("CreatedDate", false)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .ToList();
            }
            else
            {
                filteredResultsCount = _gameHistoryService.Query()
                        .Include(x => x.Game)
                        .Select()
                        .AsQueryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Where(x => x.Amount.ToString().Contains(searchBy) || x.Award.ToString().Contains(searchBy) || x.Game.Name.Contains(searchBy))
                        .Count();

                totalResultsCount = _gameHistoryService
                        .Queryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Count();

                return _gameHistoryService.Query()
                        .Include(x => x.Game)
                        .Select()
                        .AsQueryable()
                        .Where(x => x.SysUserId == user.Id)
                        .Where(x => x.Amount.ToString().Contains(searchBy) || x.Award.ToString().Contains(searchBy) || x.Game.Name.Contains(searchBy))
                        .Skip(skip)
                        .Take(take)
                        .Select(x => Mapper.Map<GameHistoryViewModel>(x))
                        .OrderBy(sortBy, sortDir)
                        .ToList();
            }
        }
    }
}