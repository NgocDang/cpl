using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
using CPL.Misc.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CPL.Controllers
{
    public class InfoController: Controller
    {
        public InfoController()
        {
        }

        [Permission(EnumRole.Guest)]
        public IActionResult WhatIsCPL()
        {
            return View();
        }

        [Permission(EnumRole.Guest)]
        public IActionResult HowToPlay()
        {
            return View();
        }

        [Permission(EnumRole.Guest)]
        public IActionResult TermsOfService()
        {
            return View();
        }

        [Permission(EnumRole.Guest)]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
