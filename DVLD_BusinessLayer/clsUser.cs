using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccessLayer;

namespace DVLD_BusinessLayer
{
    public class clsUser
    {

        public int UserID { get; private set; }
        public int PersonID { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public bool IsActive { get; private set; }

        private clsUser(int userId, int personId, string userName, string password, bool isActive)
        {
            this.UserID = userId;
            this.PersonID = personId;
            this.UserName = userName;
            this.Password = password;
            this.IsActive = isActive;
        }

         public static int VerifyUserCredentials(string username, string password)
        {
 
            return clsUserDataAccess.VerifyUserCredentials(username, password);
        }



        public static bool IsUserActive(int userId)
        {
            return clsUserDataAccess.IsUserActive(userId);
        }
        public static clsUser FindUserById(int userId)
        {

            int _personId = -1 ;
            string _userName = "" ;
            string _password = "" ;
            bool _isActive = false;



            bool isFound =  clsUserDataAccess.FindUserById(userId, ref  _personId, ref  _userName, ref  _password, ref  _isActive);

            if (isFound)
            {
                return new clsUser(userId, _personId, _userName, _password, _isActive);

            }
            else
                return null;

        }


    }
}
