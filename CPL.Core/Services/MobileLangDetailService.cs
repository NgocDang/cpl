using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class MobileLangDetailService : CoreBase<MobileLangDetail>, IMobileLangDetailService
    {
        private readonly IRepositoryAsync<MobileLangDetail> _repository;

        public MobileLangDetailService(IRepositoryAsync<MobileLangDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
