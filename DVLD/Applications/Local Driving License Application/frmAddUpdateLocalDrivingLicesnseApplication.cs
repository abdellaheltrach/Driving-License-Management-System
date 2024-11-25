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
                _FillApplicationInfo();

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


            }



        }
        void _FillComboBox()
        {
            foreach (DataRow row in _licensesClasses.Rows)
            {
                cbLicenseClass.Items.Add(row[1].ToString());
            }

        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {

            _FillComboBox();

            _RestDefaultValues();

        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enFrmMode.Update)
            {
                tcPersonInfo.SelectedIndex = 1;
            }
            else
            {
                if (ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID == -1)
                {
                    MessageBox.Show("Please select a person.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ctrlPersonCardWithFilter1.ctrlPersonCard1.ResetPersonInfo();
                    return;
                }
                else if (clsUser.IsUserExists(ctrlPersonCardWithFilter1.ctrlPersonCard1.PersonID))
                {
                    tcPersonInfo.SelectedIndex = 1; 
                    ctrlPersonCardWithFilter1.gbFilters.Enabled = false;
                    foreach (Control control in tcApplicationInfo.Controls)
                    {
                        control.Enabled = true;

                    }
                    return;
                }

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
            if (clsLocalDrivingLicenseApplications.IsNewApplicationRepeated(_Application.ApplicantPerson.NationalNo, cbLicenseClass.SelectedIndex.ToString()))
            {
                MessageBox.Show("This person already has an application in progress for the same class.",
                                "Duplicate Application", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            if (_Mode == enFrmMode.Update)
            {
                _Application.LocalDrivingLicenseApplication.LicenseClassID = cbLicenseClass.SelectedIndex + 1;

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

        }
    }
}
