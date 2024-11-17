using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Users
{
    public partial class ctrlUserCard : UserControl
    {
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public clsUser User;

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(User.PersonID);
            lblUserID.Text = User.UserID.ToString();
            lblUserName.Text = User.UserName.ToString();
            lblIsActive.Text = User.IsActive.ToString();
            if (User.IsActive == true)
            {
                lblIsActive.Text = "Yes";
            }
            else
                lblIsActive.Text = "No";



        }

       public void LoadUserCard(int userId)
        {
            User = clsUser.FindUserById(userId);

            if (User != null)
            {
                _FillUserInfo();
            }

        }

    }
}
