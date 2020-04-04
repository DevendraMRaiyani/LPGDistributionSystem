using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

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
            if (selectedCname != null)
            {
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
                TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
                List<TransactionMgnt.TxCylinder> res = client1.GetLastTxs(customer.CustomerId).ToList();
                dataGridView1.DataSource = res;
                if (res.FirstOrDefault().TxDate.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                    button2.Visible = false;
            }
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
                    TransactionMgnt.TxCylinder txCylinder = client.BookingCylinderTx(cid,tbQty);
                    if (txCylinder.TxId > 0)
                    {
                        MessageBox.Show("Cylinder is Booked Successfully !!! \nCashmemo Number is "+ txCylinder.CashMemoNo);
                        //PrintBill(txCylinder);
                        PrintReceipt(txCylinder,customer);
                        button2.Visible = false;
                        TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
                        List<TransactionMgnt.TxCylinder> res = client1.GetLastTxs(customer.CustomerId).ToList();
                        dataGridView1.DataSource = res;
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

        private static void PrintReceipt(TransactionMgnt.TxCylinder tx, CustomerMgntRef.Customer c)
        {
            Distributor dis = new Distributor();
            string disName = dis.AgencyName;
            string disCode = dis.code.ToString();
            string addr = dis.Address+", "+dis.City+"\n"+ "District: "+dis.District+", State: "+dis.State;
            string phon = dis.ContectNo;
            string gstin = dis.GSTIN;

            int cashmemo = tx.CashMemoNo;
            int custNo = tx.CustomerId;
            string custName = tx.CustomerName;
            string date = tx.TxDate.ToShortDateString();
            string custoAddr = c.Address+", "+c.City+ "\nDistrict: " + c.District+", State: "+c.State+"-"+c.PinNo;

            string prod = tx.CylinderDetails;
            int qty = tx.Quentity;
            double price = tx.Price;
            double amount = tx.Amount;
            double cgst = tx.CGST;
            double sgst = tx.SGST;
            double totle = tx.Total;


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
                PdfPTable pdfTable2 = new PdfPTable(4);
                PdfPTable pdfTable3 = new PdfPTable(4);

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

                int[] firstTablecellwidth = { 7, 43, 7, 43 };
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
                pdfTable2.AddCell(cellWithRowspan);
                Chunk c11 = new Chunk(disName + " (" + disCode + ") ", FontFactory.GetFont("Times New Roman"));
                c11.Font.Color = new iTextSharp.text.BaseColor(0, 0, 255);
                c11.Font.SetStyle(0);
                c11.Font.Size = 10;
                Phrase p11 = new Phrase();
                p11.Add(c11);
                pdfTable2.AddCell(p11);

                Chunk c2 = new Chunk(addr, FontFactory.GetFont("Times New Roman"));
                c2.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c2.Font.SetStyle(0);//0 For Normal Font
                c2.Font.Size = 8;
                Phrase p2 = new Phrase();
                p2.Add(c2);
                pdfTable2.AddCell(p2);
                pdfTable2.AddCell(p2);

                Chunk c3 = new Chunk("Phone No : " + phon + " | GSTIN : " + gstin + "\n\n", FontFactory.GetFont("Times New Roman"));
                c3.Font.Color = new iTextSharp.text.BaseColor(0, 0, 0);
                c3.Font.SetStyle(0);
                c3.Font.Size = 8;
                Phrase p3 = new Phrase();
                p3.Add(c3);
                pdfTable2.AddCell(p3);
                pdfTable2.AddCell(p3);
                #endregion


                #region section Table

                pdfTable3.AddCell(new Phrase("Cashmemo No.: ", fontH1));
                pdfTable3.AddCell(new Phrase(cashmemo + "", fontH1));
                pdfTable3.AddCell(new Phrase("Cashmemo No.: ", fontH1));
                pdfTable3.AddCell(new Phrase(cashmemo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Customer ID: ", fontH1));
                pdfTable3.AddCell(new Phrase(custNo + "", fontH1));
                pdfTable3.AddCell(new Phrase("Customer ID: ", fontH1));
                pdfTable3.AddCell(new Phrase(custNo + "", fontH1));

                pdfTable3.AddCell(new Phrase("Customer Name: ", fontH1));
                pdfTable3.AddCell(new Phrase(custName + "", fontH1));
                pdfTable3.AddCell(new Phrase("Customer Name: ", fontH1));
                pdfTable3.AddCell(new Phrase(custName + "", fontH1));

                pdfTable3.AddCell(new Phrase("Transaction Date: ", fontH1));
                pdfTable3.AddCell(new Phrase(date + "", fontH1));
                pdfTable3.AddCell(new Phrase("Transaction Date: ", fontH1));
                pdfTable3.AddCell(new Phrase(date + "", fontH1));

                pdfTable3.AddCell(new Phrase("Address: ", fontH1));
                pdfTable3.AddCell(new Phrase(custoAddr + "", fontH1));
                pdfTable3.AddCell(new Phrase("Address: ", fontH1));
                pdfTable3.AddCell(new Phrase(custoAddr + "", fontH1));

                pdfTable3.AddCell(new Phrase("Product: ", fontH1));
                pdfTable3.AddCell(new Phrase(prod + "", fontH1));
                pdfTable3.AddCell(new Phrase("Product: ", fontH1));
                pdfTable3.AddCell(new Phrase(prod + "", fontH1));

                pdfTable3.AddCell(new Phrase("Price(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(price + "", fontH1));
                pdfTable3.AddCell(new Phrase("Price(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(price + "", fontH1));

                pdfTable3.AddCell(new Phrase("Quentity: ", fontH1));
                pdfTable3.AddCell(new Phrase(qty + "", fontH1));
                pdfTable3.AddCell(new Phrase("Quentity: ", fontH1));
                pdfTable3.AddCell(new Phrase(qty + "", fontH1));

                pdfTable3.AddCell(new Phrase("Amount (Quentity * Price) (Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(amount + "", fontH1));
                pdfTable3.AddCell(new Phrase("Amount (Quentity * Price) (Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(amount + "", fontH1));

                pdfTable3.AddCell(new Phrase("CGST(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(cgst + "", fontH1));
                pdfTable3.AddCell(new Phrase("CGST(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(cgst + "", fontH1));

                pdfTable3.AddCell(new Phrase("SGST(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(sgst + "", fontH1));
                pdfTable3.AddCell(new Phrase("SGST(Rs.): ", fontH1));
                pdfTable3.AddCell(new Phrase(sgst + "", fontH1));

                pdfTable3.AddCell(new Phrase("Total(Rs.): ", boldFont));
                pdfTable3.AddCell(new Phrase(totle + "", boldFont));
                pdfTable3.AddCell(new Phrase("Total(Rs.): ", boldFont));
                pdfTable3.AddCell(new Phrase(totle + "", boldFont));

                var cell = new PdfPCell(new Phrase("\n\n'Your Safety is our Priority' In case of Emergency Call 1906 \nFor any complaints / Queries call: 1800 - 233 - 3555(Toll Free)", fontH1));
                cell.Colspan = 2;
                pdfTable3.AddCell(cell);

                var cell1 = new PdfPCell(new Phrase("Declaration: I hereby confirm receipt of the filled LPG cylinder in sound condition and that the Cylinder was checked for weight & leakage.", fontH1));
                pdfTable3.AddCell(cell1);
                var cell2 = new PdfPCell(new Phrase("\n\n\n" + "\t\t" + "Signature of Customer", fontH1));
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable3.AddCell(cell2);

                #endregion

                #endregion


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = @"C:\";
                sfd.RestoreDirectory = true;
                sfd.FileName = cashmemo.ToString();
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
                        pdfDoc.Add(pdfTable1);
                        pdfDoc.Add(pdfTable2);
                        pdfDoc.Add(pdfTable3);
                        pdfDoc.NewPage();
                        #endregion

                        pdfDoc.Close();
                        stream.Close();
                    }
                    MessageBox.Show("Invoice is generated successfully!!!");
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
        }
        */
    }
}
