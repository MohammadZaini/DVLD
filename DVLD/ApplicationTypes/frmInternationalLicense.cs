using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.ApplicationTypes
{
    public partial class frmInternationalLicense : Form
    {
        public frmInternationalLicense()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}