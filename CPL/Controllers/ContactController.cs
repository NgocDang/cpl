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
using CPL.Common.Enums;

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
            this._viewRenderService = viewRenderService;
            this._templateService = templateService;
            this._sysUserService = sysUserService;
            this._unitOfWork = unitOfWork;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            var viewModel = new ContactIndexViewModel();
            viewModel.SysUser = HttpContext.Session.GetObjectFromJson<SysUserViewModel>("CurrentUser");
            return View(viewModel);
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult DoSend(ContactIndexViewModel viewModel, MobileModel mobileModel)
        {
            // Ensure we have a valid viewModel to work with
            if (ModelState.IsValid)
            {
                try
                {
                    var contact = Mapper.Map<Contact>(viewModel);
                    contact.CreatedDate = DateTime.Now;
                    _contactService.Insert(contact);
                    _unitOfWork.SaveChanges();
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Contact.ToString());

                    var contactEmailTemplateViewModel = Mapper.Map<ContactEmailTemplateViewModel>(viewModel);
                    contactEmailTemplateViewModel.CategoryName = ((EnumContactCategory)viewModel.Category).ToString();
                    contactEmailTemplateViewModel.CategoryText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Category");
                    contactEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
                    contactEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
                    contactEmailTemplateViewModel.CPLTeamText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CPLTeam");
                    contactEmailTemplateViewModel.DescriptionText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Description");
                    contactEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
                    contactEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
                    contactEmailTemplateViewModel.MessageFromCustomerText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "MessageFromCustomer");
                    contactEmailTemplateViewModel.SubjectText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Subject");
                    contactEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
                    contactEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Contact/_ContactEmailTemplate.cshtml", contactEmailTemplateViewModel).Result;
                    template.Subject = string.Format(template.Subject, contact.Id);
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), CPLConstant.SMTP.Contact);

                    if (mobileModel.IsMobile)
                    {
                        return new JsonResult(new
                        {
                            code = EnumResponseStatus.SUCCESS,
                            success_message_key = CPLConstant.MobileAppConstant.ContactScreenEmailSentSuccessfully
                        });
                    }
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "MessageSentSuccessfully") });
                }
                catch (Exception ex)
                {
                    if (mobileModel.IsMobile)
                    {
                        return new JsonResult(new
                        {
                            code = EnumResponseStatus.ERROR,
                            error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs,
                            error_message = ex.Message
                        });
                    }
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }
            }

            if (mobileModel.IsMobile)
            {
                return new JsonResult(new
                {
                    code = EnumResponseStatus.ERROR,
                    error_message_key = CPLConstant.MobileAppConstant.CommonErrorOccurs
                });
            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }
    }
}
