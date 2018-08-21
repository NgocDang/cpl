using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly ISysUserService _sysUserService;
        private readonly ILangService _langService;

        public HeaderViewComponent(ISysUserService sysUserService, ILangService langService, INewsService newsService)
        {
            _sysUserService = sysUserService;
            _langService = langService;
        }

        public IViewComponentResult Invoke()
        {
            var viewModel = new HeaderViewModel();
            viewModel.User = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");

            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            var langId = HttpContext.Session.GetInt32("LangId");

            if (HttpContext.Session.GetInt32("LangId").HasValue)
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);
            else
                viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == (int)EnumLang.ENGLISH);

            return View(viewModel);
        }
    }
}
