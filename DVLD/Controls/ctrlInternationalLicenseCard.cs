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
    public partial class ctrlInternationalLicenseCard : UserControl
    {
        private clsInternationalLicense _internationalLicense;
        public ctrlInternationalLicenseCard()
        {
            InitializeComponent();
        }

        public void LoadInternationalLicenseInfo(int internationalLicenseID) {

            _internationalLicense = clsInternationalLicense.Find(internationalLicenseID);

            if (_internationalLicense == null) return;

            int applicationID = _internationalLicense.ApplicationID;

            _internationalLicense.Application = FillApplicationObject(applicationID);

            int personID = _internationalLicense.Application.ApplicantPersonID;

            clsPerson person = FillPersonObject(personID);

            _UpdateUI(person);
        }

        private void _UpdateUI(clsPerson person) {

            int licenseID = _internationalLicense.IssuedUsingLocalLicenseID;

            lblDriverName.Text = person.FullName();
            lblInternationalLicenseID.Text = _internationalLicense.ID.ToString();
            lblLocalLicenseID.Text = licenseID.ToString();
            lblNationalNo.Text = person.NationalityNo.ToString();
            lblGender.Text = person.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = _internationalLicense.IssueDate.ToString("yyyy/MM/dd");
            lblApplicationID.Text = _internationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _internationalLicense.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToString("yyyy/MM/dd");
            lblDriverID.Text = clsLicense.Find(licenseID).DriverID.ToString();
            lblExpirationDate.Text = _internationalLicense.ExpirationDate.ToString("yyyy/MM/dd");
            pbProfilePic.ImageLocation = person.ImagePath;
        }

        private clsPerson FillPersonObject(int personID) { 
            
           return clsPerson.Find(personID);
        }

        private clsApplication FillApplicationObject(int applicationID)
        {
            return clsApplication.Find(applicationID);
        }
    }
}