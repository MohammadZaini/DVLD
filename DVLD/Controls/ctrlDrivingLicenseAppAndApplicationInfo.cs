using DVLD.People;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlDrivingLicenseAppAndApplicationInfo : UserControl
    {
        private clsLocalLicenseApplication _localLicenseApplication;
        public ctrlDrivingLicenseAppAndApplicationInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationDetials(int localDrivingLicenseAppID) {
            lblDrivingLicenseAppID.Text = localDrivingLicenseAppID.ToString();

           _localLicenseApplication = clsLocalLicenseApplication.Find(localDrivingLicenseAppID);

            if (_localLicenseApplication == null)
                return;

            _UpdateApplicationDetails(_localLicenseApplication);

        }

        private void _UpdateApplicationDetails(clsLocalLicenseApplication localLicenseApplication) {
            lblAppID.Text = localLicenseApplication.Application.ApplicationID.ToString();
            lblApplicant.Text = localLicenseApplication.ApplicantFullName;
            lblCreatedBy.Text = localLicenseApplication.Application.CreatedByUserName;
            lblLicenseClass.Text = localLicenseApplication.LicenseClassName;
            lblPassedTests.Text = localLicenseApplication.Application.PassedTests.ToString();
            lblStatus.Text = localLicenseApplication.Application.ApplicationStatusName;
            lblStatusDate.Text = localLicenseApplication.Application.LastStatusDate.ToString("yyyy/MM/dd");
            lblDate.Text = localLicenseApplication.Application.LastStatusDate.ToString("yyyy/MM/dd");
            lblFees.Text = clsApplication.GetFees(localLicenseApplication.Application.ApplicationTypeID);
            lblType.Text = clsApplicationType.Find(localLicenseApplication.Application.ApplicationTypeID).Title;
        }

        private void ctrlDrivingLicenseAppAndApplicationInfo_Load(object sender, EventArgs e)
        {

        }

        private void lbViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_localLicenseApplication == null)
                return;

            frmPersonDetails personDetailsFrm = new frmPersonDetails(_localLicenseApplication.Application.ApplicantPersonID);
            personDetailsFrm.ShowDialog();

            
        }
    }
}
