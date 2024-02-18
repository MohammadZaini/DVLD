using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlLicenseCardWithFilter : UserControl
    {
        public int LicenseID { get; set; }
        public int PersonID { get; set; }
        public int ApplicationType { get; set; }

        private enApplicationType _applicationType = enApplicationType.AddNewInternational;
        private enum enApplicationType { AddNewInternational = 1, RenewLocalLicense = 2, ReplacementForDamageOrLost = 3 , 
            DetainLicense = 4 }

        public delegate void DataBackEventHandler(int licenseID, int personID, bool controlState);

        public DataBackEventHandler DataBack;


        public delegate void DataBackEventHandler2(int licenseID, int personID, bool controlState);
        
        public DataBackEventHandler2 DataBack2;
        public ctrlLicenseCardWithFilter()
        {
            InitializeComponent();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {    
            if (clsUtility.IsDigit(e.KeyChar))
            {
                // Set e.Handled to true to block the character
                e.Handled = true;
            }         
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtFilter.Text)) return;

            int enteredLicenseID = Convert.ToInt32(txtFilter.Text);

            if (!_IsLocalLicenseExist(enteredLicenseID))
            {
                MessageBox.Show($"There is no local license with ID = {enteredLicenseID}!", "Failure", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            _LoadLicenseInfo(enteredLicenseID);
        }
      
        private void _LoadLicenseInfo(int enteredLicenseID) {

            LicenseID = enteredLicenseID;
            _LoadLicenseCardInfo(enteredLicenseID);

            PersonID = ctrlLicenseCard1.PersonID;

            if (!clsLicense.IsLicenseActive(enteredLicenseID))
            {
                DataBack?.Invoke(enteredLicenseID, PersonID, false);
                //DataBack2?.Invoke(enteredLicenseID, PersonID, false);

                MessageBox.Show("License is Inactive !", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DataBack?.Invoke(enteredLicenseID, PersonID, true); // International

            //DataBack2?.Invoke(enteredLicenseID, PersonID, true); // Renew         
        }

        private void _LoadLicenseCardInfo(int enteredLicenseID)
        {
            ctrlLicenseCard1._LoadLicenseInfoDetails(enteredLicenseID);
        }

        private bool _IsLocalLicenseExist(int localLicenseID)
        {
            return clsLicense.IsLicenseExist(localLicenseID);
        }

        private bool _IsLicenseValid(int localLicenseID) {
            return clsLicense.IsLicenseValid(localLicenseID);
        }


        private void _Test() {

            switch (_applicationType)
            {
                case enApplicationType.AddNewInternational:
                    {
                        if (!clsLicense.IsLicenseActive(5))
                        {
                            MessageBox.Show("Not Active");
                            break;

                        }
                        if (!clsLicense.IsLicenseExpired(5))
                        {
                            MessageBox.Show("License is Expired");
                            break;

                        }

                        if (!clsLicense.IsLicenseClassOdinaryDrivingLicense(5))
                            MessageBox.Show("License must be of type Odinary Driving License");
                        break;
                    }
                    
                case enApplicationType.RenewLocalLicense:
                    if (clsLicense.IsLicenseActive(5))
                        MessageBox.Show("License Is already active and it will expire on [....]");
                    break;
                case enApplicationType.ReplacementForDamageOrLost:
                    if (!clsLicense.IsLicenseActive(5))
                        MessageBox.Show("Cannot replace a Not Active license");
                    break;
                case enApplicationType.DetainLicense:
                    break;
                default:
                    break;
            }
        }

        private void ctrlLicenseCardWithFilter_Load(object sender, EventArgs e)
        {
            _applicationType = (enApplicationType)ApplicationType; 
        }
    }
}