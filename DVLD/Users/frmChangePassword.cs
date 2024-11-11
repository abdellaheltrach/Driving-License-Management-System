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
    public partial class frmChangePassword : Form
    {
        public frmChangePassword(int UserId)
        {
            InitializeComponent();
            ctrlUserCard1.LoadUserCard(UserId);
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            //validating if the textbox empty
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

            //validating password
            if (clsUser.IsPasswordCorrect(ctrlUserCard1.User.UserID, ctrlUserCard1.User.Password))
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            // Verify the current password before proceeding
            if (!clsUser.IsPasswordCorrect(ctrlUserCard1.User.UserID, txtCurrentPassword.Text))
            {
                errorProvider1.SetError(txtCurrentPassword, "password is incorrect!");

                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);

            }



            //  ensure new password and confirmation password match
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "New Password does not match!");
                txtConfirmPassword.Text = string.Empty;
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null); // Clear any previous error

            }



            // Attempt to change the password
            if (clsUser.ChangePassword(ctrlUserCard1.User.UserID, txtNewPassword.Text))
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to change the password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
