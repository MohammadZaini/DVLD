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

namespace DVLD.Users
{
    public partial class frmListUsers : Form
    {
        public frmListUsers()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _ListUsers() {
            dgvListUsers.DataSource = clsUser.ListUsers();
            dgvListUsers.Columns["FullName"].Width = 250;
            dgvListUsers.Columns["UserName"].Width = 150;
            lblRecordsCount.Text = dgvListUsers.RowCount.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _ListUsers();
            cbFilters.SelectedIndex = 0;
            cbIsActive.SelectedIndex = 0;
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser AddNewUser = new frmAddEditUser(-1);
            AddNewUser.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo UserInfo = new frmUserInfo((int)dgvListUsers.CurrentRow.Cells[1].Value);
            UserInfo.ShowDialog();

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable FilteredData = clsUser.Filter(txtFilter.Text, cbFilters.Text.Replace(" ", ""));
            dgvListUsers.DataSource = FilteredData;                
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex != 0)
                txtFilter.Visible = true;
            else
                txtFilter.Visible = false;

            if (cbFilters.SelectedItem.ToString() == "Is Active") { 
                txtFilter.Visible = false;
                cbIsActive.Visible = true;
            }
            else
                cbIsActive.Visible = false;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.Text == "Person ID" || cbFilters.Text == "User ID")          
                if (clsUtility.IsDigit(e.KeyChar))                               
                    e.Handled = true; // Set e.Handled to true to block the character                    
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable FilteredData = clsUser.Filter(cbIsActive.Text);
            dgvListUsers.DataSource = FilteredData;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser AddNewUser = new frmAddEditUser(-1);
            AddNewUser.ShowDialog();
            _ListUsers();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser UpdateUserfrm = new frmAddEditUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
            UpdateUserfrm.ShowDialog();
            _ListUsers();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvListUsers.CurrentRow.Cells[0].Value;
            int PersonID = (int)dgvListUsers.CurrentRow.Cells[1].Value;

            frmChangePassword ChangePasswordfrm = new frmChangePassword(PersonID,UserID);
            ChangePasswordfrm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

           DialogResult result = MessageBox.Show("Are you sure you want to delete this User? ", "Deletion Confirmation", 
                                                 MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (result == DialogResult.OK)
            {
                try
                {
                    if (clsUser.DeleteUser((int)dgvListUsers.CurrentRow.Cells[0].Value))
                    {
                        MessageBox.Show("User has been deleted successfully", "User Deletion", MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                        _ListUsers();
                    }
                    else
                        MessageBox.Show("Something went wrong", "User Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch { 
                        MessageBox.Show("User cannot be deleted because some data is connected to it!", "User Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            } 
        }
    }
}