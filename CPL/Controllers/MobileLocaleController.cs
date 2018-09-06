using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Misc.Utils;
using CPL.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CPL.Controllers
{
    public class MobileLocaleController : Controller
    {
        private readonly ILangService _langService;
        private readonly ILangDetailService _langDetailService;
        private readonly ILangMsgDetailService _langMsgDetailService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _appEnvironment;

        public MobileLocaleController(
            ILangService langService,
            ILangDetailService langDetailService,
            ILangMsgDetailService langMsgDetailService,
            IMapper mapper,
            IHostingEnvironment appEnvironment
        )
        {
            this._langService = langService;
            this._langDetailService = langDetailService;
            this._langMsgDetailService = langMsgDetailService;
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
                        code = 200,
                        data = Langs
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = 500,
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

                    lang.LangDetails = _langDetailService.Queryable()
                        .Where(x => x.LangId == lang.Id)
                        .Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
                }


                return new JsonResult(
                    new
                    {
                        code = 200,
                        data = Langs
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = 500,
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
                LangMsgDetailViewModel pageDetail = _langMsgDetailService.Queryable()
                        .Where(x => x.LangId == langId && x.Name == pageName)
                        .Select(x => Mapper.Map<LangMsgDetailViewModel>(x)).FirstOrDefault();


                return new JsonResult(
                    new
                    {
                        code = 200,
                        data = pageDetail
                    }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(
                    new
                    {
                        code = 500,
                        error_message_key = ex.Message
                    }
                );
            }
        }










    }
}