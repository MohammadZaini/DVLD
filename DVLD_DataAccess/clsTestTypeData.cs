using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsTestTypeData
    {
        public static DataTable ListTestTypes()
        {

            DataTable TestTypesList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select TestTypeID As ID, TestTypeTitle As Title ,TestTypeDescription As Description, 
                             TestTypeFees As Fees 
                             From TestTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    TestTypesList.Load(reader);

                reader.Close();

            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return TestTypesList;
        }

        public static bool FindTestTypeByID(int TestID, ref string TestTitle, ref string TestDescription ,ref decimal TestFees)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From 
                             TestTypes
                             Where TestTypeID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    TestTitle = (string)reader["TestTypeTitle"];
                    TestDescription = (string)reader["TestTypeDescription"];
                    TestFees = (decimal)reader["TestTypeFees"];

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

        public static bool UpdateTestInfo(int TestID, string Title, string Description, decimal Fees)
        {

            bool IsUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update TestTypes 
                             Set TestTypeTitle = @Title, 
                             TestTypeDescription = @Description,
                             TestTypeFees = @Fees
                             Where TestTypeID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@Fees", Fees);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();

                int AffectedRows = command.ExecuteNonQuery();

                if (AffectedRows > 0)
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
    }
}
