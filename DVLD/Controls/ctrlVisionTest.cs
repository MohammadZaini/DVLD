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
        public ctrlVisionTest()
        {
            InitializeComponent();
        }

        public void LoadTestAppointmentDetails(int drivingLicenseAppID) { 
            lblDrivingLicenseAppID.Text = drivingLicenseAppID.ToString();

            clsLocalLicenseApplication localLicenseApplication = clsLocalLicenseApplication.Find(drivingLicenseAppID);

            if (localLicenseApplication == null)
                return;

            _UpdateAppointmentDetails(localLicenseApplication);
        }

        private void _UpdateAppointmentDetails(clsLocalLicenseApplication localLicenseApplication) {
            lblLicenseClass.Text = localLicenseApplication.LicenseClassName;
            lblApplicantName.Text = localLicenseApplication.ApplicantFullName;

            clsTestType testType = clsTestType.Find(1);

            if(testType != null)
                lblFees.Text = testType.Fees.ToString();
        }
        private void ctrlVisionTest_Load(object sender, EventArgs e)
        {
            dtpTestAppointentDate.MinDate = DateTime.Now;
        }
    }
}