using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Data;
using System.Text.Json.Nodes;

namespace CIS2103_Website.Models
{

    public class Accounts : ControllerBase
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CD2O8JK;Initial Catalog=CIS2103;Integrated Security=True");

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult SignUpCode(IFormCollection fc)
        {
            bool checkAccount = CheckIfEmailExists(fc["Email"]);
            if (checkAccount == false)
            {
                string signUpQuery = "INSERT INTO Accounts VALUES('" + fc["FirstName"] + "','" + fc["LastName"] + "'" +
                            ",'" + "Active" + "','" + "User" + "','" + fc["Email"] + "','" + fc["Password"] + "','" + "0" + "')";
                Query(signUpQuery);
            }

            return checkAccount ? Conflict() : Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SignInCode(IFormCollection fc)
        {
            AccountModel account = new();
            bool checkAccount = CheckIfEmailExists(fc["Email"]);
            if (checkAccount == true)
            {
                string signInQuery = "SELECT * FROM Accounts " +
                                     "WHERE email='" + fc["Email"] + "'";
                account = ReadAccountQuery(signInQuery);
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
            AccountModel account = ReadAccountQuery(getAccountQuery);
            return Ok(account);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAccountByEmailCode(string email)
        {
            string getAccountQuery = "SELECT * FROM Accounts " +
                                    "WHERE email='" + email + "'";
            AccountModel account = ReadAccountQuery(getAccountQuery);
            return Ok(account);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllAccountsCode()
        {
            string getAccountIdQuery = "SELECT accountId FROM Accounts";
            List<string> accountIds = ReadMultipleDataQuery(getAccountIdQuery);
            int accountIdCount = accountIds.Count;
            AccountModel[] accounts = new AccountModel[accountIdCount];

            for (int i = 0; i < accountIdCount; i++)
            {
                string getAccountQuery = "SELECT * FROM Accounts WHERE accountId=" + accountIds[i];
                accounts[i] = ReadAccountQuery(getAccountQuery);
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
                Query(updateAccountCredentialsQuery);
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
            Query(deleteAccountQuery);
            return Ok("Account Deleted Sucessfully");
        }

        private bool CheckIfEmailExists(string email)
        {
            bool retVal = false;
            string checkIfAccountExistsQuery = "SELECT COUNT(*) FROM Accounts " +
                                                "WHERE email='" + email + "'";
            if (ReadSingleDataQuery(checkIfAccountExistsQuery) == "1")
            {
                retVal = true;
            }
            return retVal;
        }

        private void Query(string query)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private string ReadSingleDataQuery(string query)
        {
            string retVal = "";
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    retVal = string.Format("{0}", reader[0]);
                }
            }
            con.Close();
            return retVal;
        }

        private List<string> ReadMultipleDataQuery(string query)
        {
            var list = new List<string>();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    list.Add(reader["accountId"].ToString()!);
                }
            }
            con.Close();
            return list;
        }

        private AccountModel ReadAccountQuery(string query)
        {
            AccountModel account = new();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    account.AccountId = (int)reader["accountId"];
                    account.FirstName = (string)reader["firstName"];
                    account.LastName = (string)reader["lastName"];
                    account.Status = (string)reader["status"];
                    account.Privilege = (string)reader["privilege"];
                    account.Email = (string)reader["email"];
                    account.Password = (string)reader["password"];
                    account.Cash = (decimal)reader["cash"];
                }
            }
            con.Close();
            return account;
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
