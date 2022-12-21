using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data;
using System.Text.Json.Nodes;

namespace CIS2103_Website.Models
{

    public class Accounts : ControllerBase
    {
        Database db = new Database();

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignUpCode(IFormCollection fc)
        {
            bool checkAccount = db.CheckIfEmailExists(fc["Email"]);
            if (checkAccount == false)
            {
                string signUpQuery = "INSERT INTO Accounts VALUES('" + fc["FirstName"] + "','" + fc["LastName"] + "'" +
                            ",'" + "Active" + "','" + "User" + "','" + fc["Email"] + "','" + fc["Password"] + "','" + "0" + "')";
                db.Query(signUpQuery);
            }

            return checkAccount ? Conflict() : Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SignInCode(IFormCollection fc)
        {
            AccountModel account = new();
            bool checkAccount = db.CheckIfEmailExists(fc["Email"]);
            if (checkAccount == true)
            {
                string signInQuery = "SELECT * FROM Accounts " +
                                     "WHERE email='" + fc["Email"] + "'";
                account = db.ReadAccountQuery(signInQuery);
                if (account.Password != fc["Password"])
                {
                    checkAccount = false;
                }
            }

            return checkAccount ? Ok(account) : Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAccountByIdCode(int accountId)
        {
            string getAccountQuery = "SELECT * FROM Accounts " +
                                    "WHERE accountId='" + accountId + "'";
            AccountModel account = db.ReadAccountQuery(getAccountQuery);
            return Ok(account);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAccountByEmailCode(string email)
        {
            string getAccountQuery = "SELECT * FROM Accounts " +
                                    "WHERE email='" + email + "'";
            AccountModel account = db.ReadAccountQuery(getAccountQuery);
            return Ok(account);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllAccountsCode()
        {
            string getAccountIdQuery = "SELECT accountId FROM Accounts";
            List<string> accountIds = db.ReadMultipleDataQuery(getAccountIdQuery, "accountId");
            int accountIdCount = accountIds.Count;
            AccountModel[] accounts = new AccountModel[accountIdCount];

            for (int i = 0; i < accountIdCount; i++)
            {
                string getAccountQuery = "SELECT * FROM Accounts WHERE accountId=" + accountIds[i];
                accounts[i] = db.ReadAccountQuery(getAccountQuery);
            }
            return Ok(accounts);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult UpdateAccountCredentialsCode(IFormCollection fc)
        {
            bool checkCredentials = true;
            var objectResult = (OkObjectResult)GetAccountByEmailCode(fc["OldEmail"]);
            AccountModel accountModel = (AccountModel)objectResult.Value!;
            if (accountModel.Password == fc["OldPassword"])
            {
                string updateAccountCredentialsQuery = "UPDATE Accounts SET email='" + fc["NewEmail"] + "',password='" + fc["NewPassword"] +
                                                    "' WHERE accountId='" + accountModel.AccountId + "'";
                db.Query(updateAccountCredentialsQuery);
            }
            else
            {
                checkCredentials = false;
            }

            return checkCredentials ? Ok("Account Sucessfully Updated") : Unauthorized();
        }

        public IActionResult DeleteAccountCode(string email)
        {
            string deleteAccountQuery = "DELETE FROM Accounts WHERE email='" + email + "'";
            db.Query(deleteAccountQuery);
            return Ok("Account Deleted Sucessfully");
        }

        public IActionResult AddCashCode(IFormCollection fc)
        {
            string getAccountCashQuery = "SELECT cash FROM Accounts WHERE accountId='" + fc["AccountId"] + "'";
            int currentCash = int.Parse(db.ReadSingleDataQuery(getAccountCashQuery));
            int updatedCash = currentCash + int.Parse(fc["CashAmount"]);
            string updateAccountCashQuery = "UPDATE Accounts SET cash='" + updatedCash + "' WHERE accountId='" + fc["AccountId"] + "'";
            db.Query(updateAccountCashQuery);
            JsonNode data = JsonNode.Parse("{\"message\": \"Cash Updated Sucessfully\", \"cash\": \"" + updatedCash + "\"}")!;
            return Ok(data);
        }
    }

    public class AccountModel
    {
        public int? AccountId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Status { get; set; }
        public string? Privilege { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public decimal? Cash { get; set; }
    }
}
