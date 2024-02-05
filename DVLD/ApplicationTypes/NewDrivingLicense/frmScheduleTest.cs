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
        private int _localDrivingLicenseAppID;
        public frmScheduleTest(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _localDrivingLicenseAppID = localDrivingLicenseAppID;

            ctrlVisionTest1.LoadTestAppointmentDetails(localDrivingLicenseAppID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {

        }
    }
}
