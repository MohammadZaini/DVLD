using DVLD.Controls;
using DVLD.Properties;
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
    public partial class frmVisionTestAppointment : Form
    {
        private enum enTestType { Vision = 1, Written = 2, Street = 3 };
        private enTestType _testType = enTestType.Vision;

        private int _localDrivingApplicationID;
        private clsLocalLicenseApplication _localDrivingLicenseApp;
        public frmVisionTestAppointment(int localDrivingApplicationID, int testType)
        {
            InitializeComponent();
            CenterToScreen();   
            _localDrivingApplicationID = localDrivingApplicationID;
            _testType = (enTestType)testType;

            ctrlDrivingLicenseAppAndApplicationInfo1.LoadApplicationDetials(localDrivingApplicationID);
          
        }

        private void _ListPersonAppointments() {
            dgvTestAppointments.DataSource = clsTestAppointment.ListPeronTestAppointments(_localDrivingApplicationID,
                                                                                    (int)_testType);
            lblRecordsCount.Text = dgvTestAppointments.RowCount.ToString(); 
        }

        private void frmVisionTestAppointment_Load(object sender, EventArgs e)
        {
            _UpdateUIForTestMode();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest takeTestFrm = new frmTakeTest(_localDrivingApplicationID, selectedTestAppointmentID);
            takeTestFrm.ShowDialog();
            _ListPersonAppointments();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            // Edit Mode
            _ShowScheduleTestForm((int)_testType, clsGlobalSettings.EditMode, selectedTestAppointmentID);
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            _localDrivingLicenseApp = clsLocalLicenseApplication.Find(_localDrivingApplicationID);

            if (_localDrivingLicenseApp == null)
                return;


            if (!clsTestAppointment.IsAppointmentExist(_localDrivingApplicationID, _localDrivingLicenseApp.LocalLicenseClassID,
                                                        (int)_testType))
            {
                // First Time Test Mode
                _ShowScheduleTestForm((int)_testType, clsGlobalSettings.FirstTimeMode);
                return;
            }

            if (clsTestAppointment.IsAppointmentActive(_localDrivingApplicationID, (int)_testType))
            {
                MessageBox.Show("This person already has an active appointment scheduled for this license class",
                       "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsTestAppointment.IsPersonFailed(_localDrivingApplicationID, (int)_testType))
            {
                // Show Retake Schedule Test
                _ShowScheduleTestForm((int)_testType, clsGlobalSettings.RetakeMode);
                return;
            }

            MessageBox.Show("This person has already succeeded in this test!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void _ShowScheduleTestForm(int testType, int testMode, int selectedTestAppointmentID = -1) {
            frmScheduleTest scheduleTestfrm = new frmScheduleTest(_localDrivingApplicationID, testType, testMode, selectedTestAppointmentID);
            scheduleTestfrm.ShowDialog();
            _ListPersonAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            if(clsTestAppointment.IsAppointmentLocked(selectedTestAppointmentID))
                takeTestToolStripMenuItem.Enabled = false;
            else
                takeTestToolStripMenuItem.Enabled = true;

        }

        private void _UpdateUIForTestMode() {

            switch (_testType) 
            {
                case enTestType.Vision:
                    _UpdateUIForVisionTest();
                    break;

                case enTestType.Written:
                    _UpdateUIForWrittenTest();
                    break;

                case enTestType.Street:
                    _UpdateUIForStreetTest();
                    break;
            }
        }

        private void _UpdateUIForVisionTest() {
            pbTestTypePhoto.Image = Resources.Vision_512;
            lblTestAppointmentType.Text = "Vision Test Appointment";
            _ListPersonAppointments();
        }

        private void _UpdateUIForWrittenTest()
        {
            pbTestTypePhoto.Image = Resources.Written_Test_512;
            lblTestAppointmentType.Text = "Written Test Appointment";
            _ListPersonAppointments();
        }

        private void _UpdateUIForStreetTest()
        {
            pbTestTypePhoto.Image = Resources.driving_test_512;
            lblTestAppointmentType.Text = "Street Test Appointment";
            _ListPersonAppointments();
        }

    }
}