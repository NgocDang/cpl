using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class GameHistoryService : CoreBase<GameHistory>, IGameHistoryService
    {
        private readonly IRepositoryAsync<GameHistory> _repository;

        public GameHistoryService(IRepositoryAsync<GameHistory> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
