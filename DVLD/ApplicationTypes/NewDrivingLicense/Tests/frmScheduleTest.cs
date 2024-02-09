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
        public frmScheduleTest(int localDrivingLicenseAppID, int testType, int testMode ,int testAppointmentID)
        {
            InitializeComponent();
            CenterToScreen();

            _LoadTestAppointmentDetails(localDrivingLicenseAppID, testType, testMode, testAppointmentID);
        }

        private void _LoadTestAppointmentDetails(int localDrivingLicenseAppID, int testType, int testMode, int testAppointmentID) { 
            ctrlScheduleTest11.LoadTestAppointmentDetails(localDrivingLicenseAppID, testType, testMode, testAppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmScheduleTest_Load(object sender, EventArgs e)
        {

            //bool isAppointmentLocked = clsTestAppointment.IsAppointmentLocked(_localDrivingLicenseAppID);
            //
            //if (!isAppointmentLocked)
            //{ 
            //    gbRetakeTestInfo.Enabled = false;
            //    return;
            //}
            //
            //gbRetakeTestInfo.Enabled = true;
            //
            //if (_Mode == enMode.Edit)
            //{
            //    lblTotalFees.Text = ((int)ctrlVisionTest1.PaidFees).ToString();
            //    lblRetakeTestAppFees.Text = "0";
            //    return;
            //}
            //
            //const int retakeTestAppType = 8;
            //int retakeTestFees = (int)clsApplicationType.Find(retakeTestAppType).AppFees;
            //lblRetakeTestAppFees.Text = retakeTestFees.ToString();
            //lblTotalFees.Text = (retakeTestFees + (int)ctrlVisionTest1.PaidFees).ToString();
            //
            //bool isEditMode = _Mode == enMode.Edit;
            //
            //_SetSaveButtonState(_localDrivingLicenseAppID, isEditMode);

        }
        private void btnExist_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
    }
}