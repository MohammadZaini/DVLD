using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace DVLD_DataAccess
{
    public static class clsPersonData
    {

        public static DataTable GetAllPeople() {

            DataTable PeopleDataTable = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, 
	                        People.ThirdName, People.LastName,  Case When Gendor = 0 Then 'Male' When Gendor = 1 Then 'Female' End As Gender, 
                            People.DateOfBirth, Nationality = Countries.CountryName, People.Phone, People.Email                 
                            FROM Countries INNER JOIN
                            People ON Countries.CountryID = People.NationalityCountryID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                
                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows) 
                    PeopleDataTable.Load(reader);

                reader.Close();

            }
            catch (Exception ex)
            {


            }
            finally { 
            
                connection.Close();
            }

            return PeopleDataTable;
        }

        public static int AddNewPerson(string NationalityNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath) {

            int PersonID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"Insert Into People Values(@NationalityNo, @FirstName, @SecondName, @ThirdName, @LastName, @DateOfBirth,
                            @Gender,@Address, @Phone, @Email, @NationalityCountryID, @ImagePath)
                            Select SCOPE_IDENTITY(); ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalityNo", NationalityNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "" && Email != null)
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);


            if (ImagePath != "" && ImagePath != null) {
                string destinationFolderPath = @"C:\\DVLD_People_Images";

                // Ensure the destination folder exists, if not, create it
                if (!Directory.Exists(destinationFolderPath))
                {
                    Directory.CreateDirectory(destinationFolderPath);
                }

                // Construct the destination path including the file name
                string destinationImagePath = Path.Combine(destinationFolderPath, Path.GetFileName(ImagePath));

                // Copy the file
                File.Copy(ImagePath, destinationImagePath, true);

                // Generate a new GUID for the filename
                string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(destinationImagePath);
                string newDestinationImagePath = Path.Combine(destinationFolderPath, newFileName);

                // Rename the copied file to the new filename with GUID
                File.Move(destinationImagePath, newDestinationImagePath);

                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);


            try
            {
                connection.Open();
                
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    PersonID = InsertedID;
            }
            catch (Exception ex)
            {
            }
            finally { 
                connection.Close();
            }

            return PersonID;
        }
        
        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gender, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath) {

            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update People
                             Set NationalNo           = @NationalNo,
                                 FirstName            = @FirstName ,
                                 SecondName           =  @SecondName,
                                 ThirdName            = @ThirdName, 
                                 LastName             =  @LastName, 
                                 DateOfBirth          = @DateOfBirth,
                                 Gendor               =  @Gender,  
                                 Address              = @Address,
                                 Phone                =   @Phone, 
                                 Email                =   @Email, 
                                 NationalityCountryID = @NationalityCountryID,
                                 ImagePath            = @ImagePath
                            Where PersonID = @PersonID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName != "")
                command.Parameters.AddWithValue("@ThirdName", ThirdName);
            else
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gender", Gender);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email != "")
                command.Parameters.AddWithValue("@Email", Email);
            else
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);



            if (ImagePath != "" && ImagePath != null)
            {
                //string destinationFolderPath = @"C:\\DVLD_People_Images";
                //
                //// Ensure the destination folder exists, if not, create it
                //if (!Directory.Exists(destinationFolderPath))
                //{
                //    Directory.CreateDirectory(destinationFolderPath);
                //}
                //
                //if (File.Exists(ImagePath)) { 
                //    
                //}
                //
                //// Construct the destination path including the file name
                //string destinationImagePath = Path.Combine(destinationFolderPath, Path.GetFileName(ImagePath));
                //
                //// Copy the file
                //File.Copy(ImagePath, destinationImagePath, true);
                //
                //// Generate a new GUID for the filename
                //string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(destinationImagePath);
                //string newDestinationImagePath = Path.Combine(destinationFolderPath, newFileName);
                //
                //// Rename the copied file to the new filename with GUID
                //File.Move(destinationImagePath, newDestinationImagePath);

                command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);




            try
            {
                connection.Open();
                int AffectedRows = command.ExecuteNonQuery();

                if(AffectedRows > 0)
                    isUpdated = true;
            }

            catch (Exception ex)
            {


            }
            finally {
                connection.Close();
            }


            return isUpdated;
        }


        public static bool FindByPersonID(int PersonID, ref string NationalityNo, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime DateOfBirth,ref byte Gender, ref string Address, ref string Phone,ref string Email,
            ref int NationalityCountryID, ref string ImagePath) {


            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From People 
                            Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) { 
                    
                    isFound = true;

                    NationalityNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != DBNull.Value)
                        ThirdName = (string)reader["ThirdName"];

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != DBNull.Value)
                        Email = (string)reader["Email"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != DBNull.Value)
                        ImagePath = (string)reader["ImagePath"];

                }

                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally {
                connection.Close();
                
            }


            return isFound;

        }

        public static bool FindByNationalNo(string NationalityNo, ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime DateOfBirth, ref byte Gender, ref string Address, ref string Phone, ref string Email,
            ref int NationalityCountryID, ref string ImagePath)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select * From People 
                            Where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalityNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] != null)
                        ThirdName = (string)reader["ThirdName"];

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gender = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] != null)
                        Email = (string)reader["Email"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] != null)
                        ImagePath = (string)reader["ImagePath"];

                }

                reader.Close();
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

        public static bool IsPersonExist(string NationalNo) { 
            
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 From People 
                             Where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;
                
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

        public static bool DeletePerson(int PersonID) {
            bool isDeleted = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Delete From People 
                             Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static DataTable Filter(string filterWord, string type)
        {

            DataTable FilteredData = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $"Select * From (SELECT People.PersonID, People.NationalNo, People.FirstName, People.SecondName, " +
                $"\r\n\t People.ThirdName, People.LastName,  " +
                $"Case When Gendor = 0 Then 'Male' When Gendor = 1 Then 'Female' End As Gender, " +
                $"People.DateOfBirth, \r\n\t Nationality = Countries.CountryName, People.Phone, People.Email\r\n" +
                $"\r\nFROM Countries INNER JOIN\r\nPeople ON Countries.CountryID = People.NationalityCountryID) PeopleTable" +
                $"\r\nWhere {type} Like '' + @filterWord + '%';";

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