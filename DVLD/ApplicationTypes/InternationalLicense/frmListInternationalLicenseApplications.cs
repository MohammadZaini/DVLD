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

namespace DVLD.ApplicationTypes.InternationalLicense
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void _ListInternationalLicenses() {
            dgvInternationalLicenseApps.DataSource = clsInternationalLicense.ListInternationalLicenses();
            lblRecordsCount.Text = dgvInternationalLicenseApps.RowCount.ToString();
        }
        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _ListInternationalLicenses();
            cbFilter.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
