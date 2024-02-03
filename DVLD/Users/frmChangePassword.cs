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
        private int _UserID;
        public frmChangePassword(int PersonID, int userID)
        {
            InitializeComponent();
            ctrlUserCard1.LoadUserInfo(PersonID);
            _UserID = userID;

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (clsUser.ChangePassword(_UserID, txtNewPassword.Text))
                MessageBox.Show("Password has changed successfully", "Password Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Password has NOT changed..", "Password Change", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!clsUser.IsPasswordMatch(_UserID, txtCurrentPassword.Text))
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
            txtCurrentPassword.Focus();
        }
    }
}