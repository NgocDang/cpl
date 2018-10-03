using System.Collections.Generic;

namespace CPL.Models
{
    public class AddPricePredictionCategoryAdminViewModel
    {
        public AddPricePredictionCategoryAdminViewModel()
        {
            PricePredictionCategoryDetailAdminViewModels = new List<PricePredictionCategoryDetailAdminViewModel>();
        }
        public List<PricePredictionCategoryDetailAdminViewModel> PricePredictionCategoryDetailAdminViewModels { get; set; }
    }
}
