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
using DVLD.Properties;
using DVLD_Business;
using System.IO;
using DVLD.Global_Classes;

namespace DVLD.People
{
    public partial class frmAddEditPerson : Form
    {

        public delegate void DataBackEventHandler(int PersonID);

        public DataBackEventHandler DataBack;

        private int _personID = -1;
        enum enMode { Add = 0, Update = 1 };

        private enMode _Mode = enMode.Add;

        private clsPerson _Person;

        public frmAddEditPerson()
        {
            InitializeComponent();

            _Mode = enMode.Add;
        }

        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();

            _personID = PersonID;
            _Mode = enMode.Update;          
        }

        private void FillComboBoxWithCountries() { 
            
            DataTable Countries = clsCountry.ListCountries();
            
            foreach (DataRow Country in Countries.Rows)
            {
                cbCountry.Items.Add(Country["CountryName"]);
                
            }

            cbCountry.SelectedIndex = cbCountry.Items.IndexOf("Jordan"); ;
        }

        private void _Load() {

            lbRemoveImage.Visible = pbProfilePic.Image != null;
;
            if (_Mode == enMode.Add) {
                _InitializeAddMode();
                return;
            }
            
            _Person = clsPerson.Find(_personID);
                     
            if (_Person != null) {
                _UpdateGenderSelection();
                _UpdatePersonDetails();
                _UpdateSelectedCountry();
                _SetUpdateMode();
            }
        }

        private void _InitializeAddMode() {
            lblAddEdit.Text = "Add New Person";
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            rbMale.Checked = true;
            lbRemoveImage.Visible = false;
            _Person = new clsPerson();
        }

        private void _UpdateGenderSelection() {
            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
        }

        private void _UpdatePersonDetails() {
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            txtNationalNo.Text = _Person.NationalityNo;
            txtEmail.Text = _Person.Email;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            pbProfilePic.ImageLocation = _Person.ImagePath;
        }

        private void _UpdateSelectedCountry() { 
            cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
        }

        private void _SetUpdateMode() {
            _Mode = enMode.Update;
            lblAddEdit.Text = "Update Informarion";
            lblPersonID.Text = Convert.ToString(_personID);
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            FillComboBoxWithCountries();
            _Load();
        }

        private bool _HandlePersonImage() {
            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            if (_Person.ImagePath == pbProfilePic.ImageLocation) return true;

            if (_Person.ImagePath != "")
            {
                //first we delete the old image from the folder in case there is any.
                try
                {
                    File.Delete(_Person.ImagePath);
                }
                catch (IOException)
                {
                    // We could not delete the file.
                    //log it later   
                }
            }

            if (pbProfilePic.ImageLocation == null) return true;

            //then we copy the new image to the image folder after we rename it
            string SourceImageFile = pbProfilePic.ImageLocation.ToString();

            if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
            {
                pbProfilePic.ImageLocation = SourceImageFile;
                return true;
            }
            else
            {
                MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                clsUtil.ShowErrorMessage("Invalid fields detected! Hover over the red icon(s) to view the errors.", "Validation Error");
            }

            if (!_HandlePersonImage()) return;

            int nationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.PersonID = _personID;
            _Person.NationalityNo = txtNationalNo.Text.Trim();
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.Gender = (byte)(rbMale.Checked ? 0 : 1);
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.NationalityCountryID = nationalityCountryID;
            _Person.ImagePath = pbProfilePic.ImageLocation;


            if (_Person.Save()) {

                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                lblAddEdit.Text = "Update Info";

                clsUtil.ShowInformationMessage("Data has been Saved successfully", "Success");

                DataBack?.Invoke(_Person.PersonID);
            }
            else
                clsUtil.ShowErrorMessage("Something went wrong", "Failure");            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            _UpdateProfilePicBasedOnGender();
        }

        private void _UpdateProfilePicBasedOnGender() {
            if (pbProfilePic.ImageLocation != null) return;
            pbProfilePic.Image = rbMale.Checked ? Resources.Male_512 : Resources.Female_512;
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {

                e.Cancel = true;
                txtFirstName.Focus();
                errorProvider1.SetError(txtFirstName, "First Name can't be empty");
            }
            else {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, "");
            }

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if ( (txtNationalNo.Text.Trim() != _Person.NationalityNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim())) 
                || string.IsNullOrEmpty(txtNationalNo.Text)){ 
            
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National number is used for another person / Can't be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }

        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSecondName.Text))
            {
                e.Cancel = true;
                txtSecondName.Focus();
                errorProvider1.SetError(txtSecondName, "Second name can't be empty");
            }
            else {
                e.Cancel = false;
                errorProvider1.SetError(txtSecondName, "");
            }

        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLastName.Text))
            {
                e.Cancel = true;
                txtLastName.Focus();
                errorProvider1.SetError(txtLastName, "last name can't be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, "");
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                e.Cancel = true;
                txtPhone.Focus();
                errorProvider1.SetError(txtPhone, "Phone number can't be empty");
            }
            else { 
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtAddress.Text))
            {
                e.Cancel = true;
                txtAddress.Focus();
                errorProvider1.SetError(txtAddress, "Address can't be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, "");
            }
        }

        private void lbSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                
                string SelectedFilePath = openFileDialog1.FileName;

                pbProfilePic.Load(SelectedFilePath);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (!string.IsNullOrEmpty(txtEmail.Text)) {

                if (!Regex.IsMatch(txtEmail.Text, pattern))
                {
                    e.Cancel = true;
                    txtEmail.Focus();
                    errorProvider1.SetError(txtEmail, "Invalid email format");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(txtEmail, "");
                }
            }
          
        }

        private void lbRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbProfilePic.ImageLocation = null;
            _UpdateProfilePicBasedOnGender();
            lbRemoveImage.Visible = false;
        }
    }
}