using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationsDataAccess
    {
        // Insert a new application
        public static int AddNewApplication(
            int applicantPersonID,
            DateTime applicationDate,
            int applicationTypeID,
            byte applicationStatus,
            DateTime lastStatusDate,
            float paidFees,
            int createdByUserID)
        {
            int applicationID = -1;

            string query = @"INSERT INTO [Applications] 
                        ([ApplicantPersonID], [ApplicationDate], [ApplicationTypeID], [ApplicationStatus], [LastStatusDate], [PaidFees], [CreatedByUserID])
                        VALUES 
                        (@ApplicantPersonID, @ApplicationDate, @ApplicationTypeID, @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID);
                        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
                command.Parameters.AddWithValue("@ApplicationDate", applicationDate);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                command.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
                command.Parameters.AddWithValue("@PaidFees", paidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        applicationID = Convert.ToInt32(result);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while adding a new application: " + ex.Message);
                }
            }

            return applicationID;
        }

        public static bool UpdateApplication(
            int applicationID,
            int applicantPersonID,
            DateTime applicationDate,
            int applicationTypeID,
            byte applicationStatus,
            DateTime lastStatusDate,
            float paidFees,
            int createdByUserID)
        {
            bool isUpdated = false;

            string query = @"UPDATE [Applications] SET 
                        [ApplicantPersonID] = @ApplicantPersonID, 
                        [ApplicationDate] = @ApplicationDate, 
                        [ApplicationTypeID] = @ApplicationTypeID, 
                        [ApplicationStatus] = @ApplicationStatus, 
                        [LastStatusDate] = @LastStatusDate, 
                        [PaidFees] = @PaidFees, 
                        [CreatedByUserID] = @CreatedByUserID
                        WHERE [ApplicationID] = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", applicationID);
                command.Parameters.AddWithValue("@ApplicantPersonID", applicantPersonID);
                command.Parameters.AddWithValue("@ApplicationDate", applicationDate);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
                command.Parameters.AddWithValue("@ApplicationStatus", applicationStatus);
                command.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
                command.Parameters.AddWithValue("@PaidFees", paidFees);
                command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    isUpdated = (rowsAffected > 0);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while updating the application: " + ex.Message);
                }
            }

            return isUpdated;
        }

        // Delete an application
        public static bool DeleteApplication(int applicationID)
        {
            string query = "DELETE FROM [dbo].[Applications] WHERE [ApplicationID] = @ApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationID", applicationID);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                  //  Console.WriteLine("Error while deleting application: " + ex.Message);
                    return false;
                }
            }
        }


        public static bool GetApplicationInfoByAppID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, ref byte ApplicationStatus, ref DateTime LastStatusDate,
ref float PaidFees, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from Applications where ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    // The record was found
                    isFound = true;


                    ApplicantPersonID = (int)reader["ApplicantPersonID"];
                    ApplicationDate = (DateTime)reader["ApplicationDate"];
                    ApplicationTypeID = (int)reader["ApplicationTypeID"];
                    ApplicationStatus = (byte)reader["ApplicationStatus"];
                    LastStatusDate = (DateTime)reader["LastStatusDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    CreatedByUserID = (int)reader["CreatedByUserID"];




                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
               // Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int GetApplicationIDByLocalDrivingLicenseApplicationID(int localDrivingLicenseApplicationID)
        {
            int applicationID = -1; // Nullable to handle the case when no record is found

            string query = @"
        SELECT [ApplicationID] 
        FROM [dbo].[LocalDrivingLicenseApplications] 
        WHERE [LocalDrivingLicenseApplicationID] = @LocalDrivingLicenseApplicationID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar(); // ExecuteScalar returns the first column of the first row

                        if (result != null && result != DBNull.Value)
                        {
                            applicationID = Convert.ToInt32(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception
                        // throw new Exception($"Error retrieving ApplicationID: {ex.Message}", ex);
                        return applicationID;
                    }
                }
            }

            return applicationID;
        }
    }
}
