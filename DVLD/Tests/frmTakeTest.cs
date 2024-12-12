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
using static DVLD_BusinessLayer.clsTestTypes;

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {
        private clsTestTypes.enTestType _TestType;

        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;

        private int _TestID = -1;
        private clsTest _Test = new clsTest();

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;


        public int TestAppointmentID
        {
            get
            {
                return _TestAppointmentID;
            }
        }


        public frmTakeTest(int TestAppointmentID)
        {
            InitializeComponent();
            this._TestAppointmentID=TestAppointmentID;
            this._TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            this._TestType = _TestAppointment.TestTypeID;


        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {


            //incase we did not find any appointment .
            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No  Appointment ID = " + _TestAppointmentID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }

            _TestID = _TestAppointment.TestID;
            _LocalDrivingLicenseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationsID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassesInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.ApplicantPerson.FullName;


            //this will show the trials for this test before 
            //lblTrial.Text = _LocalDrivingLicenseApplication.GetPassedTestCount(_TestTypeID).ToString();
            lblTrial.Text = clsTestAppointment.CountTestTrails(this._TestAppointment.LocalDrivingLicenseApplicationID, clsTestTypes.FindById(_TestType).TestTypeTitle).ToString();



            lblDate.Text =_TestAppointment.AppointmentDate.ToString("dd/MM/yyyy");
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblTestID.Text = (_TestAppointment.TestID == -1) ? "Not Taken Yet" : _TestAppointment.TestID.ToString();



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                        "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No
               )
            {
                return;
            }

            _Test.TestAppointmentID = _TestAppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
