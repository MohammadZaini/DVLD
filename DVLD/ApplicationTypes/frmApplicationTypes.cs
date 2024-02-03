using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationTypes
{
    public partial class frmApplicationTypes : Form
    {
        public frmApplicationTypes()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _ListAppTypes() {
            dgvApplicationTypes.DataSource = clsApplicationType.ListApplicationTypes();
            dgvApplicationTypes.Columns["ApplicationTypeTitle"].Width = 300;
            dgvApplicationTypes.Columns["ApplicationFees"].Width = 250;
            lblRecordsCount.Text = dgvApplicationTypes.RowCount.ToString();
        }

        private void frmApplicationTypes_Load(object sender, EventArgs e)
        {
            _ListAppTypes();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateApplicationType UpdateAppfrm = new frmUpdateApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            UpdateAppfrm.ShowDialog();
            _ListAppTypes();
        }
    }
}