using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_BusinessLayer;

namespace DVLD.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
            cbFilterBy.SelectedIndex = 0;
        }

        public void LoadPersonInfo (int personID)
        {
            txtFilterValue.Text = personID.ToString();
            cbFilterBy.SelectedIndex = 0;
            gbFilters.Enabled = false;
        }

        private void ReceiveAddedPersonD(object sender, int PersonID)
        {
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            txtFilterValue.Text = PersonID.ToString();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (txtFilterValue.Text.Trim()==string.Empty)
                return;

            if (cbFilterBy.SelectedIndex==0)
            {
                ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text.Trim()));
            }
            else
            {
                ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text.Trim());
            }


        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddOrUpdatePerson frmAddOrUpdatePerson = new frmAddOrUpdatePerson();
            frmAddOrUpdatePerson.DataBack += ReceiveAddedPersonD;
            frmAddOrUpdatePerson.ShowDialog();
        }



        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.SelectedIndex==0)
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;
        }
    }
}
