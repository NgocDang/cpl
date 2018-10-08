using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionDetailService : CoreBase<PricePredictionDetail>, IPricePredictionDetailService
    {
        private readonly IRepositoryAsync<PricePredictionDetail> _repository;

        public PricePredictionDetailService(IRepositoryAsync<PricePredictionDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
