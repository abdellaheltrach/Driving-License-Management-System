using System;
using System.Data;
using System.Windows.Forms;
using DVLD_BusinessLayer;

namespace DVLD
{
    public partial class PeopleForm : Form
    {
        private static DataTable _PersonsTable = clsPerson.GetAllPeople();

        // Select only the columns to show in the grid
        private DataTable _dtPeople = _PersonsTable.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                                        "FirstName", "SecondName", "ThirdName",
                                                                        "LastName", "Gendor", "DateOfBirth",
                                                                        "CountryName", "Phone", "Email");
        

        public PeopleForm()
        {
            InitializeComponent();
        }

        private void _PreformeFiltring()
        {
            string FilterColumn = FilterComboBox.SelectedItem.ToString();
            string FilterText = tbfilterBy.Text.Trim();
            string actualColumnName = ""; // To hold the actual column name for filtering

            // Cast user-friendly names to actual column names
            if (FilterColumn == "PersonID")
                actualColumnName = "PersonID";
            else if (FilterColumn == "National No.")
                actualColumnName = "NationalNo";
            else if (FilterColumn == "First Name")
                actualColumnName = "FirstName";
            else if (FilterColumn == "Second Name")
                actualColumnName = "SecondName";
            else if (FilterColumn == "Third Name")
                actualColumnName = "ThirdName";
            else if (FilterColumn == "Last Name")
                actualColumnName = "LastName";
            else if (FilterColumn == "Gendor")
                actualColumnName = "Gendor"; // Note: Keep "Gendor" if that's the exact column name
            else if (FilterColumn == "Date Of Birth")
                actualColumnName = "DateOfBirth";
            else if (FilterColumn == "Nationality")
                actualColumnName = "CountryName";
            else if (FilterColumn == "Phone")
                actualColumnName = "Phone";
            else if (FilterColumn == "Email")
                actualColumnName = "Email";

            if (string.IsNullOrWhiteSpace(tbfilterBy.Text.Trim()))
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblCountRecords.Text = dgvPeopleData.Rows.Count.ToString();
                return;
            }

            if (actualColumnName == "PersonID")
                // Integer filter for PersonID
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", actualColumnName, FilterText);
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", actualColumnName, FilterText);

            lblCountRecords.Text = dgvPeopleData.Rows.Count.ToString();
        }




        private void _ReloadPersonList()
        {
            _PersonsTable = clsPerson.GetAllPeople();
            _dtPeople = _PersonsTable.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                                        "FirstName", "SecondName", "ThirdName",
                                                                        "LastName", "Gendor", "DateOfBirth",
                                                                        "CountryName", "Phone", "Email");



            dgvPeopleData.DataSource = _dtPeople;
            lblCountRecords.Text = _dtPeople.Rows.Count.ToString();


        }

        private void PeopleForm_Load(object sender, EventArgs e)
        {
            
            dgvPeopleData.DataSource = _dtPeople;


            if (_dtPeople != null)
            {
                // Set custom headers and widths
                SetDataGridColumnHeaders();

                // Populate filter combo box based on column headers
                foreach (DataGridViewColumn column in dgvPeopleData.Columns)
                {
                    if (column.HeaderText!= "Date Of Birth")
                        FilterComboBox.Items.Add(column.HeaderText);
                }

                // Default selection for filter
                FilterComboBox.SelectedIndex = FilterComboBox.Items.Count > 0 ? 0 : -1;

                // Initial record count
                lblCountRecords.Text = _dtPeople.Rows.Count.ToString();
            }
        }

        private void SetDataGridColumnHeaders()
        {
            dgvPeopleData.Columns[0].HeaderText = "PersonID";
            dgvPeopleData.Columns[0].Width = 110;

            dgvPeopleData.Columns[1].HeaderText = "National No.";
            dgvPeopleData.Columns[1].Width = 120;

            dgvPeopleData.Columns[2].HeaderText = "First Name";
            dgvPeopleData.Columns[2].Width = 120;

            dgvPeopleData.Columns[3].HeaderText = "Second Name";
            dgvPeopleData.Columns[3].Width = 140;

            dgvPeopleData.Columns[4].HeaderText = "Third Name";
            dgvPeopleData.Columns[4].Width = 120;

            dgvPeopleData.Columns[5].HeaderText = "Last Name";
            dgvPeopleData.Columns[5].Width = 120;

            dgvPeopleData.Columns[6].HeaderText = "Gendor";
            dgvPeopleData.Columns[6].Width = 140;

            dgvPeopleData.Columns[7].HeaderText = "Date Of Birth";
            dgvPeopleData.Columns[7].Width = 120;

            dgvPeopleData.Columns[8].HeaderText = "Nationality";
            dgvPeopleData.Columns[8].Width = 120;

            dgvPeopleData.Columns[9].HeaderText = "Phone";
            dgvPeopleData.Columns[9].Width = 120;

            dgvPeopleData.Columns[10].HeaderText = "Email";
            dgvPeopleData.Columns[10].Width = 170;
        }



        private void tbfilterBy_TextChanged(object sender, EventArgs e)
        {
            _PreformeFiltring();
        }

        private void tbfilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (FilterComboBox.Text.Trim() == "PersonID")
            {
                // Allow only digits and control keys for PersonID
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
  
        }

        private void AddPersonButton_Click(object sender, EventArgs e)
        {
            frmAddOrUpdatePerson Person = new frmAddOrUpdatePerson();
            Person.ShowDialog();
            _ReloadPersonList();
        }

        private void showDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetail personDetail = new frmPersonDetail((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            personDetail.ShowDialog();
            _ReloadPersonList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddOrUpdatePerson person = new frmAddOrUpdatePerson((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            person.ShowDialog();
            _ReloadPersonList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete ApplicantPerson [" + dgvPeopleData.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeopleData.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("ApplicantPerson Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _ReloadPersonList();
                }

                else
                    MessageBox.Show("ApplicantPerson was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);



        }

        private void dgvPeopleData_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmPersonDetail personDetail = new frmPersonDetail((int)dgvPeopleData.CurrentRow.Cells[0].Value);
            personDetail.ShowDialog();
            _ReloadPersonList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
