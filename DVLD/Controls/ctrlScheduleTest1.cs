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

namespace DVLD.Controls
{
    public partial class ctrlScheduleTest1 : UserControl
    {
       
        private enum enTestType { Vision = 1, Written = 2, Street = 3 };
        private enum enTestMode { FirstTimeTest = 1, Edit = 2, Retake = 3 };

        private enTestType _testType = enTestType.Vision;
        private enTestMode _testMode = enTestMode.FirstTimeTest;
        private clsTestAppointment _testAppointment;
        private int _testAppointmentID;
        private int _localDrivingLicenseAppID;
        private decimal _retakeTestFees;
        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public ctrlScheduleTest1()
        {
            InitializeComponent();
        }

        public void LoadTestAppointmentDetails(int drivingLicenseAppID, int testType, int testMode,int testAppoinmentID = 1)
        {
            _testAppointmentID = testAppoinmentID;
            _localDrivingLicenseAppID = drivingLicenseAppID;

            _testType = (enTestType)testType;
            _testMode = (enTestMode)testMode;

            clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.Find(drivingLicenseAppID);

            if (localLicenseApplication == null)
                return;

            // Initialize Test Appointment object
            _InitializeTestAppointmentObject();

            // Update the Test Type Photo based on Test Type
            _UpdateTestTypePhoto();
 
            // Update the test Appointment UI
            _UpdateAppointmentDetails(localLicenseApplication);

            // Update The test Appointment mode based on the Mode: FirstTime / Edit / Retake
            _UpdateUIForTestAppointmentMode(testMode);

        }

        private void _UpdateAppointmentDetails(clsLocalLicenseApplication localLicenseApplication)
        {
            lblDrivingLicenseAppID.Text = _localDrivingLicenseAppID.ToString();
            dtpTestAppointentDate.Value = _testMode == enTestMode.Edit ? _testAppointment.AppointmentDate : DateTime.Now;
            lblLicenseClass.Text = localLicenseApplication?.LicenseClassName;
            lblApplicantName.Text = localLicenseApplication?.ApplicantFullName;
            lblTrial.Text = clsLocalLicenseApplication.FailureCount(localLicenseApplication.LocalLicenseApplicationID,(int)_testType).ToString();

            const int RetakeTestApplicationTypeID = 8;
            _retakeTestFees = clsApplicationType.Find(RetakeTestApplicationTypeID).Fees;

            clsTestType testType = clsTestType.Find((int)_testType); // Vision Test

            if (testType != null)
                PaidFees = testType.Fees;

            lblFees.Text = ((int)PaidFees).ToString();
            lblTotalFees.Text = (_testMode == enTestMode.Retake ? _retakeTestFees + (int)PaidFees : (int)PaidFees).ToString();

        }

        private void _UpdateUIForTestAppointmentMode(int testMode)
        {

            bool isAppointmentLocked = clsTestAppointment.IsAppointmentLocked(_testAppointmentID);


            _testMode = (enTestMode)testMode;

            if ((_testMode == enTestMode.FirstTimeTest) || (_testMode == enTestMode.Edit && !isAppointmentLocked))
            {
                _UpdateUIForFirstTimeMode();
                return;
            }

            lblScheduleTest.Text = "Schedule Retake Test";

            if (_testMode == enTestMode.Edit && isAppointmentLocked)
            {
                _UpdateUIForEditMode();
                return;
            }

            if (_testMode == enTestMode.Retake)
            {
                _UpdateUIForRetakeMode();
                return;
            }
        }

        private void _UpdateUIForFirstTimeMode()
        {
            dtpTestAppointentDate.Enabled = true;
            btnSave.Enabled = true;
            gbRetakeTestInfo.Enabled = false;
            lblAlreadySatForTest.Visible = false;
        }

