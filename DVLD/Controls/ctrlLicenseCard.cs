using DVLD.Properties;
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

namespace DVLD.Controls
{
    public partial class ctrlLicenseCard : UserControl
    {

        private clsLicense _license;
        private clsLocalLicenseApplication _localDrivingLicenseApplication;
        public ctrlLicenseCard()
        {
            InitializeComponent();
        }

        public void _LoadLicenseInfoDetails(int localDrivingLicenseAppID) {

            _InitializeLicense(localDrivingLicenseAppID);

            if (_license == null)
                return;

            _UpdateLicenseInfo();

        }

        private void _InitializeLicense(int localDrivingLicenseAppID) {

            _localDrivingLicenseApplication = clsLocalLicenseApplication.Find(localDrivingLicenseAppID);
            _license = clsLicense.Find(_localDrivingLicenseApplication.Application.ApplicationID);
        }

        private void _UpdateLicenseInfo() {

            clsPerson person = clsPerson.Find(_localDrivingLicenseApplication.Application.ApplicantPersonID);

            byte gender = person.Gender;

            _ChangeGenderIcon(gender);

            lblLicenseClass.Text = clsLicenseClass.Find(_license.LicenseClassNo).Name;

            lblPersonName.Text = _localDrivingLicenseApplication.ApplicantFullName;


            lblLicenseID.Text = _license.ID.ToString();
            lblNationalNo.Text = person.NationalityNo.ToString();
            lblGender.Text = gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = _license.IssueDate.ToString("yyy/MM/dd");
            lblIssueReason.Text =  clsGlobalSettings.GetIssueReasonString(_license.IssueReason);
            lblNotes.Text = _license.Notes;
            lblIsActive.Text = _license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToString("yyy/MM/dd");
            lblDriverID.Text = _license.DriverID.ToString();
            lblExpirationDate.Text = _license.ExpirationDate.ToString("yyy/MM/dd");
            pbPersonalPic.ImageLocation = person.ImagePath;
            lblIsDetained.Text = "Unknown yer";
        }

        private void _ChangeGenderIcon(byte gender) {

            pbGenderIcon.Image = gender == 0 ? Resources.Man_32 : Resources.Woman_32;
        }
    }
}