using DVLD.ApplicationTypes.NewDrivingLicense;
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

namespace DVLD.Detain_Licenses
{
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _personID;
        private int _licenseID;
        private decimal _releaseDetainedLicenseFees;
        private clsDetainedLicense _detainedLicense;
        public frmReleaseDetainedLicense(int licenseID = -1, int detainID = -1)
        {
            InitializeComponent();
            CenterToScreen();

            if (licenseID == -1 && detainID == -1)
                ctrlLicenseCardWithFilter1.DataBack += _UpdateUIOnLicenseSearch;
            else
                _LoadReleaseLicenseApplication(licenseID, detainID); ;
        }

        private void _LoadReleaseLicenseApplication(int licenseID , int detainID) {
            _personID = _GetPersonID(licenseID);
            _licenseID = licenseID;

            ctrlLicenseCardWithFilter1.LoadDetainedLicenseInfo(licenseID);

            lblDetainID.Text = detainID.ToString();
            _GetDetainedLicense(licenseID);

            _releaseDetainedLicenseFees = clsGlobalSettings.GetApplicationFees((int)clsGlobalSettings.enApplicationTypes.ReleaseDetainedDrivingLicsense);

            lblDate.Text = _detainedLicense.Date.ToString(clsGlobalSettings.dateFormat);
            lblLicenseID.Text = licenseID.ToString();
            lblCreatedBy.Text = clsUser.Find(_detainedLicense.CreatedByUserID).Username;
            lblFineFees.Text = _detainedLicense.FineFees.ToString();
            lblApplicationFees.Text = _releaseDetainedLicenseFees.ToString();
            lblTotalFees.Text = ((int)_releaseDetainedLicenseFees + (int)_detainedLicense.FineFees).ToString();
            btnRelease.Enabled = true;
            lbShowLicenseHistory.Enabled = true;
            lbShowLicenseInfo.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool _IsDetained(int licenseID)
        {
            return clsLicense.IsDetained(licenseID);
        }

        private void _UpdateUIOnLicenseSearch(int licenseID, int personID, bool controlState) { 

            _licenseID = licenseID;
            _personID = personID;
            lbShowLicenseHistory.Enabled = true;
            lblLicenseID.Text = licenseID.ToString();

            btnRelease.Enabled = controlState;

            if (!_IsDetained(licenseID))
            {
                btnRelease.Enabled = false;
                MessageBox.Show("Selected license is not detained", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _GetDetainedLicense();

            decimal _releaseDetainedLicenseFees = clsGlobalSettings.GetApplicationFees((int)clsGlobalSettings.enApplicationTypes.ReleaseDetainedDrivingLicsense);

            btnRelease.Enabled = true;
            lblDetainID.Text = _detainedLicense.ID.ToString();
            lblDate.Text = _detainedLicense.Date.ToString();
            lblApplicationFees.Text = ((int)_releaseDetainedLicenseFees).ToString();
            lblFineFees.Text = ((int)_detainedLicense.FineFees).ToString();
            lblTotalFees.Text = ((int)_detainedLicense.FineFees + (int)_releaseDetainedLicenseFees).ToString();
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;
        }

        private void _GetDetainedLicense() {
            _detainedLicense = clsDetainedLicense.Find(_licenseID);    
        }

        private void lbShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails licenseDetails = new frmLicenseDetails(_licenseID);
            licenseDetails.Show();
        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory= new frmLicenseHistory(_personID);
            licenseHistory.Show();
        }

        private void _InitializeReleaseLicenseApplication() { 
            
            _detainedLicense.Application.ApplicantPersonID = _personID;
            _detainedLicense.Application.ApplicationDate = DateTime.Now;
            _detainedLicense.Application.ApplicationTypeID = (int)clsGlobalSettings.enApplicationTypes.ReleaseDetainedDrivingLicsense;
            _detainedLicense.Application.ApplicationStatus = (int)clsGlobalSettings.localApplicationMode.New;
            _detainedLicense.Application.LastStatusDate = DateTime.Now;
            _detainedLicense.Application.PaidFees = _releaseDetainedLicenseFees;
            _detainedLicense.Application.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            _detainedLicense.ReleaseApplicationID = _detainedLicense.Application.AddNewApplication();
            _detainedLicense.ReleaseDate = DateTime.Now;
            _detainedLicense.ReleasedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {

            DialogResult detainLicenseResult = MessageBox.Show("Are you sure you want to release this license? ", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (detainLicenseResult == DialogResult.Yes)
            {
                _InitializeReleaseLicenseApplication();

                if (_detainedLicense.ReleaseLicense())
                {
                    clsLicense.UpdateLicenseActivation(_licenseID, true);
                    MessageBox.Show("License has been released successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _UpdateUIOnDetained();
                }
                else
                    MessageBox.Show("License has not been released...", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }             
        }

        private void _UpdateUIOnDetained() {
            lblApplicationID.Text = _detainedLicense.Application.ApplicationID.ToString();
            btnRelease.Enabled = false;
            lbShowLicenseInfo.Enabled = true;
        }

        private int _GetPersonID(int licenseID)
        {
            clsLicense license = clsLicense.FindByLicenseID(licenseID);
            clsApplication application = clsApplication.Find(license.ApplicationID);

            return application.ApplicantPersonID;
        }

        private void _GetDetainedLicense(int licenseID)
        {
            _detainedLicense = clsDetainedLicense.Find(licenseID);
        }

    }
}
