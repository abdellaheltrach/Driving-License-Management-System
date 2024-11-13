using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

        public static bool IsPasswordMatch(int userId, string password)
        {
            bool isMatch = false;

            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID AND Password = @Password";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Use parameterized queries to prevent SQL injection
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();

                        // Check if a match was found (count should be 1 if userId/password match)
                        isMatch = count == 1;
                    }
                    catch (Exception ex)
                    {
          
                    }
                }
            }

            return isMatch;
        }

        public static bool ChangePassword(int userId, string newPassword)
        {
            bool isPasswordChanged = false;

            string query = "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Use parameterized queries to prevent SQL injection
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if a row was updated
                        isPasswordChanged = rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions (e.g., log them)
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }

            return isPasswordChanged;
        }

        public static DataTable GetAllUsers()
        {
            // Updated query with concatenation for full name
            string query = @"
            SELECT 
                Users.UserID, 
                Users.PersonID, 
                (People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName) AS FullName, 
                Users.UserName, 
                Users.IsActive
            FROM     
                People 
            INNER JOIN
                Users ON People.PersonID = Users.PersonID";

            DataTable usersTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(usersTable); // Fill the DataTable with query results
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }

            return usersTable;
        }

        public static bool DeleteUser(int userId)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            bool isDeleted = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isDeleted = rowsAffected > 0; // True if deletion was successful
                    }
                    catch (Exception ex)
                    {
                        // Log the exception or handle it as needed
                    }
                }
            }

            return isDeleted;
        }



    }




}

