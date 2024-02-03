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
        public int AppTypeID { get; set; }
        public string AppTitle { get; set; }
        public decimal AppFees { get; set; }


        private clsApplicationType(int AppTypeID, string AppTitle, decimal AppFees) { 
            this.AppTypeID = AppTypeID;
            this.AppTitle = AppTitle;
            this.AppFees = AppFees;
        }

        public static DataTable ListApplicationTypes() { 
            return clsApplicationTypeData.ListApplicationTypes();
        }

        private bool _UpdateInfo(string AppTitle, decimal AppFees) {
           return clsApplicationTypeData.UpdateAppTypesInfo(this.AppTypeID, AppTitle, AppFees);
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

            return _UpdateInfo(this.AppTitle, this.AppFees);
        }
    }
}
