using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionHistoryService : CoreBase<PricePredictionHistory>, IPricePredictionHistoryService
    {
        private readonly IRepositoryAsync<PricePredictionHistory> _repository;

        public PricePredictionHistoryService(IRepositoryAsync<PricePredictionHistory> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
