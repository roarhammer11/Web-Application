using CIS2103_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Specialized;
namespace CIS2103_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Accounts accounts = new();
        private AccountModel? accountModel = new();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {

            return View();
        }

        [HttpGet("Home/Dashboard/{accountId}")]
        [Route("Home/Dashboard")]
        public IActionResult Dashboard(int accountId)
        {
            setAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/EditDVD/{accountId}")]
        [Route("Home/Dashboard/EditDVD")]
        public IActionResult EditDVD(int accountId)
        {
            setAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/Transactions/{accountId}")]
        [Route("Home/Dashboard/Transactions")]
        public IActionResult Transactions(int accountId)
        {
            setAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/Accounts/{accountId}")]
        [Route("Home/Dashboard/Accounts")]
        public IActionResult Accounts(int accountId)
        {
            setAccount(accountId);
            return View();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignUp(IFormCollection fc)
        {
            return StatusCode(getStatusCode((StatusCodeResult)accounts.SignUpCode(fc), fc.Count));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignIn(IFormCollection fc)
        {
            return accounts.SignInCode(fc);
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

        private int getStatusCode(StatusCodeResult modelStatusCode, int fcCount)
        {
            int statusCode = 0;
            if (fcCount != 0)
            {
                statusCode = modelStatusCode.StatusCode;
            }
            return statusCode;
        }

        private void setAccount(int accountId)
        {
            try
            {
                var objectResult = (OkObjectResult)accounts.GetAccount(accountId);
                accountModel = (AccountModel)objectResult.Value!;
                ViewData["AccountId"] = accountModel.AccountId;
                ViewData["FirstName"] = accountModel.FirstName;
                ViewData["LastName"] = accountModel.LastName;
                ViewData["Email"] = accountModel.Email;
                ViewData["Privilege"] = accountModel.Privilege;
                ViewData["Cash"] = accountModel.Cash;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}