using System;
using System.Data;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsTestTypes
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enum enMode { AddNew = 0, Update = 1 };

        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public int TestTypeFees { get; set; }
        public string TestTypeDescription { get; set; }



        public enMode Mode { get; set; }

        public clsTestTypes()
        {
            this.Mode = enMode.AddNew;
        }

        // Save Method
        public bool Save()
        {
            if (Mode == enMode.AddNew)
            {
                return _AddNewTestType();
            }
            else if (Mode == enMode.Update)
            {
                return _UpdateTestType();
            }
            return false;
        }

        private bool _AddNewTestType()
        {

            this.TestTypeID = (clsTestTypes.enTestType)clsTestTypesDataAccess.AddNewTestType(this.TestTypeTitle, this.TestTypeFees, this.TestTypeDescription);

            return (this.TestTypeTitle != "");
        }

        private bool _UpdateTestType()
        {
            return clsTestTypesDataAccess.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.TestTypeFees, this.TestTypeDescription);
        }

        // Existing methods for finding and retrieving test types remain unchanged.
        public static clsTestTypes FindById(enTestType testType)
        {
            string title = "";
            int fees = 0;
            string description = "";

            bool isFound = clsTestTypesDataAccess.FindTestTypeById((int)testType, ref title, ref fees, ref description);

            if (isFound)
            {
                return new clsTestTypes
                {
                    TestTypeID = testType,
                    TestTypeTitle = title,
                    TestTypeFees = fees,
                    TestTypeDescription = description,
                    Mode = enMode.Update
                };
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetTestTypes()
        {
            return clsTestTypesDataAccess.GetAllTestTypes();
        }
    }
}
