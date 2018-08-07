using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class ExchangeViewComponent : ViewComponent
    {
        private readonly IMapper _mapper;
        private readonly ISysUserService _sysUserService;

        public ExchangeViewComponent(IMapper mapper,
            ISettingService settingService,
            ISysUserService sysUserService)
        {
            this._mapper = mapper;
            this._sysUserService = sysUserService;
        }

        public IViewComponentResult Invoke()
        {
            var user = _sysUserService.Queryable().FirstOrDefault(x => x.Id == HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser").Id && x.IsDeleted == false);
            var viewModel = Mapper.Map<ExchangeViewModel>(user);
            return View(viewModel);
        }
    }
}
