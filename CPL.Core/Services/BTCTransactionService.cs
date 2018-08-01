using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CPL.Core.Services
{
    public class BTCTransactionService : CoreBase<BTCTransaction>, IBTCTransactionService
    {
        private readonly IRepositoryAsync<BTCTransaction> _repository;

        public BTCTransactionService(IRepositoryAsync<BTCTransaction> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
