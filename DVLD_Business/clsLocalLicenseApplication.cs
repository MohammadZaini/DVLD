using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Business
{
    public class clsLocalLicenseApplication
    {

        public int LocalLicenseApplicationID { get; set; }

        public int LocalLicenseClassID { get; set; }

        public string LicenseClassName { get; set; }

        public string ApplicantFullName { get; set; }

        public clsApplication Application { get; set; }

        public clsLocalLicenseApplication() {
            this.LocalLicenseApplicationID = -1;
            this.LocalLicenseClassID = -1;
            Application = new clsApplication();
        }

        public clsLocalLicenseApplication(int localLicenseApplicationID, int licenseClassID , string licenseClassName, string applicantFullName,
            int applicationID, int applicationTypeID , int applicantPersonID, string applicationStatus, DateTime statusDate, int passedTests, string createByUserName)
        {
            LocalLicenseApplicationID = localLicenseApplicationID;
            LocalLicenseClassID = licenseClassID;
            LicenseClassName = licenseClassName;
            ApplicantFullName = applicantFullName;
            Application = new clsApplication();
            Application.ApplicationID = applicationID;
            Application.ApplicationDate = statusDate;
            Application.ApplicationStatusName = applicationStatus;
            Application.PassedTests = passedTests;
            Application.CreatedByUserName = createByUserName;
            Application.ApplicationTypeID = applicationTypeID;
            Application.ApplicantPersonID = applicantPersonID;      
        }

        public static DataTable ListLocalLicenseApplications() { 
            return clsLocalLicenseApplicationData.ListLocalDrivingLicenseApplications();
        }

        public static DataTable Filter(string filterWord, string type) { 
            return clsLocalLicenseApplicationData.Filter(filterWord, type);
        }

        private bool _AddNewLocalLicenseApplication() {
            int applicationID = Application.AddNewApplication();

            this.LocalLicenseApplicationID = clsLocalLicenseApplicationData.AddNewLocalLicenseApplication(applicationID, this.LocalLicenseClassID);

            return (this.LocalLicenseApplicationID != -1);
        }
        public bool Save() {
            return _AddNewLocalLicenseApplication();
        }

        public static int GetApplicationID(int localLicenseAppID) {
            return clsLocalLicenseApplicationData.GetApplicationID(localLicenseAppID);
        }

        public bool UpdateApplicationStatus(int applicationID, decimal applicationStatus) {
            return Application.UpdateApplicationStatus(applicationID, applicationStatus);
        }

        public static clsLocalLicenseApplication Find(int localLicenseApplicationID) {

            string className = "", applicantFullName = "", applicatiomStatus = "";
            int applicationID = 0, applicationTypeID = 0, applicantPersonID = 0, licenseClassID = 0; 
            DateTime statusDate = DateTime.Now;
            int passedTests = 0;
            string createByUserName = "";
            

            if (clsLocalLicenseApplicationData.FindByLocalDrivingLicenseAppID(localLicenseApplicationID, ref licenseClassID, 
                ref className, ref applicantFullName, ref applicationID, ref applicationTypeID, ref applicantPersonID, 
                ref applicatiomStatus, ref statusDate, ref passedTests, ref createByUserName))

                return new clsLocalLicenseApplication(localLicenseApplicationID, licenseClassID, className, applicantFullName,
                    applicationID, applicationTypeID, applicantPersonID, applicatiomStatus,  statusDate,  passedTests,  createByUserName);
            else
                return null;
        }

        public static int PassedTestsCount(int localLicenseApplicationID) { 
            return clsLocalLicenseApplicationData.PassedTestsCount(localLicenseApplicationID);
        }

        public static int FailureCount(int localLicenseApplicationID, int testTypeID) {
            return clsLocalLicenseApplicationData.FailureCount(localLicenseApplicationID, testTypeID);
        }

    }
}