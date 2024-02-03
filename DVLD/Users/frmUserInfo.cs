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
    public partial class frmUserInfo : Form
    {
        public frmUserInfo(int PersonID)
        {
            InitializeComponent();
            ctrlUserCard1.LoadUserInfo(PersonID);

            this.StartPosition = FormStartPosition.CenterScreen;
        }


        private void _UpdatedPersonCard(int PersonID) { 
        

        }
    }
}