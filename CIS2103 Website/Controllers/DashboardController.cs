using Microsoft.AspNetCore.Mvc;

namespace CIS2103_Website.Controllers
{
    [Area("Dashboard")]
    public class DashboardController : Controller
    {
        public IActionResult EditDVD()
        {
            return View();
        }

    }
}
