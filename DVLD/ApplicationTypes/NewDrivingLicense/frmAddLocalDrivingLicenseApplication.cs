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
    public partial class frmAddLocalDrivingLicenseApplication : Form
    {
        private int _applicationTypeID;
        private clsLocalLicenseApplication _localLicenseApplication;
        public frmAddLocalDrivingLicenseApplication(int applicationTypeID)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            _applicationTypeID = applicationTypeID;
        }

        private void _LoadInfo() {
            lblApplicationFees.Text = clsApplication.GetFees(_applicationTypeID);
            lblApplicationDate.Text = DateTime.Today.ToString("yyyy/MM/dd");
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            TabPage appInfoTab = tcDrivingLicenseApp.TabPages[1];
            tcDrivingLicenseApp.SelectedTab = appInfoTab;                    
        }

        private void frmLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _LoadInfo();
            cbLicenseClass.SelectedIndex = 2;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _localLicenseApplication = new clsLocalLicenseApplication();

            if (_localLicenseApplication == null)
            {
                MessageBox.Show("Please select a different individual.", "Select Another Individual", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Checks If the applicant has applied to the same License Class as well as the status NEW
            if (clsApplication.IsApplicationExist(ctrlPersonCardWithFilter1.PersonID, _GetSelectedLicenseClassID()))
            {
                MessageBox.Show("This individual already has an existing local license application. Please select another option.", 
                    "Duplicate Local License Application Detected", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            _SetApplicationProperties(_localLicenseApplication.Application);

            _localLicenseApplication.LocalLicenseClassID = _GetSelectedLicenseClassID();

            if (_localLicenseApplication.Save())
            {
                MessageBox.Show("Application has been submitted successfully.", "Application Submitted Successfully.",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblLocalLicenseApplicationID.Text = _localLicenseApplication.LocalLicenseApplicationID.ToString();
                lblAddEditLocalLicenseApp.Text = "Update Local Driving License Application";
            }
            else
            { 
                MessageBox.Show("Failed to submit application. Please try again.", "Application Submission Failed",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void _SetApplicationProperties(clsApplication application)
        {
            application.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = _applicationTypeID;
            application.ApplicationStatus = 1;
            application.LastStatusDate = DateTime.Now;
            application.PaidFees = Convert.ToDecimal(lblApplicationFees.Text);
            application.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }

        private int _GetSelectedLicenseClassID()
        {
            return cbLicenseClass.SelectedIndex + 1;
        }
    }
}