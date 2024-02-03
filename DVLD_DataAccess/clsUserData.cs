using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class clsUserData
    {
        public static DataTable ListUsers() { 
            
            DataTable UsersList = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT Users.UserID, Users.PersonID, FullName = People.FirstName + ' ' + People.SecondName + ' ' 
                             + People.ThirdName + ' '+ People.LastName, Users.UserName, Users.IsActive
                             FROM People INNER JOIN
                             Users ON People.PersonID = Users.PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    UsersList.Load(reader);

                reader.Close();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return UsersList;
        }

        public static bool FindByUserName(string Username, ref int UserID, ref int PersonID, ref string Password, ref bool IsActive) {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Users Where UserName = @Username";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", Username);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) { 
                    
                    isFound = true;

                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

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

        public static bool FindByUserID(int UserID, ref string Username, ref int PersonID, ref string Password, ref bool IsActive)
        {

            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "Select * From Users Where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    isFound = true;

                    Username = (string)reader["UserName"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

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

        public static bool IsUser(int PersonID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 From Users 
                             Where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@PersonID",PersonID);

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

        public static DataTable Filter(string filterWord, string type)
        {

            DataTable FilteredData = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $"Select * From (SELECT Users.UserID, Users.PersonID,  FullName = People.FirstName + ' ' + " +
                $"People.SecondName + ' ' +  People.ThirdName + ' ' + People.LastName ,Users.UserName, Users.IsActive\r\n" +
                $"FROM  People INNER JOIN\r\nUsers ON People.PersonID = Users.PersonID) ResultSet\r\n" +
                $"Where {type} Like '' + @filterWord + '%';";

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

        public static DataTable FilterByIsActive(string filterWord)
        {

            DataTable FilteredData = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $"Select * From (SELECT Users.UserID, Users.PersonID,  FullName = People.FirstName + ' ' + " +
                $"People.SecondName + ' ' +  People.ThirdName + ' ' + People.LastName ,Users.UserName, Users.IsActive\r\n" +
                $"FROM  People INNER JOIN\r\nUsers ON People.PersonID = Users.PersonID) ResultSet\r\n";

            if (filterWord == "Yes")
                query += " Where IsActive = 1";
            else if (filterWord == "No")
                query += " Where IsActive = 0";

            SqlCommand command = new SqlCommand(query, connection);

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

        public static int AddNewUser(int PersonID, string Username, string Password, bool IsActive) {

            int UserID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into Users 
                             Values(@PersonID, @Username, @Password, @IsActive) 
                             Select SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("PersonID", PersonID);
            command.Parameters.AddWithValue("Username", Username);
            command.Parameters.AddWithValue("Password", Password);
            command.Parameters.AddWithValue("IsActive", IsActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int InsertedID))
                    UserID = InsertedID;

            }
            catch
            {
            }
            finally 
            { 
                connection.Close();
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, string Username ,string Password, bool IsActive ) {

            bool isUpdated = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Users 
                             Set UserName = @Username, Password = @Password, IsActive = @IsActive 
                             Where UserID = @UserID;";

            SqlCommand command = new SqlCommand(query ,connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@Username",Username);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();
                int AffectedRows = command.ExecuteNonQuery();  

                if(AffectedRows > 0)
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

        public static bool ChangePassword(int UserID, string NewPassword) { 
            
            bool isChanged = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Update Users 
                             Set Password = @Password 
                             Where UserID = @UserID" ;

            SqlCommand command = new SqlCommand(query ,connection);
            command.Parameters.AddWithValue("@Password", NewPassword);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                int AffectedRows = command.ExecuteNonQuery();

                if(AffectedRows > 0)
                    isChanged = true;
            }
            catch
            {
            }
            finally
            {
                connection.Close();
            }


            return isChanged;
        }

        public static bool IsPasswordMatch(int UserID, string CurrentPassword) { 

            bool IsMatch = false;

            SqlConnection connection = new SqlConnection (clsDataAccessSettings.connectionString);

            string query = @"Select Found = 1 
                             From Users 
                             Where Password = @CurrentPassword And UserID = @UserID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@CurrentPassword", CurrentPassword);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    IsMatch = true;

                reader.Close();
            }
            catch
            {

            }
            finally 
            { 
                connection.Close(); 
            }

            return IsMatch;
        }

        public static bool DeleteUser(int UserID) {

            bool IsDeleted = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"Delete From Users 
                             Where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                int AffectedRows = command.ExecuteNonQuery();

                if(AffectedRows > 0)
                    IsDeleted = true;
            }
            catch (SqlException ex)
            {
                throw new Exception();
            }
            finally
            { 
                connection.Close(); 
            }

            return IsDeleted;
        }

    }
}