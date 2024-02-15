using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsLicenseData
    {

        public static bool FindByApplicationID(int applicationID, ref int licenseID, ref int driverID, ref int licenseClassNo, ref DateTime issueDate,
               ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive, ref byte issueReason, 
               ref int createdByUserID) {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From Licenses
                             Where ApplicationID = @applicationID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    licenseID = (int)reader["LicenseID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassNo = (int)reader["LicenseClass"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "No Notes";

                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
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

        public static bool FindByLicenseID( int licenseID , ref int applicationID, ref int driverID, ref int licenseClassNo, ref DateTime issueDate,
               ref DateTime expirationDate, ref string notes, ref decimal paidFees, ref bool isActive, ref byte issueReason,
               ref int createdByUserID)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From Licenses
                             Where LicenseID = @licenseID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    applicationID = (int)reader["ApplicationID"];
                    driverID = (int)reader["DriverID"];
                    licenseClassNo = (int)reader["LicenseClass"];
                    issueDate = (DateTime)reader["IssueDate"];
                    expirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        notes = (string)reader["Notes"];
                    else
                        notes = "No Notes";

                    paidFees = (decimal)reader["PaidFees"];
                    isActive = (bool)reader["IsActive"];
                    issueReason = (byte)reader["IssueReason"];
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


        public static int AddNewDrivingLicense(int applicationID, int driverID, int licenseClassNo, DateTime issueDate,
                DateTime expirationDate, string notes, decimal paidFees, bool isActive, int issueReason, int createdByUserID) {

            int licenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Licenses 
                             Values(@applicationID, @driverID, @licenseClassNo, @issueDate, @expirationDate, @notes, @paidFees,
                             @isActive, @issueReason, @createdByUserID) 
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("applicationID", applicationID);
            command.Parameters.AddWithValue("driverID", driverID);
            command.Parameters.AddWithValue("licenseClassNo", licenseClassNo);
            command.Parameters.AddWithValue("issueDate", issueDate);
            command.Parameters.AddWithValue("expirationDate", expirationDate);

            if(notes != "")
                command.Parameters.AddWithValue("notes", notes);
            else
                command.Parameters.AddWithValue("notes", DBNull.Value);

            command.Parameters.AddWithValue("paidFees", paidFees);
            command.Parameters.AddWithValue("isActive", isActive);
            command.Parameters.AddWithValue("issueReason", issueReason);
            command.Parameters.AddWithValue("createdByUserID", createdByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    licenseID = InsertedID;

            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return licenseID;
        }


        public static DataTable ListLocalLicenses(int personID) {

            DataTable localLicensesList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select [Lic.ID] = LicenseID, [App.ID] = Licenses.ApplicationID, [Class Name] = LicenseClasses.ClassName,
                             [Issue Date] = IssueDate, [Expiration Date] = ExpirationDate,  [Is Active] = IsActive 
                             From Licenses Inner Join LicenseClasses
                             On LicenseClasses.LicenseClassID = Licenses.LicenseClass
                             Inner Join Applications
                             On Licenses.ApplicationID = Applications.ApplicationID
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

        public static bool IsLicenesValid(int licenseID) {

            bool isValid = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Valid = 1 From Licenses
                            Where LicenseID = @licenseID And IsActive = 1 And ExpirationDate > GETDATE() And LicenseClass = 3;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isValid = true;


            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

            return isValid;
        }
    }
}