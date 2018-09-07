using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class PricePredictionViewComponent : ViewComponent
    {
        public PricePredictionViewComponent()
        {
        }

        public IViewComponentResult Invoke(PricePredictionViewComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
