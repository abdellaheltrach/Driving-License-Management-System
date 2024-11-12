using DVLD.Login;
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


        PeopleForm peopleForm = new PeopleForm();
        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (peopleForm == null || peopleForm.IsDisposed)  // إذا تم إغلاقه، يتم إنشاء نموذج جديد
            {
                 peopleForm = new PeopleForm();
            }

            peopleForm.Owner = this;
            peopleForm.Show();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Close this form (main form)
            this.Close();
  

        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frmUserInfo = new frmUserInfo(clsCurrentUser.CurrentUser.UserID);
            frmUserInfo.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsCurrentUser.CurrentUser.UserID);
            frm.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.Owner = this;
            usersForm.Show();
        }
    }
}
