using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class IntroducedUsersService : CoreBase<IntroducedUsers>, IIntroducedUsersService
    {
        private readonly IRepositoryAsync<IntroducedUsers> _repository;

        public IntroducedUsersService(IRepositoryAsync<IntroducedUsers> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
