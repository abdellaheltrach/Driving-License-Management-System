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

namespace DVLD.Applications.Control
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {

        private clsApplications _Application;

        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }


        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblType.Text = _Application.ApplicationTypeInfo.Title;
            lblFees.Text = _Application.PaidFees.ToString();
            lblApplicant.Text = _Application.ApplicantPerson.FullName;
            lblDate.Text = _Application.ApplicationDate.ToString("d/MM/yyyy");
            lblStatusDate.Text = _Application.LastStatusDate.ToString("d/MM/yyyy");
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";

        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }



        private void ctrlApplicationBasicInfo_Load(object sender, EventArgs e)
        {

        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplications.Find(ApplicationID);
            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_ApplicationID != -1)
            {
                using (frmPersonDetail frm = new frmPersonDetail(_Application.ApplicantPersonID))
                {
                    frm.ShowDialog();
                    LoadApplicationInfo(_ApplicationID);
                }






            }
        }
    }
}
