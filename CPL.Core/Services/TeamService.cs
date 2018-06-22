using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class TeamService : CoreBase<Team>, ITeamService
    {
        private readonly IRepositoryAsync<Team> _repository;

        public TeamService(IRepositoryAsync<Team> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
