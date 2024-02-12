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

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        public frmListDrivers()
        {
            InitializeComponent();
            CenterToScreen();
        }


        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _ListDrivers();
        }

        private void _ListDrivers() {
            dgvDriversList.DataSource = clsDriver.ListDrivers();
            dgvDriversList.Columns["Full Name"].Width = 250;
            dgvDriversList.Columns["Created Date"].Width = 200;
            lblRecordsCount.Text = dgvDriversList.RowCount.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}