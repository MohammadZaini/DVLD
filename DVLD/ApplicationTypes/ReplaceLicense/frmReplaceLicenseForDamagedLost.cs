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

namespace DVLD.ApplicationTypes.ReplaceLicense
{
    public partial class frmReplaceLicenseForDamagedLost : Form
    {
        private int _personID;
        private clsLicense _oldLicense;
        private clsLicense _replacedLicense;
        public frmReplaceLicenseForDamagedLost()
        {
            InitializeComponent();
            CenterToScreen();

            ctrlLicenseCardWithFilter1.DataBack += _UpdateUIOnLicenseSearch;
        }

        private void _LoadReplaceLicenseApplication() { 
            lblApplicationDate.Text = DateTime.Now.ToString(clsGlobalSettings.dateFormat);
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;
            lblApplicationFees.Text = ((int)_GetReplacementForDamagedLicenseApplicationFees()).ToString();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void _UpdateUIOnLicenseSearch(int licenseID, int personID, bool controlState) {
            _GetOldLicense(licenseID);
            _personID = personID;

            lblOldLicenseID.Text = licenseID.ToString();
            lbShowLicenseHistory.Enabled = true;
            btnIssueReplacement.Enabled = controlState;
        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(_personID);
            licenseHistory.ShowDialog();
        }

        private void lbShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_replacedLicense != null)
                ShowLicenseInfo(_replacedLicense.ID);
            else
                ShowLicenseInfo(_oldLicense.ID);
        }

        private void ShowLicenseInfo(int licenseID)
        {
            frmLicenseDetails licenseDetails = new frmLicenseDetails(licenseID);
            licenseDetails.ShowDialog();
        }

        private void _InitializeReplacedLicense()
        {

            _replacedLicense = new clsLicense();

            // Initialize Replace Application
            clsApplication replaceLicenseApplication = new clsApplication();
            InitializeReplaceLicenseApplication(replaceLicenseApplication);

            // Initialze Replaced License
            _replacedLicense.ApplicationID = replaceLicenseApplication.ApplicationID;
            _replacedLicense.Application = replaceLicenseApplication;
            _replacedLicense.DriverID = _oldLicense.DriverID;
            _replacedLicense.Driver.PersonID = _personID;
            _replacedLicense.LicenseClassNo = _oldLicense.LicenseClassNo;
            _replacedLicense.IssueDate = DateTime.Now;
            _replacedLicense.ExpirationDate = DateTime.Now.AddYears(clsGlobalSettings.GetValidityLengthForLicenseClass(_oldLicense.LicenseClassNo));
            _replacedLicense.Notes = _oldLicense.Notes;
            _replacedLicense.PaidFees = 20;
            _replacedLicense.IsActive = true;
            _replacedLicense.IssueReason = rbDamagedLicense.Checked ? (byte)clsGlobalSettings.enLicenseIssueReason.ReplacementForDamaged :
                                           (byte)clsGlobalSettings.enLicenseIssueReason.ReplacementForLost;
            _replacedLicense.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }

        private void InitializeReplaceLicenseApplication(clsApplication replaceLicenseApplication)
        {
            int replacementForDamaged = (int)clsGlobalSettings.enApplicationTypes.ReplacementForADamagedDrivingLicense;
            int replacementForLost = (int)clsGlobalSettings.enApplicationTypes.ReplacementForALostDrivingLicense;

            replaceLicenseApplication.ApplicantPersonID = _personID;
            replaceLicenseApplication.ApplicationDate = DateTime.Now;

            replaceLicenseApplication.ApplicationTypeID = rbDamagedLicense.Checked ? replacementForDamaged : replacementForLost;

            replaceLicenseApplication.ApplicationStatus = (int)clsGlobalSettings.localApplicationMode.New;
            replaceLicenseApplication.LastStatusDate = _oldLicense.IssueDate;
            replaceLicenseApplication.PaidFees = rbDamagedLicense.Checked ? clsGlobalSettings.GetApplicationFees(replacementForDamaged) : 
                                                    clsGlobalSettings.GetApplicationFees(replacementForLost);
            replaceLicenseApplication.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            replaceLicenseApplication.AddNewApplication();
        }

        private decimal _GetReplacementForDamagedLicenseApplicationFees() {

            return clsApplicationType.Find((int)clsGlobalSettings.enApplicationTypes.ReplacementForADamagedDrivingLicense).Fees;
        }

        private decimal _GetReplacementForLostLicenseApplicationFees()
        {
            return clsApplicationType.Find((int)clsGlobalSettings.enApplicationTypes.ReplacementForALostDrivingLicense).Fees;
        }

        private void frmReplaceLicenseForDamagedLost_Load(object sender, EventArgs e)
        {
            _LoadReplaceLicenseApplication();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDamagedLicense.Checked)
            {
                lblApplicationFees.Text = ((int)_GetReplacementForDamagedLicenseApplicationFees()).ToString();
                lblReplacementForLostDamagedLicense.Text = "Replacement For Damaged License";
            }
            else 
            {
                lblApplicationFees.Text = ((int)_GetReplacementForLostLicenseApplicationFees()).ToString();
                lblReplacementForLostDamagedLicense.Text = "Replacement For Lost License";
            }
        }

        private void _GetOldLicense(int licenseID) {
            _oldLicense = clsLicense.FindByLicenseID(licenseID);
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            DialogResult renewLicenseResult = MessageBox.Show("Are you sure you want to replace this license?", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (renewLicenseResult == DialogResult.Yes)
            { 
                _InitializeReplacedLicense();

                if (_replacedLicense.Save())
                {
                    clsLicense.UpdateLicenseActivation(_oldLicense.ID, false);
                    MessageBox.Show("License has been replaced successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblLicenseReplacementAppID.Text = _replacedLicense.ApplicationID.ToString();
                    lblReplacedLicenseID.Text = _replacedLicense.ID.ToString();
                    lbShowNewLicenseInfo.Enabled = true;
                    btnIssueReplacement.Enabled = false;
                }
                else
                    MessageBox.Show("License has Not been replaced...", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            
        }
    }
}