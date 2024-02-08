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
    public partial class ctrlVisionTest : UserControl
    {
        public DateTime ApplicationDate { get; set; }
        public decimal PaidFees { get; set; }
        public ctrlVisionTest()
        {
            InitializeComponent();
        }

        public void LoadTestAppointmentDetails(int drivingLicenseAppID) { 
            lblDrivingLicenseAppID.Text = drivingLicenseAppID.ToString();
            dtpTestAppointentDate.Value = ApplicationDate;

            clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.Find(drivingLicenseAppID);

            if (localLicenseApplication == null)
                return;

            _UpdateAppointmentDetails(localLicenseApplication);
        }

        private void _UpdateAppointmentDetails(clsLocalLicenseApplication localLicenseApplication) {
            lblLicenseClass.Text = localLicenseApplication.LicenseClassName;
            lblApplicantName.Text = localLicenseApplication.ApplicantFullName;
            lblTrial.Text = clsLocalLicenseApplication.FailureCount(localLicenseApplication.LocalLicenseApplicationID).ToString();

            clsTestType testType = clsTestType.Find(1); // Vision Test

            if (testType != null) 
            {
                PaidFees = testType.Fees;
            }

            lblFees.Text = ((int)PaidFees).ToString();
        }
        private void ctrlVisionTest_Load(object sender, EventArgs e)
        {
            dtpTestAppointentDate.MinDate = DateTime.Now;
        }

        private void dtpTestAppointentDate_ValueChanged(object sender, EventArgs e)
        {
            ApplicationDate = dtpTestAppointentDate.Value;
        }

        public bool ToggleTestAppointmentMode(int localDrivingLicenseAppID) {

            bool isAppointmentLocked = clsTestAppointment.IsAppointmentLocked(localDrivingLicenseAppID);

            dtpTestAppointentDate.Enabled = !isAppointmentLocked;
            lblAlreadySatForTest.Visible = isAppointmentLocked;

            lblScheduleTest.Text = isAppointmentLocked ? "Schedule Retake Test" : "Schedule Test";

            return isAppointmentLocked;
        }
    }
}