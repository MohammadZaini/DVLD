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

        public delegate void DataBackEventHandler(int licenseID, int personID, bool controlState);

        public DataBackEventHandler DataBack;

        public ctrlLicenseCardWithFilter()
        {
            InitializeComponent();
        }


        public void LoadDetainedLicenseInfo(int licenseID) { 
            ctrlLicenseCard1._LoadLicenseInfoDetails(licenseID);
            gbFilter.Enabled = false;
            txtFilter.Text = licenseID.ToString();
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

                MessageBox.Show("License is Inactive !", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            DataBack?.Invoke(enteredLicenseID, PersonID, true); 
        }

        private void _LoadLicenseCardInfo(int enteredLicenseID)
        {
            ctrlLicenseCard1._LoadLicenseInfoDetails(enteredLicenseID);
        }

        private bool _IsLocalLicenseExist(int localLicenseID)
        {
            return clsLicense.IsLicenseExist(localLicenseID);
        }
    }
}