using DVLD_DataAccess;
using System;
using System.Collections.Generic;
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

        private clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            
            ID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;
        }


        public static clsDriver Find(int driverID) {
            int personID = 0, createdByUserID = 0;
            DateTime createdDate = DateTime.Now;

            if (clsDriverData.FindDriverByID(driverID, ref personID, ref createdByUserID, ref createdDate))
                return new clsDriver(driverID, personID, createdByUserID, createdDate);
            else
                return null;

        }


        public int _AddNewDriver() {

            this.ID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);

            return this.ID;
        }

        //public bool Save() {
        //    return _AddNewDriver();
        //}
    }
}
