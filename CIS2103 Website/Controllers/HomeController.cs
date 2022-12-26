using CIS2103_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text;

namespace CIS2103_Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Accounts accounts = new();
        private readonly DVDs dvds = new();
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
            SetAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/EditDVD/{accountId}")]
        [Route("Home/Dashboard/EditDVD")]
        public IActionResult EditDVD(int accountId)
        {
            SetAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/Transactions/{accountId}")]
        [Route("Home/Dashboard/Transactions")]
        public IActionResult Transactions(int accountId)
        {
            SetAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/Accounts/{accountId}")]
        [Route("Home/Dashboard/Accounts")]
        public IActionResult Accounts(int accountId)
        {
            SetAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/AddDVD/{accountId}")]
        [Route("Home/Dashboard/AddDVD")]
        public IActionResult AddDVD(int accountId)
        {
            SetAccount(accountId);
            return View();
        }

        [HttpGet("Home/Dashboard/Cart/{accountId}")]
        [Route("Home/Dashboard/Cart")]
        public IActionResult Cart(int accountId)
        {
            SetAccount(accountId);
            return View();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignUp(IFormCollection fc)
        {
            return StatusCode(GetStatusCode((StatusCodeResult)accounts.SignUpCode(fc), fc.Count));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignIn(IFormCollection fc)
        {
            return accounts.SignInCode(fc);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllAccounts()
        {
            return accounts.GetAllAccountsCode();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateAccountCredentials(IFormCollection fc)
        {
            return accounts.UpdateAccountCredentialsCode(fc);
        }

        public IActionResult DeleteAccount(IFormCollection fc)
        {
            return accounts.DeleteAccountCode(fc["Email"]);
        }

        public IActionResult AddCash(IFormCollection fc)
        {
            return accounts.AddCashCode(fc);
        }


        public IActionResult AddDVD(IFormCollection fc)
        {
            return dvds.AddDVDCode(fc);

        }

        public IActionResult EditDVD(IFormCollection fc)
        {
            return dvds.EditDVDCode(fc);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllDVDs()
        {
            return dvds.GetAllDVDsCode();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetSingleDVD(IFormCollection fc)
        {
            return dvds.GetSingleDVDCode(fc);
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

        private static int GetStatusCode(StatusCodeResult modelStatusCode, int fcCount)
        {
            int statusCode = 0;
            if (fcCount != 0)
            {
                statusCode = modelStatusCode.StatusCode;
            }
            return statusCode;
        }

        private void SetAccount(int accountId)
        {
            try
            {
                var objectResult = (OkObjectResult)accounts.GetAccountByIdCode(accountId);
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