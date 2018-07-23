using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Misc
{
    public interface ILotteryRawingService
    {
        Task Rawing();
    }

    public class LotteryRawingService : ILotteryRawingService
    {
        public Task Rawing()
        {
            throw new NotImplementedException();
        }

        private void CheckStatus()
        {
            throw new NotImplementedException();
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
