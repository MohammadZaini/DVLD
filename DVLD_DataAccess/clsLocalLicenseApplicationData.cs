using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsLocalLicenseApplicationData
    {
        public static DataTable ListLocalDrivingLicenseApplications() { 
            DataTable localDrivingLicenseAppsList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From LocalDrivingLicenseApplicationsView;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                    localDrivingLicenseAppsList.Load(reader);

                reader.Close();
            }
            catch
            {


            }
            finally
            { 
                connection.Close();
            }


            return localDrivingLicenseAppsList;
        }

        public static DataTable Filter(string filterWord, string type) { 
            
            DataTable filteredAppsList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string qurey = $"Select * From( SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID as LDLAppID, " +
                            "LicenseClasses.ClassName, People.NationalNo, FullName = People.FirstName + ' ' + People.SecondName + ' ' + " +
                            "People.ThirdName + ' ' + People.LastName, Applications.ApplicationDate, PassedTest = 0 ," +
                            "Case When ApplicationStatus = 1 then 'New' When ApplicationStatus = 2 then 'Cancelled' When ApplicationStatus = 3 " +
                            "Then 'Completed' End As Status FROM Applications INNER JOIN LocalDrivingLicenseApplications " +
                            "ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID INNER JOIN " +
                            "LicenseClasses ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID " +
                            "INNER JOIN People ON Applications.ApplicantPersonID = People.PersonID) ResultSet " +
                            $"Where {type} Like + '' + @filterword + '%';";

            SqlCommand command = new SqlCommand(qurey, connection);

            command.Parameters.AddWithValue("@filterWord", filterWord);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                    filteredAppsList.Load(reader);
            }
            catch (Exception)
            {


            }
            finally
            { 
            connection.Close(); 
            }

            return filteredAppsList;
        }
            
        public static int AddNewLocalLicenseApplication(int applicationID, int licenseClassID) {

            int newlocalLicenseAppID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into LocalDrivingLicenseApplications 
                            Values (
                            @applicationID, @licenseClassID)
                            Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@applicationID", applicationID);
            command.Parameters.AddWithValue("@licenseClassID", licenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    newlocalLicenseAppID = insertedID;

            }
            catch (Exception)
            {


            }
            finally
            {
                connection.Close();
            }

            return newlocalLicenseAppID;

        }

        public static int GetApplicationID(int LocalDrivingLicenseAppID) {

            int applicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select ApplicationID 
                            From LocalDrivingLicenseApplications 
                            Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseAppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseAppID", LocalDrivingLicenseAppID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int appID))
                    applicationID = appID;
            }
            catch
            {


            }
            finally
            {
                connection.Close();
            }

            return applicationID;
        }
    }
}