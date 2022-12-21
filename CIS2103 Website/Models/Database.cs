using System.Data;
using System.Data.SqlClient;

namespace CIS2103_Website.Models
{
    public class Database
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-CD2O8JK;Initial Catalog=CIS2103;Integrated Security=True");

        public void Query(string query)
        {
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string ReadSingleDataQuery(string query)
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

        public List<string> ReadMultipleDataQuery(string query, string id)
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
                    list.Add(reader[id].ToString()!);
                }
            }
            con.Close();
            return list;
        }

        public AccountModel ReadAccountQuery(string query)
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

        public DVDModel ReadDVDQuery(string query)
        {
            DVDModel dvd = new();
            con.Open();
            SqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    dvd.DVDId = (int)reader["dvdId"];
                    dvd.DVDName = (string)reader["dvdName"];
                    dvd.DVDImage = ConvertFromDBVal<byte[]>(reader["dvdImage"]);
                    dvd.Quantity = (int)reader["quantity"];
                    dvd.Description = (string)reader["description"];
                    dvd.Category = (string)reader["category"];
                    dvd.RatePerRent = (decimal)reader["ratePerRent"];
                }
            }
            con.Close();
            return dvd;
        }
        public bool CheckIfEmailExists(string email)
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
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T)!; // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }
    }
}
