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
        private clsTest _test;
        private int _testAppointmentID;
        public frmTakeTest(int localDrivingLicenseApplicationID, int testAppointmentID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            ctrlVisionTest1.LoadTestAppointmentDetails(localDrivingLicenseApplicationID);
            _testAppointmentID = testAppointmentID;
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
