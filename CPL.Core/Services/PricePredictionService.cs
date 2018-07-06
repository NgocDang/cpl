using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionService : CoreBase<PricePrediction>, IPricePredictionService
    {
        private readonly IRepositoryAsync<PricePrediction> _repository;

        public PricePredictionService(IRepositoryAsync<PricePrediction> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
