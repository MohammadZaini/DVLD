using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsApplicationType
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }


        private clsApplicationType(int AppTypeID, string AppTitle, decimal AppFees) { 
            this.ID = AppTypeID;
            this.Title = AppTitle;
            this.Fees = AppFees;
        }

        public static DataTable ListApplicationTypes() { 
            return clsApplicationTypeData.ListApplicationTypes();
        }

        private bool _UpdateInfo(string AppTitle, decimal AppFees) {
           return clsApplicationTypeData.UpdateAppTypesInfo(this.ID, AppTitle, AppFees);
        }

        public static clsApplicationType Find(int AppTypeID) {

            string AppTitle = "";
            decimal AppFees = 0;

            if (clsApplicationTypeData.FindAppTypeByID(AppTypeID, ref AppTitle, ref AppFees))
                return new clsApplicationType(AppTypeID, AppTitle, AppFees);
            else
                return null;
        }

        public bool Save() {

            return _UpdateInfo(this.Title, this.Fees);
        }
    }
}
