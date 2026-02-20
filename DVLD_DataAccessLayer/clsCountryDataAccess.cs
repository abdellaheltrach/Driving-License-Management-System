using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccessLayer
{
    public class clsCountryDataAccess
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryID", ID);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;
                            CountryName = (string)reader["CountryName"];
                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, nameof(GetCountryInfoByID));
                isFound = false;
            }

            return isFound;
        }

        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false;

            string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", CountryName);
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;
                            ID = (int)reader["CountryID"];
                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, nameof(GetCountryInfoByName));
                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            string query = "SELECT [CountryName] FROM Countries ORDER BY [CountryName]";

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
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
            }
            catch (Exception ex)
            {
                clsHandleExceptions.LogException(ex, nameof(GetAllCountries));
            }

            return dt;
        }
    }
}
