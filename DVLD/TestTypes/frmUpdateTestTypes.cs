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

namespace DVLD.TestTypes
{
    public partial class frmUpdateTestTypes : Form
    {
        private int _TestID;
        private clsTestType _Test;
        public frmUpdateTestTypes(int TestID)
        {
            InitializeComponent();
            _TestID = TestID;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _LoadTestInfo() {

            lblID.Text = _TestID.ToString();

            _Test = clsTestType.Find(_TestID);

            if (_Test != null) {

                txtTitle.Text = _Test.Title;
                txtDescrption.Text = _Test.Description;
                txtFees.Text = _Test.Fees.ToString();
                
            }
        }

        private void frmUpdateTestTypes_Load(object sender, EventArgs e)
        {
            _LoadTestInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Test != null)
            {
                _Test.Title = txtTitle.Text;
                _Test.Description = txtDescrption.Text;
                _Test.Fees = Convert.ToDecimal(txtFees.Text);

                if (_Test.Save())
                    MessageBox.Show("Data Saved Successfully");
                else
                    MessageBox.Show("Something went wrong");
            }
        }

        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(clsUtility.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
