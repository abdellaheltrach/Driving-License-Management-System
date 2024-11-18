using DVLD.Applications.Applications_types;
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

namespace DVLD.test_types
{
    public partial class frmListTestTypes : Form
    {
        DataTable _TestTypes;


        public frmListTestTypes()
        {
            InitializeComponent();

        }

        private void ReloadDataGridView()
        {
            _TestTypes = clsTestTypes.GetTestTypes();
            dgvTestTypes.DataSource = _TestTypes;
            dgvTestTypes.Columns[0].Width = 50;
            dgvTestTypes.Columns[1].Width = 120;
            dgvTestTypes.Columns[2].Width = 400;
            dgvTestTypes.Columns[3].Width = 70;

            lblRecordsCount.Text = dgvTestTypes.Rows.Count.ToString();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmEditTestType frm = new frmEditTestType((int)dgvTestTypes.CurrentRow.Cells[0].Value))
            {
                frm.ShowDialog();
            }
            ReloadDataGridView();
        }






    }
}
