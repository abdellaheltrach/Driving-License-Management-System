using DVLD.Licenses.International_Licenses;
using DVLD.Licenses.Local_Licenses;
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

namespace DVLD.Applications.International_License_Application
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _LicenseID = -1;
        private clsLicense _LicenseInfo;

        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicenseInfo ;

        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }
        private void _FillTextBoxes()
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("dd/MMM/yyyy");//add one year.
            lblFees.Text = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;


        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            
            if (SelectedLicenseID == -1)
            {
                return;
            }

            //fill license 
            _LicenseID = SelectedLicenseID;
            _LicenseInfo = clsLicense.Find(ctrlDriverLicenseInfoWithFilter1.LicenseID);



            lblLocalLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = (SelectedLicenseID != -1);



            //check the license class, person could not issue international license without having
            //normal license of class 3.

            if (_LicenseInfo.LicenseClass != 3)
            {
                btnIssueLicense.Enabled = false;
                MessageBox.Show("Selected License should be Class Local driver license, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                btnIssueLicense.Enabled = true;

            }

            //check if person already have an active international license
            int ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(_LicenseInfo.DriverID);

            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;

                btnIssueLicense.Enabled = false;
                _InternationalLicenseID = ActiveInternaionalLicenseID;
                return;
            }
            else
            {
                llShowLicenseInfo.Enabled = false;

            }


        }


        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            _FillTextBoxes();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_LicenseInfo.DriverInfo.PersonID))
            { 
                frm.ShowDialog();
            
            }

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID))
            {
                frm.ShowDialog();

            }
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }






            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //those are the information for the base application, because it inhirts from application, they are part of the sub class.

            InternationalLicense.ApplicantPersonID = _LicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicantPerson = clsPerson.Find(InternationalLicense.ApplicantPersonID);
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;


            InternationalLicense.DriverID = _LicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = _LicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsCurrentUser.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbFilters.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }


    }
}
