using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class AffiliateService : CoreBase<Affiliate>, IAffiliateService
    {
        private readonly IRepositoryAsync<Affiliate> _repository;

        public AffiliateService(IRepositoryAsync<Affiliate> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
