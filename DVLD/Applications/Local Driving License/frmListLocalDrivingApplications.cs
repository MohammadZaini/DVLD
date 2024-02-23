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

        private clsLocalLicenseApplication _localDrivingLicenseApplication;
        public frmListLocalDrivingApplications()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _ListLocalLicenseApplications()
        {
            dgvLocalDrivingApps.DataSource = clsLocalLicenseApplication.ListLocalLicenseApplications();
            dgvLocalDrivingApps.Columns["Full Name"].Width = 240;
            dgvLocalDrivingApps.Columns["Class Name"].Width = 180;
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

        private void cmsLicenseApplication_Opening(object sender, CancelEventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            _localDrivingLicenseApplication = _GetLocalDrivingLicenseApplication(selectedLocalDrivingLicenseAppID);

            if (_localDrivingLicenseApplication == null) return;

            string applicationStatus = _localDrivingLicenseApplication.Application.ApplicationStatusName;

            // Check if applicant has already been issued a driving license and toggle menu tools accordingly
            bool isLicensed = _IsLicensed();
            _ToggleMenuOptionsForLicensing(isLicensed);

            if (isLicensed) return;

            const string StatusTypeNew = "New";

            if (applicationStatus != StatusTypeNew)
            {
                _DisableNonNewApplicationOptions(applicationStatus);
                return;
            }

            _ToggleMenuOptionsForNewApplication(selectedLocalDrivingLicenseAppID);
        }

        private bool _IsLicensed() {

            int personID = _localDrivingLicenseApplication.Application.ApplicantPersonID;
            int licenseClass = _localDrivingLicenseApplication.LocalLicenseClassID;

            return clsDriver.IsLicenseAlreadyHeldInClass(personID, licenseClass);
        }

        private void _ToggleMenuOptionsForLicensing(bool isLicensed) {
            
            editApplicationToolStripMenuItem.Enabled = !isLicensed;
            deleteApplicationToolStripMenuItem.Enabled = !isLicensed;
            cancelApplicationToolStripMenuItem.Enabled = !isLicensed;
            scheduleTestToolStripMenuItem.Enabled = !isLicensed;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = !isLicensed;
            showLicenseToolStripMenuItem.Enabled = isLicensed;
        }

        private void _ToggleMenuOptionsForNewApplication(int localDrivingLicenseAppID) {

            int passedTestsCount = clsLocalLicenseApplication.PassedTestsCount(localDrivingLicenseAppID);

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

        private clsLocalLicenseApplication _GetLocalDrivingLicenseApplication(int localDrivingLicenseAppID) { 
            return clsLocalLicenseApplication.Find(localDrivingLicenseAppID);
        }

        private void _DisableNonNewApplicationOptions(string applicationStatus) {

            if (applicationStatus == "Cancelled")
            { 
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
            }

            scheduleTestToolStripMenuItem.Enabled = false;
        }

        private void _EnableVisionTest()
        {
            ScheduleVisionToolStripMenuItem.Enabled = true;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _EnableWrittenTest()
        {
            ScheduleVisionToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = true;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _EnableStreetTest()
        {
            ScheduleVisionToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = true;
            scheduleTestToolStripMenuItem.Enabled = true;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void _DisableAllTests()
        {
            ScheduleVisionToolStripMenuItem.Enabled = false;
            scheduleWrtitenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            scheduleTestToolStripMenuItem.Enabled = false;
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
            showLicenseToolStripMenuItem.Enabled = false;
        }

        private void scheduleWrtitenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            _ShowTestAppointmentForm(selectedLocalDrivingAppID, clsGlobalSettings.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            _ShowTestAppointmentForm(selectedLocalDrivingAppID, clsGlobalSettings.StreetTest);
        }

        private void _ShowTestAppointmentForm(int selectedLocalDrivingAppID, int testType) {

            frmVisionTestAppointment VisionTestAppointmentfrm = new frmVisionTestAppointment(selectedLocalDrivingAppID, testType);
            VisionTestAppointmentfrm.ShowDialog();
            _ListLocalLicenseApplications();
        }

        private void ScheduleVisionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            _ShowTestAppointmentForm(selectedLocalDrivingAppID, clsGlobalSettings.VisionTest);
        }

        private void showApplicationDetailtsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            frmLocalLicenseApplicationDetails localLicenseAppDetailsFrm = new frmLocalLicenseApplicationDetails(selectedLocalDrivingLicenseAppID);
            localLicenseAppDetailsFrm.ShowDialog();
            _ListLocalLicenseApplications();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;
            frmIssueDrivingLicenseFirstTime issueDrivingLicenseFrm = new frmIssueDrivingLicenseFirstTime(selectedLocalDrivingLicenseAppID);
            issueDrivingLicenseFrm.ShowDialog();
            _ListLocalLicenseApplications();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            int applicationID = clsLocalLicenseApplication.Find(selectedLocalDrivingLicenseAppID).Application.ApplicationID;

            int licenseID = clsLicense.Find(applicationID).ID;

            frmLicenseDetails licenesDetailsFrm = new frmLicenseDetails(licenseID);
            licenesDetailsFrm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            int personID = _GetPersonID(selectedLocalDrivingLicenseAppID);

            frmLicenseHistory licenseHistory = new frmLicenseHistory(personID);
            licenseHistory.ShowDialog();
        }

        private int _GetPersonID(int localDrivingLicenseAppID) { 
            return clsLocalLicenseApplication.Find(localDrivingLicenseAppID).Application.ApplicantPersonID;

        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedLocalDrivingLicenseAppID = (int)dgvLocalDrivingApps.CurrentRow.Cells[0].Value;

            DialogResult deletingResult = MessageBox.Show("Are you sure you want to delete this application?","Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if(deletingResult == DialogResult.Yes )
            {
                if (clsLocalLicenseApplication.DeleteApplication(selectedLocalDrivingLicenseAppID))
                {
                    MessageBox.Show("Application has been deleted successfully!", "Success", MessageBoxButtons.OK,
                           MessageBoxIcon.Information);

                    _ListLocalLicenseApplications();
                }
                   
                else
                    MessageBox.Show("Application has not been deleted...", "Failure", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
    }
}