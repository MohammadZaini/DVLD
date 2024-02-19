using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsDetainedLicenseData
    {
        public static DataTable ListDetainedLicenses() {
            DataTable detainedLicensesList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select [D.ID] = DetainID,[L.ID] = DetainedLicenses.LicenseID, [D.Date] = DetainDate, 
                            [Is Released]= IsReleased,[Fine Fees] = FineFees, [Release Date] = ReleaseDate , [N.No] = NationalNo,
                            [Full Name] = FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName, 
                            [Release App.ID] = ReleaseApplicationID From DetainedLicenses
                            Inner Join Licenses
                            On Licenses.LicenseID = DetainedLicenses.LicenseID
                            Inner Join Applications
                            On Applications.ApplicationID = Licenses.ApplicationID
                            Inner Join People
                            On People.PersonID = Applications.ApplicantPersonID
                            Order By [D.ID] Desc;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    detainedLicensesList.Load(reader);

                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return detainedLicensesList;
        }

        public static int DetainLicense(int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID, bool isReleased) {

            int detainID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into DetainedLicenses 
                             Values(@licenseID, @detainDate, @fineFees, @createdByUserID, @isReleased, Null,Null,Null) 
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("licenseID", licenseID);
            command.Parameters.AddWithValue("detainDate", detainDate);
            command.Parameters.AddWithValue("fineFees", fineFees);
            command.Parameters.AddWithValue("createdByUserID", createdByUserID);
            command.Parameters.AddWithValue("isReleased", isReleased);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    detainID = InsertedID;

            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }

            return detainID;
        }

        public static bool FindByLicenseID(int licenseID, ref int detainID, ref DateTime detainDate, ref decimal fineFees,
            ref int createdByUserID, ref bool isReleased, ref DateTime releaseDate, ref int releasedByUserID, ref int releaseApplicationID) {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "Select * From DetainedLicenses Where LicenseID = @licenseID Order By DetainID Desc;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    detainID = (int)reader["DetainID"];
                    detainDate = (DateTime)reader["DetainDate"];
                    fineFees = (decimal)reader["FineFees"];
                    detainDate = (DateTime)reader["DetainDate"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                    isReleased = (bool)reader["IsReleased"];
                    releaseDate = (DateTime)reader["ReleaseDate"];
                    releasedByUserID = (int)reader["ReleasedByUserID"];
                    releaseApplicationID = (int)reader["ReleaseApplicationID"];

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

        public static bool ReleaseLicense(int detainID, DateTime releaseDate, int releasedByUserID, int releaseApplicationID ) {
            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update DetainedLicenses 
                             Set IsReleased = 1 , ReleaseDate = @releaseDate, ReleasedByUserID = @releasedByUserID, 
                             ReleaseApplicationID = @releaseApplicationID
                             Where DetainID = @detainID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@detainID", detainID);
            command.Parameters.AddWithValue("@releaseDate", releaseDate);
            command.Parameters.AddWithValue("@releasedByUserID", releasedByUserID);
            command.Parameters.AddWithValue("@releaseApplicationID", releaseApplicationID);

            try
            {
                connection.Open();
                int AffectedRows = command.ExecuteNonQuery();

                if (AffectedRows > 0)
                    isUpdated = true;
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return isUpdated;
        }

        public static DataTable Filter(string filterWord, string type)
        {

            DataTable FilteredData = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = $"Select * From DetainedLicensesView " +
                           $"Where [{type}] Like '' + @filterWord + '%';";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@filterWord", filterWord);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    FilteredData.Load(reader);

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return FilteredData;
        }
    }
}