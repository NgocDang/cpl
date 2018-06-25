using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class CoinTransactionService : CoreBase<CoinTransaction>, ICoinTransactionService
    {
        private readonly IRepositoryAsync<CoinTransaction> _repository;

        public CoinTransactionService(IRepositoryAsync<CoinTransaction> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
