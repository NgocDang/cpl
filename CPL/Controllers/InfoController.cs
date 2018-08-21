using AutoMapper;
using CPL.Core.Interfaces;
using CPL.Infrastructure.Interfaces;
using CPL.Misc;
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

        public IActionResult WhatIsCPL()
        {
            return View();
        }

        public IActionResult HowToPlay()
        {
            return View();
        }

        public IActionResult TermsOfService()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
