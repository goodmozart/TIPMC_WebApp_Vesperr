using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TPMC_WebApp_Vesperr.Common;
using TPMC_WebApp_Vesperr.Models;

namespace TPMC_WebApp_Vesperr.ViewComponents
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
