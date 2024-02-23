using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Global_Classes;
using DVLD_Business;



namespace DVLD
{
    public partial class frmUserLogin : Form
    {
        public frmUserLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

           string textFilePath = "E:\\C# Full Project\\DVLD\\UsersCredentials.txt";
           
           clsUser User = clsUser.Find(txtUsername.Text);
           
            if (User == null) 
            {
                clsUtil.ShowErrorMessage("Username/Password is incorrect", "Invalid Credentials");
                return;
            }

            if ((User.Username != txtUsername.Text) || (User.Password != txtPassword.Text))
            {
                clsUtil.ShowErrorMessage("Username/Password is incorrect", "Invalid Credentials");
                return;
            }

            if (!User.IsActive)
            {
                clsUtil.ShowErrorMessage("Your account is deactivated, please contact your admin", "Account Deactivated");
                return;
            }

            string credentials = txtUsername.Text + '#' + txtPassword.Text;

            clsGlobalSettings.LoggedInUser = User;

            clsUtility.SaveCredentialsToFile(textFilePath, chkRememberMe.Checked ? credentials : string.Empty);

            _ShowMainForm();
        }

        private void _ShowMainForm() {
            Hide();
            frmMain frmMain = new frmMain(this);
            frmMain.ShowDialog();
        }

        private void frmUserLogin_Load(object sender, EventArgs e)
        {
            string textFilePath = "E:\\C# Full Project\\DVLD\\UsersCredentials.txt";
            List<string> credentials =  clsUtility.FillTextBoxWithCredentials(textFilePath);

            if (credentials.Count > 0) {
                chkRememberMe.Checked = true;
                txtUsername.Text = credentials[0];
                txtPassword.Text = credentials[1];
            }

        }
    }
}