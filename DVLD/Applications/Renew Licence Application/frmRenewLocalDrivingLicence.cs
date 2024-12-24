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

namespace DVLD.Applications.Renew_Licence_Application
{
    public partial class frmRenewLocalDrivingLicence : Form
    {
        private int _SelectedLicenseID;
        private clsLicense _SelectedLicenseInfo;


        //after renew

        int _NewLicenseID;

        public frmRenewLocalDrivingLicence()
        {
            InitializeComponent();
        }


        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;
            _SelectedLicenseInfo = clsLicense.Find(_SelectedLicenseID);

            if (_SelectedLicenseInfo == null)
            {
                return;
            }
            else if (_SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class Local driver license, Please select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlDriverLicenseInfoWithFilter1.SetFocusOnFilterTextBox();
                btnRenewLicense.Enabled = false;
                llShowLicenseHistory.Enabled = true; //enable the user to check the person licenses history
                llShowLicenseInfo.Enabled = false;
                return;

            }
            else if (_SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("Selected License is not yet expired and still useable, it will expire on: " + _SelectedLicenseInfo.ExpirationDate.ToString("dd/MMM/yyyy"), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                llShowLicenseHistory.Enabled = true; //enable the user to check the person licenses history
                llShowLicenseInfo.Enabled = false;
                return;
            }
            else if (_SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License still Active and useable", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                llShowLicenseHistory.Enabled = true; //enable the user to check the person licenses history
                llShowLicenseInfo.Enabled = false;
                return;
            }
            else
            {
                btnRenewLicense.Enabled = true;
                llShowLicenseHistory.Enabled = true; //enable the user to check the person licenses history

                int DefaultValidityLength = _SelectedLicenseInfo.LicenseClassIfo.DefaultValidityLength;
                lblExpirationDate.Text = DateTime.Now.AddYears(DefaultValidityLength).ToString("dd/MMM/yyyy");
                lblLicenseFees.Text = _SelectedLicenseInfo.LicenseClassIfo.ClassFees.ToString();
                lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
                txtNotes.Text = _SelectedLicenseInfo.Notes;

            }



        }
        private void _FillTextBoxes()
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewDrivingLicenseService).Fees.ToString();
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;

        }

        private void frmRenewLocalDrivingLicence_Load(object sender, EventArgs e)
        {
            _FillTextBoxes();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();  
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(_SelectedLicenseInfo.DriverInfo.PersonID))
            {
                frm.ShowDialog();

            }
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID))
            {
                frm.ShowDialog();

            }
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense =
                _SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(),
                clsCurrentUser.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Renewed Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnRenewLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbFilters.Enabled = false;
            llShowLicenseInfo.Enabled = true;

        }

        private void frmRenewLocalDrivingLicence_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.SetFocusOnFilterTextBox();

        }
    }
}
