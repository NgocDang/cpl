using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;

namespace CPL.Core.Services
{
    public class NewsService : CoreBase<News>, INewsService
    {
        private readonly IRepositoryAsync<News> _repository;

        public NewsService(IRepositoryAsync<News> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
