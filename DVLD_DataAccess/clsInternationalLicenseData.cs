using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsInternationalLicenseData
    {

        public static bool FindByInternationalLicenseID(int internationalLicenseID, ref int applicationID, ref int driverID, 
                ref int issuedUsingLocalLicenseID, ref DateTime issueDate, ref DateTime expirationDate, ref bool isActive, 
                ref int createdByUserID)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From InternationalLicenses
                             Where InternationalLicenseID = @internationalLicenseID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@internationalLicenseID", internationalLicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    issuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];
                    isActive = (bool)reader["IsActive"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                }

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();

            }


            return isFound;
        }

        public static int AddNewInternationalLicense(int applicationID, int driverID, int issuedUsingLocalLicenseID, 
            DateTime issueDate, DateTime expirationDate, bool isActive, int createdByUserID) {

            int internationalLicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into InternationalLicenses 
                             Values
                             (
                             @applicationID, @driverID, @issuedUsingLocalLicenseID,@issueDate,@expirationDate, @isActive,
                             @createdByUserID 
                             )
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@driverID", driverID);
            command.Parameters.AddWithValue("@issuedUsingLocalLicenseID", issuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@issueDate", issueDate);
            command.Parameters.AddWithValue("@expirationDate", expirationDate);
            command.Parameters.AddWithValue("@isActive", isActive);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);


            try
            {
                connection.Open();

                object insertedIDObj = command.ExecuteScalar();

                if (insertedIDObj != null && int.TryParse(insertedIDObj.ToString(), out int insertedID))
                    internationalLicenseID = insertedID;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                connection.Close();
            }

            return internationalLicenseID;
        }

        public static bool IsInternationalLicenseExist(int localLicenseID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From InternationalLicenses
                             Where IssuedUsingLocalLicenseID = @localLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localLicenseID", localLicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

            }
            catch (Exception ex)
            {


            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable ListInternationalLicenses(int personID) {

            DataTable localLicensesList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select  [Int.LicenseID] = InternationalLicenseID, [Application ID] = InternationalLicenses.ApplicationID, [L.License ID] = IssuedUsingLocalLicenseID, [Issue Date] = IssueDate, [Expiration Date] = ExpirationDate, 
                             [Is Active] = IsActive
                             From InternationalLicenses
                             Inner Join Applications
                             On InternationalLicenses.ApplicationID = Applications.ApplicationID
                             Inner Join People
                             On People.PersonID = Applications.ApplicantPersonID
                             Where PersonID = @personID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    localLicensesList.Load(reader);

                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return localLicensesList;
        }
    }
}