using DVLD.Users;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.Local_Driving_License_Application
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {

        enum enFrmMode  {AddNew ,Update}
        enFrmMode _Mode = enFrmMode.AddNew;


        DataTable _licensesClasses = clsLicenseClasses.GetAllLicenseClasses();

        clsApplications _Application;


        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Application = new clsApplications();
            _Mode = enFrmMode.AddNew;
        }


        public frmAddUpdateLocalDrivingLicesnseApplication(int  LocalDrivingLicenceApplicationID)
        {
            InitializeComponent();
            _Application=clsApplications.FindByLocalDrivingLicenceApplicationID(LocalDrivingLicenceApplicationID);


            _Mode = enFrmMode.Update;
        }

        void _RestDefaultValues()
        {
            if (_Mode == enFrmMode.AddNew)
            {
                lblTitle.Text = "New local Driving License Application";
                cbLicenseClass.SelectedIndex = 2;

                foreach (Control control in tcApplicationInfo.Controls)
                {
                    control.Enabled = false;

                }
            }
            else
            {
                lblTitle.Text = "Update local Driving License Application";
                ctrlPersonCardWithFilter1.LoadPersonInfo(_Application.ApplicantPerson.NationalNo);
                ctrlPersonCardWithFilter1.gbFilters.Enabled = false;
                

            }

           



        }
        void _FillApplicationInfo()
        {
            if (_Mode == enFrmMode.Update)
            {
                lblApplicationDate.Text = _Application.ApplicationDate.ToString();
                cbLicenseClass.SelectedIndex = _Application.LocalDrivingLicenseApplication.LicenseClassID-1;
                lblFees.Text = _Application.PaidFees.ToString();
                lblLocalDrivingLicebseApplicationID.Text = _Application.LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationsID.ToString();
                lblCreatedByUser.Text = clsUser.FindUserById(_Application.CreatedByUserID).UserName;

            }
            else
            {
                lblApplicationDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lblFees.Text= clsApplicationTypes.FindById(1).ApplicationFees.ToString();
                lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;
                cbLicenseClass.SelectedIndex = 2;

            }



        }
        void _FillComboBox()
        {
            foreach (DataRow row in _licensesClasses.Rows)
            {
                cbLicenseClass.Items.Add(row[1].ToString());
            }

        }
        void _FillNewApplicationObject()
        {
            if (_Mode == enFrmMode.Update)
            {
                //set the new LicenseClassID
                _Application.LocalDrivingLicenseApplication.LicenseClassID = cbLicenseClass.SelectedIndex + 1;
            }
            else
            {

                //fill the application properties
                _Application.ApplicationDate = DateTime.Now;
                _Application.ApplicationTypeID = 1;
                _Application.ApplicationStatus = 1;
                _Application.LastStatusDate = DateTime.Now;
                _Application.PaidFees = clsApplicationTypes.FindById(1).ApplicationFees;
                _Application.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

                //fill the Local Driving License Application properties
                _Application.LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplications();
                _Application.LocalDrivingLicenseApplication.LicenseClassID = cbLicenseClass.SelectedIndex + 1;
            }
        }
        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {

            _FillComboBox();

            _RestDefaultValues();


            if (_Mode == enFrmMode.Update)
            {
                _FillApplicationInfo();
            }

        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enFrmMode.Update)
            {
                tcPersonInfo.SelectedIndex = 1;
            }
            else
            {
                //check is a person has selected
                if (ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID == -1)
                {
                    MessageBox.Show("Please select a person.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ctrlPersonCardWithFilter1.ctrlPersonCard1.ResetPersonInfo();
                    return;
                }

                //load the person to the object
                _Application.ApplicantPerson = clsPerson.Find(ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID);

                //fill person info
                _FillApplicationInfo();
                // disable the filtring to prevent the user from changing the person and active the next tab controls
                ctrlPersonCardWithFilter1.gbFilters.Enabled = false;
                
                foreach (Control control in tcApplicationInfo.Controls)
                {
                    control.Enabled = true;

                }
                tcPersonInfo.SelectedIndex = 1;

                btnSave.Enabled = true;


            }






        }

        private void cbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
    


            if (_Mode == enFrmMode.Update)
            {
                if (cbLicenseClass.SelectedIndex == _Application.LocalDrivingLicenseApplication.LicenseClassID - 1)
                {
                    btnSave.Enabled = false;

                }
                else
                {
                    
                    btnSave.Enabled = true;

                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (clsLocalDrivingLicenseApplications.IsNewApplicationRepeated(_Application.ApplicantPerson.NationalNo, cbLicenseClass.SelectedItem.ToString()))
            {
                MessageBox.Show("This person already has an application in progress for the same class.",
                                "Duplicate Application", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            if (_Mode == enFrmMode.Update)
            {


                _FillNewApplicationObject();

                if (_Application.LocalDrivingLicenseApplication.Save())
                {
                    MessageBox.Show("The application was updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The application update failed. Please try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            else
            {


                _FillNewApplicationObject();


                if (_Application.Save())
                {
                    // Notify the user of success
                    MessageBox.Show("The application was saved successfully.",
                                    "Save Successful",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    _Mode = enFrmMode.Update;
                    _RestDefaultValues();
                    _FillApplicationInfo();
                }
                else
                {
                    // Notify the user of failure
                    MessageBox.Show("Failed to save the application. Please try again.",
                                    "Save Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                }





            }

        }
    }
}
