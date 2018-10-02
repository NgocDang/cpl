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
using Microsoft.EntityFrameworkCore;
using CPL.Common.Enums;

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
        private readonly IDataContextAsync _context;
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
            ITemplateService templateService,
            ISliderService sliderService,
            INewsService newsService)
        {
            this._langService = langService;
            this._langDetailService = langDetailService;
            this._mapper = mapper;
            this._viewRenderService = viewRenderService;
            this._settingService = settingService;
            this._unitOfWork = unitOfWork;
            this._lotteryService = lotteryService;
            this._templateService = templateService;
            this._context = context;
            this._newsService = newsService;
            this._sliderService = sliderService;
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            var viewModel = new HomeViewModel();

            viewModel.Sliders = _sliderService.Queryable()
                                .Include(x => x.Group)
                                .Include(x => x.SliderDetails)
                                .Where(x => x.Group.Name == EnumGroupName.HOMEPAGE.ToString()
                                            && x.Group.Filter == EnumGroupFilter.SLIDER.ToString()
                                            && x.Status == (int)EnumSliderStatus.ACTIVE)
                                .Select(x => Mapper.Map<SliderViewModel>(x))
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