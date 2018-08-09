using CPL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CPL.ViewComponents
{
    public class LotteryHistoryViewComponent : ViewComponent
    {
        public LotteryHistoryViewComponent() { }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
