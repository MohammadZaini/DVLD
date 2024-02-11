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
    public partial class frmLocalLicenseApplicationDetails : Form
    {
        public frmLocalLicenseApplicationDetails(int locaDrivinglLicenseAppID)
        {
            InitializeComponent();
            CenterToScreen();

            ctrlDrivingLicenseAppAndApplicationInfo1.LoadApplicationDetials(locaDrivinglLicenseAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
