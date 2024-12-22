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

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }

        private int _licenseID;

        public int LicenseID 
        {
            get 
            { 
                return this._licenseID;
            } 
        }

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        public void LoadLicenseInfo(int LicenseID)
        {


            this._licenseID = LicenseID;

            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);


            if (OnLicenseSelected != null )
                // Raise the event with a parameter
                OnLicenseSelected(_licenseID);


        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Suppress the key press
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {


            //check if the txt box empty
            if (txtLicenseID.Text.Trim() == string.Empty)
                return;

            _licenseID = int.Parse(txtLicenseID.Text.Trim());

            if (!clsLicense.IsLicenseExists(_licenseID) == true)
            {
                MessageBox.Show($"No user found with the License ID: {txtLicenseID.Text.Trim()}",
                "User Not Found",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
                return;
            }


            ctrlDriverLicenseInfo1.LoadInfo(_licenseID);

            LoadLicenseInfo(_licenseID);
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {

        }
    }
}
