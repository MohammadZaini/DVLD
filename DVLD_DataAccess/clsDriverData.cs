﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsDriverData
    {
        public static bool FindDriverByID(int driverID, ref int personID, ref int createdByUserID, ref DateTime createdDate) { 
            
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "Select * From Drivers Where DriverID = @driverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@driverID", driverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    personID = (int)reader["PersonID"];
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

 
    }
}
