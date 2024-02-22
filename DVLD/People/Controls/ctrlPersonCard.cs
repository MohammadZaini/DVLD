using DVLD.People;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {

        private int _personID = -1;
        public int PersonID { get { return _personID; } }

        private clsPerson _person;

        public clsPerson Person { get { return _person; } }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        
        public void LoadPersonData(int personID)
        {
            _person = clsPerson.Find(personID);

            if (_person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person Was Found With ID: " + personID, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        public void LoadPersonData(string nationalNo)
        {
            _person = clsPerson.Find(nationalNo);

            if (_person == null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No Person Was Found With Nationa No: " + nationalNo, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }

        private void _ResetPersonInfo() {
            lblPersonID.Text = "[???]";
            lblName.Text = "[???]";
            lblNationalNo.Text = "[???]";
            lblGender.Text = "[???]";
            lblEmail.Text = "[???]";
            lblAddress.Text = "[???]";
            lblDateOfBirth.Text = "[???]";
            lblPhone.Text = "[???]";
            lblCountry.Text = "[???]";
            pbPersonalPic.Image = Resources.Male_512;
        }

        private void _FillPersonInfo()
        {
            _personID = _person.PersonID;

            lblPersonID.Text = PersonID.ToString();
            lblAddress.Text = _person.Address;
            lblDateOfBirth.Text = _person.DateOfBirth.ToString("yyyy/MM/dd");
            lblEmail.Text = _person.Email;
            lblPhone.Text = _person.Phone;
            lblNationalNo.Text = _person.NationalityNo;
            lblName.Text = _person.FullName;
            lblGender.Text = _person.Gender == 0 ? "Male" : "Female";
            lblCountry.Text = _person.CountryInfo.CountryName;
            pbPersonalPic.ImageLocation = _person.ImagePath;
            _LoadPersonImage();

            // Create a circular region
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, pbPersonalPic.Width, pbPersonalPic.Height);

            // Set PictureBox's Region property to the circular region
            pbPersonalPic.Region = new Region(path);
        }

        private void _LoadPersonImage() {

            pbPersonalPic.Image = _person.Gender == 0 ? Resources.Male_512 : Resources.Female_512;

            string imagePath = _person.ImagePath;

            if (File.Exists(imagePath))
                pbPersonalPic.ImageLocation = imagePath;
            else
                MessageBox.Show("Couldn't find this image: " + imagePath, "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void lblEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson addEditFrm = new frmAddEditPerson(_personID);
            addEditFrm.ShowDialog();

            // Refresh Person Data After Being Updated
            LoadPersonData(_personID);
        }
    }
}