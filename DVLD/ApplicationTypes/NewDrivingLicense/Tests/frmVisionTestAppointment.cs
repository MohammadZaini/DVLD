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
        private clsLocalLicenseApplication _localDrivingLicenseApp;
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

            _ShowScheduleTestForm(selectedTestAppointmentID);
        }

        private void btnBookAppointment_Click(object sender, EventArgs e)
        {
            _localDrivingLicenseApp = clsLocalLicenseApplication.Find(_localDrivingApplicationID);

            if (_localDrivingLicenseApp == null)
                return;

            // IsAppointmentExist() returns True if the appointment Exists
            if (clsTestAppointment.IsAppointmentExist(_localDrivingApplicationID,_localDrivingLicenseApp.LicenseClassID)) 
            {
                if (clsTestAppointment.IsAppointmentLocked(_localDrivingApplicationID))
                {
                    if (clsTestAppointment.IsPersonFailed(_localDrivingApplicationID))
                    {
                        _ShowScheduleTestForm();
                    }
                    else
                        MessageBox.Show("You already succeeded in this one!");
                }
                else 
                {
                    MessageBox.Show("This person already has an active appointment! You cannot add a new appointment", 
                        "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
               //if (!clsTestAppointment.IsAppointmentLocked(_localDrivingApplicationID))
               //    MessageBox.Show("This person already has an active appointment! You cannot add a new appointment", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
               //else
               //{
                    _ShowScheduleTestForm();
               // }

            }
        }

        private void _ShowScheduleTestForm(int selectedTestAppointmentID = -1) {
            frmScheduleTest scheduleTestfrm = new frmScheduleTest(_localDrivingApplicationID, selectedTestAppointmentID);
            scheduleTestfrm.ShowDialog();
            _ListPersonAppointments();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(clsTestAppointment.IsAppointmentLocked(_localDrivingApplicationID))
                takeTestToolStripMenuItem.Enabled = false;
            else
                takeTestToolStripMenuItem.Enabled = true;

        }
    }
}