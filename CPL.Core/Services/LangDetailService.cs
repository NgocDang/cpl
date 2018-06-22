using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LangDetailService : CoreBase<LangDetail>, ILangDetailService
    {
        private readonly IRepositoryAsync<LangDetail> _repository;

        public LangDetailService(IRepositoryAsync<LangDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
