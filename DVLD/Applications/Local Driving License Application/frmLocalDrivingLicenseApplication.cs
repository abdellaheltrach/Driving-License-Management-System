using DVLD.Licenses.Local_Licenses;
using DVLD.Tests;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Local_Driving_License_Application
{
    public partial class frmLocalDrivingLicenseApplication : Form
    {
        DataTable _ApplicationsTable;


        public frmLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void _PerformFiltering()
        {



            string filterColumn = "";

            // Determine the column to filter by
            if (cbFilterBy.SelectedIndex == 0)
            {
                // No filter selected; clear filters
                _ApplicationsTable.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;
            }
            else if (cbFilterBy.SelectedIndex == 1) // LocalDrivingLicenseApplicationID
            {
                filterColumn = "[L.D.L. AppID]";
            }
            else if (cbFilterBy.SelectedIndex == 2) // NationalNo
            {
                filterColumn = "[National NO.]";
            }
            else if (cbFilterBy.SelectedIndex == 3) // FullName
            {
                filterColumn = "[Full Name]";
            }
            else if (cbFilterBy.SelectedIndex == 4) // Status
            {
                filterColumn = "[Status]";

                // Handle filtering for Status column
                if (cbStatus.SelectedIndex == 0) // "All"
                {
                    _ApplicationsTable.DefaultView.RowFilter = string.Empty; // No filter applied
                }
                else if (cbStatus.SelectedIndex == 1) // "New"
                {
                    _ApplicationsTable.DefaultView.RowFilter = $"{filterColumn} = 'New'";
                }
                else if (cbStatus.SelectedIndex == 2) // "Completed"
                {
                    _ApplicationsTable.DefaultView.RowFilter = $"{filterColumn} = 'Completed'";
                }
                else if (cbStatus.SelectedIndex == 3) // "Cancelled"
                {
                    _ApplicationsTable.DefaultView.RowFilter = $"{filterColumn} = 'Cancelled'";
                }

                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;
            }

            // If the selected filter is numeric (LocalDrivingLicenseApplicationID)
            if (cbFilterBy.SelectedIndex == 1)
            {
                if (int.TryParse(txtFilterValue.Text.Trim(), out int filteringNumber))
                {
                    // Filter for exact numeric match
                    _ApplicationsTable.DefaultView.RowFilter = $"{filterColumn} = {filteringNumber}";
                }
                else if (txtFilterValue.Text.Trim() == string.Empty)
                {
                    // Clear the filter if the input is empty
                    _ApplicationsTable.DefaultView.RowFilter = string.Empty;
                    return;
                }
                else
                {
                    // Invalid input for numeric filtering
                    MessageBox.Show("Please enter a valid numeric value for the selected filter.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else
            {
                // For text-based filtering (e.g., NationalNo, FullName)
                string filteringString = txtFilterValue.Text.Trim();

                if (!string.IsNullOrEmpty(filteringString))
                {
                    // Apply partial text match using LIKE
                    _ApplicationsTable.DefaultView.RowFilter = $"{filterColumn} LIKE '%{filteringString}%'";
                }
                else
                {
                    // Clear the filter if the input is empty
                    _ApplicationsTable.DefaultView.RowFilter = string.Empty;
                }
            }

            // Update the record count label after filtering
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
        }

        private void _ReloadUserList()
        {
            _ApplicationsTable = clsLocalDrivingLicenseApplications.GetAllApplications();

            dgvLocalDrivingLicenseApplications.DataSource = _ApplicationsTable;
            cbFilterBy.SelectedIndex = 2;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 200;
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 250;
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 150;


            }

        }
        private void frmLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ReloadUserList();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 0)
            {
                //none selected
                _PerformFiltering();
                txtFilterValue.Enabled = false;
                cbStatus.Enabled = false;

                txtFilterValue.Visible = false;
                cbStatus.Visible = false;
            }
            else if (cbFilterBy.SelectedIndex == 4)
            {
                //status selected
                cbStatus.SelectedIndex = 0;
                txtFilterValue.Enabled = false;
                cbStatus.Enabled = true;

                txtFilterValue.Visible = false;
                cbStatus.Visible = true;
            }
            else 
            {
                //text filtring selected
                txtFilterValue.Text = string.Empty;
                txtFilterValue.Enabled = true;
                cbStatus.Enabled = false;


                txtFilterValue.Visible = true;
                cbStatus.Visible = false;
            }

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 1 )
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _PerformFiltering();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _PerformFiltering();

        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            using (frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication())
            {
                frm.ShowDialog();
            
            }
            _ReloadUserList();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e) 
        {
            using (frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value))
            {
                frm.ShowDialog();

            }
            _ReloadUserList();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;


             clsLocalDrivingLicenseApplications LocalDrivingLicenseApplication =
             clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    _ReloadUserList();
                }
                else
                {
                    MessageBox.Show("Could not delete applicatoin, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmLocalDrivingLicenseApplicationInfo frm =  new frmLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value))
            { 
                frm.ShowDialog();
            
            
            
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplications CourantApplication = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            int TotalPassedTestes = CourantApplication.GetPassedTestCount();

            //handle tests
            switch (TotalPassedTestes)
            {
                case 0:
                    scheduleVisionTestToolStripMenuItem.Enabled = true;
                    scheduleWrittenTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;
                case 1:
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWrittenTestToolStripMenuItem.Enabled = true;
                    scheduleStreetTestToolStripMenuItem.Enabled = false;
                    break;
                case 2:
                    scheduleVisionTestToolStripMenuItem.Enabled = false;
                    scheduleWrittenTestToolStripMenuItem.Enabled = false;
                    scheduleStreetTestToolStripMenuItem.Enabled = true;
                    break;
                case 3:
                    sechduleTestsToolStripMenuItem.Enabled = false;
                    if (CourantApplication.ApplicationStatus == clsApplications.enApplicationStatus.New)
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;

                        showLicenseToolStripMenuItem.Enabled = false;

                    }
                    else
                    {
                        issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;

                        //handle License
                        if (CourantApplication.GetActiveLicenseID() > 0)
                        {

                            //have License 

                            showLicenseToolStripMenuItem.Enabled = true;


                        }
                        else
                        {
                            //does not have License 

                            showLicenseToolStripMenuItem.Enabled = false;


                        }

                    }

                    break;

            }



        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmListAppointments frm = new frmListAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.VisionTest))
            {
                frm.ShowDialog();
            }
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmListAppointments frm = new frmListAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.WrittenTest))
            {
                frm.ShowDialog();
            }
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmListAppointments frm = new frmListAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestTypes.enTestType.StreetTest))
            {
                frm.ShowDialog();
            }
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            using (frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID))
            {

                frm.ShowDialog();

            }
             
           
            //refresh
            _ReloadUserList();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            int LicenseID = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID).GetActiveLicenseID();


            using (frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID))
            {
                
                frm.ShowDialog();

            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsPerson.Find((string)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[2].Value).PersonID;


            using (frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID))
            {

                frm.ShowDialog();

            }
        }
    }
}