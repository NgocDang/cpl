using Microsoft.AspNetCore.Mvc;
using CPL.Core.Interfaces;
using AutoMapper;
using CPL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using CPL.Models;
using System;
using CPL.Misc.Enums;
using System.Linq;
using CPL.Misc.Utils;
using CPL.Domain;
using CPL.Misc;

namespace CPL.Controllers
{
    public class ContactController : Controller
    {
        private readonly ILangService _langService;
        private readonly ISysUserService _sysUserService;
        private readonly ISettingService _settingService;
        private readonly IContactService _contactService;
        private readonly IViewRenderService _viewRenderService;
        private readonly ITemplateService _templateService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public ContactController(ILangService langService, IContactService contactService, IViewRenderService viewRenderService,
            ISettingService settingService, ISysUserService sysUserService, ITemplateService templateService,
            IUnitOfWorkAsync unitOfWork)
        {
            this._langService = langService;
            this._contactService = contactService;
            this._settingService = settingService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var viewModel = new ContactIndexViewModel();
            viewModel.SysUser = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Send(ContactIndexViewModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                try
                {
                    var contact = Mapper.Map<Contact>(viewModel);
                    // Try to create a user with the given identity
                    _contactService.Insert(contact);
                    _unitOfWork.SaveChanges();
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Contact.ToString());

                    var contactViewModel = Mapper.Map<ContactEmailTemplateViewModel>(viewModel);

                    contactViewModel.ContactfullText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "MessageFromCustomer");
                    contactViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Home/_ContactEmailTemplate.cshtml", contactViewModel).Result;

                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), ETNConstant.SMTP.Contact);
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "EmailSentSuccessfully") });
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }

            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }
    }
}
