using DVLD.Global_Classes;
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
        private DataTable _dtAllUsers = clsUser.GetAllUsers();

        public frmListUsers()
        {
            InitializeComponent();
        }

        private void _ListUsers() {
            dgvListUsers.DataSource = _dtAllUsers;

            if (dgvListUsers.RowCount == 0) return;

            _ChangeHeadersText();

            lblRecordsCount.Text = dgvListUsers.RowCount.ToString();
        }

        private void _ChangeHeadersText() {

            dgvListUsers.Columns["UserID"].HeaderText = "User ID";
            dgvListUsers.Columns["UserID"].Width = 110;

            dgvListUsers.Columns["PersonID"].HeaderText = "Person ID";
            dgvListUsers.Columns["PersonID"].Width = 120;

            dgvListUsers.Columns["FullName"].HeaderText = "Full Name";
            dgvListUsers.Columns["FullName"].Width = 250;

            dgvListUsers.Columns["UserName"].HeaderText = "Username";
            dgvListUsers.Columns["UserName"].Width = 120;

            dgvListUsers.Columns["IsActive"].HeaderText = "Is Active";
            dgvListUsers.Columns["IsActive"].Width = 120;
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _ListUsers();
            cbFilters.SelectedIndex = 0;         
        }

        private string _GetSelectedFilterColumn() {

            string filterColumn = "";

            switch (cbFilters.Text)
            {
                case "User ID":
                    filterColumn = "UserID";
                    break;
                case "User Name":
                    filterColumn = "UserName";
                    break;

                case "Person ID":
                    filterColumn = "PersonID";
                    break;


                case "Full Name":
                    filterColumn = "FullName";
                    break;

                default:
                    filterColumn = "None";
                    break;
            }

            return filterColumn;
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser AddNewUser = new frmAddEditUser();
            AddNewUser.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Send the USER ID to the constructor
            frmUserInfo UserInfo = new frmUserInfo((int)dgvListUsers.CurrentRow.Cells[0].Value);
            UserInfo.ShowDialog();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            //DataTable FilteredData = clsUser.Filter(txtFilter.Text, cbFilters.Text.Replace(" ", ""));
            //dgvListUsers.DataSource = FilteredData;

            string filterColumn = _GetSelectedFilterColumn();
          
            if (filterColumn == "None" || txtFilter.Text.Trim() == "")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvListUsers.Rows.Count.ToString();
                return;
            }

            if (_dtAllUsers == null) return;

            if (filterColumn != "FullName" && filterColumn != "UserName")
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, txtFilter.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilter.Text);

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilters.SelectedIndex != 0);
            txtFilter.Text = "";

            if (cbFilters.SelectedItem.ToString() == "Is Active") {
                txtFilter.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
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
            //DataTable FilteredData = clsUser.Filter(cbIsActive.Text);
            //dgvListUsers.DataSource = FilteredData;

            string filterColumn = "IsActive";

            string filterValue = _GetIsActiveValue();

            if (filterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, filterValue);

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }

        private string _GetIsActiveValue() {
            string filterValue = cbIsActive.Text;

            switch (cbIsActive.Text)
            {
                case "All":
                    break;
                case "Yes":
                    filterValue = "1";
                    break;
                case "No":
                    filterValue = "0";
                    break;

                default:
                    break;
            }

            return filterValue;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser AddNewUser = new frmAddEditUser();
            AddNewUser.ShowDialog();
            frmListUsers_Load(null,null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditUser UpdateUserfrm = new frmAddEditUser((int)dgvListUsers.CurrentRow.Cells[0].Value);
            UpdateUserfrm.ShowDialog();
            frmListUsers_Load(null,null);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Send User ID to Change Password Form
            frmChangePassword ChangePasswordfrm = new frmChangePassword((int)dgvListUsers.CurrentRow.Cells[0].Value);
            ChangePasswordfrm.ShowDialog();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
       
           DialogResult result = clsUtil.ShowWarningMessage("Are you sure you want to delete this User?", "Deletion Confirmation");

            if (result == DialogResult.OK)
            {
                try
                {
                    if (clsUser.DeleteUser((int)dgvListUsers.CurrentRow.Cells[0].Value))
                    {
                        clsUtil.ShowInformationMessage("User has been deleted successfully", "Deletion Successful");
                        frmListUsers_Load(null, null); 
                    }
                    else
                        clsUtil.ShowErrorMessage("The deletion of the user was unsuccessful.", "Deletion Unsuccessful");
                }
                catch {
                    clsUtil.ShowErrorMessage("The user cannot be deleted as it is associated with other data.", "Deletion Unsuccessful");
                }
            } 
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsUtil.ShowInformationMessage("This feature has not been implemented!", "Not Implemented Yet");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsUtil.ShowInformationMessage("This feature has not been implemented!", "Not Implemented Yet");
        }
    }
}