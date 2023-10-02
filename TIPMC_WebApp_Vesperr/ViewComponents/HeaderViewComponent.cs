using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TIPMC_WebApp_Vesperr.Common;
using TIPMC_WebApp_Vesperr.Models;

namespace TIPMC_WebApp_Vesperr.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {

        public HeaderViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
