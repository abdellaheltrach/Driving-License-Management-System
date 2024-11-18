using System;
using System.Collections.Generic;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestTypes
    {
        public int TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public int TestTypeFees { get; set; }
        public string TestTypeDescription { get; set; }

        // Constructor
        public clsTestTypes(int testTypeID, string title, int fees, string description)
        {
            this.TestTypeID = testTypeID;
            this.TestTypeTitle = title;
            this.TestTypeFees = fees;
            this.TestTypeDescription = description;
        }

        // Find a TestType by its ID
        public static clsTestTypes FindById(int testTypeID)
        {
            string title = "";
            int fees = 0;
            string description = "";

            bool isFound = clsTestTypesDataAccess.FindTestTypeById(testTypeID, ref title, ref fees, ref description);

            if (isFound)
            {
                return new clsTestTypes(testTypeID, title, fees, description);
            }
            else
            {
                return null;
            }
        }

        // Get all TestTypes
        public static DataTable GetTestTypes()
        {
            return clsTestTypesDataAccess.GetAllTestTypes();
        }

        // Update a TestType
        public static bool UpdateTestType(int testTypeID, string testTypeTitle, int testTypeFees, string testTypeDescription)
        {
            return clsTestTypesDataAccess.UpdateTestType(testTypeID, testTypeTitle, testTypeFees, testTypeDescription);
        }
    }
}
