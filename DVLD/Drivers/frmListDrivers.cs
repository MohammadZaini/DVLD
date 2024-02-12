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

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        public frmListDrivers()
        {
            InitializeComponent();
            CenterToScreen();
        }


        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _ListDrivers();
            cbFilter.SelectedIndex = 0; // None
        }

        private void _ListDrivers() {
            dgvDriversList.DataSource = clsDriver.ListDrivers();
            dgvDriversList.Columns["Full Name"].Width = 250;
            dgvDriversList.Columns["Created Date"].Width = 200;
            lblRecordsCount.Text = dgvDriversList.RowCount.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable FilteredData = clsDriver.Filter(txtFilter.Text, cbFilter.Text);
            dgvDriversList.DataSource = FilteredData;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex != 0)
                txtFilter.Visible = true;
            else
            { 
                txtFilter.Visible = false;
                _ListDrivers();
            }
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Driver ID" || cbFilter.Text == "Person ID")
                if (clsUtility.IsDigit(e.KeyChar))
                    e.Handled = true; // Set e.Handled to true to block the character  
        }
    }
}