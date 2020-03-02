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
    public partial class ChangeCustomerDetails : Form
    {
        public ChangeCustomerDetails()
        {
            InitializeComponent();
        }
        string[] customersNames = null;
        string selectedCname = null;
        int cid = 0;
        CustomerMgntRef.Customer customer = null;

        private void ChangeCustomerDetails_Load(object sender, EventArgs e)
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
            string gen = customer.Gender;
            if (gen.Equals(radioButton1.Text))
                radioButton1.Checked = true;
            else if (gen.Equals(radioButton2.Text))
                radioButton2.Checked = true;
            else if (gen.Equals(radioButton3.Text))
                radioButton3.Checked = true;

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
            if (textBox1.Text != null)
            {
                string value = "";
                bool isChecked1 = radioButton1.Checked;
                bool isChecked2 = radioButton2.Checked;
                bool isChecked3 = radioButton3.Checked;
                if (isChecked1)
                    value = radioButton1.Text;
                else if (isChecked2)
                    value = radioButton2.Text;
                else if (isChecked3)
                    value = radioButton3.Text;
                
                customer.CustomerId = this.customer.CustomerId;
                this.customer.CustomerName = textBox1.Text;
                customer.CustomerType = textBox16.Text;
                customer.AadharNo = textBox3.Text;
                customer.RashanCardNo = textBox14.Text;
                customer.Gender = value;
                customer.City = textBox4.Text;
                customer.PinNo = int.Parse(textBox15.Text);
                customer.Taluka = textBox5.Text;
                customer.District = textBox6.Text;
                customer.State = textBox7.Text;
                customer.ContactNo = textBox8.Text;
                customer.Email = textBox9.Text;
                customer.BankIFSC = textBox10.Text;
                customer.BankAccountNo = textBox11.Text;
                customer.Address = textBox13.Text;

                using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
                {
                    var msg = client.UpdateCustomer(customer);
                    if (msg.Equals("OK"))
                    {
                        MessageBox.Show("Success Fully Upadted Customer Details!!");
                        textBox1.Text = null;
                        textBox2.Text = null;
                        textBox3.Text = null;
                        textBox4.Text = null;
                        textBox5.Text = null;
                        textBox6.Text = null;
                        textBox7.Text = null;
                        textBox8.Text = null;
                        textBox9.Text = null;
                        textBox10.Text = null;
                        textBox11.Text = null;
                        textBox13.Text = null;
                        textBox14.Text = null;
                        textBox15.Text = null;
                        radioButton1.Checked = false;
                        radioButton2.Checked = false;
                        radioButton3.Checked = false;
                    }
                    else
                        MessageBox.Show("Something went wrong!!!");
                    //PrintNewCustomerReciept(customer);
                }
            }
            else
                MessageBox.Show("Fild must be filled!!");
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
