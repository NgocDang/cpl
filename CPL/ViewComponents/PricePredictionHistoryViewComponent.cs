using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class PricePredictionHistoryViewComponent : ViewComponent
    {
        public PricePredictionHistoryViewComponent()
        {
        }

        public IViewComponentResult Invoke(PricePredictionHistoryViewComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
