using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LotteryDetailService : CoreBase<LotteryDetail>, ILotteryDetailService
    {
        private readonly IRepositoryAsync<LotteryDetail> _repository;

        public LotteryDetailService(IRepositoryAsync<LotteryDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
