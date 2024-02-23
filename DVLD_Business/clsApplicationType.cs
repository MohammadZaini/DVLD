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

        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode = enMode.AddNew;
        public int ID { get; set; }
        public string Title { get; set; }
        public decimal Fees { get; set; }


        private clsApplicationType()
        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;

            _Mode = enMode.AddNew;
        }
        private clsApplicationType(int AppTypeID, string AppTitle, decimal AppFees) { 
            this.ID = AppTypeID;
            this.Title = AppTitle;
            this.Fees = AppFees;

            _Mode = enMode.Update;
        }

        public static DataTable ListApplicationTypes() { 
            return clsApplicationTypeData.ListApplicationTypes();
        }

        private bool _UpdateApplicationType() {
           return clsApplicationTypeData.UpdateAppTypesInfo(this.ID, this.Title, this.Fees);
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

            switch (_Mode)
            {
                case enMode.AddNew:
                    break;
                case enMode.Update:
                    if(_UpdateApplicationType())
                        return true;
                    break;

                default:
                    break;
            }

             return false;
        }
    }
}