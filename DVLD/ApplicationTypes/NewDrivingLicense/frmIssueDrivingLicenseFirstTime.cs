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
    public partial class frmIssueDrivingLicenseFirstTime : Form
    {
        public frmIssueDrivingLicenseFirstTime(int localDrivingLicenseAppID)
        {
            InitializeComponent();
            ctrlDrivingLicenseAppAndApplicationInfo1.LoadApplicationDetials(localDrivingLicenseAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

        }
    }
}