using TIPMC_WebApp_Vesperr.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using TIPMC_WebApp_Vesperr.Common.Extensions;
using TIPMC_WebApp_Vesperr.Common;
namespace TIPMC_WebApp_Vesperr.ViewComponents
{
    public class MenuMessageViewComponent : ViewComponent
    {

        public MenuMessageViewComponent()
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
                UserId = ((ClaimsPrincipal)User).GetUserProperty(CustomClaimTypes.NameIdentifier),
                DisplayName = "Support Team",
                AvatarURL = "/images/user.png",
                ShortDesc = "Why not buy a new awesome theme?",
                TimeSpan = "5 mins",
                URLPath = "#",
            });

            messages.Add(new MessageNew
            {
                Id = 1,
                UserId = ((ClaimsPrincipal)User).GetUserProperty(CustomClaimTypes.NameIdentifier),
                DisplayName = "Ken",
                AvatarURL = "/images/user.png",
                ShortDesc = "For approval",
                TimeSpan = "15 mins",
                URLPath = "#",
            });

            return messages;
        }
    }
}
