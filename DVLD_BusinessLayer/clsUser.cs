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

         public static int VerifyUserCredentials(string username, string password)
        {
 
            return clsUserDataAccess.VerifyUserCredentials(username, password);
        }

        public static bool IsUserActive(int userId)
        {
            return clsUserDataAccess.IsUserActive(userId);
        }


    }
}
