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

namespace DVLD.Applications.Applications_types
{
    public partial class frmApplicationListTypes : Form
    {
        DataTable _ApplicationTypes ;
        public frmApplicationListTypes()
        {
            InitializeComponent();
        }

        private void ReloadDataGridView()
        {
            _ApplicationTypes = clsApplicationTypes.GetAllApplicationTypes();
            dgvApplicationTypes.DataSource = _ApplicationTypes ;


            if (dgvApplicationTypes.Rows.Count>0)
            {
                dgvApplicationTypes.DataSource = _ApplicationTypes;
                dgvApplicationTypes.Columns[1].Width = 256;

            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString(); 
            }
        }

        private void frmApplicationListTypes_Load(object sender, EventArgs e)
        {
            ReloadDataGridView();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmEditApplicationType frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value))
            {
                frm.ShowDialog();
            }
                ReloadDataGridView();
        }
    }
}
