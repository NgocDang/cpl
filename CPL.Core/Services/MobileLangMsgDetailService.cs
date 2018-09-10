using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class MobileLangMsgDetailService : CoreBase<MobileLangMsgDetail>, IMobileLangMsgDetailService
    {
        private readonly IRepositoryAsync<MobileLangMsgDetail> _repository;

        public MobileLangMsgDetailService(IRepositoryAsync<MobileLangMsgDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
