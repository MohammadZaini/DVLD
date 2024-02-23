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

namespace DVLD.ApplicationTypes.InternationalLicense
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        private int _personID;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex != 0) // 0 = None
                txtFilter.Visible = true;
            else
            {
                txtFilter.Visible = false;
                _ListInternationalLicenses();
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "International ID" || cbFilter.Text == "Application ID" || cbFilter.Text == "Local License ID"
                || cbFilter.Text == "Driver ID")
                if (clsUtility.IsDigit(e.KeyChar))
                    e.Handled = true; // Set e.Handled to true to block the character 
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {        
            DataTable FilteredData = clsInternationalLicense.Filter(txtFilter.Text, _GetSelectedFilterType());
            dgvInternationalLicenseApps.DataSource = FilteredData;
        }

        private string _GetSelectedFilterType() {
            string filterType = string.Empty;

            switch (cbFilter.Text)
            {
                case "International License ID":
                    filterType = "Int.LicenseID";
                    break;

                case "Application ID":
                    filterType = "Application ID";
                    break;
                case "Local License ID":
                    filterType = "L.License ID";
                    break;
                case "Driver ID":
                    filterType = "Driver ID";
                    break;

                case "Issue Date":
                    filterType = "Issue Date";
                    break;

                case "Expiration Date":
                    filterType = "Expiration Date";
                    break;

                case "Is Active":
                    filterType = "Is Active";
                    break;
            }

            return filterType;
        }

        private void btnAddInternationalLicense_Click(object sender, EventArgs e)
        {
            frmInternationalLicense internationalLicenseFrm = new frmInternationalLicense();    
            internationalLicenseFrm.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedApplicationID = (int)dgvInternationalLicenseApps.CurrentRow.Cells[1].Value;
            _personID = clsApplication.Find(selectedApplicationID).ApplicantPersonID;

            frmPersonDetails personDetails = new frmPersonDetails(_personID);
            personDetails.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalLicenseID = (int)dgvInternationalLicenseApps.CurrentRow.Cells[3].Value;
            frmLicenseDetails licenseDetails = new frmLicenseDetails(selectedLocalLicenseID);
            licenseDetails.ShowDialog();
        }

        private void showPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedApplicationID = (int)dgvInternationalLicenseApps.CurrentRow.Cells[1].Value;
            _personID = clsApplication.Find(selectedApplicationID).ApplicantPersonID;

            frmLicenseHistory personLicenseHistory = new frmLicenseHistory(_personID);
            personLicenseHistory.ShowDialog();
        }
    }
}