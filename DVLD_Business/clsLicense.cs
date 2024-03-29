﻿using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
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
        public byte IssueReason { get; set; }
        public int CreatedByUserID { get; set; }


        public clsApplication Application { get; set; }
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
            Application = new clsApplication();
            Driver = new clsDriver();
        }

        private clsLicense(int licenesID, int applicationID, int driverID, int licenseClassNo, DateTime issueDate,
            DateTime eExpirationDate, string notes, decimal paidFees, bool isActive, byte issueReason, int createdByUserID) { 
            
            this.ID = licenesID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassNo = licenseClassNo;
            this.IssueDate = issueDate;
            this.ExpirationDate = eExpirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;

            Application = new clsApplication();
            Driver = new clsDriver();
        }

        public static clsLicense Find(int applicationID) {

            int licenesID = 0, driverID = 0, licenseClassNo = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;
            byte issueReason = 0;
            int createdByUserID = 0;

            if (clsLicenseData.FindByApplicationID(applicationID, ref licenesID, ref driverID, ref licenseClassNo, ref issueDate,
                ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))

                return new clsLicense(licenesID, applicationID,  driverID,  licenseClassNo,  issueDate,
                 expirationDate,  notes,  paidFees,  isActive,  issueReason,  createdByUserID);
            else
                return null;
        }


        public static clsLicense FindByLicenseID(int licenesID)
        {

            int applicationID = 0, driverID = 0, licenseClassNo = 0;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            string notes = "";
            decimal paidFees = 0;
            bool isActive = false;
            byte issueReason = 0;
            int createdByUserID = 0;

            if (clsLicenseData.FindByLicenseID(licenesID, ref applicationID, ref driverID, ref licenseClassNo, ref issueDate,
                ref expirationDate, ref notes, ref paidFees, ref isActive, ref issueReason, ref createdByUserID))

                return new clsLicense(licenesID, applicationID, driverID, licenseClassNo, issueDate,
                 expirationDate, notes, paidFees, isActive, issueReason, createdByUserID);
            else
                return null;
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

        public static DataTable ListLocalLicenses(int personID) { 
            return clsLicenseData.ListLocalLicenses(personID);
        }

        public static bool IsLicenseValid(int licenesID) { 
            
            return clsLicenseData.IsLicenesValid(licenesID);
        }

        public static bool IsLicenseExist(int licenesID) {

            return clsLicenseData.IsLicenseExist(licenesID);
        }

        public static bool IsLicenseActive(int licenseID) { 
            return clsLicenseData.IsLicenseActive(licenseID);
        }

        public static bool IsLicenseExpired(int licenseID)
        {
            return clsLicenseData.IsLicenseExpired(licenseID);
        }

        public static bool IsLicenseClassOdinaryDrivingLicense(int licenseID)
        {
            return clsLicenseData.IsLicenseClassOdinaryDrivingLicense(licenseID);
        }

        public static bool IsDetained(int licenseID)
        {
            return clsLicenseData.IsDetained(licenseID);
        }

        public  bool RenewLocalLicense() {
            return this.Save();
        }

        public static bool UpdateLicenseActivation(int licenseID, bool isActive) {

            return clsLicenseData.UpdateLicenseActivation(licenseID, isActive);
        }

    }
}