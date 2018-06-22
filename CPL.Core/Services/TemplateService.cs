using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class TemplateService : CoreBase<Template>, ITemplateService
    {
        private readonly IRepositoryAsync<Template> _repository;

        public TemplateService(IRepositoryAsync<Template> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
