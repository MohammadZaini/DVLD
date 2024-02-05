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

        private bool _AddNewAppointment() {

            this.TestAppointmentID = clsTestAppointmentData.AddNewAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked);

            return (this.TestAppointmentID != -1); // -1 means the appointment has not been added
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
                    if (_AddNewAppointment())
                    {
                        return true;
                    }
                    else
                        return false;
            }

            return false;
        }

        public static bool IsAppointmentExist(int localDrivingLicenseAppID) {
            return clsTestAppointmentData.IsAppointmentExist(localDrivingLicenseAppID);
        }

        public static DataTable ListPeronTestAppointments(int localDrivingLicenseAppID) {
            return clsTestAppointmentData.ListPersonTestAppointments(localDrivingLicenseAppID);
        }

        public static bool LockTestAppointment(int testAppointmentID) { 
            
            return clsTestAppointmentData.LockTestAppointment(testAppointmentID);
        }

        public static bool IsAppointmentLocked(int testAppointmentID) {

            return clsTestAppointmentData.IsAppointmentLocked(testAppointmentID);
        }
    }
}