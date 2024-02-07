using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsApplicationTypeData
    {
        public static DataTable ListApplicationTypes() { 
            
            DataTable AppTypesList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From ApplicationTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try 
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    AppTypesList.Load(reader);

                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally 
            { 
                connection.Close();
            }

            return AppTypesList;
        }

        public static bool UpdateAppTypesInfo(int AppTypeID, string AppTitle, decimal AppFees ) { 
            
            bool IsUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update ApplicationTypes 
                             Set ApplicationTypeTitle = @AppTitle, 
                             ApplicationFees = @AppFees 
                             Where ApplicationTypeID = @AppTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@AppTitle", AppTitle);
            command.Parameters.AddWithValue("@AppFees", AppFees);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

            try
            {
                connection.Open();

                int AffectedRows = command.ExecuteNonQuery();

                if(AffectedRows > 0)
                    IsUpdated = true;
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsUpdated;
        }

        public static bool FindAppTypeByID(int AppTypeID, ref string AppTitle, ref decimal AppFees) { 
            
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From 
                             ApplicationTypes
                             Where ApplicationTypeID = @AppTypeID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;

                    AppTitle = (string)reader["ApplicationTypeTitle"];
                    AppFees = (decimal)reader["ApplicationFees"];

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
    }
}