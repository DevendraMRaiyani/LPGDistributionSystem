using LPG_Distribution_System.Accounting;
using LPG_Distribution_System.CustomerMgnt;
using LPG_Distribution_System.CustomerTx;
using LPG_Distribution_System.File;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (MessageBox.Show("About to exit program?", "Confirm Exit", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                {
                    e.Cancel = true;
                }
            }
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

        private void distributorInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DistributorInfo distributorInfo = new DistributorInfo();
            distributorInfo.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void printBillForCylinderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintBillFroCylinder printBillFroCylinder = new PrintBillFroCylinder();
            printBillFroCylinder.Show();
        }

        private void summaryReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SummaryReport summaryReport = new SummaryReport();
            summaryReport.Show();
        }

        private void cylinderReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CylinderReport cylinderReport = new CylinderReport();
            cylinderReport.Show();
        }

        private void stoveAndRegulatorReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StoveandRegulatorReport stoveandRegulatorReport = new StoveandRegulatorReport();
            stoveandRegulatorReport.Show();
        }

        private void customerRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerRegister customerRegister = new CustomerRegister();
            customerRegister.Show();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("http://google.com/");
            Process.Start(sInfo);
        }
    }
}
