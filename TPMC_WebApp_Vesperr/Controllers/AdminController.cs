using Microsoft.AspNetCore.Mvc;

namespace TPMC_WebApp_Vesperr.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}
