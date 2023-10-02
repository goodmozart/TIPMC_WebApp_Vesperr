using TPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace TPMC_WebApp_Vesperr.ViewComponents
{
    public class PageAlertViewComponent : ViewComponent
    {

        public PageAlertViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            List<MessageNew> messages;
            if (ViewBag.PageAlerts == null)
            {
                messages = new List<MessageNew>();
            }
            else
            {
                messages = new List<MessageNew>(ViewBag.PageAlerts);
            }
            return View(messages);
        }
    }
}
