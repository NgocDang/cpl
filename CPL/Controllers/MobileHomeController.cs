using AutoMapper;
using CPL.Common.Enums;
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
        private readonly ILotteryService _lotteryService;

        public MobileHomeController(
            IMobileLangDetailService mobileLangDetailService,
            IMapper mapper,
            INewsService newsService,
            ILotteryService lotteryService,
            IHostingEnvironment appEnvironment
        )
        {
            this._newsService = newsService;
            this._mobileLangDetailService = mobileLangDetailService;
            this._mapper = mapper;
            this._appEnvironment = appEnvironment;
            this._lotteryService = lotteryService;
        }

        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetBannersList(MobileModel mobileModel)
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
        [Permission(EnumRole.User)]
        public IActionResult GetLatestNewsList(MobileModel mobileModel)
        {
            try
            {
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

        [HttpPost]
        [Permission(EnumRole.User)]
        public IActionResult GetLotteriesList(MobileModel mobileModel)
        {
            try
            {
                var lotteries = _lotteryService.Query()
                                .Include(x => x.LotteryHistories)
                                .Select()
                                .Where(x => !x.IsDeleted 
                                        && (x.LotteryHistories.Count() < x.Volume 
                                        && (x.Status == (int)EnumLotteryGameStatus.ACTIVE || x.Status == (int)EnumLotteryGameStatus.DEACTIVATED)))
                                .OrderByDescending(x => x.CreatedDate);

                var viewModel = new HomeViewModel();
                viewModel.Lotteries = lotteries
                    .Select(x => Mapper.Map<HomeLotteryViewModel>(x))
                    .ToList();

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.SUCCESS,
                        data = lotteries.Select(x => Mapper.Map<HomeLotteryViewModel>(x)).ToList()
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