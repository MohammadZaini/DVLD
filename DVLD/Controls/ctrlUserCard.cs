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
        public ctrlUserCard()
        {
            InitializeComponent();            
        }

        public void LoadUserInfo(int PersonID) { 
            
            ctrlPersonCard1.LoadPersonData(PersonID);

            clsUser User = clsGlobalSettings.LoggedInUser;
            lblUserID.Text = User.UserID.ToString();
            lblUsername.Text = User.Username;

            if (User.IsActive)
                lblIsActive.Text = "Yes";
            else
                lblIsActive.Text = "No";
        }
    }
}
