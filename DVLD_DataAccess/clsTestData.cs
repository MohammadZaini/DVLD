using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsTestData
    {
        public static int PerformNewTest(int testAppointmentID, bool testResult, string notes, int createdByUserID) {

            int testID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Tests Values (
                             @testAppointmentID, @testResult, @notes, @createdByUserID )
                              Select SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testAppointmentID", testAppointmentID);
            command.Parameters.AddWithValue("@testResult", testResult);
            command.Parameters.AddWithValue("@notes", notes);
            command.Parameters.AddWithValue("@createdByUserID", createdByUserID);

            try
            {
                connection.Open();

                object testIDObj = command.ExecuteScalar();

                if (testIDObj != null && int.TryParse(testIDObj.ToString(), out int insertedID))
                    testID = insertedID;
                    
            }
            catch (Exception)
            {


            }

            finally
            {
                connection.Close();
            }

            return testID;
        }
    }
}
