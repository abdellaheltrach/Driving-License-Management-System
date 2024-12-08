using DVLD.Properties;
using DVLD.Users;
using DVLD_Buisness;
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
using System.Windows.Forms.VisualStyles;
using static DVLD_BusinessLayer.clsTestTypes;

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {

        public enum enMode { AddNew = 0, Update = 1 ,s};
        private enMode _Mode = enMode.AddNew;

        clsLocalDrivingLicenseApplications _localDrivingLicenseApplication;
        clsTestTypes.enTestType _TestType;
        clsTestAppointment _TestAppointment;

        public frmScheduleTest(int TestAppointment)
        {
            InitializeComponent();
            this._TestAppointment = clsTestAppointment.Find(TestAppointment);
            this._TestType = _TestAppointment.TestTypeID;
            this._Mode = enMode.Update;

        }
        public frmScheduleTest(int LocalDrivingLiecenseApplicationID, clsTestTypes.enTestType TestType )
        {
            InitializeComponent();
            this._localDrivingLicenseApplication = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLiecenseApplicationID);
            this._TestType = TestType;
            this._Mode = enMode.AddNew;
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {

                case clsTestTypes.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestTypes.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestTypes.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }

            _FillApplicationInfo();
        }
        private void _FillApplicationInfo()
        {
            if (_localDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _localDrivingLicenseApplication.LocalDrivingLicenseApplicationsID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }


            lblLocalDrivingLicenseAppID.Text= _localDrivingLicenseApplication.LocalDrivingLicenseApplicationsID.ToString();
            lblDrivingClass.Text = clsLicenseClasses.Find(_localDrivingLicenseApplication.LicenseClassID).ClassName.Substring(10);
            lblFullName.Text= _localDrivingLicenseApplication.ApplicantPerson.FullName;
            lblTrial.Text = clsTestAppointment.CountTestTrails(this._localDrivingLicenseApplication.LocalDrivingLicenseApplicationsID,clsTestTypes.FindById(_TestType).TestTypeTitle).ToString();
            dtpTestDate.Value = DateTime.Now.AddDays(7);
            dtpTestDate.MinDate = DateTime.Now; 
            lblFees.Text = clsTestTypes.FindById(_TestType).TestTypeFees.ToString();
            if (int.Parse(lblTrial.Text) == 0)
            {
                //disable retake group 

                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
                lblTotalFees.Text = ((int.Parse(lblFees.Text) + int.Parse(lblRetakeAppFees.Text)).ToString());
                gbRetakeTestInfo.Enabled = false;

            }
            else 
            {
                //set retake test info

                lblRetakeAppFees.Text = clsApplicationTypes.Find("Retake Test").Fees.ToString();
                lblRetakeTestAppID.Text = "N/A";
                lblTotalFees.Text = ((int.Parse(lblFees.Text)+int.Parse(lblRetakeAppFees.Text)).ToString());
                gbRetakeTestInfo.Enabled = true;
            }




        }


        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _TestAppointment = new clsTestAppointment();

            _TestAppointment.TestTypeID = _TestType;
            _TestAppointment.LocalDrivingLicenseApplicationID = _localDrivingLicenseApplication.LocalDrivingLicenseApplicationsID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;
            _TestAppointment.IsLocked = false;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
    
}
