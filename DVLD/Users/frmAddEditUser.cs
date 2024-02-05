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
    public partial class frmAddEditUser : Form
    {

        private int _UserID;
        private clsUser _User;

        enum enMode { Add = 0, Update = 1 };

        private enMode _Mode = enMode.Add;

        public frmAddEditUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;

            if (UserID == -1)
                _Mode = enMode.Add;
            else
                _Mode = enMode.Update;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (!clsUser.IsUser(ctrlPersonCardWithFilter1.PersonID) && ctrlPersonCardWithFilter1.PersonID != 0)
            {
                TabPage LoginInfoTab = tcUserInfo.TabPages[1];
                tcUserInfo.SelectedTab = LoginInfoTab;
            }
            else
                MessageBox.Show("This Person is already a User, choose another one","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tcUserInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage LoginInfoTab = tcUserInfo.TabPages[1];

            if (_Mode == enMode.Update)
                btnSave.Enabled = true;
            else
                btnSave.Enabled = tcUserInfo.SelectedTab == LoginInfoTab;

            //if (tcUserInfo.SelectedTab == LoginInfoTab)
            //    txtUsername.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _Load() {

            if (_Mode == enMode.Add) { 
            
                lblAddEditUser.Text = "Add New User";
                _User = new clsUser();
                return;
            }

            lblAddEditUser.Text = "Update User";

            _User = clsUser.Find(_UserID);
            ctrlPersonCardWithFilter1.LoadUserData(_User.PersonID);
            lblUserID.Text = _UserID.ToString();
            txtUsername.Text = _User.Username;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Enabled = _User.IsActive;

            btnNext.Enabled = false;

            clsGlobalSettings.LoggedInUser = _User;

        }

        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            _Load();


        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsUtility.IsPasswordMatch(txtPassword.Text, txtConfirmPassword.Text))
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Password doesn't match");
            }
            else 
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                txtUsername.Focus();
                errorProvider1.SetError(txtUsername, "Username cannot be empty");
            }
            else
            { 
                e.Cancel = false;
                errorProvider1.SetError(txtUsername, "");
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password cannot be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.Username = txtUsername.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;

            if (_User.Save())
                MessageBox.Show("Data Saved Successfully","User Added",MessageBoxButtons.OK,MessageBoxIcon.Information);
            else
                MessageBox.Show("Something went wrong","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);

            _Mode = enMode.Update;
            lblAddEditUser.Text = "Update User";
            lblUserID.Text = _User.UserID.ToString();

        }
    }
}