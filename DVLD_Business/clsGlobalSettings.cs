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

        public static readonly int IssueLicenseForFirstTime = 1;

        public enum localApplicationMode { New = 1, Cancelled = 2, Completed = 3 };
    }
}