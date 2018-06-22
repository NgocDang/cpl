using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class CurrencyService : CoreBase<Currency>, ICurrencyService
    {
        private readonly IRepositoryAsync<Currency> _repository;

        public CurrencyService(IRepositoryAsync<Currency> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
