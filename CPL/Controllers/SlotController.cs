using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class SlotController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
