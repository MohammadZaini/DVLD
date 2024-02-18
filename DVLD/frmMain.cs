using DVLD.ApplicationTypes;
using DVLD.ApplicationTypes.InternationalLicense;
using DVLD.ApplicationTypes.NewDrivingLicense;
using DVLD.ApplicationTypes.RenewLicense;
using DVLD.ApplicationTypes.ReplaceLicense;
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
        public frmMain()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople frm = new frmListPeople();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo UserInfo = new frmUserInfo(clsGlobalSettings.LoggedInUser.PersonID);
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
            this.Close();
            this.Dispose();
          
            frmUserLogin userLogin = new frmUserLogin();
            userLogin.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword ChangePasswordform = new frmChangePassword(clsGlobalSettings.LoggedInUser.PersonID, 
                                                                         clsGlobalSettings.LoggedInUser.UserID);
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
    }
}