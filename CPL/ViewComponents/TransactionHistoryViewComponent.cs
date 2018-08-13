using Microsoft.AspNetCore.Mvc;

namespace CPL.ViewComponents
{
    public class TranscationHistoryViewComponent : ViewComponent
    {
        public TranscationHistoryViewComponent() { }

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
