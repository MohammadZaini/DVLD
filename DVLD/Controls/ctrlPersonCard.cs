using DVLD.People;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {

        public int PersonID;
        private clsPerson _Person;
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        
        public void LoadPersonData(int PersonID)
        {
            this.PersonID = PersonID;
            _Person = clsPerson.Find(PersonID);

            if (_Person != null)
            {
                clsCountry Country = clsCountry.Find(_Person.NationalityCountryID);

                string Gender = "Male";

                if (_Person.Gender == 1)
                    Gender = "Female";

                lblPersonID.Text = PersonID.ToString();
                lblAddress.Text = _Person.Address;
                lblDateOfBirth.Text = _Person.DateOfBirth.ToString("yyyy/MM/dd");
                lblEmail.Text = _Person.Email;
                lblPhone.Text = _Person.Phone;
                lblNationalNo.Text = _Person.NationalityNo;
                lblName.Text = _Person.FullName();
                lblGender.Text = Gender;
                pbPersonalPic.ImageLocation = _Person.ImagePath;

                if(Country != null)
                    lblCountry.Text = Country.CountryName;

                // Create a circular region
                GraphicsPath path = new GraphicsPath();
                path.AddEllipse(0, 0, pbPersonalPic.Width, pbPersonalPic.Height);

                // Set PictureBox's Region property to the circular region
                pbPersonalPic.Region = new Region(path);

            }
        }

        private void lblEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddEditPerson addEditFrm = new frmAddEditPerson(PersonID);
            addEditFrm.DataBack += LoadPersonData;
            addEditFrm.ShowDialog();
        }
    }
}