using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLD_Buisness;
using DVLD_BusinessLayer;
using DVLD_DataAccessLayer;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsApplications
    {


        public int ApplicationID { get; set; }
        public int ApplicantPersonID;
        public clsPerson ApplicantPerson;
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }

        public clsApplicationTypes ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { get; set; }
        public string StatusText
        {
            get
            {

                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }

        }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID;

        public clsUser CreatedByUserInfo;


        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode;

        public enum enApplicationType
        {
            NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService = 2, ReplacementForALostDrivingLicense = 3,
            ReplacementForADamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7


        }
        private enApplicationType applicationType;

        public enum enApplicationStatus { New=1 , Completed = 2 , Cancelled = 3 }

        public clsApplications()
        {
            Mode = enMode.AddNew;
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicantPerson = null;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = 0;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = -1;
            this.CreatedByUserID = -1;



        }


        private clsApplications(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, byte ApplicationStatus, DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
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
            Mode = enMode.Update;


        }




        public static clsApplications Find(int ApplicationID)
        {
            int ApplicantPersonID = -1;
            DateTime ApplicationDate = DateTime.Now;
            int ApplicationTypeID = -1;
            byte ApplicationStatus = 0; 
            DateTime LastStatusDate = DateTime.Now;
            float PaidFees = -1; 
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
                (byte)this.ApplicationStatus,
                this.LastStatusDate,
                this.PaidFees,
                this.CreatedByUserID
            );

            return (this.ApplicationID != -1); // Return true if ApplicationID is valid
        }




        public bool Save()
        {


            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApp())
                    {


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




        private bool _UpdateApp()
        {
            // Call DataAccess Layer
            return clsApplicationsDataAccess.UpdateApplication(
                this.ApplicationID,
                this.ApplicantPerson.PersonID,
                this.ApplicationDate,
                this.ApplicationTypeID,
                (byte)this. ApplicationStatus,
                this.LastStatusDate,
                this.PaidFees,
                this.CreatedByUserID
            );
        }

        public bool DeleteApplication()
        {
            return clsApplicationsDataAccess.DeleteApplication(this.ApplicationID);
        }
    }
}
