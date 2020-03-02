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
    public partial class CloseConnection : Form
    {
        string[] customersNames = null;
        string selectedCname = null;
        int cid = 0;
        CustomerMgntRef.Customer customer = null;

        public CloseConnection()
        {
            InitializeComponent();
        }

        private void RemoveCustomer_Load(object sender, EventArgs e)
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
            for (int i = 0; i < customersNames.Length; i++)
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
            textBox12.Text = customer.Gender;
            textBox2.Text = customer.CustomerName;
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
            if (cid > 0)
            {
                var confirmResult = MessageBox.Show("Are you sure to close this connection ??", "", MessageBoxButtons.YesNo);
                if (confirmResult == DialogResult.Yes)
                {
                    if (MessageBox.Show("About to close connection?", "", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient();
                        var msg = client.DeleteCustomer(cid);
                        MessageBox.Show("Connection is Closed Successfully!!!");
                        textBox12.Text = "";
                        textBox2.Text = "";
                        textBox16.Text = "";
                        textBox3.Text = "";
                        textBox14.Text = "";
                        textBox13.Text = "";
                        textBox4.Text = "";
                        textBox15.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox8.Text = "";
                        textBox9.Text = "";
                        textBox10.Text = "";
                        textBox11.Text = "";
                    }
                    else
                    {
                        // If 'Cancel', do something here.
                    }
                }
                else 
                { 
                    // If 'No', do something here. 
                }
            }
            else
                MessageBox.Show("Something went wrong!!!");
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
        }*/
    }
}
