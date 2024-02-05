using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsTestAppointmentData
    {
        public static int AddNewAppointment(int testTypeID, int localDrivingLicenseAppID, DateTime appointmentDate, 
            decimal paidFees, int createdByUserID, bool isLocked) {

            int appointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into TestAppointments 
                            Values (
                            @TestID, @LocalDrivingLicenseAppID, @AppointmentDate, @PaidFees, 
                            @CreatedByUserID, @IsLocked)
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testTypeID);
            command.Parameters.AddWithValue("@LocalDrivingLicenseAppID", localDrivingLicenseAppID);
            command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@PaidFees", paidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsLocked", isLocked);

            try
            {
                connection.Open();

                object insertedIDObj = command.ExecuteScalar();

                if (insertedIDObj != null && int.TryParse(insertedIDObj.ToString(), out int insertedID))
                    appointmentID = insertedID;
            }
            catch (Exception)
            {


            }

            finally
            { 
                connection.Close();
            }

            return appointmentID;
        }

        public static bool IsAppointmentExist(int localDrivingLicenseAppID) {

            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 
                             From TestAppointments
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseAppID And (IsLocked = 1 And TestResult = 1);";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseAppID", localDrivingLicenseAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isExist = true;
            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return isExist;
        }

        public static DataTable ListPersonTestAppointments(int localDrivingLicenseAppID) { 
            
            DataTable personTestAppointmentsList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select [Appointment ID] = TestAppointmentID, [Appointment Date] = AppointmentDate, 
                            [Paid Fees] = PaidFees, [Is Locked] = IsLocked From TestAppointments
                            Where LocalDrivingLicenseApplicationID = @localDrivingLicenseAppID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseAppID", localDrivingLicenseAppID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    personTestAppointmentsList.Load(reader);


            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return personTestAppointmentsList;
        }

        public static bool LockTestAppointment(int testAppointmentID) {

            bool isLocked = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestAppointments 
                             Set IsLocked = 1
                             Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                int affectedRows = command.ExecuteNonQuery();

                if (affectedRows > 0)
                    isLocked = true;
            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return isLocked;
        }

        public static bool IsAppointmentLocked(int testAppointmentID) {
            bool isLocked = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 
                             From TestAppointments 
                             Where TestAppointmentID = @testAppointmentID And IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isLocked = true;
            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return isLocked;
        }
    }
}