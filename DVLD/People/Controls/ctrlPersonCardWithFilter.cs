using DVLD.Global_Classes;
using DVLD.People;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnPersonSelected;

        // Create a protected method to Raise the event with a parameter

        protected virtual void PersonSelected(int personID) { 
            
            Action<int> handler = OnPersonSelected;

            if (handler == null) return;

            handler(personID);
        }

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public clsPerson SelectedPersonInfo { 
            get { return ctrlPersonCard1.Person;}
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        private bool _showAddPerson = true;
        public bool ShowAddPerson
        {
            get {
                return _showAddPerson;        
            }

            set { 
                _showAddPerson = value;
                btnAddNewPerson.Visible = _showAddPerson;
            }
        }

        private bool _filterEnabled = true;
        public bool FilterEnabled {
        
            get { 
                return  _filterEnabled;
            }
            
            set { 
                _filterEnabled = value; 
                gbFilter.Enabled = _filterEnabled;
            }
        }

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilters.SelectedIndex = 0;
            txtFilter.Visible = true;
        }

        public void LoadUserData(int PersonID) { 
            ctrlPersonCard1.LoadPersonData(PersonID);
            cbFilters.SelectedIndex = 0; // Person ID
            gbFilter.Enabled = false;
            txtFilter.Text = PersonID.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                clsUtil.ShowErrorMessage("Some fields are not valid! Hover over the red icon(s) to see the message", "Failure");
                return;
            }

            _FindPersonNow();

            //if (cbFilters.Text == "Person ID" && txtFilter.Text != string.Empty)
            //{
            //    clsPerson person = clsPerson.Find(Convert.ToInt32(txtFilter.Text));
            //
            //    if (person != null) {
            //
            //        ctrlPersonCard1.LoadPersonData(person.PersonID);
            //        PersonID = person.PersonID;
            //    }
            //    else
            //        MessageBox.Show("No Person found With Person ID " + txtFilter.Text, "Select another person",
            //            MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //else if (cbFilters.Text == "National No" && txtFilter.Text != string.Empty) {
            //
            //    clsPerson person = clsPerson.Find(txtFilter.Text);
            //
            //    if (person != null) {
            //        ctrlPersonCard1.LoadPersonData(person.PersonID);
            //        PersonID = person.PersonID;
            //    }
            //    else
            //        MessageBox.Show("No Person found With National No. " + txtFilter.Text,"Select another person",
            //            MessageBoxButtons.OK,MessageBoxIcon.Error);
            //}
            

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.Text == "Person ID")
            {
                if (clsUtility.IsDigit(e.KeyChar))
                {
                    // Set e.Handled to true to block the character
                    e.Handled = true;
                }
            }
        }

        private void _UpdatePersonCard(int PersonID) {
            cbFilters.SelectedIndex = 0; // Person ID
            txtFilter.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonData(PersonID);
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson addEditPerson = new frmAddEditPerson();
            addEditPerson.DataBack += _UpdatePersonCard; // Subcribe to the event
            addEditPerson.ShowDialog();
        }

        private void _LoadPersonInfo(int personID) {
            cbFilters.SelectedIndex = 0;
            txtFilter.Text = personID.ToString();
            _FindPersonNow();
        }


        private void _FindPersonNow() {
            switch (cbFilters.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonData(int.Parse(txtFilter.Text));
                    break;

                case "National No":
                    ctrlPersonCard1.LoadPersonData(txtFilter.Text);
                    break;

                default:
                    break;
            }

            if (OnPersonSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnPersonSelected(ctrlPersonCard1.PersonID);
        }
    }
}