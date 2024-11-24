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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Users
{
    public partial class frmAddNewOrUpdateUser : Form
    {
        private enum enMode { enAddNew  , enUpdate  }
        enMode _FrmMode;

        int UserId = -1;
        clsUser _User;

        private void _LoadData()
        {
            if (_FrmMode == enMode.enUpdate)
            {

                lblUserID.Text = UserId.ToString();
                txtUserName.Text = _User.UserName;
                txtPassword.Text = _User.Password;
                txtConfirmPassword.Text = _User.Password;
                chkIsActive.Checked = _User.IsActive;
            }
            else 
            {
                lblUserID.Text = string.Empty;
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtConfirmPassword.Text = string.Empty;
                chkIsActive.Checked = true;
            }






        }

        private void _ResetDefaultValues(enMode mode, int userId=0)
        {
            _FrmMode = mode;

            if (mode == enMode.enUpdate)
            {

                _User = clsUser.FindUserById(userId);
                lblTitle.Text = "Update User";
                ctrlPersonCardWithFilter1.ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
                ctrlPersonCardWithFilter1.gbFilters.Enabled = false;


                _LoadData();


                // Enable all controls in the Login Info tab for updates
                foreach (Control control in tpLoginInfo.Controls)
                {
                    control.Enabled = true;
                }
            }
            else
            {
                lblTitle.Text = "Add New User";

                _LoadData();

                foreach (Control control in tpLoginInfo.Controls)
                {
                    control.Enabled = false;
                }

                ctrlPersonCardWithFilter1.gbFilters.Enabled = true;
                btnSave.Enabled = false;

            }
        }

        public frmAddNewOrUpdateUser()
        {
            InitializeComponent();
            _ResetDefaultValues(enMode.enAddNew);
        }

        public frmAddNewOrUpdateUser(int UserID)
        {
            InitializeComponent();
            this.UserId=UserID;
            _ResetDefaultValues(enMode.enUpdate, UserID);

        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_FrmMode == enMode.enUpdate)
            {
                tcUserInfo.SelectedIndex = 1;
                return;

            }
            else
            {
                if (ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID == -1)
                {
                    //no person selected
                    MessageBox.Show("Please select a person.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ctrlPersonCardWithFilter1.ctrlPersonCard1.ResetPersonInfo();
                    return;
                }
                else if (clsUser.IsUserExists(ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID))
                {
                    //no person selected

                    MessageBox.Show("This person is already a user.", "User Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                else
                {
                    tcUserInfo.SelectedIndex = 1;
                    ctrlPersonCardWithFilter1.gbFilters.Enabled = false;

                    foreach (Control control in tpLoginInfo.Controls)
                    {
                        control.Enabled = true;
                    }
                }


            }



        
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            

            //validating if the password matches
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                errorProvider1.SetError(txtConfirmPassword, "Password does not match!");
                txtConfirmPassword.Text = string.Empty;
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null); // Clear any previous error

            }

            bool isActive = chkIsActive.Checked ;

            if (_FrmMode == enMode.enAddNew)
            {
                int userId = clsUser.AddNewUser(
                    ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID,
                    txtUserName.Text.Trim(),
                    txtPassword.Text.Trim(),
                    isActive
                );

                if (userId > 0)
                {
                    MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblUserID.Text = userId.ToString();
                    btnSave.Enabled = false;
                    _ResetDefaultValues(enMode.enUpdate, userId);



                }
                else
                {
                    MessageBox.Show("Failed to add user. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Handle Update User
            else if (_FrmMode == enMode.enUpdate)
            {
                bool isUpdated = clsUser.UpdateUser(
                    int.Parse(lblUserID.Text.Trim()),
                    txtUserName.Text.Trim(),
                    txtPassword.Text.Trim(),
                    isActive
                );

                if (isUpdated)
                {
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ResetDefaultValues(enMode.enUpdate, int.Parse(lblUserID.Text.Trim()));

                }
                else
                {
                    MessageBox.Show("Failed to update user. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }




        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);


            if (_FrmMode == enMode.enUpdate && Temp.Text.Trim() == _User.UserName)
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);

                btnSave.Enabled = false;
            }


            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");


                btnSave.Enabled = false;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);

                btnSave.Enabled = true;
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);


            if (_FrmMode == enMode.enUpdate && Temp.Text.Trim() == _User.Password)
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);

                btnSave.Enabled = false;
            }


            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");


                btnSave.Enabled = false;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);

                btnSave.Enabled = true;
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox)sender);




            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");


                btnSave.Enabled = false;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(Temp, null);

                btnSave.Enabled = true;
            }
        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {


            if (_FrmMode == enMode.enUpdate && chkIsActive.Checked == _User.IsActive)
            {
                btnSave.Enabled = false;
            }
            else 
            { 
                btnSave.Enabled = true;
            }



        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }
    }
}
