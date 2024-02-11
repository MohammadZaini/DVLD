using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsLicenseClassData
    {
        public static bool FindByClassID(int classID, ref string name, ref string description, ref byte minimumAllowedAge, 
            ref byte defaultValidityLength, ref decimal fees) { 
            
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From 
                             LicenseClasses
                             Where LicenseClassID = @classID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@classID", classID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    name = (string)reader["ClassName"];
                    description = (string)reader["ClassDescription"];
                    minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                    defaultValidityLength = (byte)reader["DefaultValidityLength"];
                    fees = (decimal)reader["ClassFees"];

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
