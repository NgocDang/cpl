using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionCategoryDetailService : CoreBase<PricePredictionCategoryDetail>, IPricePredictionCategoryDetailService
    {
        private readonly IRepositoryAsync<PricePredictionCategoryDetail> _repository;

        public PricePredictionCategoryDetailService(IRepositoryAsync<PricePredictionCategoryDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
