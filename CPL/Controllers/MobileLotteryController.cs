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
using Microsoft.EntityFrameworkCore;

namespace CPL.Controllers
{
    public class MobileLotteryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly ILotteryService _lotteryService;

        public MobileLotteryController(
            IMapper mapper,
            ILotteryService lotteryService,
            IHostingEnvironment appEnvironment
        )
        {
            this._mapper = mapper;
            this._appEnvironment = appEnvironment;
            this._lotteryService = lotteryService;
        }

        [HttpGet]
        [Permission(EnumRole.User)]
        public IActionResult GetLotteryDetail(MobileModel mobileModel, int lotteryId)
        {
            try
            {
                var lottery = _lotteryService
                                 .Query()
                                 .Include(x => x.LotteryDetails)
                                 .Include(x => x.LotteryHistories)
                                 //.Include(x => x.LotteryPrizes)
                                 .FirstOrDefault(x => x.Id == lotteryId && !x.IsDeleted && (x.Status == (int)EnumLotteryGameStatus.ACTIVE || x.Status == (int)EnumLotteryGameStatus.DEACTIVATED));

                if (lottery != null)
                {
                    return new JsonResult(
                        new
                        {
                            code = EnumResponseStatus.SUCCESS,
                            data = Mapper.Map<LotteryIndexViewModel>(lottery)
                        }
                    );
                }

                return new JsonResult(
                    new
                    {
                        code = EnumResponseStatus.WARNING,
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