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
    public partial class frmChangePassword : Form
    {
        private int _userID;
        private clsUser _user;
        public frmChangePassword(int userID)
        {
            InitializeComponent();
            _userID = userID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                clsUtil.ShowErrorMessage("Some fields are not valid, hover over the red icon(s) to see the message");
                return;
            }

            _user.Password = txtNewPassword.Text.Trim();

            if(_user.Save())
                clsUtil.ShowInformationMessage("Password has changed successfully", "Password Change");
            else
                clsUtil.ShowErrorMessage("Password has NOT changed..", "Password Change");
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (_user.Password != txtCurrentPassword.Text)
            {
                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetError(txtCurrentPassword, "Current Password Is Incorrect");
            }
            else { 
                e.Cancel = false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }

        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text)) {
                e.Cancel = true;
                txtNewPassword.Focus();
                errorProvider1.SetError(txtNewPassword, "Password Can't be empty");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNewPassword, "");
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsUtility.IsPasswordMatch(txtNewPassword.Text, txtConfirmPassword.Text))
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            _user = clsUser.Find(_userID);

            if (_user == null)
            {
                clsUtil.ShowErrorMessage("Couldn't find user with ID: " + _userID);
                Close();
                return;
            }

            ctrlUserCard1.LoadUserInfo(_userID);
        }

        private void _ResetDefaultValues() {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
    }
}