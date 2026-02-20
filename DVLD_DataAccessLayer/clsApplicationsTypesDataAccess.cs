using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsApplicationTypesDataAccessLayer
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
                  ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsHandleExceptions.LogException(ex, nameof(GetApplicationTypeInfoByID));
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static bool GetApplicationTypeInfoByTitle(string ApplicationTypeTitle, ref int ApplicationTypeID, ref float ApplicationFees)
        {
            bool isFound = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeTitle = @ApplicationTypeTitle";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsHandleExceptions.LogException(ex, nameof(GetApplicationTypeInfoByTitle));
                        isFound = false;
                    }
                }
            }

            return isFound;
        }

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = "SELECT * FROM [dbo].[ApplicationTypes] ORDER BY ApplicationTypeID";

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
                        clsHandleExceptions.LogException(ex, nameof(GetAllApplicationTypes));
                    }
                }
            }

            return dt;
        }

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
                                VALUES (@Title, @Fees)
                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    try
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationTypeID = insertedID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsHandleExceptions.LogException(ex, nameof(AddNewApplicationType));
                    }
                }
            }

            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            bool isUpdated = false;

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"UPDATE ApplicationTypes
                                SET ApplicationTypeTitle = @Title,
                                    ApplicationFees = @Fees
                                WHERE ApplicationTypeID = @ApplicationTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                    command.Parameters.AddWithValue("@Title", Title);
                    command.Parameters.AddWithValue("@Fees", Fees);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        isUpdated = (rowsAffected > 0);
                    }
                    catch (Exception ex)
                    {
                        clsHandleExceptions.LogException(ex, nameof(UpdateApplicationType));
                        isUpdated = false;
                    }
                }
            }

            return isUpdated;
        }
    }
}
