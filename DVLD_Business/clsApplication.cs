using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplication
    {

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public decimal ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public int PassedTests { get; set; }
        public string ApplicationStatusName { get; set; }


        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationStatus = 1;
            this.LastStatusDate = DateTime.Now;
            this.ApplicationTypeID = 1;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
        }

        public static string GetFees(int applicationTypeID)
        {
            return clsApplicationData.GetApplicationFees(applicationTypeID);
        }

        public static bool IsApplicationExist(int personID, int licenseClassID)
        {
            return clsApplicationData.IsApplicationExist(personID, licenseClassID);
        }

        public int AddNewApplication() {

            return clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }

        public bool UpdateApplicationStatus(int applicationID, decimal applicationStatus)
        {
            return clsApplicationData.UpdateApplicationStatus(applicationID, applicationStatus);
        }
    }
}