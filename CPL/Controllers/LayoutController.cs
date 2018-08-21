using Microsoft.AspNetCore.Mvc;
using CPL.Core.Interfaces;
using AutoMapper;
using CPL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using CPL.Misc.Enums;
using CPL.Misc;

namespace CPL.Controllers
{
    public class LayoutController : Controller
    {
        private readonly ILangService _langService;
        private readonly ISysUserService _sysUserService;
        private readonly ISettingService _settingService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public LayoutController(ILangService langService, 
            ISettingService settingService, ISysUserService sysUserService,
            IUnitOfWorkAsync unitOfWork)
        {
            this._langService = langService;
            this._settingService = settingService;
            this._sysUserService = sysUserService;
            this._unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult SwitchLang(int id)
        {
            HttpContext.Session.SetInt32("LangId", id);
            return new JsonResult(new { success = true });
        }
    }
}
