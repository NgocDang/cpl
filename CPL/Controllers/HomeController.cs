using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CPL.Models;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Core.Interfaces;
using AutoMapper;
using CPL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using CPL.Common.Enums;

namespace CPL.Controllers
{
    [Permission(EnumRole.Guest)]
    public class HomeController : Controller
    {
        private readonly ILangService _langService;
        private readonly ILangDetailService _langDetailService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;

        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;

        public HomeController(
            ILangService langService,
            ILangDetailService langDetailService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService)
        {
            this._langService = langService;
            this._langDetailService = langDetailService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._teamService = teamService;
            this._templateService = templateService;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.GetInt32("LangId").HasValue)
                HttpContext.Session.SetInt32("LangId", (int)EnumLang.ENGLISH);
            var viewModel = new HomeViewModel();
            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void UpdateLangDetail()
        {
            LangDetailHelper.LangDetails = _langDetailService.Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
        }

        public IActionResult LoadHeaderViewComponent()
        {
            return ViewComponent("Header");
        }

    }
}