using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsCountry
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }

        private clsCountry(int CountryID, string CountryName) 
        { 
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }

        public clsCountry(string CountryName)
        {
            this.CountryID = FindByName(CountryName).CountryID;
            this.CountryName = CountryName;
        }

        public clsCountry(int CountryID)
        {
            this.CountryID = CountryID;
            this.CountryName = FindByID(CountryID).CountryName;
        }


        public static clsCountry FindByID(int ID)
        {

            string CountryName = "";
            DateTime DateOfBirth = DateTime.Now;
            int CountryID = -1;

            if (clsCountryDataAccess.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }

        public static clsCountry FindByName(string CountryName)
        {

            int ID = -1;


            if (clsCountryDataAccess.GetCountryInfoByName(CountryName, ref ID))

                return new clsCountry(ID, CountryName);
            else
                return null;

        }
        public static DataTable GetAllCountries()
        {
            return clsCountryDataAccess.GetAllCountries();

        }


    }
}
