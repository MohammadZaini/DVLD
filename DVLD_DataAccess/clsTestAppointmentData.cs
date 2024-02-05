using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsTestAppointmentData
    {
        public static int AddNewAppointment(int testID, int localDrivingLicenseAppID, DateTime appointmentDate, 
            decimal paidFees, int createdByUserID, bool isLocked) {
            int appointmentID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into TestAppointments 
                            Values (
                            @TestID, @LocalDrivingLicenseAppID, @AppointmentDate, @PaidFees, 
                            @CreatedByUserID, @IsLocked)
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);
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
    }
}
