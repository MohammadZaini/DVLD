using DVLD.Global_Classes;
using DVLD.People;
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

namespace DVLD.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private int _userID = -1;
        private clsUser _user;
        public ctrlUserCard()
        {
            InitializeComponent();            
        }

        public void LoadUserInfo(int userID) { 
            _userID = userID;
            _user = clsUser.Find(userID);

            if (_user == null)
            {
                clsUtil.ShowErrorMessage("No User Was Found With Person ID = " + userID.ToString() , "Failure");
                return;
            }

            _FillUserInfo();
        }

        private void _FillUserInfo() {
            ctrlPersonCard1.LoadPersonData(_user.PersonID);
            lblUserID.Text = _user.UserID.ToString();
            lblUsername.Text = _user.Username;
            lblIsActive.Text = _user.IsActive ? "Yes" : "No";
        }
    }
}