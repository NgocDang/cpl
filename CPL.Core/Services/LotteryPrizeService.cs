using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LotteryPrizeService : CoreBase<LotteryPrize>, ILotteryPrizeService
    {
        private readonly IRepositoryAsync<LotteryPrize> _repository;

        public LotteryPrizeService(IRepositoryAsync<LotteryPrize> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
