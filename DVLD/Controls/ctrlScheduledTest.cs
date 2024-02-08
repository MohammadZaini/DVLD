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
    public partial class ctrlScheduledTest : UserControl
    {
        private enum enTestType { Vision = 0, Written = 1, Street = 2 }

        private enTestType _testType = enTestType.Vision;
        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadTestAppointmentDetails(int drivingLicenseAppID, int testTypeID = 1)
        {
            lblDrivingLicenseAppID.Text = drivingLicenseAppID.ToString();

            clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.Find(drivingLicenseAppID);

            if (localLicenseApplication == null)
                return;

            _InitializeTestMode(testTypeID);
            _UpdateAppointmentDetails(localLicenseApplication);
            _UpdateTestTypePhoto();
        }

        private void _UpdateAppointmentDetails(clsLocalLicenseApplication localLicenseApplication)
        {
            lblLicenseClass.Text = localLicenseApplication.LicenseClassName;
            lblApplicantName.Text = localLicenseApplication.ApplicantFullName;
            lblFees.Text = ((int)PaidFees).ToString();
            lblTrial.Text = clsLocalLicenseApplication.FailureCount(localLicenseApplication.LocalLicenseApplicationID).ToString();
            lblTestDate.Text = ApplicationDate.ToString("yyyy/MM/dd");
            clsTestType testType = clsTestType.Find(1);

            if (testType != null)
            {
                PaidFees = testType.Fees;
            }
        }

        private void _InitializeTestMode(int testTypeID) { 
            
            switch (testTypeID)
            {
                case 1:
                    _testType = enTestType.Vision;
                    break;

                case 2:
                    _testType = enTestType.Written;
                    break;

                case 3:
                    _testType = enTestType.Street;
                    break;
            }
        }
        private void _UpdateTestTypePhoto() {

            switch (_testType) {
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