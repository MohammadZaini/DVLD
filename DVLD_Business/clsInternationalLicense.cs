using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsInternationalLicense
    {

        public int ID { get; set; } // InternationalLicenseID
        public int ApplicationID { get; set; }     
        public clsApplication Application { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedByUserID { get; set; }

        public clsInternationalLicense() { 
            
            this.ID = -1;
            this.ApplicationID = -1;
            this.Application = new clsApplication();
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
            this.CreatedByUserID = -1;
        }


        private clsInternationalLicense(int internationLicenseID, int applicationID, int driverID,
                int issuedUsingLocalLicenseID, DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID)
        {

            this.ID = internationLicenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.IssuedUsingLocalLicenseID = issuedUsingLocalLicenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;
            this.CreatedByUserID = createdByUserID;

            this.Application = new clsApplication();
        }

        private bool _AddNewInternationalLicense() {

            this.ApplicationID = Application.AddNewApplication();

            this.ID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID,this.DriverID, 
                this.IssuedUsingLocalLicenseID,this.IssueDate,this.ExpirationDate,this.IsActive, this.CreatedByUserID);

            return (this.ID != -1);
        }

        public bool Save() { 
            return _AddNewInternationalLicense();
        }

        public static bool IsInternationalLicenseExist(int localLicenseID) {
            return clsInternationalLicenseData.IsInternationalLicenseExist(localLicenseID);
        }

        public static clsInternationalLicense Find(int internationLicenseID) { 

            int applicationID = 0,  driverID = 0, issuedUsingLocalLicenseID = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;


            if (clsInternationalLicenseData.FindByInternationalLicenseID(internationLicenseID, ref applicationID, ref driverID,
                ref issuedUsingLocalLicenseID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))

                return new clsInternationalLicense(internationLicenseID, applicationID, driverID,
                 issuedUsingLocalLicenseID, issueDate, expirationDate, isActive, createdByUserID);
            else
                return null;
        }

        public static clsInternationalLicense FindByLicenseID(int localLicenseID)
        {

            int applicationID = 0, driverID = 0, internationLicenseID = 0, createdByUserID = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;


            if (clsInternationalLicenseData.FindByLocalLicenseID(localLicenseID, ref internationLicenseID, ref applicationID,
                ref driverID, ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))

                return new clsInternationalLicense(internationLicenseID, applicationID, driverID,
                 localLicenseID, issueDate, expirationDate, isActive, createdByUserID);
            else
                return null;
        }

        public static DataTable GetInternationalLicense(int personID) {

            return clsInternationalLicenseData.GetInternationalLicense(personID);
        }

        public static DataTable ListInternationalLicenses()
        {
            return clsInternationalLicenseData.ListInternationalLicenses();
        }
    }
}