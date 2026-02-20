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
        public static int AddUser(int personId, string userName, string password, bool isActive)
        {
            string query = "INSERT INTO Users (PersonID, UserName, Password, IsActive) OUTPUT INSERTED.UserID VALUES (@PersonID, @UserName, @Password, @IsActive)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", personId);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@IsActive", isActive);

                try
                {
                    connection.Open();
                    int userId = (int)command.ExecuteScalar();
                    return userId; // Return the newly created UserID
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while adding user", "AddUser");
                    return -1; // Return -1 to indicate failure
                }
            }
        }

        public static bool UpdateUser(int userId, string userName, string password, bool isActive)
        {
            string query = "UPDATE Users SET UserName = @UserName, Password = @Password, IsActive = @IsActive WHERE UserID = @UserID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@UserName", userName);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@IsActive", isActive);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while updating user", "UpdateUser");
                    return false;
                }
            }
        }

        public static int VerifyUserCredentials(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT UserID FROM Users WHERE UserName = @UserName AND Password = @Password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", username);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int UserID) && UserID != -1)
                    {
                        return UserID;
                    }
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while verifying user credentials", "VerifyUserCredentials");
                }
                return -1;
            }
        }

        public static bool IsUserActive(int userId)
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT IsActive FROM Users WHERE UserID = @UserID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != null && Convert.ToBoolean(result);
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while checking user activity", "IsUserActive");
                    return false;
                }
            }
        }

        public static bool FindUserById(int userId, ref int personId, ref string userName, ref string password, ref bool isActive)
        {
            string query = "SELECT PersonID, UserName, Password, IsActive FROM Users WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personId = reader.GetInt32(reader.GetOrdinal("PersonID"));
                            userName = reader.GetString(reader.GetOrdinal("UserName"));
                            password = reader.GetString(reader.GetOrdinal("Password"));
                            isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while finding user by ID", "FindUserById");
                }
            }
            return false;
        }

        public static bool IsPasswordMatch(int userId, string password)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE UserID = @UserID AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@Password", password);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count == 1;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while checking password match", "IsPasswordMatch");
                    return false;
                }
            }
        }

        public static bool ChangePassword(int userId, string newPassword)
        {
            string query = "UPDATE Users SET Password = @NewPassword WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                command.Parameters.AddWithValue("@NewPassword", newPassword);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while changing password", "ChangePassword");
                    return false;
                }
            }
        }

        public static DataTable GetAllUsers()
        {
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
                SqlCommand command = new SqlCommand(query, connection);
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
                    clsHandleExceptions.LogException(ex, "Error while retrieving all users", "GetAllUsers");
                }
            }
            return usersTable;
        }

        public static bool DeleteUser(int userId)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while deleting user", "DeleteUser");
                    return false;
                }
            }
        }

        public static bool IsUserExistsById(int PersonId)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE PersonID = @PersonID";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PersonID", PersonId);

                try
                {
                    connection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while checking if user exists by ID", "IsUserExistsById");
                    return false;
                }
            }
        }

        public static bool IsUserExistsByUsername(string userName)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName";
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserName", userName);

                try
                {
                    connection.Open();
                    int result = Convert.ToInt32(command.ExecuteScalar());
                    return result > 0;
                }
                catch (Exception ex)
                {
                    clsHandleExceptions.LogException(ex, "Error while checking if user exists by username", "IsUserExistsByUsername");
                    return false;
                }
            }
        }
    }
}
