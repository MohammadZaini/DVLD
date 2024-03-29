﻿using DVLD_DataAccess;
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
        public byte ApplicationStatus { get; set; }
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

        private clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID,
                byte applicationStatus, DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {
            this.ApplicationID = applicationID;
            this.ApplicantPersonID = applicantPersonID;
            this.ApplicationDate = applicationDate;
            this.ApplicationStatus = applicationStatus;
            this.LastStatusDate = lastStatusDate;
            this.ApplicationTypeID = applicationTypeID;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdByUserID;
        }

        public static clsApplication Find(int applicationID) {

            int applicantPersonID = 0, applicationTypeID = 0, createdByUserID = 0;
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            decimal paidFees = 0;
            byte applicationStatus = 0;

            if (clsApplicationData.FindByApplicationID(applicationID, ref applicantPersonID, ref applicationDate, ref applicationTypeID,
                ref applicationStatus, ref lastStatusDate, ref paidFees, ref createdByUserID))

                return new clsApplication(applicationID, applicantPersonID, applicationDate, applicationTypeID, applicationStatus,
                    lastStatusDate,paidFees, createdByUserID);
            else
                return null;    
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

            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return this.ApplicationID;
        }

        public bool UpdateApplicationStatus(int applicationID, decimal applicationStatus)
        {
            return clsApplicationData.UpdateApplicationStatus(applicationID, applicationStatus);
        }
    }
}