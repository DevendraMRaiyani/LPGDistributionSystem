using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Aspose.Pdf;

namespace LPG_Distribution_System
{
    public partial class AddNewCustomer : Form
    {
        public AddNewCustomer()
        {
            InitializeComponent();
        }

        private void AddNewCustomer_Load(object sender, EventArgs e)
        {
            Location = new System.Drawing.Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);

            string[] installs = new string[] {"APL - Single Cylinder", "APL - Double Cylinder", "BPL - Single Cylinder", "BPL - Double Cylinder", "Ujjavala - Single Cylinder", "Industrial" };
            comboBox1.Items.AddRange(installs);
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

        private void button1_Click(object sender, EventArgs e)
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

                CustomerMgntRef.Customer c = new CustomerMgntRef.Customer();
                
                //c.CustomerId = 2;
                c.CustomerName = textBox1.Text + " " + textBox2.Text + " " + textBox12.Text;
                c.CustomerType = comboBox1.Text;
                c.AadharNo = textBox3.Text;
                c.RashanCardNo = textBox14.Text;
                c.Gender = value;
                c.City = textBox4.Text;
                c.PinNo = int.Parse(textBox15.Text);
                c.Taluka = textBox5.Text;
                c.District = textBox6.Text;
                c.State = textBox7.Text;
                c.ContactNo = textBox8.Text;
                c.Email = textBox9.Text;
                c.BankIFSC = textBox10.Text;
                c.BankAccountNo = textBox11.Text;
                c.Address = textBox13.Text;

                using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
                {
                    CustomerMgntRef.Customer customer = client.AddCustomer(c);
                    int id = customer.CustomerId;
                    MessageBox.Show("Success Fully Created New Customer!! Customer Id is: " + id + " Please, Note this Id for future customer interactions.");
                    //PrintNewCustomerReciept(customer);
                }
            }
            else
                MessageBox.Show("Fild must be filled!!");
        }

       /* public void PrintNewCustomerReciept(LPGDSServiceRef.Customer customer)
        {
            Document doc = new Document();
            Page p = doc.Pages.Add();
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Customer ID:" + customer.CustomerId));
            doc.Save(customer.CustomerName + ".pdf");
        }
        */
        /*public void PrintNewCustomerReciept(CustomerMgntRef.Customer c)
        {
            Document doc = new Document();
            Page p = doc.Pages.Add();
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("LPG CYLINDER DISTRIBUTION SYSTEM"));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("--------------------------------"));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Customer Name :-\t\t" + c.CustomerName));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Customer Type :-\t\t" + c.CustomerType + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t Gender :-\t\t" + c.Gender));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Full Address :-\t\t" + c.Address));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Taluka :-\t\t" + c.Taluka + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t City:-\t\t" + c.City));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("District :-\t\t" + c.District + "\t\t\t\t\t\t\t\t\t\t\t\t PinCode :-\t\t" + c.PinNo + "\t\t\t\t\t\t\t\t\t\t\t\t State :-\t\t" + c.State));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Aadhar No. :-\t\t" + c.AadharNo + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t Rashan Card No. :-\t\t" + c.RashanCardNo));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Contact No. :-\t\t" + c.ContactNo + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t Email:-\t\t" + c.Email));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(" "));
            p.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment("Bank Account No. :-\t\t" + c.BankAccountNo + "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t Bank IFSC Code:-\t\t" + c.BankIFSC));

            doc.Save("../../../"+c.CustomerId+ " " + c.CustomerName+".pdf");


        }
        */
        private void button2_Click(object sender, EventArgs e)
        {
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
            textBox12.Text = null;
            textBox13.Text = null;
            textBox14.Text = null;
            textBox15.Text = null;
            comboBox1.Text = null;
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

        }
    }
}
