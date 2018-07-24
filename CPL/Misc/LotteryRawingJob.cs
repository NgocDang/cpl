using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
using CPL.Core.Services;
using CPL.Misc.Quartz;
using CPL.Models;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public class LotteryRawingJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Rawing(context);
            return Task.FromResult(0);
        }

        public void Rawing(IJobExecutionContext context)
        {
            var lotteries = ((LotteryService)context.JobDetail.JobDataMap["LotteryService"])
                    .Query()
                    .Include(x => x.LotteryHistories)
                    .Include(x => x.LotteryPrizes)
                    .Select()
                    .Where(x => x.Status.Equals((int)EnumLotteryGameStatus.ACTIVE) && x.Volume.Equals(x.LotteryHistories.Count))
                    .Select(x => Mapper.Map<LotteryViewModel>(x))
                    .ToList();

            var hasRawing = CheckStatus(lotteries);

            if (hasRawing)
            {
                foreach (var lottery in lotteries)
                {
                    var rawingTime = lottery.LotteryHistories.LastOrDefault().CreatedDate.Hour;
                }
            }
        }

        private bool CheckStatus(List<LotteryViewModel> lottery)
        {
            return !(lottery is null);
        }

        private void PickWinner()
        {
            throw new NotImplementedException();
        }

        private void UpdateWinner()
        {
            throw new NotImplementedException();

        }
    }
}
