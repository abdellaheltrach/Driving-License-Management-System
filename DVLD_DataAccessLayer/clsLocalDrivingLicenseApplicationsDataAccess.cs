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
