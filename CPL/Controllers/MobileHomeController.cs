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
    public class MobileHomeController : Controller
    {
        private readonly IMobileLangDetailService _mobileLangDetailService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly INewsService _newsService;

        public MobileHomeController(
            IMobileLangDetailService mobileLangDetailService,
            IMapper mapper,
            INewsService newsService,
            IHostingEnvironment appEnvironment
        )
        {
            this._newsService = newsService;
            this._mobileLangDetailService = mobileLangDetailService;
            this._mapper = mapper;
            this._appEnvironment = appEnvironment;
        }

        [HttpPost]
        [Permission(EnumRole.Guest)]
        public IActionResult GetBannersList(int? mobileLangId)
        {
            try
            {
                List<MobileBannerViewModel> banners = new List<MobileBannerViewModel>();

                MobileBannerViewModel item = new MobileBannerViewModel();
                item.Id = 1;
                item.Src = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}"+"/images/lottery/1_slide_probability_mobile.jpg";
                banners.Add(item);
                item.Id = 2;
                item.Src = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/images/lottery/2_slide_crypto_lot_mobile.jpg";
                banners.Add(item);
                item.Id = 3;
                item.Src = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/images/lottery/3_slide_lottery_mobile.jpg";
                banners.Add(item);
                item.Id = 4;
                item.Src = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/images/lottery/4_slide_smartcontract_mobile.jpg";
                banners.Add(item);

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = banners
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
        public IActionResult GetLatestNewsList(int? mobileLangId)
        {
            try
            {
                LangDetailHelper.LangDetails = _mobileLangDetailService.Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();

                var lastestNews = _newsService.Queryable().LastOrDefault();

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = Mapper.Map<NewsViewModel>(lastestNews)
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