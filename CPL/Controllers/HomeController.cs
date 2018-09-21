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
using LinqKit;
using CPL.Misc.Utils;
using CPL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

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
        }

        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            var lotteries = _lotteryService.Query()
                .Include(x => x.LotteryCategory)
                .Include(x => x.LotteryDetails)
                .Include(x => x.LotteryHistories)
                .Select()
                .Where(x => !x.IsDeleted && (x.LotteryHistories.Count() < x.Volume && (x.Status == (int)EnumLotteryGameStatus.ACTIVE || x.Status == (int)EnumLotteryGameStatus.DEACTIVATED)))
                .OrderByDescending(x => x.CreatedDate);

            var viewModel = new HomeViewModel();
            viewModel.Lotteries = lotteries
                .Select(x => Mapper.Map<HomeLotteryViewModel>(x))
                .ToList();

            var lastNews = _newsService.Queryable().LastOrDefault();
            viewModel.News = Mapper.Map<NewsViewModel>(lastNews);

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