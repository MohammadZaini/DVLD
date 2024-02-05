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
    public partial class frmScheduleTest : Form
    {
        private clsTestAppointment _testAppointment;
        private int _localDrivingLicenseAppID;
        public frmScheduleTest(int localDrivingLicenseAppID, int testAppointmentID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _localDrivingLicenseAppID = localDrivingLicenseAppID;

            ctrlVisionTest1.LoadTestAppointmentDetails(localDrivingLicenseAppID);

            _SetSaveButtonState(testAppointmentID);
        }

        private void _SetSaveButtonState(int testAppointmentID) {
            if (ctrlVisionTest1.ToggleTestAppointmentMode(testAppointmentID))
                btnSave.Enabled = false;
        }

        private void _InitialzeTestAppointment()
        {
            _testAppointment.TestTypeID = 1; // Vision Test
            _testAppointment.LocalDrivingLicenseApplicationID = _localDrivingLicenseAppID;
            _testAppointment.AppointmentDate = ctrlVisionTest1.ApplicationDate;
            _testAppointment.PaidFees = ctrlVisionTest1.PaidFees;
            _testAppointment.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
            _testAppointment.IsLocked = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            gbRetakeTestInfo.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _testAppointment = new clsTestAppointment();

            if (_testAppointment == null)
                return;

            _InitialzeTestAppointment();

            if (_testAppointment.Save())
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Something went wrong", "Failuer", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnExist_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}