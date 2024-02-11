using DVLD.Controls;
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
    public partial class frmTakeTest : Form
    {
        public delegate void DataBackEventHandler(int passedTests);

        public DataBackEventHandler DataBack;
        enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        private enTestType _testType = enTestType.VisionTest;



        private clsTest _test;
        private int _testAppointmentID;
        private clsTestAppointment _testAppointment;
      
        public frmTakeTest(int localDrivingLicenseApplicationID, int testAppointmentID, int testTypeID )
        {
            InitializeComponent();
            CenterToScreen();

            _testAppointmentID = testAppointmentID;
            _testAppointment = clsTestAppointment.Find(testAppointmentID);
            _testType = (enTestType)testTypeID;

            if (_testAppointment != null)
            { 
                _InitializeUI();
                _LoadTestAppointmentDetails(localDrivingLicenseApplicationID);
            }
        }

        private void _LoadTestAppointmentDetails(int localDrivingLicenseApplicationID) {
            ctrlScheduledTest1.LoadTestAppointmentDetails(localDrivingLicenseApplicationID, (int)_testType);
        }
        private void _InitializeUI()
        {
            ctrlScheduledTest1.ApplicationDate = _testAppointment.AppointmentDate;
            ctrlScheduledTest1.PaidFees = _testAppointment.PaidFees;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _test = new clsTest();

            if (_test == null)
                return;

            _InitializeTest();

            if (_test.Save())
            { 
                MessageBox.Show("Data Saved Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clsTestAppointment.LockTestAppointment(_testAppointmentID);

            }
            else
                MessageBox.Show("Data failed to save!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _InitializeTest() {
            _test.Result = rbPass.Checked;
            _test.Notes  = txtNotes.Text;
            _test.TestAppointmentID = _testAppointmentID;
            _test.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }
    }
}
