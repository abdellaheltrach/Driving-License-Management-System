using DVLD.Applications.Applications_types;
using DVLD.Login;
using DVLD.test_types;
using DVLD.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }



        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PeopleForm peopleForm = new PeopleForm())
            {
                peopleForm.ShowDialog();

            }

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Close this form (main form)
            this.Close();
  

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmUserInfo frmUserInfo = new frmUserInfo(clsCurrentUser.CurrentUser.UserID))
            {
                frmUserInfo.ShowDialog();
            }
                
        
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmChangePassword frm = new frmChangePassword(clsCurrentUser.CurrentUser.UserID))
            {
                frm.ShowDialog();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (UsersForm usersForm = new UsersForm())
            {
                usersForm.ShowDialog();
            }

        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmApplicationListTypes frm = new frmApplicationListTypes())
            {
                frm.ShowDialog();


            }
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmListTestTypes frm = new frmListTestTypes())
            {
                frm.ShowDialog();


            }
        }
    }
}
