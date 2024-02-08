using DVLD.ApplicationTypes.NewDrivingLicense;
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
    public partial class frmListLocalDrivingApplications : Form
    {
        private enum ApplicationMode { New = 1, Cancelled = 2, Completed = 3 }

        public frmListLocalDrivingApplications()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _ListLocalLicenseApplications()
        {
            dgvLocalDrivingApps.DataSource = clsLocalLicenseApplication.ListLocalLicenseApplications();
            lblRecordsCount.Text = dgvLocalDrivingApps.RowCount.ToString();
        }

        private void frmListLocalDrivingApplications_Load(object sender, EventArgs e)
        {
            _ListLocalLicenseApplications();
            cbFilters.SelectedIndex = 0; // "None"
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = cbFilters.Text != "None";

            if (cbFilters.Text == "None")
                _ListLocalLicenseApplications();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filterType = string.Empty;

            switch (cbFilters.Text)
            {
                case "L.D.L.AppID":
                    filterType = "LDLAppID";
                    break;

                case "National No.":
                    filterType = "NationalNo";
                    break;
                case "Full Name":
                    filterType = "FullName";
                    break;
                case "Status":
                    filterType = "Status";
                    break;
            }

            dgvLocalDrivingApps.DataSource = clsLocalLicenseApplication.Filter(txtFilter.Text, filterType);
            lblRecordsCount.Text = dgvLocalDrivingApps.RowCount.ToString();

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.Text == "L.D.L.AppID")
                if (clsUtility.IsDigit(e.KeyChar))
                    e.Handled = true;
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to cancel this application?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            int selectedLocalLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;
            int applicationID = clsLocalLicenseApplication.GetApplicationID(selectedLocalLicenseAppID);

            clsLocalLicenseApplication localLicenseApplication = new clsLocalLicenseApplication();

            // Update the application status to "Cancelled"
            localLicenseApplication.UpdateApplicationStatus(applicationID, (decimal)ApplicationMode.Cancelled);

            _ListLocalLicenseApplications();
        }

        private void btnAddApplication_Click(object sender, EventArgs e)
        {
            frmAddLocalDrivingLicenseApplication newLocalLicneseAppFrm = new frmAddLocalDrivingLicenseApplication(1);
            newLocalLicneseAppFrm.ShowDialog();
            _ListLocalLicenseApplications();
        }

        private void shToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;
            frmVisionTestAppointment VisionTestAppointmentfrm = new frmVisionTestAppointment(selectedLocalDrivingAppID);
            VisionTestAppointmentfrm.ShowDialog();
            _ListLocalLicenseApplications();
        }

        private void cmsLicenseApplication_Opening(object sender, CancelEventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            int passedTestsCount = clsLocalLicenseApplication.PassedTestsCount(selectedLocalDrivingLicenseAppID);

            switch (passedTestsCount)
            {
                case 0:
                    _EnableVisionTest();
                    break;

                case 1:
                    _EnableWrittenTest();
                    break;

                case 2:
                    _EnableStreetTest();
                    break;

                case 3:
                    _DisableAllTests();
                    break;
            }
        }

        private void _EnableVisionTest()
        {
            shToolStripMenuItem.Enabled = true;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _EnableWrittenTest()
        {
            shToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = true;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _EnableStreetTest()
        {
            shToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = true;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _DisableAllTests()
        {
            shToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            showLicenseToolStripMenuItem.Enabled = true;
        }
    }
}