using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLocalLicenseApplication
    {

        public int LocalLicenseApplicationID { get; set; }

        public int LocalLicenseClassID { get; set; }

        public clsApplication Application { get; set; }

        public clsLocalLicenseApplication() {
            this.LocalLicenseApplicationID = -1;
            this.LocalLicenseClassID = -1;
            Application = new clsApplication();
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
    }
}