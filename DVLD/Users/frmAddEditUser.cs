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
    public partial class frmAddEditUser : Form
    {

        private int _UserID = -1;
        private clsUser _User;

        enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode = enMode.AddNew;

        public frmAddEditUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddEditUser(int UserID)
        {
            InitializeComponent();

            _UserID = UserID;
            _Mode = enMode.Update;
        }
     
        private void btnNext_Click(object sender, EventArgs e)
        {

            if (_Mode == enMode.Update)
            { 
               btnSave.Enabled = true;
               tpLoginInfo.Enabled = true;
               tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID == -1)
            {
                clsUtil.ShowErrorMessage("Please Select A Person", "Failure");
                return;
            }
             
            if (!clsUser.IsUserExist(ctrlPersonCardWithFilter1.PersonID) && ctrlPersonCardWithFilter1.PersonID != 0)
            {
                TabPage LoginInfoTab = tcUserInfo.TabPages[1];
                tcUserInfo.SelectedTab = LoginInfoTab;
            }
            else
                clsUtil.ShowErrorMessage("This Person is already a User, choose another one","Failure");
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

            if (_Mode == enMode.AddNew) { 
            
                lblAddEditUser.Text = "Add New User";
                _User = new clsUser();
                return;
            }

            lblAddEditUser.Text = "Update User";

            _User = clsUser.Find(_UserID);

            if (_User == null)
            {
                clsUtil.ShowErrorMessage("No User Was Found With ID = " + _UserID, "Failure");
                return;
            }

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
                return;
            }
            else
            { 
                e.Cancel = false;
                errorProvider1.SetError(txtUsername, "");
            }


            if (_Mode == enMode.Update)
            {
                if (_User.Username != txtUsername.Text.Trim())
                    CheckUsernameExistance(e);

                return;
            }

            // Add New Mode
            CheckUsernameExistance(e);
        }

        private void CheckUsernameExistance(CancelEventArgs e) {

            if (clsUser.IsUserExist(txtUsername.Text.Trim()))
            {
                e.Cancel = true;
                txtUsername.Focus();
                errorProvider1.SetError(txtUsername, "Username is already in use, choose another one");
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

            if (!this.ValidateChildren())
            {
                clsUtil.ShowErrorMessage("Some fields are not valid!", "Failure");
                return;
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.Username = txtUsername.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = chkIsActive.Checked;

            if (_User.Save())
            {
                clsUtil.ShowInformationMessage("Data Saved Successfully", "User Added");
                _Mode = enMode.Update;
                lblAddEditUser.Text = "Update User";
                lblUserID.Text = _User.UserID.ToString();

            }
            else
                clsUtil.ShowErrorMessage("Something went wrong","Error");
        
        }
    }
}