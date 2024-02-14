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
            int licenseID = Convert.ToInt32(txtFilter.Text);

            ctrlLicenseCard1._LoadLicenseInfoDetails(-1, licenseID);
        }
    }
}