using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LotteryCategoryService : CoreBase<LotteryCategory>, ILotteryCategoryService
    {
        private readonly IRepositoryAsync<LotteryCategory> _repository;

        public LotteryCategoryService(IRepositoryAsync<LotteryCategory> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
