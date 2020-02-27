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
    public partial class BookCylinder : Form
    {
        public BookCylinder()
        {
            InitializeComponent();
        }
        string[] customersNames = null;
        string selectedCname = null;
        int cid = 0;
        CustomerMgntRef.Customer customer = null;
        private void BookCylinder_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient();
            customersNames = client.GetCustomersName();
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            acsc.AddRange(customersNames);
            textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox1.AutoCompleteCustomSource = acsc;
            button2.Enabled = false;
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = textBox1.Text;
            for(int i=0; i<customersNames.Length;i++)
            {
                if (customersNames[i].Equals(temp))
                {
                    selectedCname = customersNames[i];
                    button2.Enabled = true;
                    break;
                }
            }
            button2.Visible = true;
            using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
            {
                customer = client.GetCustomer(selectedCname);
                cid = customer.CustomerId;
            }
            if (customer != null && customer.CustomerType.Equals("Industrial"))
            {
                label2.Visible = true;
                textBox17.Visible = true;
            }
            textBox2.Text = customer.CustomerName;
            textBox12.Text = customer.Gender;
            textBox16.Text = customer.CustomerType;
            textBox3.Text = customer.AadharNo;
            textBox14.Text = customer.RashanCardNo;
            textBox13.Text = customer.Address;
            textBox4.Text = customer.City;
            textBox15.Text = customer.PinNo.ToString();
            textBox5.Text = customer.Taluka;
            textBox6.Text = customer.District;
            textBox7.Text = customer.State;
            textBox8.Text = customer.ContactNo;
            textBox9.Text = customer.Email;
            textBox10.Text = customer.BankIFSC;
            textBox11.Text = customer.BankAccountNo;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int imageWidth, tbQty = 0;
            if (Int32.TryParse(textBox17.Text, out imageWidth))
            {
                tbQty = imageWidth;
            }
            if (customer.CustomerType.Equals("Industrial") && tbQty == 0)
            {
                MessageBox.Show("Please, Enter Quentity of Cylender!!!");
            }
            else if (customer.CustomerType.Equals("Industrial") && tbQty > 10)
            {
                MessageBox.Show("At a time at most 10 cylinders can be sold!!!");
            }
            else
            {
                button2.Enabled = false;
                using(TransactionMgnt.TransactionMgntClient client = new TransactionMgnt.TransactionMgntClient())
                {
                    int msg = client.BookingCylinderTx(cid,tbQty);
                    if (msg>0)
                    {
                        MessageBox.Show("Cylinder is Booked Successfully !!! \nCashmemo Number is "+msg);
                        button2.Visible = false;
                    }
                }
                
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }


        /*protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_MOVE = 0xF010;

            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }
        */
    }
}
