using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LangService : CoreBase<Lang>, ILangService
    {
        private readonly IRepositoryAsync<Lang> _repository;

        public LangService(IRepositoryAsync<Lang> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
