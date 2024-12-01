using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsDataAccess
    {
        public static bool FindByLocalDrivingLicenseApplicationID(
            int localDrivingLicenseApplicationsID,
            ref int applicationID,
            ref int licenseClassID)
        {
            bool isFound = false;

            string query = "SELECT [LocalDrivingLicenseApplicationID], [ApplicationID], [LicenseClassID] " +
                           "FROM [dbo].[LocalDrivingLicenseApplications] " +
                           "WHERE [LocalDrivingLicenseApplicationID] = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationsID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        applicationID = reader["ApplicationID"] != DBNull.Value ? (int)reader["ApplicationID"] : 0;
                        licenseClassID = reader["LicenseClassID"] != DBNull.Value ? (int)reader["LicenseClassID"] : 0;
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while finding the application: " + ex.Message);
                }
            }

            return isFound;
        }

        public static bool FindLocalDrivingLicenseApplicationByApplicationId(
            int applicationID,
            ref int localDrivingLicenseApplicationsID,
            ref int licenseClassID)
        {
            bool isFound = false;

            string query = "SELECT [LocalDrivingLicenseApplicationID], [ApplicationID], [LicenseClassID] " +
                           "FROM [dbo].[LocalDrivingLicenseApplications] " +
                           "WHERE [ApplicationID] = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", applicationID);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        isFound = true;

                        localDrivingLicenseApplicationsID = reader["LocalDrivingLicenseApplicationID"] != DBNull.Value ? (int)reader["LocalDrivingLicenseApplicationID"] : 0;
                        licenseClassID = reader["LicenseClassID"] != DBNull.Value ? (int)reader["LicenseClassID"] : 0;
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while finding the application: " + ex.Message);
                }
            }

            return isFound;
        }

        public static int AddNewLocalDrivingLicenseApplication(int applicationID, int licenseClassID)
        {
            int newID = -1;

            string query = "INSERT INTO [dbo].[LocalDrivingLicenseApplications] ([ApplicationID], [LicenseClassID]) " +
                           "OUTPUT INSERTED.[LocalDrivingLicenseApplicationID] " +
                           "VALUES (@ApplicationID, @LicenseClassID)";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", applicationID);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                try
                {
                    connection.Open();
                    newID = (int)command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding a new LocalDrivingLicenseApplication: " + ex.Message);
                }
            }

            return newID;
        }


        public static bool UpdateLocalDrivingLicenseApplication(int localDrivingLicenseApplicationsID, int applicationID, int licenseClassID)
        {
            bool isUpdated = false;

            string query = "UPDATE [dbo].[LocalDrivingLicenseApplications] " +
                           "SET [ApplicationID] = @ApplicationID, [LicenseClassID] = @LicenseClassID " +
                           "WHERE [LocalDrivingLicenseApplicationID] = @LocalDrivingLicenseApplicationsID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationsID", localDrivingLicenseApplicationsID);
                command.Parameters.AddWithValue("@ApplicationID", applicationID);
                command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                try
                {
                    connection.Open();
                    isUpdated = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the LocalDrivingLicenseApplication: " + ex.Message);
                }
            }

            return isUpdated;
        }

        public static bool IsNewApplicationRepeated(string nationalNo, string className)
        {
            bool exists = false;

            string query = "SELECT COUNT(*) FROM LocalDrivingLicenseApplications_View " +
                           "WHERE Status LIKE 'New' AND NationalNo = @NationalNo AND ClassName LIKE @ClassName";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@NationalNo", nationalNo);
                command.Parameters.AddWithValue("@ClassName", className);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    exists = count > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while checking the applications: " + ex.Message);
                }
            }

            return exists;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            string query = @"
            SELECT 
                [LocalDrivingLicenseApplicationID] as 'L.D.L. AppID',
                [ClassName] as 'Driving Class',
                [NationalNo] as 'National NO.',
                [FullName] as 'Full Name',
                [ApplicationDate] as 'Application Date',
                [PassedTestCount] as 'Passed Test',
                [Status]
            FROM [dbo].[LocalDrivingLicenseApplications_View]
            ORDER BY [ApplicationDate] DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable applicationsTable = new DataTable();
                        adapter.Fill(applicationsTable);
                        return applicationsTable;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                    //throw new Exception("An error occurred while fetching Local Driving License Applications: " + ex.Message);
                }
            }
        }

        public static bool DeleteLocalDrivingApplication(int localDrivingApplicationID)
        {
            string query = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingApplicationID", localDrivingApplicationID);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Execute the delete query
                    return rowsAffected > 0; // Return true if a row was deleted
                }
                catch (Exception ex)
                {
                    // Log error if necessary
                    throw new Exception("An error occurred while deleting the application: " + ex.Message);
                }
            }
        }


        public static byte GetPassedTestCount(int localDrivingLicenseApplicationID)
        {
            string query = @"
        SELECT PassedTestCount 
        FROM LocalDrivingLicenseApplications_View 
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar(); // ExecuteScalar retrieves a single value
                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToByte(result); // Return the value as an integer
                    }
                    else
                    {
                        return 0; // Default value if no result is found
                    }
                }
                catch (Exception ex)
                {
                    // Handle exception (log it if necessary)
                    throw new Exception("An error occurred while retrieving PassedTestCount: " + ex.Message);
                }
            }
        }

    }
}
