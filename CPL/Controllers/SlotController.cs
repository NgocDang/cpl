using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CPL.Misc;
using CPL.Misc.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CPL.Controllers
{
    public class SlotController : Controller
    {
        [Permission(EnumRole.Guest)]
        public IActionResult Index()
        {
            return View();
        }
    }
}