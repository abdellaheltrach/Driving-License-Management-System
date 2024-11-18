using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;



namespace DVLD_DataAccessLayer
{

    public class clsApplicationTypesDataAccessLayer
    {



        public static bool FindApplicationTypeById(int applicationTypeID, ref string title, ref int fees)
        {
            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assign values to ref parameters
                            title = reader["ApplicationTypeTitle"].ToString();
                            fees = Convert.ToInt32(reader["ApplicationFees"]);

                            return true;    
                        }
                        else
                        {

                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }



        public static DataTable GetAllApplicationTypes()
        {
            string query = "SELECT [ApplicationTypeID] AS [ID], [ApplicationTypeTitle] AS [Title], [ApplicationFees] AS [Fees] FROM [dbo].[ApplicationTypes]";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                DataTable applicationTypesTable = new DataTable();

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(applicationTypesTable); // Fills the DataTable with the query results
                }
                catch (Exception ex)
                {
                    //throw new Exception("An error occurred while retrieving application types: " + ex.Message);
                }

                return applicationTypesTable;
            }
        }


        public static bool UpdateApplicationType(int applicationTypeId, string newTitle, decimal newFees)
        {
            string query = @"
        UPDATE [dbo].[ApplicationTypes]
        SET [ApplicationTypeTitle] = @ApplicationTypeTitle, 
            [ApplicationFees] = @ApplicationFees
        WHERE [ApplicationTypeID] = @ApplicationTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ApplicationTypeTitle", newTitle);
                command.Parameters.AddWithValue("@ApplicationFees", newFees);
                command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Execute the update query
                    return rowsAffected > 0; // Returns true if at least one row was updated
                }
                catch (Exception ex)
                {
                    //throw new Exception("An error occurred while updating the application type: " + ex.Message);
                    return false;
                }
            }
        }




    }
}