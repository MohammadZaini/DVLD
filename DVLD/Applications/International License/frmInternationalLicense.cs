﻿using DVLD.ApplicationTypes.InternationalLicense;
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

namespace DVLD.ApplicationTypes
{
    public partial class frmInternationalLicense : Form
    {
        private clsLicense _localLicense;
        private clsInternationalLicense _internationalLicense;
        private int _personID;
        private int _internationalLicenseID;
        public frmInternationalLicense()
        {
            InitializeComponent();
            CenterToScreen();

            ctrlLicenseCardWithFilter1.DataBack += _UpdateUIOnSearchButtonClicked;
        }
        private void _UpdateApplicationInfoUI() {
            lblApplicationDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            lblIssueDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            lblFees.Text = _GetInternationlLicenseAppFees().ToString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToString("yyyy/MM/dd");
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.Username;           
        }

        private int _GetInternationlLicenseAppFees() { 
            return (int)clsApplicationType.Find((int)clsGlobalSettings.enApplicationTypes.NewInternationalLicense).Fees;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmInternationalLicense_Load(object sender, EventArgs e)
        {
            _UpdateApplicationInfoUI();
           // lbShowLicenseInfo.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseCardWithFilter1.LicenseID;

            if (clsInternationalLicense.IsInternationalLicenseExist(licenseID))
            {
                MessageBox.Show("This individual already has an International License!", "Failure", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);

                return;
            }

            DialogResult issueResult = MessageBox.Show("Are you sure you want to issue an international license for this individual? ",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (issueResult == DialogResult.No) return;

            GetLocalLicense(licenseID);

            _InitializeInternationlLicense();

            if (_internationalLicense.Save())
            {
                MessageBox.Show("International license has been issued successfully for this individual!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

                _UpdateUI();
            }
            else
                MessageBox.Show("Something went wrong!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void GetLocalLicense(int licenseID) {
            _localLicense = clsLicense.FindByLicenseID(licenseID);
        }

        private void _InitializeInternationlLicense() {

            _internationalLicense = new clsInternationalLicense();

            DateTime CurrentDate = DateTime.Now;

            int personID = clsApplication.Find(_localLicense.ApplicationID).ApplicantPersonID;

            // Initializing new international application
            _internationalLicense.Application.ApplicantPersonID = personID;
            _internationalLicense.Application.ApplicationDate = CurrentDate;
            _internationalLicense.Application.ApplicationTypeID = (int)clsGlobalSettings.enApplicationTypes.NewInternationalLicense;
            _internationalLicense.Application.ApplicationStatus = (byte)clsGlobalSettings.localApplicationMode.New;
            _internationalLicense.Application.LastStatusDate = CurrentDate;
            _internationalLicense.Application.PaidFees = _GetInternationlLicenseAppFees();
            _internationalLicense.Application.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            // Iintialzing international license
            _internationalLicense.DriverID = _localLicense.DriverID;
            _internationalLicense.IssuedUsingLocalLicenseID = _localLicense.ID;
            _internationalLicense.IssueDate = CurrentDate;
            _internationalLicense.ExpirationDate = CurrentDate.AddYears(1);
            _internationalLicense.IsActive = true;
            _internationalLicense.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

        }

        private void _UpdateUI() {

            lblInternationalAppID.Text = _internationalLicense.ApplicationID.ToString();
            lblInternationalLocalLicenseID.Text = _internationalLicense.ID.ToString();         
            lbShowLicenseInfo.Enabled = true;
        }
        
        private void lbShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (_internationalLicense != null)
                _internationalLicenseID = _internationalLicense.ID;

            frmInternationalDriverInfo internationalDriverInfoFrm = new frmInternationalDriverInfo(_internationalLicenseID);
            internationalDriverInfoFrm.ShowDialog();
        }

        private void lbShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmLicenseHistory licenseHistory = new frmLicenseHistory(_personID);
            licenseHistory.ShowDialog();
        }

        private void _UpdateUIOnSearchButtonClicked(int localLicenseID,  int personID , bool controlState) { 
            _personID = personID;

            _UpdateInternationalLicenseInfo(localLicenseID);
   
            lblLocalLicenseID.Text = localLicenseID.ToString();
            btnIssue.Enabled = controlState;
            lbShowLicenseInfo.Enabled = !controlState;
            lbShowLicenseHistory.Enabled = true;

            if (controlState == false) return;

            if (!_IsLicenseClassOdinaryDrivingLicense(localLicenseID))
            {
                btnIssue.Enabled = false;

                MessageBox.Show("Local license must be Odinary Driving License in order to issue the international license",
                    "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _CheckInternationalLicenseExistence(localLicenseID);          
        }

        private void _UpdateInternationalLicenseInfo(int localLicenseID)
        {
            FillInternationalLicenseObject(localLicenseID);

            if (_internationalLicense != null)
            {
                _internationalLicenseID = _internationalLicense.ID;
                lblInternationalAppID.Text = _internationalLicense.ApplicationID.ToString();
                lblInternationalLocalLicenseID.Text = _internationalLicense.ID.ToString();
            }
        }

        private void _CheckInternationalLicenseExistence(int localLicenseID)
        {         
            if (clsInternationalLicense.IsInternationalLicenseExist(localLicenseID))
            {
                btnIssue.Enabled = false;
                lbShowLicenseInfo.Enabled = true;
                MessageBox.Show("This individual already has an International License!", "Failure", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FillInternationalLicenseObject(int localLicenseID) {
            _internationalLicense = clsInternationalLicense.FindByLicenseID(localLicenseID);

            if (_internationalLicense == null) return;
        }

        private bool _IsLicenseClassOdinaryDrivingLicense(int localLicenseID) {

            return clsLicense.IsLicenseClassOdinaryDrivingLicense(localLicenseID);
        }
    }
}