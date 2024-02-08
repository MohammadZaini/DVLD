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
        private enum enMode {Addnew = 0, Edit = 1};

        private enMode _Mode = enMode.Addnew;

        private clsTestAppointment _testAppointment;
        private int _localDrivingLicenseAppID;
        private int _testAppointmentID;
        public frmScheduleTest(int localDrivingLicenseAppID, int testAppointmentID)
        {
            InitializeComponent();
            CenterToScreen();

            _localDrivingLicenseAppID = localDrivingLicenseAppID;
            _testAppointmentID = testAppointmentID;

            _InitializeFormMode(testAppointmentID);
            _InitializeTestAppointmentObject();
            _InitializeUI();
            _LoadTestAppointmentDetails(localDrivingLicenseAppID);
        }

        private void _LoadTestAppointmentDetails(int localDrivingLicenseAppID) { 
            ctrlVisionTest1.LoadTestAppointmentDetails(localDrivingLicenseAppID);
        }
        private void _SetSaveButtonState(int testAppointmentID) {
            if (ctrlVisionTest1.ToggleTestAppointmentMode(testAppointmentID))
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
        }
        private void _InitializeTestAppointmentObject() {
            _testAppointment = (_Mode == enMode.Addnew) ? new clsTestAppointment() : clsTestAppointment.Find(_testAppointmentID);
        }
        private void _InitializeFormMode(int testAppointmentID) {

            _Mode = (testAppointmentID == -1) ? enMode.Addnew : enMode.Edit;                       
        }
        private void _InitializeUI() {
            ctrlVisionTest1.PaidFees = _testAppointment.PaidFees;
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

            bool isAppointmentLocked = clsTestAppointment.IsAppointmentLocked(_localDrivingLicenseAppID);

            if (!isAppointmentLocked)
            { 
                gbRetakeTestInfo.Enabled = false;
                return;
            }

            gbRetakeTestInfo.Enabled = true;

            if (_Mode == enMode.Edit)
            {
                lblTotalFees.Text = ((int)ctrlVisionTest1.PaidFees).ToString();
                lblRetakeTestAppFees.Text = "0";
                return;
            }

            const int retakeTestAppType = 8;
            int retakeTestFees = (int)clsApplicationType.Find(retakeTestAppType).AppFees;
            lblRetakeTestAppFees.Text = retakeTestFees.ToString();
            lblTotalFees.Text = (retakeTestFees + (int)ctrlVisionTest1.PaidFees).ToString();
            _SetSaveButtonState(_testAppointmentID);

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_testAppointment == null)
                return;

            _InitializeTestAppointment(); 

            if(clsTestAppointment.IsPersonFailed(_localDrivingLicenseAppID))
            {
                _CreateNewRetakeTestApplication();
            }

            if (_testAppointment.Save())
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Something went wrong", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void btnExist_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _CreateNewRetakeTestApplication() {
            // current localLicenseApplication 
            clsLocalLicenseApplication currentLocalLicenseApplication = clsLocalLicenseApplication.Find(_localDrivingLicenseAppID);
            int applicantPersonID = currentLocalLicenseApplication.Application.ApplicantPersonID;

            // create a new local license application with a retake test type
            clsApplication newApplication = new clsApplication();

            const int retakeTestTypeID = 8;
            const int appStatusNew = 1;

            newApplication.ApplicantPersonID = applicantPersonID;
            newApplication.ApplicationDate = DateTime.Now;
            newApplication.ApplicationStatus = appStatusNew;
            newApplication.ApplicationTypeID = retakeTestTypeID;
            newApplication.LastStatusDate = DateTime.Now;
            clsApplicationType testType = clsApplicationType.Find(retakeTestTypeID);
            newApplication.PaidFees = testType.AppFees;
            newApplication.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            int applicationID = newApplication.AddNewApplication();

           // _CreateNewLocalLicenseApplicationWithRetakeTest(newLocalLicenseApplication, licenseClassID);

        }

        private void _CreateNewLocalLicenseApplicationWithRetakeTest(clsLocalLicenseApplication newLocalLicenseApplication, int licenseClassID) { 
            newLocalLicenseApplication.LocalLicenseClassID = licenseClassID;

            if (newLocalLicenseApplication.Save())
                MessageBox.Show("Data Saved Successfully2", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Something went wrong2", "Failuer", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}