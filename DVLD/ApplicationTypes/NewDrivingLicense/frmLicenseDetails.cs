using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationTypes.NewDrivingLicense
{
    public partial class frmLicenseDetails : Form
    {
        public frmLicenseDetails(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            CenterToScreen();

            ctrlLicenseCard1._LoadLicenseInfoDetails(localDrivingLicenseAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
