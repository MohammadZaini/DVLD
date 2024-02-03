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

namespace DVLD.TestTypes
{
    public partial class frmTestTypes : Form
    {
        public frmTestTypes()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }


        private void _ListTestTypes() {
            dgvTestTypes.DataSource = clsTest.ListTestTypes();
            dgvTestTypes.Columns["Description"].Width = 300;
            dgvTestTypes.Columns["Title"].Width = 150;
            lblRecordsCount.Text = dgvTestTypes.RowCount.ToString();
        }
        private void frmTestTypes_Load(object sender, EventArgs e)
        {
            _ListTestTypes();
        }

        private void editTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUpdateTestTypes UpdateTestTypefrm = new frmUpdateTestTypes((int)dgvTestTypes.CurrentRow.Cells[0].Value);
            UpdateTestTypefrm.ShowDialog();
            _ListTestTypes();
        }
    }
}
