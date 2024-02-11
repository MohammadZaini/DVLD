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
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
        private clsLocalLicenseApplication _localDrivingLicenseApplication;
        private int _localDrivingLicenseAppID;
        private clsLicense _license;
        public frmIssueDrivingLicenseFirstTime(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            CenterToScreen();

            _localDrivingLicenseAppID = localDrivingLicenseAppID;

            ctrlDrivingLicenseAppAndApplicationInfo1.LoadApplicationDetials(localDrivingLicenseAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            _localDrivingLicenseApplication = clsLocalLicenseApplication.Find(_localDrivingLicenseAppID);

            if (_localDrivingLicenseApplication == null)
                return;

            _InitializeLicense();

        }

        private void _InitializeLicense() { 
        
            _license = new clsLicense();

            int classID = _localDrivingLicenseApplication.LocalLicenseClassID;
            int createdByUserID = clsGlobalSettings.LoggedInUser.UserID;
            int applicationID = _localDrivingLicenseApplication.Application.ApplicationID;
            clsLicenseClass licenseClass = clsLicenseClass.Find(classID);

            // Initializing Driver
            _license.Driver.PersonID = _localDrivingLicenseApplication.Application.ApplicantPersonID;
            _license.Driver.CreatedByUserID = createdByUserID;
            _license.Driver.CreatedDate = DateTime.Now;

            // Initializing License
            _license.ApplicationID = applicationID;
            _license.LicenseClassNo = classID;
            _license.IssueDate = DateTime.Now;
            _license.ExpirationDate = DateTime.Now.AddYears(licenseClass.DefaultValidityLength);
            _license.Notes = txtNotes.Text;
            _license.PaidFees = 20m; // needs to be updated
            _license.IsActive = true;
            _license.IssueReason = clsGlobalSettings.IssueLicenseForFirstTime;
            _license.CreatedByUserID = createdByUserID;

            if (_license.Save())
            {               
                MessageBox.Show("Data Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _localDrivingLicenseApplication.UpdateApplicationStatus(applicationID, 
                                    (decimal)clsGlobalSettings.localApplicationMode.Completed);
            }
            else
                MessageBox.Show("Something went wrong", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}