using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsLicenseData
    {
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
            command.Parameters.AddWithValue("notes", notes);
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
    }
}
