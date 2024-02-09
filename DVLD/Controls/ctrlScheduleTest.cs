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
    public partial class ctrlScheduleTest : UserControl
    {
        private enum enTestType { Vision = 1, Written = 2, Street = 3 };
        private enum enTestMode { FirstTimeTest = 1, Edit = 2, Retake = 3 };

        private enTestType _testType = enTestType.Vision;
        private enTestMode _testMode = enTestMode.FirstTimeTest;

        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public void LoadTestAppointmentDetails(int drivingLicenseAppID, int testType)
        {
            lblDrivingLicenseAppID.Text = drivingLicenseAppID.ToString();
            dtpTestAppointentDate.Value = ApplicationDate;

            _testType = (enTestType)testType;

            clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.Find(drivingLicenseAppID);
            if (localLicenseApplication == null)
                return;

            _UpdateTestTypePhoto();
            _UpdateAppointmentDetails(localLicenseApplication);
        }

        private void _UpdateAppointmentDetails(clsLocalLicenseApplication localLicenseApplication)
        {
            lblLicenseClass.Text = localLicenseApplication.LicenseClassName;
            lblApplicantName.Text = localLicenseApplication.ApplicantFullName;
            lblTrial.Text = clsLocalLicenseApplication.FailureCount(localLicenseApplication.LocalLicenseApplicationID).ToString();

            clsTestType testType = clsTestType.Find((int)_testMode); // Vision Test

            if (testType != null)
                PaidFees = testType.Fees;

            lblFees.Text = ((int)PaidFees).ToString();
        }

        private void _ToggleTestAppointmentMode(int testMode, bool isLocked = false) {

            _testMode = (enTestMode)testMode;

            if ((_testMode == enTestMode.FirstTimeTest) || (_testMode == enTestMode.Edit && !isLocked))
            { 
                dtpTestAppointentDate.Enabled = true;
                btnSave.Enabled = true;
                gbRetakeTestInfo.Enabled = false;
                lblAlreadySatForTest.Visible = false;
                return;
            }

            lblScheduleTest.Text = "Schedule Retake Test";

            if (_testMode == enTestMode.Edit && isLocked)
            {
                gbRetakeTestInfo.Enabled = true;
                lblAlreadySatForTest.Visible = true;
                btnSave.Enabled = false;
                dtpTestAppointentDate.Enabled = false;
                return;
            }

            if (_testMode == enTestMode.Retake)
            {
                gbRetakeTestInfo.Enabled = true;
                lblAlreadySatForTest.Visible = false;
                btnSave.Enabled = true;
                dtpTestAppointentDate.Enabled = true;
                lblRetakeTestAppFees.Text = "some fees";
                return;
            }
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
    }
}
