using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public static class clsLocalLicenseApplicationData
    {
        public static DataTable ListLocalDrivingLicenseApplications() { 
            DataTable localDrivingLicenseAppsList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From LocalDrivingLicenseApplicationsListView;";

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

        public static bool FindByLocalDrivingLicenseAppID(int localLicenseApplicationID, ref int licenseClassID, 
            ref string className, ref string fullName, ref int applicationID, ref int applicationTypeID, ref int applicantPersonID,
            ref string applicationStatus, ref DateTime statusDate, ref int passedTests, ref string createdByUsername) {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * 
                             From LocalDrivingLicenseApplicationsView
                             Where [L.D.L.App ID] = @localLicenseApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localLicenseApplicationID", localLicenseApplicationID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    className = (string)reader["Class Name"];
                    fullName = (string)reader["Full Name"];
                    applicationID = (int)reader["ApplicationID"];
                    applicationTypeID = (int)reader["ApplicationTypeID"];
                    applicantPersonID = (int)reader["ApplicantPersonID"];
                    licenseClassID = (int)reader["LicenseClassID"];
                    applicationStatus = (string)reader["Status"];
                    statusDate = (DateTime)reader["Status Date"];
                    passedTests = (int)reader["Passed Tests"];
                    createdByUsername = (string)reader["UserName"];

                }

                reader.Close();
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

        public static int PassedTestsCount(int localLicenseApplicationID) {
            int passedTestsCount = 0;


            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select PassedTestsCount = (Select Count(TestTypeID)) From TestAppointments
                            inner Join Tests
                            On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            Inner Join LocalDrivingLicenseApplications
                            On LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = 
                            TestAppointments.LocalDrivingLicenseApplicationID And TestResult = 1
                            Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseApplicationID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localLicenseApplicationID", localLicenseApplicationID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int passedTestsNo))
                    passedTestsCount = passedTestsNo;


            }
            catch
            {


            }
            finally
            {
                connection.Close();
            }

            return passedTestsCount;
        }

        public static int FailureCount(int localLicenseApplicationID, int testTypeID) { 

            int failureCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select FailureCount = Count(*) From TestAppointments
                             Inner Join Tests
                             On TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             Inner Join LocalDrivingLicenseApplications
                             On TestAppointments.LocalDrivingLicenseApplicationID = 
                             LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                             Where Tests.TestResult = 0 
                             And LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseApplicationID
                             And TestTypeID = @testTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localLicenseApplicationID", localLicenseApplicationID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int failureNo))
                    failureCount = failureNo;


            }
            catch
            {


            }
            finally
            {
                connection.Close();
            }

            return failureCount;
        }

        public static bool DeleteLocalLicenesApplication(int localDrivingLicenseAppID) {
            bool isDeleted = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete LocalDrivingLicenseApplications
                             Where LocalDrivingLicenseApplicationID = @localDrivingLicenseAppID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@localDrivingLicenseAppID", localDrivingLicenseAppID);

            try
            {
                connection.Open();
                int AffectedRows = command.ExecuteNonQuery();

                if (AffectedRows > 0)
                    isDeleted = true;
            }
            catch (SqlException ex)
            {
                throw new Exception();
            }
            finally
            {
                connection.Close();
            }

            return isDeleted;
        }
    }
}