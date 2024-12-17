using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsDetainedLicenseDataAccess
    {
        public static bool GetDetainedLicenseInfoByID(int DetainID,
    ref int LicenseID, ref DateTime DetainDate,
    ref float FineFees, ref int CreatedByUserID,
    ref bool IsReleased, ref DateTime ReleaseDate,
    ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", DetainID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            LicenseID = (int)reader["LicenseID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];

                            ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];
                            ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
            ref int DetainID, ref DateTime DetainDate,
            ref float FineFees, ref int CreatedByUserID,
            ref bool IsReleased, ref DateTime ReleaseDate,
            ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            string query = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            isFound = true;
                            DetainID = (int)reader["DetainID"];
                            DetainDate = (DateTime)reader["DetainDate"];
                            FineFees = Convert.ToSingle(reader["FineFees"]);
                            CreatedByUserID = (int)reader["CreatedByUserID"];
                            IsReleased = (bool)reader["IsReleased"];

                            ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];
                            ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"];
                            ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"];
                        }
                    }
                }
                catch (Exception ex)
                {
                    isFound = false;
                }
            }

            return isFound;
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM detainedLicenses_View ORDER BY IsReleased, DetainID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dt.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while retrieving detained licenses: " + ex.Message);
                }
            }

            return dt;
        }

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;

            string query = @"
        INSERT INTO dbo.DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
        VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
        SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@DetainDate", DetainDate);
                command.Parameters.AddWithValue("@FineFees", FineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        DetainID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error while adding new detained license: " + ex.Message);
                }
            }

            return DetainID;
        }

        public static bool UpdateDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, float FineFees, int CreatedByUserID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE dbo.DetainedLicenses
                     SET LicenseID = @LicenseID, 
                         DetainDate = @DetainDate, 
                         FineFees = @FineFees,
                         CreatedByUserID = @CreatedByUserID   
                     WHERE DetainID = @DetainID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", DetainID);
                command.Parameters.AddWithValue("@LicenseID", LicenseID);
                command.Parameters.AddWithValue("@DetainDate", DetainDate);
                command.Parameters.AddWithValue("@FineFees", FineFees);
                command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return rowsAffected > 0;
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;

            string query = @"UPDATE dbo.DetainedLicenses
                     SET IsReleased = 1, 
                         ReleaseDate = @ReleaseDate, 
                         ReleaseApplicationID = @ReleaseApplicationID
                     WHERE DetainID = @DetainID;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DetainID", DetainID);
                command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return rowsAffected > 0;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;

            string query = @"SELECT IsDetained = 1 
                     FROM detainedLicenses 
                     WHERE LicenseID = @LicenseID AND IsReleased = 0;";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LicenseID", LicenseID);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        IsDetained = Convert.ToBoolean(result);
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return IsDetained;
        }

    }
}
