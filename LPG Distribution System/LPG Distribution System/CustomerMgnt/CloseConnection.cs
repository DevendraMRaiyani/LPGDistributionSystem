using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
                        PrintCloseCustomerReciept(customer);

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

        public void PrintCloseCustomerReciept(CustomerMgntRef.Customer customer)
        {
                Distributor dis = new Distributor();
                string disName = dis.AgencyName;
                string disCode = dis.code.ToString();
                string addr = dis.Address + ", " + dis.City + "\n" + "District: " + dis.District + ", State: " + dis.State;
                string phon = dis.ContectNo;
                string gstin = dis.GSTIN;
            
                try
                {
                    #region Page
                    #region Section-1

                    iTextSharp.text.Font fontH1 = new iTextSharp.text.Font(BaseFont.CreateFont(
                            BaseFont.TIMES_ROMAN,
                            BaseFont.CP1252,
                            BaseFont.EMBEDDED), 8, iTextSharp.text.Font.NORMAL);

                    var boldFont = FontFactory.GetFont(FontFactory.TIMES_BOLD, 8);

                    PdfPTable pdfTable1 = new PdfPTable(1);//Here 1 is Used For Count of Column
                    PdfPTable pdfTable2 = new PdfPTable(2);
                    PdfPTable pdfTable3 = new PdfPTable(2);

                    pdfTable1.WidthPercentage = 100;
                    pdfTable1.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable1.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfTable1.DefaultCell.BorderWidth = 0;

                    pdfTable2.WidthPercentage = 100;
                    pdfTable2.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    pdfTable2.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfTable2.DefaultCell.BorderWidth = 0;

                    pdfTable3.WidthPercentage = 100;
                    pdfTable3.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    pdfTable3.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    pdfTable3.DefaultCell.BorderWidth = 0.5f;

                    int[] firstTablecellwidth = { 10,90 };
                    pdfTable2.SetWidths(firstTablecellwidth);
                    string imageURL = @"./../Debug/Bill/logo.png";
                    iTextSharp.text.Image myImage = iTextSharp.text.Image.GetInstance(imageURL);
                    myImage.ScaleToFit(38f, 38f);
                    myImage.SpacingBefore = 5f;
                    myImage.SpacingAfter = 10f;
                
                    PdfPCell cellWithRowspan = new PdfPCell(myImage);
                    cellWithRowspan.BorderWidth = 0.0f;
                    cellWithRowspan.Rowspan = 3;
                    cellWithRowspan.HorizontalAlignment = Element.ALIGN_RIGHT;
                    pdfTable2.AddCell(cellWithRowspan);
                
                    Chunk c1 = new Chunk(disName + " (" + disCode + ") ", FontFactory.GetFont("Times New Roman"));
                    c1.Font.Color = new iTextSharp.text.BaseColor(255, 0, 0);
                    c1.Font.SetStyle(0);
                    c1.Font.Size = 10;
                    Phrase p1 = new Phrase();
                    p1.Add(c1);
                    pdfTable2.AddCell(p1);
                    
                    Chunk c2 = new Chunk(addr, FontFactory.GetFont("Times New Roman"));
                    c2.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                    c2.Font.SetStyle(0);//0 For Normal Font
                    c2.Font.Size = 8;
                    Phrase p2 = new Phrase();
                    p2.Add(c2);
                    pdfTable2.AddCell(p2);
                    //pdfTable2.AddCell(p2);

                    Chunk c3 = new Chunk("Phone No : " + phon + " | GSTIN : " + gstin + "\n\n", FontFactory.GetFont("Times New Roman"));
                    c3.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                    c3.Font.SetStyle(0);
                    c3.Font.Size = 8;
                    Phrase p3 = new Phrase();
                    p3.Add(c3);
                    pdfTable2.AddCell(p3);
                //pdfTable2.AddCell(p3);
                #endregion


                #region section Table

                pdfTable1.AddCell(new Phrase("Close Connetion Reciept"));

                pdfTable3.AddCell(new Phrase(""));

                pdfTable3.AddCell(new Phrase("Customer Id: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.CustomerId+"", fontH1));
                
                pdfTable3.AddCell(new Phrase("Customer Name: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.CustomerName + "", fontH1));

                pdfTable3.AddCell(new Phrase("Gender: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.Gender + "", fontH1));

                pdfTable3.AddCell(new Phrase("Customer Type: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.CustomerType + "", fontH1));

                pdfTable3.AddCell(new Phrase("Adhar Number: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.AadharNo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Ration Card Number: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.RashanCardNo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Address: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.Address + "", fontH1));

                pdfTable3.AddCell(new Phrase("City/Village: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.City + "", fontH1));

                pdfTable3.AddCell(new Phrase("Taluka: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.Taluka + "", fontH1));

                pdfTable3.AddCell(new Phrase("District: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.District + "", fontH1));

                pdfTable3.AddCell(new Phrase("State: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.State + "", fontH1));

                pdfTable3.AddCell(new Phrase("PIN No.: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.PinNo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Contect No.: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.ContactNo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Email: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.Email + "", fontH1));

                pdfTable3.AddCell(new Phrase("Bank IFSC: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.BankIFSC + "", fontH1));

                pdfTable3.AddCell(new Phrase("Bank Account No.: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.BankAccountNo + "", fontH1));

                var cell = new PdfPCell(new Phrase("\n'Your Safety is our Priority' In case of Emergency Call 1906 For any complaints / Queries call: 1800 - 233 - 3555(Toll Free)", fontH1));
                cell.Colspan = 2;
                pdfTable3.AddCell(cell);

                var cell1 = new PdfPCell(new Phrase("\n\n"+dis.AgencyName+"\n" + "\t\t" + "Signature & Sign of Distributor", fontH1));
                var cell2 = new PdfPCell(new Phrase("\n\n\n" + "\t\t" + "Signature of Customer", fontH1));
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable3.AddCell(cell1);
                pdfTable3.AddCell(cell2);
                #endregion

                #endregion


                SaveFileDialog sfd = new SaveFileDialog();
                    sfd.InitialDirectory = @"C:\";
                    sfd.RestoreDirectory = true;
                    sfd.FileName = "CloseCust";
                    sfd.DefaultExt = "pdf";
                    sfd.Filter = "Pdf Files|*.pdf";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string path = sfd.FileName;
                        using (FileStream stream = new FileStream(path, FileMode.Create))
                        {
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            #region PAGE-1
                            pdfDoc.Add(pdfTable2);
                            pdfDoc.Add(pdfTable1);
                            pdfDoc.Add(pdfTable3);
                            pdfDoc.NewPage();
                            #endregion

                            pdfDoc.Close();
                            stream.Close();
                        }
                        MessageBox.Show("Close Connetion Reciept is generated successfully!!!");
                    }

                }
                catch (Exception ex)
                {
                    throw;
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
        }*/
    }
}
