using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationTypes.InternationalLicense
{
    public partial class frmInternationalDriverInfo : Form
    {
        public frmInternationalDriverInfo(int internationalLicenseID)
        {
            InitializeComponent();
            CenterToScreen();

            ctrlInternationalLicenseCard1.LoadInternationalLicenseInfo(internationalLicenseID);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}