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
        private readonly ISliderService _sliderService;

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
            IPricePredictionService pricePredictionService,
            ILotteryHistoryService lotteryHistoryService,
            ISliderService sliderService,
            INewsService newsService)
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
            this._sliderService = sliderService;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            var activeLotteries = _lotteryService.Queryable()
                .Where(x => !x.IsDeleted && x.Status == (int)EnumLotteryGameStatus.ACTIVE).ToList();

            var closestPricePrediction = _pricePredictionService.Query()
                .Include(x => x.PricePredictionDetails)
                .Where(x =>!x.UpdatedDate.HasValue && x.CloseBettingTime > DateTime.Now && x.Status == (int)EnumPricePredictionGameStatus.ACTIVE)
                .OrderBy(x => x.CloseBettingTime)
                .FirstOrDefault();

            int randomIndex = RandomPicker.Random.Next(activeLotteries.Count);
            var randomLottery = _lotteryService.Query().Include(x => x.LotteryDetails)
                .FirstOrDefault(x => x.Id == activeLotteries[randomIndex].Id && !x.IsDeleted);

            var viewModel = new HomeViewModel { RandomLotteryId = randomLottery?.Id,
                                                RandomLotteryCategoryId = randomLottery != null ? randomLottery.LotteryCategoryId : 0,
                                                RandomLotteryTitle = randomLottery?.Title,
                                                RandomLotteryDescription = randomLottery?.LotteryDetails.FirstOrDefault(x => x.LangId == HttpContext.Session.GetInt32("LangId").Value).ShortDescription,
                                                ClosestPricePredictionId = closestPricePrediction?.Id,
                                                ClosestPricePredictionTitle = closestPricePrediction?.PricePredictionDetails
                                                                                                     .FirstOrDefault(x => x.LangId == HttpContext.Session.GetInt32("LangId").Value).Title,
                                                ClosestPricePredictionDescription = closestPricePrediction?.PricePredictionDetails
                                                                                                           .FirstOrDefault(x => x.LangId == HttpContext.Session.GetInt32("LangId").Value).ShortDescription };

            viewModel.Sliders = _sliderService.Queryable()
                                .Include(x => x.Group)
                                .Include(x => x.SliderDetails)
                                .Where(x => x.Group.Name == EnumGroupName.HOMEPAGE.ToString()
                                            && x.Group.Filter == EnumGroupFilter.SLIDER.ToString()
                                            && x.Status == (int)EnumSliderStatus.ACTIVE)
                                .Select(x => Mapper.Map<SliderViewModel>(x))
                                .ToList();

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
        [HttpPost]
        public IActionResult DoSend(HomeMessageViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var template = _templateService.Queryable().FirstOrDefault(x => x.Name == EnumTemplate.Message.ToString());

                    var messageEmailTemplateViewModel = Mapper.Map<MessageEmailTemplateViewModel>(viewModel);
                    messageEmailTemplateViewModel.NameText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Name");
                    messageEmailTemplateViewModel.CheersText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Cheers");
                    messageEmailTemplateViewModel.ContactInfoText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ContactInfo");
                    messageEmailTemplateViewModel.CPLTeamText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "CPLTeam");
                    messageEmailTemplateViewModel.MessageText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Message");
                    messageEmailTemplateViewModel.EmailText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Email");
                    messageEmailTemplateViewModel.HiText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Hi");
                    messageEmailTemplateViewModel.MessageFromCustomerText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "MessageFromCustomer");
                    messageEmailTemplateViewModel.PhoneNumberText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "PhoneNumber");
                    messageEmailTemplateViewModel.WebsiteText = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "Website");
                    messageEmailTemplateViewModel.RootUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    template.Body = _viewRenderService.RenderToStringAsync("/Views/Home/_MessageEmailTemplate.cshtml", messageEmailTemplateViewModel).Result;
                    EmailHelper.Send(Mapper.Map<TemplateViewModel>(template), CPLConstant.SMTP.Contact);
                    return new JsonResult(new { success = true, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "MessageSentSuccessfully") });
                }
                catch (Exception ex)
                {
                    return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
                }

            }
            return new JsonResult(new { success = false, message = LangDetailHelper.Get(HttpContext.Session.GetInt32("LangId").Value, "ErrorOccurs") });
        }

        [Permission(EnumRole.Guest)]
        public IActionResult UpdateLangDetail()
        {
            LangDetailHelper.LangDetails = _langDetailService.Queryable().Select(x => Mapper.Map<LangDetailViewModel>(x)).ToList();
            return new JsonResult( new{ success = true });
        }
    }
}