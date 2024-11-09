using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsUserDataAccess
    {
        public static int VerifyUserCredentials(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT UserID FROM Users WHERE UserName = @UserName AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                connection.Open();
                object result = command.ExecuteScalar();

                //  query = query + " ";

                if (result != null && int.TryParse(result.ToString(), out int UserID) && UserID != -1)
                {
                    return UserID;



                }

                return  -1;
            }
        }

        public static bool IsUserActive(int userId)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT IsActive FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                connection.Open();
                object result = command.ExecuteScalar();
                return result != null && Convert.ToBoolean(result);
            }
        }



    }
}
