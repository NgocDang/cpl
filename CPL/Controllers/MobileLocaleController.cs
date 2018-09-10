using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CPL.Controllers
{
    public class MobileLocaleController : Controller
    {
        private readonly ILangService _langService;
        private readonly IMobileLangDetailService _mobileLangDetailService;
        private readonly IMobileLangMsgDetailService _mobileLangMsgDetailService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _appEnvironment;

        public MobileLocaleController(
            ILangService langService,
            IMobileLangDetailService mobileLangDetailService,
            IMobileLangMsgDetailService mobileLangMsgDetailService,
            IMapper mapper,
            IHostingEnvironment appEnvironment
        )
        {
            this._langService = langService;
            this._mobileLangDetailService = mobileLangDetailService;
            this._mobileLangMsgDetailService = mobileLangMsgDetailService;
            this._mapper = mapper;
            this._appEnvironment = appEnvironment;
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult GetLanguagesList()
        {
            try
            {
                IList<LangViewModel> Langs = _langService.Queryable()
                        .Select(x => Mapper.Map<LangViewModel>(x))
                        .ToList();

                foreach (var lang in Langs)
                {
                    byte[] imageArray = System.IO.File.ReadAllBytes(@"" + _appEnvironment.WebRootPath + "/flags/" + lang.Image + ".png");
                    string base64ImageRepresentation = "data:image/png;base64," + Convert.ToBase64String(imageArray);
                    lang.Image = base64ImageRepresentation;
                }

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = Langs
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.ERROR,
                        error_message_key = ex.Message
                    }
                );
            }
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult GetLanguagesDetailList()
        {
            try
            {
                IList<LangViewModel> Langs = _langService.Queryable()
                        .Select(x => Mapper.Map<LangViewModel>(x))
                        .ToList();

                foreach (var lang in Langs)
                {
                    //byte[] imageArray = System.IO.File.ReadAllBytes(@"" + _appEnvironment.WebRootPath + "/flags/" + lang.Image + ".png");
                    //string base64ImageRepresentation = "data:image/png;base64," + Convert.ToBase64String(imageArray);
                    //lang.Image = base64ImageRepresentation;

                    lang.LangDetails = _mobileLangDetailService.Queryable()
                        .Where(x => x.LangId == lang.Id)
                        .Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
                }

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = Langs
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.ERROR,
                        error_message_key = ex.Message
                    }
                );
            }
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult GetPageDetail(string pageName, int langId)
        {
            try
            {
                MobileLangMsgDetailViewModel pageDetail = _mobileLangMsgDetailService.Queryable()
                        .Where(x => x.LangId == langId && x.Name == pageName)
                        .Select(x => Mapper.Map<MobileLangMsgDetailViewModel>(x)).FirstOrDefault();

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = pageDetail
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.ERROR,
                        error_message_key = ex.Message
                    }
                );
            }
        }
    }
}