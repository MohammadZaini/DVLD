using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsDriver
    {

        public int  ID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver() { 
            
            ID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
        }

        private clsDriver(int personID, int driverID, int createdByUserID, DateTime createdDate)
        {
            
            ID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
        }

        public static DataTable ListDrivers() {
            return clsDriverData.ListDrivers();
        }

        public static clsDriver Find(int personID) {
            int driverID = 0, createdByUserID = 0;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.FindDriverByID(personID, ref driverID, ref createdByUserID, ref createdDate))
                return new clsDriver(personID, driverID, createdByUserID, createdDate);
            else
                return null;
        }

        public int _AddNewDriver() {

            if(!clsDriverData.IsDriver(this.PersonID))
                this.ID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            else 
                this.ID = Find(this.PersonID).ID;

            return this.ID;
        }

        public static bool IsDriver(int personID)
        {
            return clsDriverData.IsDriver(personID);
        }

        public static DataTable Filter(string filterWord, string type) {
            return clsDriverData.Filter(filterWord, type);
        }
    }
}