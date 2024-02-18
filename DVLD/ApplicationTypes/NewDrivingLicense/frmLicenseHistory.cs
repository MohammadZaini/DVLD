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

namespace DVLD.ApplicationTypes.NewDrivingLicense
{
    public partial class frmLicenseHistory : Form
    {
        private int _personID;
        public frmLicenseHistory(int personID)
        {
            InitializeComponent();
            CenterToScreen();

            _personID = personID;

            ctrlPersonCardWithFilter1.LoadUserData(_personID);
        }

        private void _ListLocalLicenses() {
            dgvLocalLicensesList.DataSource = clsLicense.ListLocalLicenses(_personID);

            if (dgvLocalLicensesList.RowCount == 0) return;

            dgvLocalLicensesList.Columns["Class Name"].Width = 210;
            dgvLocalLicensesList.Columns["Issue Date"].Width = 180;
            dgvLocalLicensesList.Columns["Expiration Date"].Width = 180;

            lblRecordsCount.Text = dgvLocalLicensesList.RowCount.ToString();    
        }

        private void _ListInternationalLicenses()
        {
            dgvInternationalLicensesList.DataSource = clsInternationalLicense.GetInternationalLicense(_personID);

            if (dgvInternationalLicensesList.RowCount == 0) return;

            dgvInternationalLicensesList.Columns["Issue Date"].Width = 180;
            dgvInternationalLicensesList.Columns["Expiration Date"].Width = 180;
            lblRecordsCount.Text = dgvInternationalLicensesList.RowCount.ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            _ListInternationalLicenses();
            _ListLocalLicenses();
        }

        private void tcLicenses_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage localLicensePage = tcLicenses.TabPages[0];

            if (tcLicenses.SelectedTab == localLicensePage)
                lblRecordsCount.Text = dgvLocalLicensesList.RowCount.ToString();
            else
                lblRecordsCount.Text = dgvInternationalLicensesList.RowCount.ToString();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLicenseID = (int)dgvLocalLicensesList.CurrentRow.Cells[0].Value;
            frmLicenseDetails licenseDetails = new frmLicenseDetails(selectedLicenseID);
            licenseDetails.ShowDialog();
        }
    }
}