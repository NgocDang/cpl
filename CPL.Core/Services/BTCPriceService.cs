using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class BTCPriceService : CoreBase<BTCPrice>, IBTCPriceService
    {
        private readonly IRepositoryAsync<BTCPrice> _repository;

        public BTCPriceService(IRepositoryAsync<BTCPrice> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
