using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class DepositAndWithdrawController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;

        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;

        public DepositAndWithdrawController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
        }

        public IActionResult Index()
        {
            //var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            //DepositAndWithdrawViewModel model = new DepositAndWithdrawViewModel();
            return View();
        }

        [HttpPost]
        public IActionResult DepositeWithdrawBTC(DepositAndWithdrawViewModel viewModel)
        {
            //Validate ETH wallet address
            //if (string.IsNullOrEmpty(viewModel.EthAdderss) || (!string.IsNullOrEmpty(viewModel.EthAdderss) && !ValidateAddressHelper.IsValidETHAddress(viewModel.EthAdderss)))
            //    return new JsonResult(new { success = false, name = "eth-wallet", message = "Invalid ETH wallet address. Please try another." });

            //Validate BTC wallet address
            if (string.IsNullOrEmpty(viewModel.BtcAddress) || (!string.IsNullOrEmpty(viewModel.BtcAddress) && !ValidateAddressHelper.IsValidBTCAddress(viewModel.BtcAddress)))
                return new JsonResult(new { success = false, name = "btc-wallet", message = "Invalid BTC wallet address. Please try another." });

            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id);
            // Validate max BTC Amount
            //if (viewModel.EthAmount > user.ETHAmount)
            //    return new JsonResult(new { success = false, name = "eth-amount", message = "Insufficient money. Please try another." });

            // Validate max BTC Amount
            if (viewModel.BtcAmount > user.BTCAmount)
                return new JsonResult(new { success = false, name = "btc-amount", message = "Insufficient money. Please try another." });

            return new JsonResult(new { success = true, message = "success" });
        }

    }
}
