using AutoMapper;
using CPL.Common.Enums;
using CPL.Core.Interfaces;
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

    public class LotteryRawingService : ILotteryRawingService
    {
        public void Rawing()
        {
            var lotteries = Resolver.LotteryService
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
