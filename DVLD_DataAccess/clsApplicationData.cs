using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsApplicationData
    {
        public static int AddNewApplication(int applicantPersonID, DateTime applicationDate, int applicationTypeID, decimal applicationStatus,
            DateTime lastStatusDate, decimal paidFees, int createdByUserID)
        {

            int applicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Applications 
                            Values (
                            @applicantPersonID, @applicationDate, @applicationTypeID, 
                            @applicationStatus, @lastStatusDate, @paidFees, @createdByUserID )
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicantPersonID", applicantPersonID);
            command.Parameters.AddWithValue("@applicationDate", applicationDate);
            command.Parameters.AddWithValue("@applicationTypeID", applicationTypeID);
            command.Parameters.AddWithValue("@applicationStatus", applicationStatus);
            command.Parameters.AddWithValue("@lastStatusDate", lastStatusDate);
            command.Parameters.AddWithValue("@paidFees", paidFees);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    applicationID = insertedID;

            }
            catch (Exception)
            {


            }
            finally
            {
                connection.Close();
            }

            return applicationID;
        }

        public static string GetApplicationFees(int applicationTypeID)
        {

            decimal fees = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select ApplicationFees 
                             From ApplicationTypes 
                             Where ApplicationTypeID = @applicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationTypeID", applicationTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && decimal.TryParse(result.ToString(), out decimal appTypeFees))
                    fees = appTypeFees;

                fees = Convert.ToInt32(fees);

            }
            catch (Exception)
            {


            }
            finally
            {
                connection.Close();
            }

            return fees.ToString();
        }

        public static bool IsApplicationExist(int personID, int licenseClassID)
        {

            bool isExist = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT Found = 1
                            FROM Applications INNER JOIN  LocalDrivingLicenseApplications
                            ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            Where ApplicantPersonID = @personID And LicenseClassID = @licenseClassID And ApplicationStatus <> 2;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);
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

        public static bool UpdateApplicationStatus(int applicationID, decimal applicationStatus)
        {

            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @" Update Applications
                              Set ApplicationStatus = @applicationStatus
                              Where ApplicationID = @applicationID;;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@applicationStatus", applicationStatus);

            try
            {
                connection.Open();

                int AffectedRows = command.ExecuteNonQuery();

                if (AffectedRows > 0)
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

    }
}