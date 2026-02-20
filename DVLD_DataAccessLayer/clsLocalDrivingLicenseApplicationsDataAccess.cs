using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationsID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            applicationID = reader["ApplicationID"] != DBNull.Value ? (int)reader["ApplicationID"] : 0;
                            licenseClassID = reader["LicenseClassID"] != DBNull.Value ? (int)reader["LicenseClassID"] : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while finding by LocalDrivingLicenseApplicationID");
                throw;
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

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            localDrivingLicenseApplicationsID = reader["LocalDrivingLicenseApplicationID"] != DBNull.Value ? (int)reader["LocalDrivingLicenseApplicationID"] : 0;
                            licenseClassID = reader["LicenseClassID"] != DBNull.Value ? (int)reader["LicenseClassID"] : 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while finding LocalDrivingLicenseApplication by ApplicationID");
                throw;
            }

            return isFound;
        }

        public static int AddNewLocalDrivingLicenseApplication(int applicationID, int licenseClassID)
        {
            int newID = -1;

            string query = "INSERT INTO [dbo].[LocalDrivingLicenseApplications] ([ApplicationID], [LicenseClassID]) " +
                           "OUTPUT INSERTED.[LocalDrivingLicenseApplicationID] " +
                           "VALUES (@ApplicationID, @LicenseClassID)";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                    connection.Open();
                    newID = (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while adding new LocalDrivingLicenseApplication");
                throw;
            }

            return newID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int localDrivingLicenseApplicationsID, int applicationID, int licenseClassID)
        {
            bool isUpdated = false;

            string query = "UPDATE [dbo].[LocalDrivingLicenseApplications] " +
                           "SET [ApplicationID] = @ApplicationID, [LicenseClassID] = @LicenseClassID " +
                           "WHERE [LocalDrivingLicenseApplicationID] = @LocalDrivingLicenseApplicationsID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationsID", localDrivingLicenseApplicationsID);
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);

                    connection.Open();
                    isUpdated = command.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while updating LocalDrivingLicenseApplication");
                throw;
            }

            return isUpdated;
        }

        public static bool IsNewApplicationRepeated(string nationalNo, string className)
        {
            bool exists = false;

            string query = "SELECT COUNT(*) FROM LocalDrivingLicenseApplications_View " +
                           "WHERE Status LIKE 'New' AND NationalNo = @NationalNo AND ClassName LIKE @ClassName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);
                    command.Parameters.AddWithValue("@ClassName", className);

                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    exists = count > 0;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while checking if the application is repeated");
                throw;
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

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable applicationsTable = new DataTable();
                    adapter.Fill(applicationsTable);
                    return applicationsTable;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while fetching Local Driving License Applications");
                throw;
            }
        }

        public static bool DeleteLocalDrivingApplication(int localDrivingApplicationID)
        {
            string query = "DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingApplicationID", localDrivingApplicationID);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while deleting LocalDrivingApplication");
                throw;
            }
        }

        public static byte GetPassedTestCount(int localDrivingLicenseApplicationID)
        {
            string query = @"
        SELECT PassedTestCount 
        FROM LocalDrivingLicenseApplications_View 
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        return Convert.ToByte(result);
                    }

                    return 0;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while retrieving PassedTestCount");
                throw;
            }
        }

        public static bool IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationID, int testTypeID)
        {
            bool result = false;

            string query = @" SELECT TOP 1 Found=1
                      FROM LocalDrivingLicenseApplications INNER JOIN
                           TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                      WHERE
                           (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)  
                           AND (TestAppointments.TestTypeID = @TestTypeID) AND IsLocked = 0
                      ORDER BY TestAppointments.TestAppointmentID DESC";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeID);

                    connection.Open();
                    object queryResult = command.ExecuteScalar();

                    if (queryResult != null)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while checking for active scheduled test");
                throw;
            }

            return result;
        }

        public static bool UpdateStatus(int applicationID, short newStatus)
        {
            string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID=@ApplicationID;";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationID);
                    command.Parameters.AddWithValue("@NewStatus", newStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, "Error while updating application status");
            }
            return false;
        }
    }
}
