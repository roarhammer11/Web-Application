using CIS2103_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace CIS2103_Website.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Accounts accounts = new Accounts();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        [Route("Home/Dashboard/EditDVD")]
        public IActionResult EditDVD()
        {

            return View();
        }

        [Route("Home/Dashboard/Transactions")]
        public IActionResult Transactions()
        {
            return View();
        }

        [Route("Home/Dashboard/Accounts")]
        public IActionResult Accounts()
        {
            return View();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddAccount(IFormCollection fc)
        {
            int statusCode = 0;
            if (fc.Count != 0)
            {
                StatusCodeResult result = (StatusCodeResult)accounts.AddAccountCode(fc);
                statusCode = result.StatusCode;
            }
            return StatusCode(statusCode);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}