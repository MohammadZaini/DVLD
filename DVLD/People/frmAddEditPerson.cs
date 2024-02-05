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

namespace DVLD.People
{
    public partial class frmAddEditPerson : Form
    {

        public delegate void DataBackEventHandler(int PersonID);

        public DataBackEventHandler DataBack;

        private int _PersonID;
        enum enMode { Add = 0, Update = 1 };

        private enMode _Mode = enMode.Add;

        private clsPerson _Person;

        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();        
            _PersonID = PersonID;

            if(PersonID == -1)
                _Mode = enMode.Add;
            else
                _Mode = enMode.Update;

            this.StartPosition = FormStartPosition.CenterScreen;
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
            
            _Person = clsPerson.Find(_PersonID);
                     
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
            lblPersonID.Text = Convert.ToString(_PersonID);
        }

        private void frmAddEditPerson_Load(object sender, EventArgs e)
        {
            FillComboBoxWithCountries();
            _Load();
           // btnSave.Enabled = false; 
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            int CountryID = clsCountry.Find(cbCountry.Text).ID;

            byte Gender = 0; 

            if(rbFemale.Checked)
                Gender = 1;
 
            _Person.PersonID = _PersonID;
            _Person.NationalityNo = txtNationalNo.Text;
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = Gender;
            _Person.Address = txtAddress.Text;
            _Person.Phone = txtPhone.Text;
            _Person.Email = txtEmail.Text;
            _Person.NationalityCountryID = CountryID;
            _Person.ImagePath = pbProfilePic.ImageLocation;


            if (_Person.Save()) { 
                MessageBox.Show("Data has been Saved successfully");
                
            }
            else
                MessageBox.Show("Something went wrong");


            _Mode = enMode.Update;
            lblAddEdit.Text = "Update Info";
            lblPersonID.Text = Convert.ToString(_Person.PersonID);

            DataBack?.Invoke(_Person.PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked)
                pbProfilePic.Image = Resources.patient_boy;
            else
                pbProfilePic.Image = Resources.person_woman;

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
            if (clsPerson.IsPersonExist(txtNationalNo.Text) || string.IsNullOrEmpty(txtNationalNo.Text)){ 
            
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
            lbRemoveImage.Visible = false;
        }
    }
}