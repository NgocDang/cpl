using CPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPL.ViewComponents
{
    public class TransactionHistoryViewComponent : ViewComponent
    {
        public TransactionHistoryViewComponent() { }

        public IViewComponentResult Invoke(TransactionHistoryViewComponentViewModel viewModel)
        {
            return View(viewModel);
        }
    }
}
