using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPG_Distribution_System
{
    public partial class home : Form
    {
        public home()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void createCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewCustomer addNewCustomer = new AddNewCustomer();
            addNewCustomer.Show();
        }

        private void changeCustomerDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeCustomerDetails changeCustomerDetails = new ChangeCustomerDetails();
            changeCustomerDetails.Show();
        }

        private void removeCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseConnection removeCustomer = new CloseConnection();
            removeCustomer.Show();
        }

        private void transferCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TransferCustomer transferCustomer = new TransferCustomer();
            transferCustomer.Show();
        }

        private void addCylindersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddCylinder addCylinder = new AddCylinder();
            addCylinder.Show();
        }

        private void addStovesAndRegulatorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddStoveRegulator addStoveRegulator = new AddStoveRegulator();
            addStoveRegulator.Show();
        }

        private void viewStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewStock viewStock = new ViewStock();
            viewStock.Show();
        }

        private void setPriceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetPrice setPrice = new SetPrice();
            setPrice.Show();
        }

        private void home_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("About to exit program?", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    e.Cancel = true;
            }*/
        }

        private void generateBillForStovesAndRegulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BillStoveRegulator billStoveRegulator = new BillStoveRegulator();
            billStoveRegulator.Show();
        }

        private void bookCylinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookCylinder bookCylinder = new BookCylinder();
            bookCylinder.Show();
        }
    }
}
