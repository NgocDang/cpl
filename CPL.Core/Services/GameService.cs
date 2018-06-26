using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPL.Core.Services
{
    public class GameService : CoreBase<Game>, IGameService
    {
        private readonly IRepositoryAsync<Game> _repository;

        public GameService(IRepositoryAsync<Game> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
