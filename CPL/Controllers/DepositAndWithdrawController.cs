using AutoMapper;
using CPL.Common.Enums;
using CPL.Common.Misc;
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
using ZXing;

namespace CPL.Controllers
{
    public class DepositAndWithdrawController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ITemplateService _templateService;
        private readonly ISysUserService _sysUserService;
        private readonly ICoinTransactionService _coinTransactionService;
        private readonly IETHTransactionService _ethTransactionService;
        private readonly IBTCTransactionService _btcTransactionService;

        public DepositAndWithdrawController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITemplateService templateService,
            ICoinTransactionService coinTransactionService,
            IETHTransactionService ethTransactionService,
            IBTCTransactionService btcTransactionService,
            ISysUserService sysUserService)
        {
            this._langService = langService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._templateService = templateService;
            this._coinTransactionService = coinTransactionService;
            this._ethTransactionService = ethTransactionService;
            this._btcTransactionService = btcTransactionService;
            this._sysUserService = sysUserService;
        }

        [Permission(EnumRole.User)]
        public IActionResult Index()
        {
            var viewModel = new DepositAndWithdrawIndexViewModel();
            viewModel.SysUserId = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id;
            return View(viewModel);
        }

        [Permission(EnumRole.User)]
        public IActionResult GetRequireProfile(ConfirmExchangeViewModel viewModel)
        {
            return PartialView("_RequireProfile", viewModel);
        }

        [Permission(EnumRole.User)]
        public IActionResult GetRequireKYC(ConfirmExchangeViewModel viewModel)
        {
            return PartialView("_RequireKYC", viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DoWithdraw(WithdrawViewModel viewModel, MobileModel mobileModel)
        {
            if (viewModel.Amount <= 0)
            {
                if (mobileModel.IsMobile)
                {
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.WARNING,
                        name = "amount",
                        error_message_key = CPLConstant.MobileAppConstant.DepositAndWithdrawScreenInvalidWithdrawAmount
                    });
                }
                return new JsonResult(new { success = false, name = "amount", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidWithdrawAmount") });
            }

            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);

            var txHashId = "";

            if (string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName)
                || !user.DOB.HasValue || string.IsNullOrEmpty(user.Country) || string.IsNullOrEmpty(user.City) || string.IsNullOrEmpty(user.StreetAddress)
                || string.IsNullOrEmpty(user.Mobile))

            {
                if (mobileModel.IsMobile)
                {
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.WARNING,
                        require_profile = true
                    });
                }

                return new JsonResult(new
                {
                    success = false,
                    requireProfile = false
                });
            }
                

            if (user.KYCVerified == null || !user.KYCVerified.Value)
            {
                if (mobileModel.IsMobile)
                {
                    return new JsonResult(new
                    {
                        code = EnumResponseStatus.WARNING,
                        require_kyc = true
                    });
                }

                return new JsonResult(new
                {
                    success = false,
                    requireKyc = false
                });
            }

            if (viewModel.Currency == EnumCurrency.BTC.ToString())
            {
                try
                {
                    // Validate max BTC Amount
                    var btcToTokenRate = decimal.Parse(_settingService.Queryable().FirstOrDefault(x => x.Name == CPLConstant.BTCToTokenRate).Value);
                    var availableBTCAmount = user.TokenAmount / btcToTokenRate;

                    if (viewModel.Amount > availableBTCAmount)
                    {
                        if (mobileModel.IsMobile)
                        {
                            return new JsonResult(new
                            {
                                code = EnumResponseStatus.WARNING,
                                name = "amount",
                                error_message_key = CPLConstant.MobileAppConstant.DepositAndWithdrawScreenInsufficientFunds
                            });
                        }
                        return new JsonResult(new { success = false, name = "amount", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });
                    }

                    //Validate BTC wallet address
                    if (string.IsNullOrEmpty(viewModel.Address) || (!string.IsNullOrEmpty(viewModel.Address) && !ValidateAddressHelper.IsValidBTCAddress(viewModel.Address)))
                    {
                        if (mobileModel.IsMobile)
                        {
                            return new JsonResult(new
                            {
                                code = EnumResponseStatus.WARNING,
                                name = "wallet",
                                error_message_key = CPLConstant.MobileAppConstant.DepositAndWithdrawScreenInvalidBTCAddress
                            });
                        }
                        return new JsonResult(new { success = false, name = "wallet", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidBTCAddress") });
                    }

                    // Transfer
                    var txHashIdTask = ServiceClient.BAccountClient.TransferAsync(Authentication.Token, CPLConstant.BTCWithdrawPrivateKey, viewModel.Address, viewModel.Amount);
                    txHashIdTask.Wait();
                    txHashId = txHashIdTask.Result.TxId;

                    // Save to DB
                    if (txHashId != null)
                    {
                        _coinTransactionService.Insert(new CoinTransaction()
                        {
                            SysUserId = user.Id,
                            FromWalletAddress = CPLConstant.BTCWithdrawAddress,
                            ToWalletAddress = viewModel.Address,
                            CoinAmount = viewModel.Amount,
                            CreatedDate = DateTime.Now,
                            CurrencyId = (int)EnumCurrency.BTC,
                            Status = EnumCoinTransactionStatus.PENDING.ToBoolean(),
                            TxHashId = txHashId,
                            Type = (int)EnumCoinTransactionType.WITHDRAW_BTC
                        });

                        user.TokenAmount -= viewModel.Amount * btcToTokenRate;
                        _sysUserService.Update(user);
                        _unitOfWork.SaveChanges();

                    }
                    else
                    {
                        if (mobileModel.IsMobile)
                        {
                            return new JsonResult(new
                            {
                                code = EnumResponseStatus.WARNING,
                                error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs
                            });
                        }

                        return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                    }
                }
                catch (Exception ex)
                {
                    if (mobileModel.IsMobile)
                    {
                        return new JsonResult(new
                        {
                            code = EnumResponseStatus.WARNING,
                            error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs
                        });
                    }
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }
            }

            if (mobileModel.IsMobile)
            {
                return new JsonResult(new
                {
                    code = EnumResponseStatus.SUCCESS,
                    success_message_key = CPLConstant.MobileAppConstant.DepositAndWithdrawScreenWithdrawedSuccessfully,
                    token = user.TokenAmount,
                    profile_kyc = true,
                    txhashid = txHashId
                });
            }
            return new JsonResult(new { success = true, token = user.TokenAmount.ToString("N0"), profileKyc = true, txhashid = txHashId, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "WithdrawedSuccessfully") });
        }

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult DecodeQR(DecodeQrViewModel viewModel)
        {
            System.DrawingCore.Bitmap bitmap = new System.DrawingCore.Bitmap(viewModel.FormFile.OpenReadStream());
            try
            {
                BarcodeReader reader = new BarcodeReader { AutoRotate = true, TryInverted = true };
                string qrcode = reader.Decode(bitmap).Text;
                return new JsonResult(new { success = true, address = qrcode, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "GeneratedQRCodeSuccessfully") });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "GeneratedQRCodeError") });
            }
        }
    }
}
