using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_BusinessLayer;
using DVLD_DataAccessLayer;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsApplications
    {

        public int ApplicationID { get; set; }
        public clsPerson ApplicantPerson { get; set; }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public byte ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public Decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }

        public clsLocalDrivingLicenseApplications LocalDrivingLicenseApplication { get; set ; }


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enApplicationType
        {
            NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService = 2, ReplacementForALostDrivingLicense = 3,
            ReplacementForADamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7


        }
        private enApplicationType applicationType;



        public clsApplications()
        {
            Mode = enMode.AddNew;
            this.ApplicationID = -1;
            this.ApplicantPerson = null;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;



        }

        public static clsApplications Find(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = DateTime.Now;
            int ApplicationTypeID = -1;
            byte ApplicationStatus = 0; 
            DateTime LastStatusDate = DateTime.Now;
            Decimal PaidFees = -1; 
            int CreatedByUserID = -1;

            bool IsFound = clsApplicationsDataAccess.GetApplicationInfoByAppID
                                (
                                     ApplicationID, ref  ApplicantPersonID, ref  ApplicationDate, ref  ApplicationTypeID, ref  ApplicationStatus, ref  LastStatusDate,
                                     ref  PaidFees, ref  CreatedByUserID
                                );




            if (IsFound)
                //we return new object of that person with the right data
                return new clsApplications(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus,  LastStatusDate,
                                      PaidFees, CreatedByUserID);



            else
                return null;
        }


        private bool _AddNewApp()
        {
            // Call DataAccess Layer
            this.ApplicationID = clsApplicationsDataAccess.AddNewApplication(
                this.ApplicantPerson.PersonID,
                this.ApplicationDate,
                this.ApplicationTypeID,
                this.ApplicationStatus,
                this.LastStatusDate,
                this.PaidFees,
                this.CreatedByUserID
            );

            return (this.ApplicationID != -1); // Return true if ApplicationID is valid
        }


        private void _AssignObjectClass()
        {
            switch (ApplicationTypeID)
            { 
                case 1:
                    applicationType = enApplicationType.NewLocalDrivingLicenseService;
                    break;
                default:
                    break;



            }



        }

        public bool Save()
        {

            _AssignObjectClass();

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApp())
                    {
                        if (applicationType == enApplicationType.NewLocalDrivingLicenseService)
                        {
                            LocalDrivingLicenseApplication.ApplicationID = this.ApplicationID;
                           return LocalDrivingLicenseApplication.Save();
                        
                        }


                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateApp();

                default:
                    return false;
            }
        }

        private clsApplications(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, Decimal PaidFees, int CreatedByUserID)
        {
            Mode = enMode.Update;
            this.ApplicationID = ApplicationID;
            this.ApplicantPerson = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            if (ApplicationTypeID == 1)
            {
                this.applicationType = enApplicationType.NewLocalDrivingLicenseService;
                this.LocalDrivingLicenseApplication= clsLocalDrivingLicenseApplications.FindByApplicationId(ApplicationID);

            }
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;



        }
        public static clsApplications FindByLocalDrivingLicenceApplicationID(int LocalDrivingLicenceApplicationID)
        {
            //using Local Driving License Application ID here we return the ApplicationID of the Local Driving License Application that we needs
            int ApplicationID = clsApplicationsDataAccess.GetApplicationIDByLocalDrivingLicenseApplicationID(LocalDrivingLicenceApplicationID);

            if (ApplicationID == -1)
            {
                return null;

            }
            else
            {
                return clsApplications.Find(ApplicationID);


            }

        }

        private bool _UpdateApp()
        {
            // Call DataAccess Layer
            return clsApplicationsDataAccess.UpdateApplication(
                this.ApplicationID,
                this.ApplicantPerson.PersonID,
                this.ApplicationDate,
                this.ApplicationTypeID,
                this.ApplicationStatus,
                this.LastStatusDate,
                this.PaidFees,
                this.CreatedByUserID
            );
        }

        public static bool DeleteApplication(ref int applicationID)
        {
            return clsApplicationsDataAccess.DeleteApplication(ref applicationID);
        }
    }
}
