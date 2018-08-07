using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
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
    [Permission(EnumRole.User)]
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
            return View();
        }

        [HttpPost]
        public IActionResult DoDepositWithdraw(WithdrawViewModel viewModel)
        {
            if (viewModel.Amount <= 0)
                return new JsonResult(new { success = false, name = "amount", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidWithdrawAmount") });

            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            if (viewModel.Currency == EnumCurrency.BTC.ToString())
            {
                // Validate max BTC Amount
                if (viewModel.Amount > user.BTCAmount)
                    return new JsonResult(new { success = false, name = "amount", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });

                //Validate BTC wallet address
                if (string.IsNullOrEmpty(viewModel.Address) || (!string.IsNullOrEmpty(viewModel.Address) && !ValidateAddressHelper.IsValidBTCAddress(viewModel.Address)))
                    return new JsonResult(new { success = false, name = "wallet", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidBTCAddress") });

                // Save to DB
                user.BTCAmount -= viewModel.Amount;
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
            }
            else if (viewModel.Currency == EnumCurrency.ETH.ToString())
            {
                // Validate max ETH Amount
                if (viewModel.Amount > user.ETHAmount)
                    return new JsonResult(new { success = false, name = "amount", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InsufficientFunds") });

                //Validate ETH wallet address
                if (string.IsNullOrEmpty(viewModel.Address) || (!string.IsNullOrEmpty(viewModel.Address) && !ValidateAddressHelper.IsValidETHAddress(viewModel.Address)))
                    return new JsonResult(new { success = false, name = "wallet", message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "InvalidETHAddress") });

                // Save to DB
                user.ETHAmount -= viewModel.Amount;
                _sysUserService.Update(user);
                _unitOfWork.SaveChanges();
            }

            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "WithdrawedSuccessfully") });
        }

        [HttpPost]
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

        public IActionResult LoadDepositWithdrawViewComponent()
        {
            return ViewComponent("DepositWithdraw");
        }
    }
}
