using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicense
    {
        public int ID { get; set; }
        public int DriverID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassNo { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public decimal PaidFees { get; set; }
        public bool IsActive { get; set; }
        public int IssueReason { get; set; }
        public int CreatedByUserID { get; set; }

        public clsDriver Driver { get; set; }

        public clsLicense() {
            ID = -1;
            DriverID = -1;
            ApplicationID = -1;
            LicenseClassNo = 0;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = string.Empty;
            PaidFees = 0;
            IsActive = true;
            IssueReason = 0;
            CreatedByUserID = 0;
            Driver = new clsDriver();
        }

        private bool _AddNewLicense() {

            this.DriverID = Driver._AddNewDriver();

            this.ID = clsLicenseData.AddNewDrivingLicense(this.ApplicationID, this.DriverID, this.LicenseClassNo, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason, this.CreatedByUserID);

            return ID != -1;

        }

        public bool Save() {
            return _AddNewLicense();
        }
    }
}