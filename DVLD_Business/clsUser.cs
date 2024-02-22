using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsUser 
    {
        enum enMode { AddNew = 0, Update = 1};

        private enMode _Mode = enMode.AddNew;

        public int UserID { get; set; }
        public int PersonID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        public clsPerson Person { get; set; }
        
        public clsUser() { 
            this.UserID = -1;
            this.Username = string.Empty;
            this.Password = string.Empty;
            this.IsActive = true;
            Person = new clsPerson();
        }

        private clsUser(int UserID, int PersonID, string Username, string Password, bool IsActive) { 
            
            this.UserID = UserID;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonID = PersonID;
            this.Person = clsPerson.Find(PersonID);

            _Mode = enMode.Update;
        }

        public static DataTable ListUsers() { 
            return clsUserData.ListUsers();
        }

        public static clsUser Find(string UserName) {

            int UserID = 0, PersonID = 0;
            string Password = "";
            bool IsActive = false;

            if (clsUserData.FindByUserName(UserName, ref UserID, ref PersonID, ref Password, ref IsActive))
                return new clsUser(UserID, PersonID , UserName, Password, IsActive);
            else
                return null;

        }

        public static clsUser Find(int UserID)
        {

            string Username = "";
            string Password = "";
            int PersonID = 0;
            bool IsActive = false;

            if (clsUserData.FindByUserID( UserID, ref Username, ref PersonID, ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static clsUser FindByPersonID(int PersonID)
        {

            string Username = "";
            string Password = "";
            int UserID = 0;
            bool IsActive = false;

            if (clsUserData.FindByPersonID(PersonID, ref Username, ref UserID, ref Password, ref IsActive))
                return new clsUser(UserID, PersonID, Username, Password, IsActive);
            else
                return null;
        }

        public static bool IsUserExist(int PersonID) {
            return clsUserData.IsUserExist(PersonID);
        }

        public static bool IsUserExist(string username)
        {
            return clsUserData.IsUserExist(username);
        }


        public static DataTable Filter(string filterWord, string type) {
            return clsUserData.Filter(filterWord, type);
        }

        public static DataTable Filter(string filterWord) { 
            return clsUserData.FilterByIsActive(filterWord);
        }

        private bool _AddNewUser() {

            this.UserID = clsUserData.AddNewUser(this.PersonID,this.Username,this.Password,this.IsActive);

            return (UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.Username, this.Password, this.IsActive);
        }

        public bool Save() {
        
            switch (_Mode) {
                case enMode.AddNew:
                    if(_AddNewUser())
                        return true;
                    else
                        return false;
        
                case enMode.Update:
                    if (_UpdateUser())
                        return true;
                    else
                        return false;                     
            }

            return false;
        }

        public static bool IsPasswordMatch(int UserID, string CurrentPassword) {
            return clsUserData.IsPasswordMatch(UserID,CurrentPassword);
        }

        public static bool ChangePassword(int UserID, string NewPassword) {
            return clsUserData.ChangePassword(UserID, NewPassword);
        }

        public static bool DeleteUser(int UserID) {
            return clsUserData.DeleteUser(UserID);
        }

    }
}
