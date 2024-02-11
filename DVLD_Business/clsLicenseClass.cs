using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DVLD_Business
{
    public class clsLicenseClass
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public decimal Fees { get; set; }

        public clsLicenseClass() {
            ID = -1;
            Name = string.Empty;
            Description = string.Empty;
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            Fees = 0;
        }

        private clsLicenseClass(int id, string name, string description, byte minimumAllowedAge, byte defaultValidityLength, decimal fees)
        {
            ID = id;
            Name = name;
            Description = description;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            Fees = fees;
        }

        public static clsLicenseClass Find(int classID) {

            string name = "", description = "";
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            decimal fees = 0;

            if (clsLicenseClassData.FindByClassID(classID, ref name, ref description, ref minimumAllowedAge,
                ref defaultValidityLength, ref fees))

                return new clsLicenseClass(classID, name, description, minimumAllowedAge, defaultValidityLength, fees);
            else
                return null;

        }

    }
}