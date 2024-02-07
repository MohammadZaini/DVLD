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

        public static bool IsAppointmentExist(int localDrivingLicenseAppID, int licenseClassID) {

            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 From LocalDrivingLicenseApplications
                             Inner Join LicenseClasses
                             On LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                             Inner Join TestAppointments
                             On TestAppointments.LocalDrivingLicenseApplicationID = 
                             LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = 
                             @localDrivingLicenseAppID 
                             And LicenseClasses.LicenseClassID = @licenseClassID And IsLocked = 0;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseAppID", localDrivingLicenseAppID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

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

        public static bool IsAppointmentLocked(int localDrivingLicenseAppID) {
            bool isLocked = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 
                             From TestAppointments 
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseAppID And IsLocked = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseAppID", localDrivingLicenseAppID);

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

        public static bool UpdateTestAppointment(int testAppoinmentID, DateTime appointmentDate) {

            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestAppointments 
                             Set AppointmentDate = @appointmentDate 
                             Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appointmentDate", appointmentDate);
            command.Parameters.AddWithValue("@testAppointmentID", testAppoinmentID);

            try
            {
                connection.Open();

                int affectedRows = command.ExecuteNonQuery();

                if(affectedRows > 0)
                    isUpdated = true;
                
            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return isUpdated; 
        }

        public static bool FindTestAppointmentByID(int testAppointmentID, ref int testTypeID, ref int localDrivingLicenseApplicationID,
            ref int createdByUserID, ref DateTime appointmentDate, ref decimal paidFees, ref bool isLocked) {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From 
                             TestAppointments
                             Where TestAppointmentID = @testAppointmentID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);


            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    testTypeID = (int)reader["TestTypeID"];
                    localDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    appointmentDate = (DateTime)reader["AppointmentDate"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    paidFees = (decimal)reader["PaidFees"];
                    isLocked = (bool)reader["IsLocked"];

                }
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonFailed(int localDrivingLicenseApplicationID ) {
            bool isFailed = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Top 1 found = 1 From TestAppointments
                             Inner Join Tests
                             On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             Where Tests.TestResult = 1 And LocalDrivingLicenseApplicationID = @localDrivingLicenseApplicationID
                             Order By TestAppointments.TestAppointmentID Desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isFailed = true;

            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return isFailed;
        }


    }
}