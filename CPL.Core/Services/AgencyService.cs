using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class AgencyService : CoreBase<Agency>, IAgencyService
    {
        private readonly IRepositoryAsync<Agency> _repository;

        public AgencyService(IRepositoryAsync<Agency> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
