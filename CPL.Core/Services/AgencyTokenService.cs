using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class AgencyTokenService : CoreBase<AgencyToken>, IAgencyTokenService
    {
        private readonly IRepositoryAsync<AgencyToken> _repository;

        public AgencyTokenService(IRepositoryAsync<AgencyToken> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
