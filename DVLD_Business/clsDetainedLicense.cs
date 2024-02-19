using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDetainedLicense
    {
        public int ID { get; set; }
        public int LicenseID { get; set; }
        public DateTime Date { get; set; }
        public decimal FineFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public int ReleaseApplicationID { get; set; }

        public clsApplication Application { get; set; }

        public clsDetainedLicense() {

            ID = -1;
            LicenseID = -1;
            Date = DateTime.Now;
            FineFees = -1;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.Now;
            ReleasedByUserID = -1;
            ReleaseApplicationID = -1;

            Application = new clsApplication();
        }


        private clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID,
                bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {

            ID = detainID;
            LicenseID = licenseID;
            Date = detainDate;
            FineFees = fineFees;
            CreatedByUserID = createdByUserID;
            IsReleased = isReleased;
            ReleaseDate = releaseDate;
            ReleasedByUserID = releasedByUserID;
            ReleaseApplicationID = releaseApplicationID;

            Application = new clsApplication();
        }


        public static DataTable ListDetainedLicenses() { 
            
            return clsDetainedLicenseData.ListDetainedLicenses();
        }

        public bool DetainLicense() {

            this.ID = clsDetainedLicenseData.DetainLicense(this.LicenseID, this.Date, this.FineFees,this.CreatedByUserID, this.IsReleased);

            return (this.ID != -1);
        }

        public bool ReleaseLicense()
        {
            return clsDetainedLicenseData.ReleaseLicense(this.ID, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        }

        public static clsDetainedLicense Find(int licenseID) {

            int detainID = 0, createdByUserID = 0, releasedByUserID = 0, releaseApplicationID = 0;
            DateTime detainDate = DateTime.Now;
            DateTime releaseDate = DateTime.Now;
            bool isReleased = false;
            decimal fineFees = 0;

            if (clsDetainedLicenseData.FindByLicenseID(licenseID, ref detainID, ref detainDate, ref fineFees, ref createdByUserID,
                ref isReleased, ref releaseDate, ref releasedByUserID, ref releaseApplicationID))

                return new clsDetainedLicense(detainID, licenseID, detainDate, fineFees, createdByUserID, isReleased, releaseDate,
                    releasedByUserID, releaseApplicationID);
            else
                return null;
        }

        public static DataTable Filter(string filterWord, string type) {

            return clsDetainedLicenseData.Filter(filterWord, type);
        }
    }
}