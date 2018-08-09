using Microsoft.AspNetCore.Mvc;

namespace CPL.ViewComponents
{
    public class GameHistoryViewComponent : ViewComponent
    {
        public GameHistoryViewComponent() { }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
