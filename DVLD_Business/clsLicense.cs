using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsLicense
    {
        public int LicenseID { get; set; }
        public int LicenseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public string PaidFees { get; set; }
        public bool IsActive { get;}
        public int IssueReason { get; set; }
        public int CreatedByUserID { get; set; }



    }
}
