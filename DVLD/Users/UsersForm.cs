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
    public partial class UsersForm : Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        DataTable _UsersTable = clsUser.GetAllUsers();

        private void _PreformeFiltring()
        {
            string filterColumn = "";

            if (cbFilterBy.SelectedIndex == 0)
            {
                // No filter selected; clear any existing filters
                _UsersTable.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }
            else if (cbFilterBy.SelectedIndex == 1)
            {
                filterColumn = "UserID";
            }
            else if (cbFilterBy.SelectedIndex == 2)
            {
                filterColumn = "UserName";
            }
            else if (cbFilterBy.SelectedIndex == 3)
            {
                filterColumn = "PersonID";
            }
            else if (cbFilterBy.SelectedIndex == 4)
            {
                filterColumn = "FullName";
            }
            else if (cbFilterBy.SelectedIndex == 5)
            {
                filterColumn = "IsActive";

                // Handle filtering based on the selected index
                if (cbIsActive.SelectedIndex == 0) // "All"
                {
                    // No filter applied, show all records
                    _UsersTable.DefaultView.RowFilter = string.Empty;
                }
                else if (cbIsActive.SelectedIndex == 1) // "Yes" (Active)
                {
                    // Filter for records where IsActive = true
                    _UsersTable.DefaultView.RowFilter = $"{filterColumn} = true";
                }
                else if (cbIsActive.SelectedIndex == 2) // "No" (Inactive)
                {
                    // Filter for records where IsActive = false
                    _UsersTable.DefaultView.RowFilter = $"{filterColumn} = false";
                }

                // Update the record count label after filtering
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }


            // Check if the selected filter is numeric
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 3)
            {
                // Attempt to parse the filter value as an integer
                if (int.TryParse(txtFilterValue.Text.Trim(), out int filteringNumber))
                {
                    // Apply filter for exact numeric match
                    _UsersTable.DefaultView.RowFilter = $"{filterColumn} = {filteringNumber}";
                }
                else if (txtFilterValue.Text.Trim() == string.Empty)
                {
                    _UsersTable.DefaultView.RowFilter = string.Empty;
                    //  MessageBox.Show("Please enter a valid numeric value for the selected filter.");
                    return;
                }
            }
            else
            {
                // For text-based filtering (e.g., FullName, UserName, IsActive)
                string filteringString = txtFilterValue.Text.Trim();

                // Apply filter for partial text match using LIKE


                _UsersTable.DefaultView.RowFilter = $"{filterColumn} LIKE '%{filteringString}%'";
            }

            // Update the record count label after filtering
            lblRecordsCount.Text = dgvUsers.RowCount.ToString();
        }
        /*
        private void _PreformeFiltring()
        {
            //case one empty filter text

            if (txtFilterValue.Text==string.Empty)
            {
                _UsersTable.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }


            string filterColumn = "";

            if (cbFilterBy.SelectedIndex == 0)
            {
                // No filter selected; clear any existing filters
                _UsersTable.DefaultView.RowFilter = string.Empty;
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }
            else if (cbFilterBy.SelectedIndex == 1)
            {
                filterColumn = "UserID";
                if (int.TryParse(txtFilterValue.Text.Trim(), out int filteringNumber))
                {
                    // Apply filter for exact numeric match
                    _UsersTable.DefaultView.RowFilter = $"{filterColumn} = {filteringNumber}";
                }
            }
            else if (cbFilterBy.SelectedIndex == 3)
            {
                filterColumn = "PersonID";
                if (int.TryParse(txtFilterValue.Text.Trim(), out int filteringNumber))
                {
                    // Apply filter for exact numeric match
                    _UsersTable.DefaultView.RowFilter = $"{filterColumn} = {filteringNumber}";
                }
            }
            else if (cbFilterBy.SelectedIndex == 2)
            {

                string filteringString = txtFilterValue.Text.Trim();

                // Apply filter for partial text match using LIKE
                _UsersTable.DefaultView.RowFilter = string.Format("UserName LIKE '{1}%'", filteringString);
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }
            else if (cbFilterBy.SelectedIndex == 4)
            {

                string filteringString = txtFilterValue.Text.Trim();

                // Apply filter for partial text match using LIKE
                _UsersTable.DefaultView.RowFilter = string.Format("FullName LIKE '{1}%'", filteringString);
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }
            else if (cbFilterBy.SelectedIndex == 5)
            {

                // Handle filtering based on the selected index
                if (cbIsActive.SelectedIndex == 0) // "All"
                {
                    // No filter applied, show all records
                    _UsersTable.DefaultView.RowFilter = string.Empty;
                }
                else if (cbIsActive.SelectedIndex == 1) // "Yes" (Active)
                {
                    // Filter for records where IsActive = true
                    _UsersTable.DefaultView.RowFilter = "IsActive = true";
                }
                else if (cbIsActive.SelectedIndex == 2) // "No" (Inactive)
                {
                    // Filter for records where IsActive = false
                    _UsersTable.DefaultView.RowFilter = "IsActive = false";
                }

                // Update the record count label after filtering
                lblRecordsCount.Text = dgvUsers.RowCount.ToString();
                return;
            }


        }
        */


        private void UsersForm_Load(object sender, EventArgs e)
        {
            dgvUsers.DataSource = _UsersTable;
            lblRecordsCount.Text=dgvUsers.RowCount.ToString();
            cbFilterBy.SelectedIndex= 0;


            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[1].HeaderText = "Person ID";

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 200;

                dgvUsers.Columns[3].HeaderText = "User Name";
                dgvUsers.Columns[4].HeaderText = "Is Active";

            }


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;

            if (cbFilterBy.SelectedIndex == 0)
            {
                txtFilterValue.Enabled = false;
                txtFilterValue.Visible = false;

            }
            else if (cbFilterBy.SelectedIndex == 5)
            {
                txtFilterValue.Enabled = false;
                txtFilterValue.Visible = false;


                cbIsActive.Enabled = true;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;

            }
            else
            {
                
                cbIsActive.Enabled = false;
                cbIsActive.Visible = false;


                txtFilterValue.Enabled = true;
                txtFilterValue.Visible = true;

                txtFilterValue.Text = string.Empty;
            }




        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex == 1 || cbFilterBy.SelectedIndex == 3)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            _PreformeFiltring();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            _PreformeFiltring();
        }
    }
}
