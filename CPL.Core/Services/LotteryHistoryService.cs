using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LotteryHistoryService : CoreBase<LotteryHistory>, ILotteryHistoryService
    {
        private readonly IRepositoryAsync<LotteryHistory> _repository;

        public LotteryHistoryService(IRepositoryAsync<LotteryHistory> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
