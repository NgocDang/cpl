using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;

namespace CPL.Core.Services
{
    public class SliderService : CoreBase<Slider>, ISliderService
    {
        private readonly IRepositoryAsync<Slider> _repository;

        public SliderService(IRepositoryAsync<Slider> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
