using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Controls;
using DVLD_Business;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void _LoadPeople() {
            dgvPeopleList.DataSource = clsPerson.ListPeople();
            lblRecordsCount.Text = dgvPeopleList.RowCount.ToString();
            cbFilters.SelectedIndex = 0;
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            _LoadPeople();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.ShowDialog();
            _LoadPeople();       
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilters.SelectedIndex != 0)
                txtFilter.Visible = true;
            else
                txtFilter.Visible = false;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddPerson_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeopleList.CurrentRow.Cells[0].Value;
            DialogResult result = MessageBox.Show($"Are you sure you want to delete Person[{PersonID}]", "Confirmation",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);

            if (result == DialogResult.OK) {
                try {

                    if (clsPerson.DeletePerson(PersonID))
                    {
                        MessageBox.Show("Person has been deleted successfully");
                        _LoadPeople();
                    }
                    else
                        MessageBox.Show("Something went wrong");
                } 
                catch {
                    MessageBox.Show("Person cannot be deleted because some data is connected to it!", "Person Deletion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }                                                 
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            DataTable FilteredData = clsPerson.Filter(txtFilter.Text, cbFilters.Text.Replace(" ", ""));
            dgvPeopleList.DataSource = FilteredData;
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilters.Text == "Person ID") 
                if (clsUtility.IsDigit(e.KeyChar))               
                    e.Handled = true; // Prevent non-digit characters from being entered          
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sending Email is not implemented yet");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Phone Call is not implemented yet");

        }
    }
}