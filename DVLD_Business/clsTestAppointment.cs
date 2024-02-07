using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestAppointment
    {
        enum enMode { AddNew = 0, Update = 1}

        private enMode _Mode = enMode.AddNew;
        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }

        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            TestTypeID = -1;
            LocalDrivingLicenseApplicationID = -1;
            AppointmentDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            IsLocked = false;

            _Mode = enMode.AddNew;
        }

        private clsTestAppointment(int testAppointmentID,  int testTypeID,  int localDrivingLicenseApplicationID,
             int createdByUserID,  DateTime appointmentDate, decimal paidFees, bool isLocked) { 
            
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = testTypeID;
            this.LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            this.CreatedByUserID = createdByUserID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.IsLocked = isLocked;

            _Mode = enMode.Update;
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {

            int testTypeID = 0, localDrivingLicenseApplicationID = 0, createdByUserID = 0;
            DateTime appointmentDate = DateTime.Now;
            decimal paidFees = 0;
            bool isLocked = false;
           
            if (clsTestAppointmentData.FindTestAppointmentByID(TestAppointmentID, ref testTypeID, ref localDrivingLicenseApplicationID,
                ref createdByUserID, ref appointmentDate, ref paidFees, ref isLocked))
                return new clsTestAppointment(TestAppointmentID, testTypeID, localDrivingLicenseApplicationID,
                 createdByUserID,  appointmentDate,  paidFees,  isLocked);
            else
                return null;
        }

        private bool _AddNewAppointment() {

            this.TestAppointmentID = clsTestAppointmentData.AddNewAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);

            return (this.TestAppointmentID != -1); // -1 means the appointment has not been added
        }

        private bool _UpdateAppointment() {

            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.AppointmentDate);
        }

        public bool Save() { 
            
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewAppointment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    if (_UpdateAppointment())
                    {
                        return true;
                    }
                    else
                        return false;
            }

            return false;
        }

        public static bool IsAppointmentExist(int localDrivingLicenseAppID, int licenseClassID) {
            return clsTestAppointmentData.IsAppointmentExist(localDrivingLicenseAppID, licenseClassID);
        }

        public static DataTable ListPeronTestAppointments(int localDrivingLicenseAppID) {
            return clsTestAppointmentData.ListPersonTestAppointments(localDrivingLicenseAppID);
        }

        public static bool LockTestAppointment(int testAppointmentID) { 
            
            return clsTestAppointmentData.LockTestAppointment(testAppointmentID);
        }

        public static bool IsAppointmentLocked(int localDrivingLicenseAppID) {

            return clsTestAppointmentData.IsAppointmentLocked(localDrivingLicenseAppID);
        }

        public static bool IsPersonFailed(int localDrivingLicenseAppID) {
            return clsTestAppointmentData.IsPersonFailed(localDrivingLicenseAppID);
        }
    }
}