        private void _UpdateUIForEditMode()
        {
            gbRetakeTestInfo.Enabled = true;
            lblAlreadySatForTest.Visible = true;
            btnSave.Enabled = false;
            dtpTestAppointentDate.Enabled = false;
          
            int retakeTestApplicationID = (int)_testAppointment?.RetakeTestApplicationID;
            lblRetakeTestAppID.Text = (retakeTestApplicationID == 0 ? "N/A" : retakeTestApplicationID.ToString());

            bool personFailedAndRetakeExists = clsTestAppointment.IsPersonFailed(_localDrivingLicenseAppID, (int)_testType) 
                                                && retakeTestApplicationID != 0;

            if (personFailedAndRetakeExists)
                lblRetakeTestAppFees.Text = ((int)_retakeTestFees).ToString();
        }

        private void _UpdateUIForRetakeMode() {
            gbRetakeTestInfo.Enabled = true;
            lblAlreadySatForTest.Visible = false;
            btnSave.Enabled = true;
            dtpTestAppointentDate.Enabled = true;
            lblRetakeTestAppFees.Text = ((int)_retakeTestFees).ToString();
        }
  
        private void _UpdateTestTypePhoto()
        {

            switch (_testType)
            {
                case enTestType.Vision:
                    pbTestTypePhoto.Image = Resources.Vision_512;
                    break;

                case enTestType.Written:
                    pbTestTypePhoto.Image = Resources.Written_Test_512;
                    break;

                case enTestType.Street:
                    pbTestTypePhoto.Image = Resources.driving_test_512;
                    break;
            }

        }

        private void _InitializeTestAppointment()
        {
            _testAppointment.TestTypeID = (int)_testType; 
            _testAppointment.LocalDrivingLicenseApplicationID = _localDrivingLicenseAppID;
            _testAppointment.AppointmentDate = dtpTestAppointentDate.Value;
            _testAppointment.PaidFees = clsTestType.Find((int)_testType).Fees;
            _testAppointment.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
            _testAppointment.IsLocked = false;
        }

        private void _InitializeTestAppointmentObject()
        {
            _testAppointment = (_testMode == enTestMode.Edit) ? clsTestAppointment.Find(_testAppointmentID) : new clsTestAppointment();
        }

        private void _CreateNewRetakeTestApplication()
        {
            // current localLicenseApplication to get the applicant person ID 
            clsLocalLicenseApplication currentLocalLicenseApplication = _CurrentLocalLicenseApplication(_localDrivingLicenseAppID);
            int applicantPersonID = currentLocalLicenseApplication.Application.ApplicantPersonID;

            const int retakeTestTypeID = 8;
            const int appStatusNew = 1;

            // create a new retake test type and connect it with the current Local License Application
            clsApplication newApplication = _InitializeNewRetakeTestApplication(applicantPersonID, retakeTestTypeID, appStatusNew);

            int applicationID = newApplication.AddNewApplication();
            _testAppointment.RetakeTestApplicationID = applicationID;
        }

        private clsLocalLicenseApplication _CurrentLocalLicenseApplication(int localDrivingLicenseAppID)
        {
            return clsLocalLicenseApplication.Find(localDrivingLicenseAppID);
        }

        private clsApplication _InitializeNewRetakeTestApplication(int applicantPersonID, int retakeTestTypeID, byte appStatusNew)
        {

            clsApplication newApplication = new clsApplication();

            newApplication.ApplicantPersonID = applicantPersonID;
            newApplication.ApplicationStatus = appStatusNew;
            newApplication.ApplicationTypeID = retakeTestTypeID;
            newApplication.ApplicationDate = DateTime.Now;
            newApplication.LastStatusDate = DateTime.Now;

            clsApplicationType testType = clsApplicationType.Find(retakeTestTypeID);
            newApplication.PaidFees = testType.Fees;
            newApplication.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            return newApplication;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_testAppointment == null)
                return;

            _InitializeTestAppointment();

            if (clsTestAppointment.IsPersonFailed(_localDrivingLicenseAppID, (int)_testType))
            {
                _CreateNewRetakeTestApplication();
                lblRetakeTestAppID.Text = _testAppointment.RetakeTestApplicationID.ToString();
            }

            if (_testAppointment.Save())
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Something went wrong", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ctrlScheduleTest1_Load(object sender, EventArgs e)
        {
            dtpTestAppointentDate.MinDate = DateTime.Now;
        }
    }
}