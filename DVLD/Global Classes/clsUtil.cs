using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Global_Classes
{
    public static class clsUtil
    {
        public static string GenerateGUID() { 
            
            Guid newGuid = Guid.NewGuid();

            return newGuid.ToString();
        }

        public static bool CreateFolderIfDoesNotExist(string folderPath) {

            if (Directory.Exists(folderPath)) return true;

            try
            {
                // Create the folder if it's not exist
                Directory.CreateDirectory(folderPath);
                return true;
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Error While Creating Folder: " + ex.Message, "Failure");
                return false;
            }
        }

        public static string ReplaceFileNameWithGUID(string sourceFile ) { 
            
            string fileName = sourceFile;
            FileInfo fi = new FileInfo(fileName);
            string extn = fi.Extension;

            return GenerateGUID() + extn;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile) {

            string destinationFolder = @"E:\DVLD_People_Images\";

            if (!CreateFolderIfDoesNotExist(destinationFolder)) return false;

            string detinationFile = destinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {
                File.Copy(sourceFile, detinationFile, true);
            }
            catch (IOException ios)
            {
                ShowErrorMessage(ios.Message, "Failure");
                return false;
            }

            sourceFile = detinationFile;
            return true;
        }

        public static void ShowErrorMessage(string errorMessage, string errorCaption = "Failure") {
            MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        public static void ShowInformationMessage(string errorMessage, string errorCaption)
        {
            MessageBox.Show(errorMessage, errorCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}