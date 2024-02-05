using DVLD.Controls;
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
    public partial class frmVisionTestAppointment : Form
    {
        private int _localDrivingApplicationID;
        public frmVisionTestAppointment(int localDrivingApplicationID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;    
            _localDrivingApplicationID = localDrivingApplicationID;

            ctrlDrivingLicenseAppAndApplicationInfo1.LoadApplicationDetials(localDrivingApplicationID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReserveAppointment_Click(object sender, EventArgs e)
        {
            frmScheduleTest scheduleTestfrm = new frmScheduleTest(_localDrivingApplicationID);
            scheduleTestfrm.ShowDialog();
        }
    }
}
