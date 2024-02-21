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
        private enum enTestType { Vision = 1, Written = 2, Street = 3 };
        private enTestType _testType = enTestType.Vision; 

        public frmScheduleTest(int localDrivingLicenseAppID, int testType, int testMode ,int testAppointmentID)
        {
            InitializeComponent();
            CenterToScreen();

            _testType = (enTestType)testType;

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
            _InitializeTestType();
        }

        private void _InitializeTestType() {
            switch (_testType)
            {
                case enTestType.Vision:
                    gbTestType.Name = "Vision Test";
                    break;

                case enTestType.Written:
                    gbTestType.Name = "Written Test";
                    break;

                case enTestType.Street:
                    gbTestType.Name = "Street Test";
                    break;
            }
        }
        private void btnExist_Click(object sender, EventArgs e)
        {
            this.Close();
        }
  
    }
}