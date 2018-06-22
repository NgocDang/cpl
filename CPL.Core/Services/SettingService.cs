using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class SettingService : CoreBase<Setting>, ISettingService
    {
        private readonly IRepositoryAsync<Setting> _repository;

        public SettingService(IRepositoryAsync<Setting> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
