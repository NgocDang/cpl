using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class LangMsgDetailService : CoreBase<LangMsgDetail>, ILangMsgDetailService
    {
        private readonly IRepositoryAsync<LangMsgDetail> _repository;

        public LangMsgDetailService(IRepositoryAsync<LangMsgDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
