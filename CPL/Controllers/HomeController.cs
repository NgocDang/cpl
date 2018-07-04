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
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel viewModel)
        {
            var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Contact.ToString());
            var contactEmailTemplateViewModel = new ContactEmailTemplateViewModel();
            contactEmailTemplateViewModel.Name = viewModel.Name;
            contactEmailTemplateViewModel.Message = viewModel.Message;
            contactEmailTemplateViewModel.Email = viewModel.Email;
            contactEmailTemplateViewModel.Subject = template.Subject;
            template.Body = _viewRenderService.RenderToStringAsync("/Views/Home/_ContactEmailTemplate.cshtml", contactEmailTemplateViewModel).Result;
            EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), CPLConstant.AdminEmail);
            return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactEmailSent") });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
