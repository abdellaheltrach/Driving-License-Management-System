using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsTestTypesDataAccess
    {
        public static bool FindTestTypeById(int testTypeId, ref string title, ref int fees, ref string description)
        {
            string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Assign values to ref parameters
                            title = reader["TestTypeTitle"].ToString();
                            fees = Convert.ToInt32(reader["TestTypeFees"]);
                            description = reader["TestTypeDescription"].ToString();

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
                    // Log error (if needed)
                    return false;
                }
            }
        }

        public static int AddNewTestType(string title, int fees, string description)
        {
            int TestTypeID = -1;

            string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeTitle,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@ApplicationFees)
                            where TestTypeID = @TestTypeID;
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@TestTypeTitle", title);
                command.Parameters.AddWithValue("@TestTypeDescription", description);
                command.Parameters.AddWithValue("@ApplicationFees", fees);

                try
                {
                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        TestTypeID = insertedID;
                    }
                }
                catch (Exception ex)
                {
                    // Log error if needed
                }
                return TestTypeID;

            }
        }

        public static DataTable GetAllTestTypes()
        {
            string query = @"
            SELECT 
                [TestTypeID] AS [ID], 
                [TestTypeTitle] AS [Title], 
                [TestTypeDescription] AS [Description],
                [TestTypeFees] AS [Fees]
            FROM [dbo].[TestTypes]";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                DataTable testTypesTable = new DataTable();

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    // Load the data from the reader into the DataTable
                    testTypesTable.Load(reader);

                    reader.Close(); // Close the reader when done
                }
                catch (Exception ex)
                {
                    // Log error (if needed)
                    throw new Exception("An error occurred while retrieving test types: " + ex.Message);
                }

                return testTypesTable;
            }

        }

        public static bool UpdateTestType(int testTypeId, string newTitle, int newFees, string newDescription)
        {
            string query = @"
            UPDATE [dbo].[TestTypes]
            SET 
                [TestTypeTitle] = @TestTypeTitle, 
                [TestTypeFees] = @TestTypeFees,
                [TestTypeDescription] = @TestTypeDescription
            WHERE [TestTypeID] = @TestTypeID";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TestTypeTitle", newTitle);
                command.Parameters.AddWithValue("@TestTypeFees", newFees);
                command.Parameters.AddWithValue("@TestTypeDescription", newDescription);
                command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery(); // Execute the update query
                    return rowsAffected > 0; // Returns true if at least one row was updated
                }
                catch (Exception ex)
                {
                    // Log error (if needed)
                    return false;
                }
            }
        }


    }
}
