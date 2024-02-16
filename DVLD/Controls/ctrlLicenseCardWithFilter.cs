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

    public delegate void DataBackEventHandler(int licenseID, int personID , int internationalLicenseID, bool controlState);

        public DataBackEventHandler DataBack;
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

            int internationalLicenseID = _GetInternationalID();

            PersonID = ctrlLicenseCard1.PersonID;
            DataBack?.Invoke(enteredLicenseID, PersonID, internationalLicenseID, true);

            if (!_IsLicenseValid(enteredLicenseID))
            {
                DataBack?.Invoke(enteredLicenseID, PersonID, internationalLicenseID, false);
                MessageBox.Show("License is invalid!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            if (_IsInternationalLicenseExist(enteredLicenseID))
            {
                DataBack?.Invoke(enteredLicenseID, PersonID, internationalLicenseID, false);

                MessageBox.Show("This individual already has an International License!", "Failure", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
        }

        private void _LoadLicenseCardInfo(int enteredLicenseID)
        {
            ctrlLicenseCard1._LoadLicenseInfoDetails(-1, enteredLicenseID);
        }

        private int _GetInternationalID() {

            clsInternationalLicense internationalLicense = clsInternationalLicense.FindByLicenseID(LicenseID);

            if (internationalLicense == null) return -1;

            return internationalLicense.ID;
        }

        private bool _IsInternationalLicenseExist(int localLicenseID) {
            return clsInternationalLicense.IsInternationalLicenseExist(localLicenseID);
        }

        private bool _IsLocalLicenseExist(int localLicenseID)
        {
            return clsLicense.IsLicenseExist(localLicenseID);
        }

        private bool _IsLicenseValid(int localLicenseID) {
            return clsLicense.IsLicenseValid(localLicenseID);
        }
    }
}