using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LangContentService : CoreBase<LangContent>, ILangContentService
    {
        private readonly IRepositoryAsync<LangContent> _repository;

        public LangContentService(IRepositoryAsync<LangContent> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
