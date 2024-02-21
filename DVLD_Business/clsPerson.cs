using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsPerson
    {
        public enum enMode { Add = 0, Update = 1}
        public enMode Mode = enMode.Add;
        public int PersonID { set; get; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set;}
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string NationalityNo { get; set; }
        public string ImagePath { get; set; }
        public int NationalityCountryID { get; set; }

        public clsCountry CountryInfo { get; set; }
        public string FullName() { 
            return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
        }
        public clsPerson() {
            PersonID = -1;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Now.AddYears(-18);
            Gender = 0;
            Address = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            NationalityNo = string.Empty;
            ImagePath = string.Empty;

            Mode = enMode.Add;
        }

        private clsPerson(int PersonID, string NationalityNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalityNo = NationalityNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gender = Gender;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.CountryInfo = clsCountry.Find(this.NationalityCountryID);
            this.ImagePath = ImagePath;

            Mode = enMode.Update;

        }

        public static DataTable ListPeople() { 
            return clsPersonData.ListPeople();
        }

        private bool _AddNewPerson() {

            this.PersonID = clsPersonData.AddNewPerson(this.NationalityNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

            return (PersonID != -1);    
        }

        private bool _UpdatePerson() {

            return clsPersonData.UpdatePerson(this.PersonID,this.NationalityNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.DateOfBirth, this.Gender, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
        }

        public static clsPerson Find(int PersonID) {

            string NationalityNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gender = 0;
            string Address = "", Phone = "", Email = "",  ImagePath = "";
            int NationalityCountryID = 0;


            if (clsPersonData.FindByPersonID(PersonID, ref NationalityNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))

                return new clsPerson(PersonID,NationalityNo, FirstName, SecondName, ThirdName, LastName,
                 DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "";
            DateTime DateOfBirth = DateTime.Now;
            byte Gender = 0;
            string Address = "", Phone = "", Email = "", ImagePath = "";
            int NationalityCountryID = 0, PersonID = 0;


            if (clsPersonData.FindByNationalNo(NationalNo, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                ref DateOfBirth, ref Gender, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))

                return new clsPerson(PersonID,NationalNo, FirstName, SecondName, ThirdName, LastName,
                 DateOfBirth, Gender, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }

        public static bool IsPersonExist(string NationalityNo) {

            return clsPersonData.IsPersonExist(NationalityNo);
        }
        public bool Save() {

            switch (Mode) {
                case enMode.Add:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    if (_UpdatePerson())
                        return true;
                    else
                        return false;
            }

            return false;
        }

        public static bool DeletePerson(int PersonID) {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static DataTable Filter(string filterWord, string type) { 
            return clsPersonData.Filter(filterWord, type);
        }
    }
}