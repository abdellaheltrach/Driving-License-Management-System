using DVLD_BusinessLayer;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLD.test_types
{
    public partial class frmEditTestType : Form
    {
        private int TestID;
        private clsTestTypes _TestType;

        public frmEditTestType(int TestID)
        {
            InitializeComponent();
            this.TestID = TestID;
            this._TestType = clsTestTypes.FindById((clsTestTypes.enTestType)TestID);
        }


        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            txtTitle.Text = _TestType.TestTypeTitle;
            txtFees.Text = _TestType.TestTypeFees.ToString();
            txtDescription.Text = _TestType.TestTypeDescription;
            lblApplicationTypeID.Text = TestID.ToString();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }


            //fill the object 
            _TestType.TestTypeTitle = txtTitle.Text.Trim();
            _TestType.TestTypeFees = int.Parse(txtFees.Text.Trim());
            _TestType.TestTypeDescription = txtDescription.Text.Trim();

            if (_TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true; // Prevent the character from being entered
                errorProvider1.SetError(txtFees, "This field accept only numbers.");
            }
            else
            {
                // Clear the error if there is text
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtFees_TextChanged(object sender, EventArgs e)
        {
            if (txtFees.Text.Trim() == _TestType.TestTypeFees.ToString())
            {
                btnSave.Enabled = false;


            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void EmptyTextBox_Validating(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox txtBox = (System.Windows.Forms.TextBox)sender;

            // Check if the TextBox is empty
            if (string.IsNullOrEmpty(txtBox.Text.Trim()))
            {
                // Cancel the event and show error
                e.Cancel = true; // Prevent focus loss
                btnSave.Enabled = false;
                errorProvider1.SetError(txtBox, "This field cannot be empty.");
            }
            else
            {
                // Clear the error if there is text
                btnSave.Enabled = true;

                errorProvider1.SetError(txtBox, null);
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (txtDescription.Text.Trim() == _TestType.TestTypeDescription.ToString())
            {
                btnSave.Enabled = false;


            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == _TestType.TestTypeTitle.ToString())
            {
                btnSave.Enabled = false;


            }
            else
            {
                btnSave.Enabled = true;
            }
        }
    }
}
