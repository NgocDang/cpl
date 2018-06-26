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

namespace CPL.Controllers
{
    [Permission(EnumRole.Guest)]
    public class HomeController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;

        private readonly ITeamService _teamService;
        private readonly ITemplateService _templateService;

        public HomeController(
            ILangService langService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ITeamService teamService,
            ITemplateService templateService)
        {
            this._langService = langService;
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

            //Team section
            viewModel.Teams = _teamService.Queryable()
                .Select(x => Mapper.Map<TeamViewModel>(x)).ToList();

            //Languages
            viewModel.Langs = _langService.Queryable()
                .Select(x => Mapper.Map<LangViewModel>(x))
                .ToList();

            viewModel.Lang = viewModel.Langs.FirstOrDefault(x => x.Id == HttpContext.Session.GetInt32("LangId").Value);

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
