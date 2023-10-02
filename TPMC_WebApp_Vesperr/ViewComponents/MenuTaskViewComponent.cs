using TPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TPMC_WebApp_Vesperr.ViewComponents
{
    public class MenuTaskViewComponent : ViewComponent
    {

        public MenuTaskViewComponent()
        {
        }

        public IViewComponentResult Invoke(string filter)
        {
            var messages = GetData();
            return View(messages);
        }

        private List<MessageNew> GetData()
        {
            var messages = new List<MessageNew>();
            messages.Add(new MessageNew
            {
                Id = 1,
                ShortDesc = "Design some buttons",
                URLPath = "#",
                Percentage = 20,
            });

            return messages;
        }
    }
}
