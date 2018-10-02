using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionCategoryService : CoreBase<PricePredictionCategory>, IPricePredictionCategoryService
    {
        private readonly IRepositoryAsync<PricePredictionCategory> _repository;

        public PricePredictionCategoryService(IRepositoryAsync<PricePredictionCategory> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
