using TPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TPMC_WebApp_Vesperr.ViewComponents
{
    public class MenuUserViewComponent : ViewComponent
    {

        public MenuUserViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            return View();
        }
    }
}
