using DVLD.ApplicationTypes.NewDrivingLicense;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationTypes.RenewLicense
{
    public partial class frmRenewLicense : Form
    {
        private clsLicense _renewedLicense;
        private clsLicense _oldLicense;
        private int _personID;
        public frmRenewLicense()
        {
            InitializeComponent();
            CenterToScreen();

            ctrlLicenseCardWithFilter1.DataBack += _UpdateUIOnLicenseSearch;
        }

        private void _LoadRenewApplication() {
            lblApplicationDate.Text = DateTime.Now.ToString(clsGlobalSettings.dateFormat);
            lblIssueDate.Text = DateTime.Now.ToString(clsGlobalSettings.dateFormat);
            lblApplicationFees.Text = ((int)_GetRenewApplicationFees()).ToString();
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;
        }

        private decimal _GetRenewApplicationFees() {
            return clsApplicationType.Find((int)clsGlobalSettings.enApplicationTypes.RenewDrivingLicenseService).Fees;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {
            _LoadRenewApplication();
        }

        private bool _IsLicenseExpired(int licenesID) { 
            return clsLicense.IsLicenseExpired(licenesID);  
        }

        private byte _GetValidityLengthForLicenseClass() {

            return clsLicenseClass.Find(_oldLicense.LicenseClassNo).DefaultValidityLength;
        } 

        private void _UpdateUIOnLicenseSearch(int licenseID, int personID, bool controlState)
        {
            _GetOldLicense(licenseID);
            _personID = personID;

            btnRenew.Enabled = controlState;

            lbShowLicenseHistory.Enabled = true;
            lblExpirationDate.Text = DateTime.Now.AddYears(_GetValidityLengthForLicenseClass()).ToString(clsGlobalSettings.dateFormat);
            lblLicenseFees.Text = 20.ToString();
            lblTotalFees.Text = (((int)_GetRenewApplicationFees()) + 20).ToString();
            lblOldLicenseID.Text = licenseID.ToString();

            if (controlState == false) return;

            _ShowLicenseIsNotExpiredErrorMessage(licenseID);
        }

        private void _ShowLicenseIsNotExpiredErrorMessage(int licenseID) {

            if (_IsLicenseExpired(licenseID))
            {
                btnRenew.Enabled = false;

                MessageBox.Show("License Is Not Expired yet, it will expire on " +
                        _oldLicense.ExpirationDate.ToString(clsGlobalSettings.dateFormat), "Failure", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }         
            else
                btnRenew.Enabled = true;
        }

        private void _GetOldLicense(int licenseID) { 
            _oldLicense = clsLicense.FindByLicenseID(licenseID);
        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(_personID);
            licenseHistory.ShowDialog();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            DialogResult renewLicenseResult = MessageBox.Show("Are you sure you want to renew this license?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (renewLicenseResult == DialogResult.Yes)
                if (_renewedLicense.Save())
                {
                    _InitializeRenewedLicense();

                    clsLicense.UpdateLicenseActivation(_oldLicense.ID, false);
                    MessageBox.Show("License has been renewed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblRenewLicenseAppID.Text = _renewedLicense.ApplicationID.ToString();
                    lblRenewedLicenseID.Text = _renewedLicense.ID.ToString();
                    lbShowNewLicenseInfo.Enabled = true;
                    btnRenew.Enabled = false;
                }
                else
                    MessageBox.Show("License has Not been renewed...", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);           
        }

        private void _InitializeRenewedLicense() {

            _renewedLicense = new clsLicense();   

            // Initialize Renew Application
            clsApplication renewLicenseApplication = new clsApplication();
            InitializeRenewApplication(renewLicenseApplication);

            // Initialze Renewed License
            _renewedLicense.ApplicationID = renewLicenseApplication.ApplicationID;
            _renewedLicense.Application = renewLicenseApplication;
            _renewedLicense.DriverID = _oldLicense.DriverID;
            _renewedLicense.Driver.PersonID = _personID;
            _renewedLicense.LicenseClassNo = _oldLicense.LicenseClassNo;
            _renewedLicense.IssueDate = DateTime.Now;
            _renewedLicense.ExpirationDate = DateTime.Now.AddYears(_GetValidityLengthForLicenseClass());
            _renewedLicense.Notes = txtNotes.Text;
            _renewedLicense.PaidFees = 20;
            _renewedLicense.IsActive = true;
            _renewedLicense.IssueReason = (byte)clsGlobalSettings.enLicenseIssueReason.Renew;
            _renewedLicense.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }

        private void InitializeRenewApplication(clsApplication renewLicenseApplication) {

            renewLicenseApplication.ApplicantPersonID = _personID;
            renewLicenseApplication.ApplicationDate = DateTime.Now;
            renewLicenseApplication.ApplicationTypeID = (int)clsGlobalSettings.enApplicationTypes.RenewDrivingLicenseService;
            renewLicenseApplication.ApplicationStatus = (int)clsGlobalSettings.localApplicationMode.New;
            renewLicenseApplication.LastStatusDate = _oldLicense.IssueDate;
            renewLicenseApplication.PaidFees = _GetRenewApplicationFees();
            renewLicenseApplication.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            renewLicenseApplication.AddNewApplication();           
        }

        private void lbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_renewedLicense != null)
                ShowLicenseInfo(_renewedLicense.ID);
            else
                ShowLicenseInfo(_oldLicense.ID);
        }

        private void ShowLicenseInfo(int licenseID) {
            frmLicenseDetails licenseDetails = new frmLicenseDetails(licenseID);
            licenseDetails.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}