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

    public delegate void DataBackEventHandler(int licenseID, int personID ,bool controlState);

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


            this.LicenseID = enteredLicenseID;
            _LoadLicenseCardInfo(enteredLicenseID);

            PersonID = ctrlLicenseCard1.PersonID;
            DataBack?.Invoke(enteredLicenseID, PersonID, true);

            if (clsInternationalLicense.IsInternationalLicenseExist(enteredLicenseID))
            {
                DataBack?.Invoke(enteredLicenseID, PersonID , false);

                MessageBox.Show("This individual already has an International License!", "Failure", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            if (!clsLicense.IsLicenseValid(enteredLicenseID))
                MessageBox.Show("License is invalid!", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void _LoadLicenseCardInfo(int enteredLicenseID) { 
            ctrlLicenseCard1._LoadLicenseInfoDetails(-1, enteredLicenseID);
        }
    }
}