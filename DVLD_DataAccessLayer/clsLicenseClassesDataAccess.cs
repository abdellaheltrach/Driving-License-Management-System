using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public class clsLicenseClassesDataAccess
    {

        public static DataTable GetAllLicenseClasses()
        {
            DataTable applicationsTable = new DataTable();

            string query = @"select * from  [dbo].[LicenseClasses]";

            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            applicationsTable.Load(reader);
                        }
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
