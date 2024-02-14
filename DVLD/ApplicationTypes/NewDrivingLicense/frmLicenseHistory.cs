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

namespace DVLD.ApplicationTypes.NewDrivingLicense
{
    public partial class frmLicenseHistory : Form
    {
        private clsLocalLicenseApplication _localDrivingLicenseApplication;

        public frmLicenseHistory(int localDrivingLicensAppID)
        {
            InitializeComponent();
            CenterToScreen();

            _localDrivingLicenseApplication = clsLocalLicenseApplication.Find(localDrivingLicensAppID);

            if (_localDrivingLicenseApplication == null)
                return;

            int personID = _localDrivingLicenseApplication.Application.ApplicantPersonID;

            ctrlPersonCardWithFilter1.LoadUserData(personID);
        }

        private void _ListLocalLicenses() {
            int personID = _localDrivingLicenseApplication.Application.ApplicantPersonID;
            dgvLocalLicensesList.DataSource = clsLicense.ListLocalLicenses(personID);

            dgvLocalLicensesList.Columns["Class Name"].Width = 210;
            dgvLocalLicensesList.Columns["Issue Date"].Width = 180;
            dgvLocalLicensesList.Columns["Expiration Date"].Width = 180;

            lblRecordsCount.Text = dgvLocalLicensesList.RowCount.ToString();    
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            _ListLocalLicenses();
        }
    }
}