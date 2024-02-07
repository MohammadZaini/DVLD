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
        private enum enMode {Addnew = 0, Update = 1};

        private enMode _Mode = enMode.Addnew;

        private clsTestAppointment _testAppointment;
        private int _localDrivingLicenseAppID;
        private int _testAppointmentID;
        public frmScheduleTest(int localDrivingLicenseAppID, int testAppointmentID)
        {
            InitializeComponent();

             StartPosition = FormStartPosition.CenterScreen;
            _localDrivingLicenseAppID = localDrivingLicenseAppID;
            _testAppointmentID = testAppointmentID;

            _InitializeFormMode(testAppointmentID);
            _InitializeTestAppointmentObject();
            _InitializeUI();

            ctrlVisionTest1.LoadTestAppointmentDetails(localDrivingLicenseAppID);

        }

        private void _SetSaveButtonState(int testAppointmentID) {
            if (ctrlVisionTest1.ToggleTestAppointmentMode(testAppointmentID))
                btnSave.Enabled = false;
        }
        private void _InitializeTestAppointmentObject() {
            _testAppointment = (_Mode == enMode.Addnew) ? new clsTestAppointment() : clsTestAppointment.Find(_testAppointmentID);
        }
        private void _InitializeFormMode(int testAppointmentID) {

            _Mode = (testAppointmentID == -1) ? enMode.Addnew : enMode.Update;                       
        }
        private void _InitializeUI() { 
            lblTotalFees.Text = ((int)ctrlVisionTest1.PaidFees).ToString();
            ctrlVisionTest1.ApplicationDate = _testAppointment.AppointmentDate;
            _SetSaveButtonState(_localDrivingLicenseAppID);
        }
        private void _InitializeTestAppointment()
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
            
            if (clsTestAppointment.IsAppointmentLocked(_localDrivingLicenseAppID))
            {
                gbRetakeTestInfo.Enabled = true;
                int retakeTestFees =  (int)clsApplicationType.Find(8).AppFees;
                lblRetakeTestAppFees.Text = retakeTestFees.ToString();
                lblTotalFees.Text = (retakeTestFees + (int)ctrlVisionTest1.PaidFees).ToString();
               
            }
            else
                gbRetakeTestInfo.Enabled = false;

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_testAppointment == null)
                return;

            _InitializeTestAppointment(); 

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