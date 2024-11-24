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
        public static bool FindLocalDrivingLicenseApplicationByLocalDrivingLicenseApplicationsID(
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

        /// <summary>
        /// Update an existing LocalDrivingLicenseApplication.
        /// </summary>
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

    }
}
