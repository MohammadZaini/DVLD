using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {
        public int TestID { get; set; }
        public string Title { get; set; }
        public string Description { get; set;}
        public decimal Fees { get; set; }


        private clsTestType(int TestID, string Title, string Description, decimal Fees) { 
        
            this.TestID = TestID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }

        public static DataTable ListTestTypes() { 
            return clsTestTypeData.ListTestTypes();
        }

        public static clsTestType Find(int TestID) {

            string TestTitle = "", TestTypeDescription = "";
            decimal TestFees = 0;

            if (clsTestTypeData.FindTestTypeByID(TestID, ref TestTitle, ref TestTypeDescription, ref TestFees))
                return new clsTestType(TestID, TestTitle, TestTypeDescription, TestFees);
            else
                return null;
        }

        private bool _UpdateInfo(string Title, string Description, decimal Fees)
        {
            return clsTestTypeData.UpdateTestInfo(this.TestID, Title, Description, Fees);
        }

        public bool Save() {

            return _UpdateInfo(this.Title, this.Description, this.Fees);
        }
    }
}
