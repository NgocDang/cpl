using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class FAQService : CoreBase<FAQ>, IFAQService
    {
        private readonly IRepositoryAsync<FAQ> _repository;

        public FAQService(IRepositoryAsync<FAQ> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
