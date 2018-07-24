using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Misc.Quartz;
using CPL.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public interface ILotteryRawingService
    {
        void Rawing();
    }

    public class LotteryRawingService 
    {
        
    }
}
