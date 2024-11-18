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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.Applications_types
{
    public partial class frmEditApplicationType : Form
    {
        private int AppTypeID;
        private clsApplicationTypes ApplicationType;

        public frmEditApplicationType(int AppTypeID)
        {
            InitializeComponent();
            this.AppTypeID = AppTypeID;
            ApplicationType = clsApplicationTypes.FindById(AppTypeID);
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            txtTitle.Text = ApplicationType.ApplicationTypeTitle;
            txtFees.Text = ApplicationType.ApplicationFees.ToString();
            lblApplicationTypeID.Text = AppTypeID.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }


            bool isUpdateSuccess = clsApplicationTypes.UpdateApplicationType(AppTypeID, txtTitle.Text.Trim(),int.Parse( txtFees.Text.Trim()));

            if (isUpdateSuccess)
            {
                MessageBox.Show("Application Type updated successfully.", "Update Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to update Application Type.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void EmptyTextBox_Validating(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox txtBox = (System.Windows.Forms.TextBox)sender;

            // Check if the TextBox is empty
            if (string.IsNullOrEmpty(txtBox.Text.Trim()))
            {
                // Cancel the event and show error
                e.Cancel = true; // Prevent focus loss
                errorProvider1.SetError(txtBox, "This field cannot be empty.");
            }
            else
            {
                // Clear the error if there is text
                errorProvider1.SetError(txtBox, null);
            }
        }

        private void txtTitle_TextChanged(object sender, EventArgs e)
        {
            if (txtTitle.Text.Trim() == ApplicationType.ApplicationTypeTitle)
            {
                btnSave.Enabled = false;


            }
            else 
            {
                btnSave.Enabled = true;
            }
        }

        private void txtFees_TextChanged(object sender, EventArgs e)
        {
            if (txtFees.Text.Trim() == ApplicationType.ApplicationFees.ToString())
            {
                btnSave.Enabled = false;


            }
            else
            {
                btnSave.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
