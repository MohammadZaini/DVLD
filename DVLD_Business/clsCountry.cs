using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsCountry
    {

        public int ID { get; set; }
        public string CountryName { get; set; }

        public static DataTable ListCountries() { 
            return clsCountryData.ListCountries();
        }

        clsCountry(int CountryID, string CountryName) { 
            
            ID = CountryID;
            this.CountryName = CountryName;
        }

        public static clsCountry Find(int CountryID) {

            string CountryName = "";

            if (clsCountryData.FindCountryInfoByID( CountryID, ref CountryName))
                return new clsCountry(CountryID,CountryName);
            else 
                return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;
            
            if (clsCountryData.FindCountryInfoByName(ref CountryID, CountryName))
                return new clsCountry(CountryID, CountryName);
            else
                return null;
        }
    }
}