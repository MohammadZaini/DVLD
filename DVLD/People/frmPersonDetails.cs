﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Controls;
using DVLD_Business;


namespace DVLD.People
{
    public partial class frmPersonDetails : Form
    {

        public int  _PersonID;
        public frmPersonDetails(int personID)
        {
            InitializeComponent();
            CenterToScreen();

            ctrlPersonCard1.LoadPersonData(personID);
        }

        public frmPersonDetails(string nationalNo)
        {
            InitializeComponent();
            CenterToScreen();

            ctrlPersonCard1.LoadPersonData(nationalNo);
        }

    }
}
