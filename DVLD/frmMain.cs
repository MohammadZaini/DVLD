﻿using DVLD.ApplicationTypes;
using DVLD.ApplicationTypes.InternationalLicense;
using DVLD.ApplicationTypes.NewDrivingLicense;
using DVLD.ApplicationTypes.RenewLicense;
using DVLD.ApplicationTypes.ReplaceLicense;
using DVLD.Detain_Licenses;
using DVLD.Drivers;
using DVLD.People;
using DVLD.TestTypes;
using DVLD.Users;
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

namespace DVLD
{
    public partial class frmMain : Form
    {
        private frmUserLogin _frmLogin;
        public frmMain(frmUserLogin frmLogin)
        {
            InitializeComponent();

            _frmLogin = frmLogin;        
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo UserInfo = new frmUserInfo(clsGlobalSettings.LoggedInUser.UserID);
            UserInfo.ShowDialog();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListUsers ListUsers = new frmListUsers();
            ListUsers.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsGlobalSettings.LoggedInUser = null;
            _frmLogin.Show();
            Close();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword ChangePasswordform = new frmChangePassword(clsGlobalSettings.LoggedInUser.UserID);
            ChangePasswordform.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmApplicationTypes AppTypesfrm = new frmApplicationTypes();
            AppTypesfrm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestTypes TestTypesfrm = new frmTestTypes();
            TestTypesfrm.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddLocalDrivingLicenseApplication LocalLicenseFrm = new frmAddLocalDrivingLicenseApplication(1);
            LocalLicenseFrm.ShowDialog();
        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingApplications localDrivingApplicationsfrm = new frmListLocalDrivingApplications();    
            localDrivingApplicationsfrm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers listDriversFrm = new frmListDrivers();
            listDriversFrm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInternationalLicense internationalLicenseAppFrm = new frmInternationalLicense();
            internationalLicenseAppFrm.ShowDialog();
        }

        private void internationalDrivingLiceseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications internationalLicenseApplications = new frmListInternationalLicenseApplications();
            internationalLicenseApplications.ShowDialog();
        }

        private void rnewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLicense renewLicense = new frmRenewLicense();
            renewLicense.ShowDialog();
        }

        private void replacementForDamagedOrLostLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLicenseForDamagedLost replaceDamagedLostLicense = new frmReplaceLicenseForDamagedLost();
            replaceDamagedLostLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingApplications localLicenseApplicationsList = new frmListLocalDrivingApplications();
            localLicenseApplicationsList.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicense detainLicense = new frmDetainLicense();
            detainLicense.ShowDialog();
        }

        private void managedDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses detainedLicensesList = new frmListDetainedLicenses();
            detainedLicensesList.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense releaseDetainedLicense = new frmReleaseDetainedLicense();
            releaseDetainedLicense.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense releaseDetainedLicense = new frmReleaseDetainedLicense();
            releaseDetainedLicense.ShowDialog();
        }
    }
}