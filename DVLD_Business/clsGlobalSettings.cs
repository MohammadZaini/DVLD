using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Business
{
    public static class clsGlobalSettings
    {
        public static clsUser LoggedInUser { get; set; }

        public static readonly int FirstTimeMode = 1;
        public static readonly int EditMode = 2;
        public static readonly int RetakeMode = 3;

        public static readonly int VisionTest = 1;
        public static readonly int WrittenTest = 2;
        public static readonly int StreetTest = 3;

        public static readonly byte IssueLicenseForFirstTime = 1;

        public enum localApplicationMode { New = 1, Cancelled = 2, Completed = 3 };

        public enum enLicenseIssueReason { FirstTime = 1, Renew = 2, ReplacementForLost = 3, ReplacementForDamaged = 4 };

        public enum enApplicationTypes { NewLocalDrivingLicenseService = 1, RenewDrivingLicenseService = 2,
            ReplacementForALostDrivingLicense = 3, ReplacementForADamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5,
            NewInternationalLicense = 6, RetakeDrivingLicenseTestService = 8
        };


        public static string GetIssueReasonString(int issueReason)
        {

            switch (issueReason)
            {
                case (int)enLicenseIssueReason.FirstTime:
                    return "First Time";

                case (int)enLicenseIssueReason.Renew:
                    return "Renew";

                case (int)enLicenseIssueReason.ReplacementForLost:
                    return "Replacement For Lost";

                case (int)enLicenseIssueReason.ReplacementForDamaged:
                    return "Replacement For Damaged";
            }

            return "Unkown";
        }

        public static string dateFormat = "yyyy/MM/dd";


        public static byte GetValidityLengthForLicenseClass(int licenseClassNo) {
            return clsLicenseClass.Find(licenseClassNo).DefaultValidityLength;
        }


        public static decimal GetApplicationFees(int applicationTypeID)
        {
            return clsApplicationType.Find(applicationTypeID).Fees;
        }

    }
}