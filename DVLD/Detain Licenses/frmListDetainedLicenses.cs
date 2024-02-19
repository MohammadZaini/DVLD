using DVLD.ApplicationTypes.NewDrivingLicense;
using DVLD.People;
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

namespace DVLD.Detain_Licenses
{
    public partial class frmListDetainedLicenses : Form
    {
        public frmListDetainedLicenses()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void _ListDetainedLicenses() {
            dgvDetainedLicenses.DataSource = clsDetainedLicense.ListDetainedLicenses();
            lblRecordsCount.Text = dgvDetainedLicenses.RowCount.ToString();

            if (dgvDetainedLicenses.RowCount == 0) return;

            dgvDetainedLicenses.Columns["Full Name"].Width = 220;
            dgvDetainedLicenses.Columns["Release Date"].Width = 120;
            dgvDetainedLicenses.Columns["D.Date"].Width = 120;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _ListDetainedLicenses();
            cbFilter.SelectedIndex = 0; // None
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable FilteredData = clsDetainedLicense.Filter(txtFilter.Text, _GetSelectedFilterType());
            dgvDetainedLicenses.DataSource = FilteredData;
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense releaseDetainedLicense = new frmReleaseDetainedLicense();
            releaseDetainedLicense.ShowDialog();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense detainLicense = new frmDetainLicense();
            detainLicense.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int personID = _GetPersonID(licenseID);

            frmPersonDetails personDetails = new frmPersonDetails(personID);
            personDetails.ShowDialog();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int personID = _GetPersonID(licenseID);

            frmLicenseHistory licenseHistory = new frmLicenseHistory(personID);
            licenseHistory.ShowDialog();
        }

        private int _GetPersonID(int licenseID) {
            clsLicense license = clsLicense.FindByLicenseID(licenseID);
            clsApplication application = clsApplication.Find(license.ApplicationID);

            return application.ApplicantPersonID;
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            frmLicenseDetails licenseDetails = new frmLicenseDetails(licenseID);
            licenseDetails.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int detainID = (int)dgvDetainedLicenses.CurrentRow.Cells[0].Value;

            frmReleaseDetainedLicense detainLicense = new frmReleaseDetainedLicense(licenseID, detainID);
            detainLicense.ShowDialog();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex != 0)
                txtFilter.Visible = true;
            else
            {
                txtFilter.Visible = false;
                _ListDetainedLicenses();
            }
        }

        private string _GetSelectedFilterType()
        {
            string filterType = string.Empty;

            switch (cbFilter.Text)
            {
                case "Detain ID":
                    filterType = "D.ID";
                    break;
                case "Is Released":
                    filterType = "Is Released";
                    break;
                case "National No":
                    filterType = "N.No";
                    break;
                case "Full Name":
                    filterType = "Full Name";
                    break;

                case "Release Application ID":
                    filterType = "Release App.ID";
                    break;
            }

            return filterType;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int licenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;          
            releaseDetainedLicenseToolStripMenuItem.Enabled = clsLicense.IsDetained(licenseID) ? true : false;
        }
    }
}