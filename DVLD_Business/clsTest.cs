using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTest
    {
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public bool Result { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest() { 
            TestAppointmentID = -1;
            Result = false;
            Notes = string.Empty;
            CreatedByUserID = -1;
        }


        private bool _PerformNewTest() {

            this.TestID = clsTestData.PerformNewTest(this.TestAppointmentID, this.Result, this.Notes, this.CreatedByUserID);

            return (this.TestID != -1);
        }

        public bool Save() {
            return _PerformNewTest();
        }


    }
}
