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

        private void _ListPersonAppointments() {
            dgvTestAppointments.DataSource = clsTestAppointment.ListPeronTestAppointments(_localDrivingApplicationID);
            lblRecordsCount.Text = dgvTestAppointments.RowCount.ToString(); 
        }

        private void frmVisionTestAppointment_Load(object sender, EventArgs e)
        {
            _ListPersonAppointments();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest takeTestFrm = new frmTakeTest(_localDrivingApplicationID, selectedTestAppointmentID);
            takeTestFrm.ShowDialog();
            _ListPersonAppointments();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;
       
            frmScheduleTest scheduleTestFrm = new frmScheduleTest(_localDrivingApplicationID, selectedTestAppointmentID);
            scheduleTestFrm.ShowDialog();
            _ListPersonAppointments();
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            int selectedTestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;

            if (!clsTestAppointment.IsAppointmentExist(_localDrivingApplicationID))
            {
                frmScheduleTest scheduleTestfrm = new frmScheduleTest(_localDrivingApplicationID, selectedTestAppointmentID);
                scheduleTestfrm.ShowDialog();
                _ListPersonAppointments();
            }
            else
                MessageBox.Show("This person already has a test appointment!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}