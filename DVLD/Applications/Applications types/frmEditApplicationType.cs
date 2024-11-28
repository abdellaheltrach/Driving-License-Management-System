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
            this.ApplicationType = clsApplicationTypes.Find(AppTypeID);
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            txtTitle.Text = ApplicationType.Title;
            txtFees.Text = ApplicationType.Fees.ToString();
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

            ApplicationType.ID = this.AppTypeID;
            ApplicationType.Fees = int.Parse(txtFees.Text.Trim());
            ApplicationType.Title = txtTitle.Text.Trim();

            bool isUpdateSuccess = ApplicationType.Save();

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)
            {
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
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                // Cancel the event and show error
                btnSave.Enabled = false;
                errorProvider1.SetError(txtFees, "This field cannot be empty.");
                return;
            }
            else
            {
                // Clear the error if there is text
                btnSave.Enabled = true;

                errorProvider1.SetError(txtFees, null);
            }


            if (txtFees.Text.Trim() == ApplicationType.Fees.ToString())
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
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                // Cancel the event and show error
                btnSave.Enabled = false;
                errorProvider1.SetError(txtTitle, "This field cannot be empty.");
                return;
            }
            else
            {
                // Clear the error if there is text
                btnSave.Enabled = true;

                errorProvider1.SetError(txtTitle, null);
            }

            if (txtTitle.Text.Trim() == ApplicationType.Title)
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
