using Microsoft.AspNetCore.Mvc;

namespace TIPMC_WebApp_Vesperr.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Main()
        {
            return View();
        }
    }
}
