using CIS2103_Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
namespace CIS2103_Website.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CD2O8JK;Initial Catalog=CIS2103;Integrated Security=True");
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
        public IActionResult AddAccountCode(IFormCollection fc)
        {
            if (fc.Count != 0)
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "INSERT INTO Accounts VALUES('" + fc["FirstName"] + "','" + fc["LastName"] + "'" +
                                  ",'" + "Active" + "','" + "User" + "','" + fc["Email"] + "','" + fc["Password"] + "','" + "0" + "')";
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return View("Index");
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