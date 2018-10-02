using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;

namespace CPL.Core.Services
{
    public class GroupService : CoreBase<Group>, IGroupService
    {
        private readonly IRepositoryAsync<Group> _repository;

        public GroupService(IRepositoryAsync<Group> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
