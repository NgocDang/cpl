using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class PaymentService : CoreBase<Payment>, IPaymentService
    {
        private readonly IRepositoryAsync<Payment> _repository;

        public PaymentService(IRepositoryAsync<Payment> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
