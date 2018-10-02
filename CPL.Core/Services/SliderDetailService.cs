using CPL.Core.Interfaces;
using CPL.Domain;
using CPL.Infrastructure.Interfaces;

namespace CPL.Core.Services
{
    public class SliderDetailService : CoreBase<SliderDetail>, ISliderDetailService
    {
        private readonly IRepositoryAsync<SliderDetail> _repository;

        public SliderDetailService(IRepositoryAsync<SliderDetail> repository)
            : base(repository)
        {
            this._repository = repository;
        }
    }
}
