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

namespace DVLD.ApplicationTypes
{
    public partial class frmUpdateApplicationType : Form
    {
        private int _AppTypeID;
        private clsApplicationType _ApplicationType;
        public frmUpdateApplicationType(int appTypeID)
        {
            InitializeComponent();
            _AppTypeID = appTypeID;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _LoadAppTypeInfo() { 
            lblID.Text = _AppTypeID.ToString();

            _ApplicationType = clsApplicationType.Find(_AppTypeID);

            if (_ApplicationType != null) {
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_ApplicationType != null) { 
                
                _ApplicationType.Title = txtTitle.Text;
                _ApplicationType.Fees = Convert.ToDecimal(txtFees.Text);

                if (_ApplicationType.Save())
                    MessageBox.Show("Data Saved Successfully");
                else
                    MessageBox.Show("Something went wrong");

            }       
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _LoadAppTypeInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
