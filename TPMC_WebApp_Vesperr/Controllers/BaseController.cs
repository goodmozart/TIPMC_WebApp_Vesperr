using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TPMC_WebApp_Vesperr.Models;
using System.Runtime.CompilerServices;

namespace TPMC_WebApp_Vesperr.Controllers
{
    public class BaseController : Controller
    {
        internal void AddBreadcrumb(string displayName, string urlPath)
        {
            List<MessageNew> messages;

            if (ViewBag.Breadcrumb == null)
            {
                messages = new List<MessageNew>();
            }
            else
            {
                messages = ViewBag.Breadcrumb as List<MessageNew>;
            }

            messages.Add(new MessageNew { DisplayName = displayName, URLPath = urlPath });
            ViewBag.Breadcrumb = messages;
        }

        internal void AddPageHeader(string pageHeader = "", string pageDescription = "")
        {
            ViewBag.PageHeader = Tuple.Create(pageHeader, pageDescription);
        }

        internal enum PageAlertType
        {
            Error,
            Info,
            Warning,
            Success
        }

        internal void AddPageAlerts(PageAlertType pageAlertType, string description)
        {
            List<MessageNew> messages;

            if (ViewBag.PageAlerts == null)
            {
                messages = new List<MessageNew>();
            }
            else
            {
                messages = ViewBag.PageAlerts as List<MessageNew>;
            }

            messages.Add(new MessageNew { Type = pageAlertType.ToString().ToLower(), ShortDesc = description });
            ViewBag.PageAlerts = messages;
        }
    }
}