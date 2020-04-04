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
            using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
            {
                string[] installs = client.GetCustomersTypes().Distinct().ToArray();
                comboBox1.Items.AddRange(installs);
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
                c.DistributorCode = 292029;
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
                    PrintNewCustomerReciept(customer);
                    MessageBox.Show("Success Fully Created New Customer!! Customer Id is: " + id + " Please, Note this Id for future customer interactions.");
                }
            }
            else
                MessageBox.Show("Fild must be filled!!");
        }

        public void PrintNewCustomerReciept(CustomerMgntRef.Customer customer)
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

                int[] firstTablecellwidth = { 10, 90 };
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

                pdfTable1.AddCell(new Phrase("New Connetion Reciept"));

                pdfTable3.AddCell(new Phrase(""));

                pdfTable3.AddCell(new Phrase("Customer Id: ", fontH1));
                pdfTable3.AddCell(new Phrase(customer.CustomerId + "", fontH1));

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

                var cell1 = new PdfPCell(new Phrase("\n\n" + dis.AgencyName + "\n" + "\t\t" + "Signature & Sign of Distributor", fontH1));
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
                    MessageBox.Show("New Connetion Reciept is generated successfully!!!");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox15_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox11_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
