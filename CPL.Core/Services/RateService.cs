using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class RateService : CoreBase<Rate>, IRateService
    {
        private readonly IRepositoryAsync<Rate> _repository;

        public RateService(IRepositoryAsync<Rate> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
