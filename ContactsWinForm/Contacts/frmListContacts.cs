using ContactsBusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contacts
{
    public partial class frmListContacts : Form
    {
        public frmListContacts()
        {
            InitializeComponent();
        }

        private void _RefreshContactsList()
        {
            dgvAllContacts.DataSource = clsContact.GetAllContacts();
        }

       

        private void dgvAllContacts_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
          //  MessageBox.Show(dgvAllContacts.CurrentRow.Cells[0].Value.ToString());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Convert the value safely instead of casting
            int contactID = Convert.ToInt32(dgvAllContacts.CurrentRow.Cells[0].Value);

            frmAddEditContact frm = new frmAddEditContact(contactID);
            frm.ShowDialog();

            _RefreshContactsList();
        }


        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure a row is actually selected
            if (dgvAllContacts.CurrentRow == null || dgvAllContacts.CurrentRow.Cells[0].Value == null)
            {
                MessageBox.Show("No contact selected to delete.");
                return;
            }

            // Get the ContactID safely
            int contactID;
            try
            {
                contactID = Convert.ToInt32(dgvAllContacts.CurrentRow.Cells[0].Value);
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid Contact ID.");
                return;
            }

            // Ask user for confirmation
            var result = MessageBox.Show(
                $"Are you sure you want to delete contact [{contactID}]?",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.OK)
            {
                // Call the DeleteContact method
                bool deleted = clsContact.DeleteContact(contactID);
                if (deleted)
                {
                    MessageBox.Show("Contact Deleted Successfully.");
                    _RefreshContactsList();
                }
                else
                {
                    MessageBox.Show("Contact could not be deleted.");
                }
            }
        }


        private void frmListContacts_Load(object sender, EventArgs e)
        {

            _RefreshContactsList();


        }

        private void btnAddNewContact_Click(object sender, EventArgs e)
        {
            frmAddEditContact frm = new frmAddEditContact(-1);
            frm.ShowDialog();
            _RefreshContactsList();
        }

        private void dgvAllContacts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
