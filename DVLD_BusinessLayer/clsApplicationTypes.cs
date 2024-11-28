using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccessLayer;

namespace DVLD_Buisness
{
    public class clsApplicationTypes
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public clsApplicationTypes()

        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }

        public clsApplicationTypes(int ID, string ApplicationTypeTitel, float ApplicationTypeFees)

        {
            this.ID = ID;
            this.Title = ApplicationTypeTitel;
            this.Fees = ApplicationTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            //call DataAccess Layer 

            this.ID = clsApplicationTypesDataAccessLayer.AddNewApplicationType(this.Title, this.Fees);


            return (this.ID != -1);
        }

        private bool _UpdateApplicationType()
        {
            //call DataAccess Layer 

            return clsApplicationTypesDataAccessLayer.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }

        public static clsApplicationTypes Find(int ID)
        {
            string Title = ""; float Fees = 0;

            if (clsApplicationTypesDataAccessLayer.GetApplicationTypeInfoByID((int)ID, ref Title, ref Fees))

                return new clsApplicationTypes(ID, Title, Fees);
            else
                return null;

        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesDataAccessLayer.GetAllApplicationTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();

            }

            return false;
        }

    }
}
