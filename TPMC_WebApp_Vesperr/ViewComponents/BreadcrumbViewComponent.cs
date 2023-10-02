using TPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TPMC_WebApp_Vesperr.ViewComponents
{
    public class BreadcrumbViewComponent : ViewComponent
    {

        public BreadcrumbViewComponent()
        {
            
        }

        public IViewComponentResult Invoke(string filter)
        {
            if (ViewBag.Breadcrumb == null)
            {
                ViewBag.Breadcrumb = new List<MessageNew>();
            }
            
            return View(ViewBag.Breadcrumb as List<MessageNew>);
        }
    }
}
