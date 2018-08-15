using Microsoft.AspNetCore.Mvc;

namespace CPL.ViewComponents
{
    public class TransactionHistoryViewComponent : ViewComponent
    {
        public TransactionHistoryViewComponent() { }

        public IViewComponentResult Invoke(int? sysUserId)
        {
            return View(sysUserId);
        }
    }
}
