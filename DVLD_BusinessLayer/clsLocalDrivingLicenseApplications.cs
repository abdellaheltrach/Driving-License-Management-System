using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsLocalDrivingLicenseApplications
    {
        public int LocalDrivingLicenseApplicationsID { get; private set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public enum enMode { AddNew, Update }
        public enMode Mode { get; private set; }

        // Private constructor
        private clsLocalDrivingLicenseApplications(int localDrivingLicenseApplicationsID, int applicationID, int licenseClassID)
        {
            this.LocalDrivingLicenseApplicationsID = localDrivingLicenseApplicationsID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;
            this.Mode = enMode.Update;
        }

        // Public constructor for new applications
        public clsLocalDrivingLicenseApplications()
        {
            this.LocalDrivingLicenseApplicationsID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;
            this.Mode = enMode.AddNew;
        }

        // Static method to find and build the object
        public static clsLocalDrivingLicenseApplications FindByLocalDrivingLicenseApplicationsID(int localDrivingLicenseApplicationsID)
        {
            int appID = 0;
            int licenseClassID = 0;

            bool isFound = clsLocalDrivingLicenseApplicationsDataAccess.FindLocalDrivingLicenseApplicationByLocalDrivingLicenseApplicationsID(
                localDrivingLicenseApplicationsID,
                ref appID,
                ref licenseClassID
            );

            if (isFound)
            {
                return new clsLocalDrivingLicenseApplications(localDrivingLicenseApplicationsID, appID, licenseClassID);
            }

            return null;
        }

        public static clsLocalDrivingLicenseApplications FindByApplicationId(int appID)
        {
            int localDrivingLicenseApplicationsID = 0;
            int licenseClassID = 0;

            bool isFound = clsLocalDrivingLicenseApplicationsDataAccess.FindLocalDrivingLicenseApplicationByApplicationId(
                appID,
                ref localDrivingLicenseApplicationsID,
                ref licenseClassID
            );

            if (isFound)
            {
                return new clsLocalDrivingLicenseApplications(localDrivingLicenseApplicationsID, appID, licenseClassID);
            }

            return null;
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            int newID = clsLocalDrivingLicenseApplicationsDataAccess.AddNewLocalDrivingLicenseApplication(
                this.ApplicationID, this.LicenseClassID
            );

            if (newID != -1)
            {
                this.LocalDrivingLicenseApplicationsID = newID;
                return true;
            }
            return false;
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.UpdateLocalDrivingLicenseApplication(
                this.LocalDrivingLicenseApplicationsID, this.ApplicationID, this.LicenseClassID
            );
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();

                default:
                    return false;
            }
        }

        public static DataTable GetAllApplications()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetAllLocalDrivingLicenseApplications();
        }
    }
}
