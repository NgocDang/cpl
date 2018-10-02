using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PricePredictionSettingService : CoreBase<PricePredictionSetting>, IPricePredictionSettingService
    {
        private readonly IRepositoryAsync<PricePredictionSetting> _repository;

        public PricePredictionSettingService(IRepositoryAsync<PricePredictionSetting> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
