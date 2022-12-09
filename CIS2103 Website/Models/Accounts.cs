using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace CIS2103_Website.Models
{

    public class Accounts : ControllerBase
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CD2O8JK;Initial Catalog=CIS2103;Integrated Security=True");
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public IActionResult AddAccountCode(IFormCollection fc)
        {
            bool sentinel = CheckIfEmailExists(fc);
            if (sentinel == false)
            {
                string addAccountQuery = "INSERT INTO Accounts VALUES('" + fc["FirstName"] + "','" + fc["LastName"] + "'" +
                            ",'" + "Active" + "','" + "User" + "','" + fc["Email"] + "','" + fc["Password"] + "','" + "0" + "')";
                Query(addAccountQuery);
            }

            return sentinel ? Conflict() : Ok();
        }

        /* public StatusCodeResult checkStatus(IActionResult action)
         {
             StatusCodeResult retVal = action.;

             return retVal;
         }*/

        private bool CheckIfEmailExists(IFormCollection fc)
        {
            bool retVal = false;
            string checkIfAccountExistsQuery = "SELECT COUNT(*) FROM Accounts " +
                                                "WHERE email='" + fc["Email"] + "'";
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
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            con.Close();
        }
        private string ReadSingleDataQuery(string query)
        {
            string retVal = "";
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
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

    }
}
