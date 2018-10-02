using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionSettingDetailService : CoreBase<PricePredictionSettingDetail>, IPricePredictionSettingDetailService
    {
        private readonly IRepositoryAsync<PricePredictionSettingDetail> _repository;

        public PricePredictionSettingDetailService(IRepositoryAsync<PricePredictionSettingDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
