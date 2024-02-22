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
        public int PersonID { get; set; }
        public ctrlLicenseCard()
        {
            InitializeComponent();
        }

        public void _LoadLicenseInfoDetails(int licenseID) {

            _InitializeLicense(licenseID);
           
            if (_license == null)
                return;

            FillApplicationInfo();

            clsPerson person = _GetPerson();

            PersonID = person.PersonID;

            _FillPersonInfo(person);

            _UpdateLicenseUI(person);
        }

        private void _InitializeLicense(int licenseID)
        {
            _license = clsLicense.FindByLicenseID(licenseID);
        }

        private void _UpdateLicenseUI(clsPerson person ) {
       
            lblLicenseClass.Text = _GetLicenseClassName();
            lblPersonName.Text = person.FullName;
            lblLicenseID.Text = _license.ID.ToString();
            lblNationalNo.Text = person.NationalityNo.ToString();
            lblGender.Text = person.Gender == 0 ? "Male" : "Female";
            lblIssueDate.Text = _license.IssueDate.ToString(clsGlobalSettings.dateFormat);
            lblIssueReason.Text =  clsGlobalSettings.GetIssueReasonString(_license.IssueReason);
            lblNotes.Text = _license.Notes;
            lblIsActive.Text = _license.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = person.DateOfBirth.ToString(clsGlobalSettings.dateFormat);
            lblDriverID.Text = _license.DriverID.ToString();
            lblExpirationDate.Text = _license.ExpirationDate.ToString(clsGlobalSettings.dateFormat);
            pbPersonalPic.ImageLocation = person.ImagePath;
            lblIsDetained.Text = clsLicense.IsDetained(_license.ID) ? "Yes" : "No";
        }

        private clsApplication _GetApplication() {         
            return clsApplication.Find(_license.ApplicationID);
        }

        private clsPerson _GetPerson() { 
            return clsPerson.Find(_license.Application.ApplicantPersonID);
        }
        
        private string _GetLicenseClassName() {
            return clsLicenseClass.Find(_license.LicenseClassNo).Name;
        }

        private void _ChangeGenderIcon(byte gender) {

            pbGenderIcon.Image = gender == 0 ? Resources.Man_32 : Resources.Woman_32;
        }

        private void FillApplicationInfo()
        {
            _license.Application = _GetApplication();
        }

        private void _FillPersonInfo(clsPerson person)
        {
            byte gender = person.Gender;
            _ChangeGenderIcon(gender);
        }
    }
}