using TPMC_WebApp_Vesperr.Common.Attributes;
using TPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TPMC_WebApp_Vesperr.Controllers
{
    public class HomeControllerNew : Controller
    {
        private readonly ILogger<HomeControllerNew> _logger;

        public HomeControllerNew(ILogger<HomeControllerNew> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Main()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
