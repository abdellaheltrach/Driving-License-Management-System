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

        public static bool FindUserById(int userId, ref int personId, ref string userName, ref string password, ref bool isActive)
        {
            bool isFound = false;  // Variable to track if the user is found

            // SQL query to find the user by UserID
            string query = "SELECT PersonID, UserName, Password, IsActive FROM Users WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read()) // Check if user exists
                    {
                        // Update ref parameters with values from the database
                        personId = reader.GetInt32(reader.GetOrdinal("PersonID"));
                        userName = reader.GetString(reader.GetOrdinal("UserName"));
                        password = reader.GetString(reader.GetOrdinal("Password"));
                        isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                        isFound = true; // Mark as found
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions (optional: log or display error message)
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    connection.Close(); // Ensure the connection is closed
                }
            }

            return isFound; // Return whether the user was found or not
        }

    }


}

