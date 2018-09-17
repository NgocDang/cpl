using CPL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CPL.ViewComponents
{
    public class PieChartViewComponent : ViewComponent
    {
        public PieChartViewComponent()
        {
        }

        public IViewComponentResult Invoke(PieChartViewComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
