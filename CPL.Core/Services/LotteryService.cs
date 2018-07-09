using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LotteryService : CoreBase<Lottery>, ILotteryService
    {
        private readonly IRepositoryAsync<Lottery> _repository;

        public LotteryService(IRepositoryAsync<Lottery> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
