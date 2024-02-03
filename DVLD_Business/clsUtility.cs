using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public static class clsUtility
    {
        public static void SaveCredentialsToFile(string filePath, string data)
        {
            try
            {

                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllText(filePath, data);
            }
            catch (Exception ex)
            {
                // Handle exception
                 Console.WriteLine("An error occurred while writing to the file: " + ex.Message);

                //   throw new Exception("An error occurred while writing to the file: " + ex.Message);
            }
        }
        public static List<string> FillTextBoxWithCredentials(string filePath)
        {

            List<string> credentials = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line = "";

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] splitString = line.Split('#');

                        credentials.Add(splitString[0]);
                        credentials.Add(splitString[1]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while reading the file: " + ex.Message);
            }

            return credentials;
        }
        public static bool IsPasswordMatch(string password, string confirmPassword) {
            return password == confirmPassword;
        }

        public static bool IsDigit(char inputChar) {
            return (!char.IsDigit(inputChar) && !char.IsControl(inputChar));
        }
    }
}