using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CPL.Models;
using CPL.Misc;
using CPL.Misc.Enums;
using CPL.Core.Interfaces;
using AutoMapper;
using CPL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using CPL.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace CPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILangService _langService;
        private readonly ILangDetailService _langDetailService;
        private readonly IMapper _mapper;
        private readonly IViewRenderService _viewRenderService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly ISettingService _settingService;
        private readonly ILotteryService _lotteryService;
        private readonly ITemplateService _templateService;
        private readonly INewsService _newsService;
        private readonly IFAQService _faqService;
        private readonly IDataContextAsync _context;
        private readonly IPricePredictionService _pricePredictionService;
        private readonly ILotteryHistoryService _lotteryHistoryService;

        public HomeController(
            ILangService langService,
            ILangDetailService langDetailService,
            IMapper mapper,
            IViewRenderService viewRenderService,
            IUnitOfWorkAsync unitOfWork,
            ISettingService settingService,
            ILotteryService lotteryService,
            IDataContextAsync context,
            IFAQService faqService,
            ITemplateService templateService,
            INewsService newsService,
            IPricePredictionService pricePredictionService,
            ILotteryHistoryService lotteryHistoryService)
        {
            this._langService = langService;
            this._langDetailService = langDetailService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._faqService = faqService;
            this._lotteryService = lotteryService;
            this._templateService = templateService;
            this._context = context;
            this._newsService = newsService;
            this._pricePredictionService = pricePredictionService;
            this._lotteryHistoryService = lotteryHistoryService;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            var activeLotteries = _lotteryService.Queryable()
                .Where(x => !x.IsDeleted && x.Status == (int)EnumLotteryGameStatus.ACTIVE).ToList();

            var closestPricePrediction = _pricePredictionService.Query()
                .Include(x => x.PricePredictionSetting)
                    .ThenInclude(y => y.PricePredictionSettingDetails)
                .Where(x => !x.UpdatedDate.HasValue && x.CloseBettingTime > DateTime.Now)
                .OrderBy(x => x.CloseBettingTime)
                .FirstOrDefault();

            int randomIndex = RandomPicker.Random.Next(activeLotteries.Count);
            var randomLottery = _lotteryService.Query().Include(x => x.LotteryCategory)
                .FirstOrDefault(x => x.Id == activeLotteries[randomIndex].Id);

            var viewModel = new HomeViewModel { RandomLotteryId = randomLottery?.Id,
                                                RandomLotteryCategoryId = randomLottery != null ? randomLottery.LotteryCategoryId : 0,
                                                RandomLotteryTitle = randomLottery?.Title,
                                                RandomLotteryDescription = randomLottery?.LotteryCategory.Description,
                                                ClosestPricePredictionId = closestPricePrediction?.Id,
                                                ClosestPricePredictionTitle = closestPricePrediction
                                                    ?.PricePredictionSetting
                                                    .PricePredictionSettingDetails
                                                    .FirstOrDefault(x => x.LangId == HttpContext.Session.GetInt32("LangId").Value).Title,
                                                ClosestPricePredictionDescription = closestPricePrediction
                                                    ?.PricePredictionSetting
                                                    .PricePredictionSettingDetails
                                                    .FirstOrDefault(x => x.LangId == HttpContext.Session.GetInt32("LangId").Value).ShortDescription };
            viewModel.FAQs = _faqService.Query()
                .Include(x => x.Group)
                .Where(x => x.Group.Filter == EnumGroupFilter.FAQ.ToString() && x.LangId == HttpContext.Session.GetInt32("LangId").Value)
                .Select(x => Mapper.Map<FAQViewModel>(x))
                .ToList();

            return View(viewModel);
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Error403()
        {
            return View();
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Error403Ajax()
        {
            return StatusCode(403);
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Error401Ajax()
        {
            return StatusCode(401);
        }

        [Permission(EnumRole.Guest)]
        public IActionResult UpdateLangDetail()
        {
            LangDetailHelper.LangDetails = _langDetailService.Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
            return new JsonResult( new{ success = true });
        }
    }
}