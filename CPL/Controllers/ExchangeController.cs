using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class ExchangeController : Controller
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

        public ExchangeController(
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
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<ExchangeViewModel>(user);
            viewModel.ETHToBTCRate = CoinExchangeExtension.CoinExchanging();
            viewModel.BTCToTokenrate = decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == "BTCToTokenRate").Value);
            return View(viewModel);
        }

        public IActionResult GetConfirm(ConfirmExchangeViewModel viewModel)
        {
            return PartialView("_Confirm",viewModel);
        }

        [HttpPost]
        public IActionResult Confirm(ConfirmExchangeViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (user != null)
            {
                if (viewModel.FromCurrency == "BTC")
                {
                    user.BTCAmount -= viewModel.FromAmount;
                    user.TokenAmount += viewModel.ToAmount;
                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = "Success!" });
                }
                else if (viewModel.FromCurrency == "ETH")
                {
                    user.ETHAmount -= viewModel.FromAmount;
                    user.TokenAmount += viewModel.ToAmount;
                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = "Success!" });
                }
                else
                {
                    if (viewModel.ToCurrency == "BTC")
                    {
                        user.BTCAmount += viewModel.ToAmount;
                        user.TokenAmount -= viewModel.FromAmount;
                        _sysUserService.Update(user);
                        _unitOfWork.SaveChanges();
                        return new JsonResult(new { success = true, message = "Success!" });
                    }
                    else
                    {
                        user.ETHAmount += viewModel.ToAmount;
                        user.TokenAmount -= viewModel.FromAmount;
                        _sysUserService.Update(user);
                        _unitOfWork.SaveChanges();
                        return new JsonResult(new { success = true, message = "Success!" });
                    }
                }
            }
            else
                return new JsonResult(new { success = true, message = "Success" });
        }
    }
}
