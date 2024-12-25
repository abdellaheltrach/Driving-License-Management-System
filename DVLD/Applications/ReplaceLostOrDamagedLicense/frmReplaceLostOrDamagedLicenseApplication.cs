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
using static DVLD_BusinessLayer.clsLicense;

namespace DVLD.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _SelectedLicenseID = -1;
        private clsLicense _SelectedLicenseInfo ;

        //after replace
        private int _NewLicenseID;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {
            //this will decide which application type to use accirding 
            // to user selection.

            if (rbDamagedLicense.Checked)

                return (int)clsApplications.enApplicationType.ReplacementForADamagedDrivingLicense;
            else
                return (int)clsApplications.enApplicationType.ReplacementForALostDrivingLicense;
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToString("dd/MMM/yyyy") ;
            lblCreatedByUser.Text = clsCurrentUser.CurrentUser.UserName;

            rbDamagedLicense.Checked = true;
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.SetFocusOnFilterTextBox();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            if (SelectedLicenseID == -1)
            {
                return;
            }

            _SelectedLicenseID= SelectedLicenseID;
            _SelectedLicenseInfo = clsLicense.Find(_SelectedLicenseID);

            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = (SelectedLicenseID != -1);


            if (!_SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
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
            using (frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID))
            {
                frm.ShowDialog();

            }
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            //get the issue reason
           clsLicense.enIssueReason IssueReason = rbDamagedLicense.Checked ? enIssueReason.DamagedReplacement : enIssueReason.LostReplacement;

            clsLicense NewLicense =
               _SelectedLicenseInfo.Replace(IssueReason,
               clsCurrentUser.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Failed to Issue a replacement for this  License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;

            lblRreplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.gbFilters.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationTypeID()).Fees.ToString();
        }
    }
}
