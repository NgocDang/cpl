using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class ETHTransactionService : CoreBase<ETHTransaction>, IETHTransactionService
    {
        private readonly IRepositoryAsync<ETHTransaction> _repository;

        public ETHTransactionService(IRepositoryAsync<ETHTransaction> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
