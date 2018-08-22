using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CPL.Controllers
{
    public class ExchangeController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ICoinTransactionService _coinTransactionService;

        public ExchangeController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService,
            ISysUserService sysUserService,
            ICoinTransactionService coinTransactionService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._coinTransactionService = coinTransactionService;
        }

        [Permission(EnumRole.User)]
        public IActionResult Index()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<ExchangeViewModel>(user);
            viewModel.ETHToBTCRate = CoinExchangeExtension.CoinExchanging();
            viewModel.BTCToTokenrate = decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value);
            return View(viewModel);
        }

        [Permission(EnumRole.User)]
        public IActionResult GetConfirm(ConfirmExchangeViewModel viewModel)
        {
            return PartialView("_Confirm", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult Confirm(ConfirmExchangeViewModel viewModel)
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var btcToTokenRate = float.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value);
            if (user != null)
            {
                if (viewModel.FromCurrency == EnumCurrency.BTC.ToString() && viewModel.FromAmount <= user.BTCAmount)
                {
                    user.BTCAmount -= viewModel.FromAmount;
                    var tokenAmount = viewModel.FromAmount * (decimal)btcToTokenRate;
                    user.TokenAmount += tokenAmount;

                    _coinTransactionService.Insert(new CoinTransaction()
                    {
                        SysUserId = user.Id,
                        CoinAmount = viewModel.FromAmount,
                        CreatedDate = DateTime.Now,
                        CurrencyId = (int)EnumCurrency.BTC,
                        TokenAmount = tokenAmount,
                        Rate = btcToTokenRate,
                        Status = EnumCoinTransactionStatus.SUCCESS.ToBoolean(),
                        Type = (int)EnumCoinTransactionType.EXCHANGE_BTC_TO_CPL
                    });

                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExchangedSuccessfully") });
                }
                else if (viewModel.FromCurrency == EnumCurrency.ETH.ToString() && viewModel.FromAmount <= user.ETHAmount)
                {
                    user.ETHAmount -= viewModel.FromAmount;
                    var tokenAmount = viewModel.FromAmount * CoinExchangeExtension.CoinExchanging() * (decimal)btcToTokenRate;
                    user.TokenAmount += tokenAmount;

                    _coinTransactionService.Insert(new CoinTransaction()
                    {
                        SysUserId = user.Id,
                        CoinAmount = viewModel.FromAmount,
                        CreatedDate = DateTime.Now,
                        CurrencyId = (int)EnumCurrency.ETH,
                        TokenAmount = tokenAmount,
                        Rate = btcToTokenRate * (float)CoinExchangeExtension.CoinExchanging(),
                        Status = EnumCoinTransactionStatus.SUCCESS.ToBoolean(),
                        Type = (int)EnumCoinTransactionType.EXCHANGE_ETH_TO_CPL
                    });

                    _sysUserService.Update(user);
                    _unitOfWork.SaveChanges();
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExchangedSuccessfully") });
                }
                else if (viewModel.FromCurrency == EnumCurrency.CPL.ToString() && viewModel.FromAmount <= user.TokenAmount)
                {
                    if (viewModel.ToCurrency == EnumCurrency.BTC.ToString())
                    {
                        user.TokenAmount -= viewModel.FromAmount;
                        var currencyAmount = viewModel.FromAmount / (decimal)btcToTokenRate;
                        user.BTCAmount += currencyAmount;

                        _coinTransactionService.Insert(new CoinTransaction()
                        {
                            SysUserId = user.Id,
                            CoinAmount = currencyAmount,
                            CreatedDate = DateTime.Now,
                            CurrencyId = (int)EnumCurrency.BTC,
                            TokenAmount = viewModel.FromAmount,
                            Rate = btcToTokenRate,
                            Status = EnumCoinTransactionStatus.SUCCESS.ToBoolean(),
                            Type = (int)EnumCoinTransactionType.EXCHANGE_CPL_TO_BTC
                        });

                        _sysUserService.Update(user);
                        _unitOfWork.SaveChanges();
                        return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExchangedSuccessfully") });
                    }
                    else
                    {
                        user.TokenAmount -= viewModel.FromAmount;
                        var currencyAmount = viewModel.FromAmount / ((decimal)btcToTokenRate * CoinExchangeExtension.CoinExchanging());
                        user.ETHAmount += currencyAmount;

                        _coinTransactionService.Insert(new CoinTransaction()
                        {
                            SysUserId = user.Id,
                            CoinAmount = currencyAmount,
                            CreatedDate = DateTime.Now,
                            CurrencyId = (int)EnumCurrency.ETH,
                            TokenAmount = viewModel.FromAmount,
                            Rate = btcToTokenRate * (float)CoinExchangeExtension.CoinExchanging(),
                            Status = EnumCoinTransactionStatus.SUCCESS.ToBoolean(),
                            Type = (int)EnumCoinTransactionType.EXCHANGE_CPL_TO_ETH
                        });

                        _sysUserService.Update(user);
                        _unitOfWork.SaveChanges();
                        return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ExchangedSuccessfully") });
                    }
                }
                else
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });
            }
            else
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [Permission(EnumRole.User)]
        public IActionResult LoadExchangeViewComponent()
        {
            return ViewComponent("Exchange");
        }

        [Permission(EnumRole.User)]
        public IActionResult LoadRateViewComponent()
        {
            return ViewComponent("Rate");
        }
    }
}
