using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsApplicationTypes
    {

        public int ApplicationTypeID { get; set; }
        public string ApplicationTypeTitle { get; set; }
        public decimal ApplicationFees { get; set; }


        clsApplicationTypes(int applicationTypeID, string title, int fees) 
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTypeTitle = title;
            this.ApplicationFees = fees;

        }

        public static clsApplicationTypes FindById(int applicationTypeID)
        {
             string title="";
             int fees=0;

            bool isFound= clsApplicationTypesDataAccessLayer.FindApplicationTypeById(applicationTypeID, ref title, ref fees);

            if (isFound)
            {
                return new clsApplicationTypes(applicationTypeID, title, fees);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetApplicationTypes()
        {
            return clsApplicationTypesDataAccessLayer.GetAllApplicationTypes();
        }

        static public bool UpdateApplicationType(int ApplicationTypeID,string ApplicationTypeTitle,int ApplicationFees)
        {
            return clsApplicationTypesDataAccessLayer.UpdateApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);
        }
    }
}
