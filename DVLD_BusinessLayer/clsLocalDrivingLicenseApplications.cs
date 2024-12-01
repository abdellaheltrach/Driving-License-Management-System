using DVLD_Buisness;
using DVLD_DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsLocalDrivingLicenseApplications : clsApplications
    {
        public int LocalDrivingLicenseApplicationsID { get;  set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClasses LicenseClassesInfo;

        public enum enMode { AddNew, Update }
        public enMode Mode;

        // Private constructor
        private clsLocalDrivingLicenseApplications(int localDrivingLicenseApplicationsID, int applicationID, int licenseClassID,
            int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.LocalDrivingLicenseApplicationsID = localDrivingLicenseApplicationsID;
            this.ApplicationID = applicationID;
            this.LicenseClassID = licenseClassID;

            //to the base class

            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicantPerson = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeInfo = clsApplicationTypes.Find(ApplicationTypeID);


            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = (enApplicationStatus)ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindUserById(CreatedByUserID);


            this.Mode = enMode.Update;
        }

        // Public constructor for new applications
        public clsLocalDrivingLicenseApplications()
        {
            this.LocalDrivingLicenseApplicationsID = -1;
            this.ApplicationID = -1;
            this.LicenseClassID = -1;

            //set parameters of the base class



            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicantPerson = null;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;


            this.Mode = enMode.AddNew;
        }

        // Static method to find and build the object
        public static clsLocalDrivingLicenseApplications FindByLocalDrivingLicenseApplicationID(int localDrivingLicenseApplicationsID)
        {
            int appID = -1;
            int licenseClassID = -1;

            bool isFound = clsLocalDrivingLicenseApplicationsDataAccess.FindByLocalDrivingLicenseApplicationID(
                localDrivingLicenseApplicationsID,
                ref appID,
                ref licenseClassID
            );

            if (isFound)
            {
                clsApplications application = clsApplications.Find(appID);



                return new clsLocalDrivingLicenseApplications(localDrivingLicenseApplicationsID, appID, licenseClassID,
            application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, (byte)application.ApplicationStatus, application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
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
                clsApplications application = clsApplications.Find(appID);


                return new clsLocalDrivingLicenseApplications(localDrivingLicenseApplicationsID, appID, licenseClassID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, (byte)application.ApplicationStatus, application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
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

            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.


            base.Mode = (clsApplications.enMode)this.Mode;

            if (!base.Save())
                return false;

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



        public bool Delete()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;
            //First we delete the Local Driving License Application
            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationsDataAccess.DeleteLocalDrivingApplication(this.LocalDrivingLicenseApplicationsID);

            if (!IsLocalDrivingApplicationDeleted)
                return false;


            //Then we delete the base Application
            IsBaseApplicationDeleted = base.DeleteApplication();
            return IsBaseApplicationDeleted;

        }


        public static bool IsNewApplicationRepeated(string nationalNo, string className)
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.IsNewApplicationRepeated(nationalNo, className);
        }


        public byte GetPassedTestCount()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetPassedTestCount(this.LocalDrivingLicenseApplicationsID);
        }

        public static DataTable GetAllApplications()
        {
            return clsLocalDrivingLicenseApplicationsDataAccess.GetAllLocalDrivingLicenseApplications();
        }
    }
}
