using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsDriverData
    {
        public static bool FindDriverByID(int personID, ref int driverID, ref int createdByUserID, ref DateTime createdDate) { 
            
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "Select * From Drivers Where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    driverID = (int)reader["DriverID"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    createdDate = (DateTime)reader["CreatedDate"];

                }
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

        public static int AddNewDriver(int personID, int createdByUserID,  DateTime createdDate) {

            int driverID = -1; 

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Drivers 
                             Values(@personID, @createdByUserID, @createdDate) 
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("personID", personID);
            command.Parameters.AddWithValue("createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("createdDate", createdDate);;

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    driverID = InsertedID;

            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return driverID;
        }

        public static bool IsDriver(int personID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 From Drivers
                             Where PersonID = @personID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isFound = true;


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

    }
}