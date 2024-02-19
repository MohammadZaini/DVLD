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
    public partial class frmDetainLicense : Form
    {
        private int _personID;
        private int _licenseID;
        private clsDetainedLicense _detainedLicense;
        public frmDetainLicense()
        {
            InitializeComponent();
            CenterToScreen();

            ctrlLicenseCardWithFilter1.DataBack += _UpdateUIOnLicenseSearch;        
        }

        private void _LoadDetainInfo() {
            lblDetainDate.Text = DateTime.Now.ToString(clsGlobalSettings.dateFormat);
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;
                   
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

            _licenseID = licenseID;
            _personID = personID;
            lbShowLicenseHistory.Enabled = true;
            btnDetainLicense.Enabled = controlState;

            if (_IsDetained(licenseID))
            {
                btnDetainLicense.Enabled = false;
                MessageBox.Show("Selected License Is Already Detained!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lbShowLicenseInfo.Enabled = false;
        }

        private bool _IsDetained(int licenseID) { 
            
            return clsLicense.IsDetained(licenseID);
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            _LoadDetainInfo();        
        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(_personID);
            licenseHistory.ShowDialog();
        }

        private void lbShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseDetails licenseDetails = new frmLicenseDetails(_licenseID);
            licenseDetails.ShowDialog();
        }

        private void _InitializeDetainedLicense() {
            _detainedLicense = new clsDetainedLicense();

            _detainedLicense.LicenseID = _licenseID;
            _detainedLicense.Date = DateTime.Now;
            _detainedLicense.IsReleased = false;
            _detainedLicense.FineFees = Convert.ToDecimal(txtFineFees.Text);
            _detainedLicense.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            _InitializeDetainedLicense();

            DialogResult detainLicenseResult = MessageBox.Show("Are you sure you want to detain this license? ", "Confirmation",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if(detainLicenseResult == DialogResult.Yes)
                if (_detainedLicense.DetainLicense())
                {
                    clsLicense.UpdateLicenseActivation(_licenseID, false);
                    MessageBox.Show("License has been detained successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _UpdateUIOnDetained();
                }
                else
                    MessageBox.Show("License has not been detained...", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _UpdateUIOnDetained() {
            lblDetainID.Text = _detainedLicense.ID.ToString();
            btnDetainLicense.Enabled = false;
            lbShowLicenseInfo.Enabled = true;
        }        
      
    }
}