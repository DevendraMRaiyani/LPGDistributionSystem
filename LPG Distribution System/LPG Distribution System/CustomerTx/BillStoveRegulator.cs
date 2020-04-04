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
    public partial class BillStoveRegulator : Form
    {
        List<StockMgntRef.Stove> stoves = null;
        public BillStoveRegulator()
        {
            InitializeComponent();
        }
        string[] customersNames = null;
        private void BillStoveRegulator_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            dataGridView3.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                stoves = client.GetStoves().ToList();
                List<StockMgntRef.Stove> stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x).ToList();
                dataGridView2.DataSource = stoveTypes;
            }
            using (CustomerMgntRef.CustomerMgntClient client1 = new CustomerMgntRef.CustomerMgntClient())
            {
                customersNames = client1.GetCustomersName();
                AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
                acsc.AddRange(customersNames);
                textBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox1.AutoCompleteCustomSource = acsc;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
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
        private void button1_Click(object sender, EventArgs e)
        {
            string rowcontent = "";
            int f = 1;
            foreach (DataGridViewRow row in dataGridView2.SelectedRows)
            {
                rowcontent = row.Cells[0].Value.ToString();

                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (row1.Cells[0].Value != null)
                    {
                        if (row1.Cells[0].Value.Equals(rowcontent))
                        {
                            f = 0;
                            break;
                        }
                        else
                            f = 1;
                    }
                }
                if (f == 1)
                {
                    DataGridViewRow nrow = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    nrow.Cells[0].Value = rowcontent;
                    nrow.Cells[1].Value = 0;
                    dataGridView1.Rows.Add(nrow);
                }
                f = 1;
            }
            dataGridView1.Sort(dataGridView1.Columns["Product"], ListSortDirection.Descending);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row1 in dataGridView1.SelectedRows)
            {
                if(row1.Cells[0].Value!=null)
                    dataGridView1.Rows.Remove(row1);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string cname = textBox1.Text.Trim();
            int cmnum = 0;
            string product = "";
            int qty = 0;
            if (cname.Length>0)
            {
                int imageWidth, tbQty = 0;
                if (Int32.TryParse(textBox2.Text, out imageWidth))
                {
                    tbQty = imageWidth;
                }
                if (tbQty > 0)
                {
                    StockMgntRef.StockMgntClient stockMgntClient = new StockMgntRef.StockMgntClient();
                    int regStock=stockMgntClient.GetRegulators().Quentity;
                    if (regStock>=tbQty)
                    {
                        TransactionMgnt.TransactionMgntClient client = new TransactionMgnt.TransactionMgntClient();
                        cmnum = client.RegulatorTx(cname, tbQty);
                        //PrintBill();
                    }
                    else
                        MessageBox.Show("Not Enough Stock of Regulators!!!");
                }
                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (row1.Cells[0].Value != null)
                    {
                        product = row1.Cells[0].Value.ToString();
                        if (int.Parse(row1.Cells[1].Value.ToString()) > 0)
                        {
                            int stoveStock = stoves.Where(x => x.Product.Equals(product)).FirstOrDefault().Quentity;
                            if (stoveStock >= qty)
                            {
                                qty = int.Parse(row1.Cells[1].Value.ToString());
                                TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
                                cmnum = client1.StoveTx(cname, product, qty, cmnum);
                            }
                            else
                                MessageBox.Show("Not Enough Stock of Stove '"+product +"' !!!");
                        }
                    }
                }
                if (cmnum > 0)
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    dataGridView1.Rows.Clear();
                    MessageBox.Show("Transaction is done Successfully !!! \nCashmemo Number is " + cmnum);
                }

                TransactionMgnt.TransactionMgntClient txClient = new TransactionMgnt.TransactionMgntClient();
                List<TransactionMgnt.GSTRates> gstrates = txClient.GetGSTRates().ToList();

                int f = 0;
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.Cells[1].Value != null)
                    {
                        if (int.Parse(r.Cells[1].Value.ToString()) != 0)
                        {
                            f = 1;
                            break;
                        }
                        else
                        {
                            f = 0;
                        }
                    }
                }
                if (tbQty > 0)
                    f = 1;
                if (f == 1)
                {
                    //MessageBox.Show("In Print");
                    PrintReceipt(cname, cmnum, stoves, dataGridView3, gstrates);
                }
                else
                {
                    MessageBox.Show("Can't Generate Invoice for zero Quentity !!!");
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Clear();
            string cname = textBox1.Text.Trim();
            double totalAmt = 0;
            string product = "";
            int qty = 0;
            if (cname.Length > 0)
            {
                int imageWidth, tbQty = 0;
                if (Int32.TryParse(textBox2.Text, out imageWidth))
                {
                    tbQty = imageWidth;
                }
                TransactionMgnt.TransactionMgntClient txClient = new TransactionMgnt.TransactionMgntClient();
                List<TransactionMgnt.GSTRates> gstrates = txClient.GetGSTRates().ToList();
                if (tbQty > 0)
                {
                    StockMgntRef.StockMgntClient stockMgntClient = new StockMgntRef.StockMgntClient();
                    var reg = stockMgntClient.GetRegulators();
                    int regStock = reg.Quentity;
                    if (regStock >= tbQty)
                    {
                        double cgst = gstrates.Where(x => x.Comodity.Equals("Regulator")).FirstOrDefault().CGST;
                        double sgst = gstrates.Where(x => x.Comodity.Equals("Regulator")).FirstOrDefault().SGST;
                        DataGridViewRow nrow = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                        nrow.Cells[0].Value = "Regulator(s)";
                        nrow.Cells[1].Value = tbQty;
                        nrow.Cells[2].Value = reg.Price;
                        nrow.Cells[3].Value = (100+cgst+sgst)*(reg.Price*tbQty)/100 ;
                        totalAmt += double.Parse(nrow.Cells[3].Value.ToString());
                        dataGridView3.Rows.Add(nrow);
                    }
                    else
                        MessageBox.Show("Not Enough Stock of Regulators!!!");
                }

                foreach (DataGridViewRow row1 in dataGridView1.Rows)
                {
                    if (row1.Cells[0].Value != null)
                    {
                        product = row1.Cells[0].Value.ToString();
                        if (int.Parse(row1.Cells[1].Value.ToString()) > 0)
                        {
                            var selectedStove = stoves.Where(x => x.Product.Equals(product)).FirstOrDefault();
                            if (selectedStove.Quentity >= qty)
                            {
                                qty = int.Parse(row1.Cells[1].Value.ToString());
                                double cgst = gstrates.Where(x => x.Comodity.Equals("Stove")).FirstOrDefault().CGST;
                                double sgst = gstrates.Where(x => x.Comodity.Equals("Stove")).FirstOrDefault().SGST;
                                DataGridViewRow nrow = (DataGridViewRow)dataGridView3.Rows[0].Clone();
                                nrow.Cells[0].Value = product;
                                nrow.Cells[1].Value = qty;
                                nrow.Cells[2].Value = selectedStove.Price;
                                nrow.Cells[3].Value = (100 + cgst + sgst) * (selectedStove.Price * qty) / 100;
                                totalAmt += double.Parse(nrow.Cells[3].Value.ToString());
                                dataGridView3.Rows.Add(nrow);
                            }
                            else
                                MessageBox.Show("Not Enough Stock of Stove '" + product + "' !!!");
                        }
                    }
                }
                label7.Text = totalAmt.ToString();
                
            }
        }

        private static void PrintReceipt(string cname,int cmnum, List<StockMgntRef.Stove> stoves, DataGridView dataGridView3, List<TransactionMgnt.GSTRates> gstrates)
        {
            Distributor dis = new Distributor();
            string disName = dis.AgencyName;
            string disCode = dis.code.ToString();
            string addr = dis.Address + ", \n" + dis.City + " District: " + dis.District + ", State: " + dis.State;
            string phon = dis.ContectNo;
            string gstin = dis.GSTIN;

            int cashmemo = cmnum;
            int custNo = 15;
            string custName = cname;
            string date = DateTime.Now.ToShortDateString();
            string custoAddr = "-";
            
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

                double totalAmt = 0;
                PdfPTable pdfTable = new PdfPTable(4);
                pdfTable.DefaultCell.Padding = 3;
                pdfTable.WidthPercentage = 100;
                pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
                int[] cellWidth = { 40,10,25,25 };
                pdfTable.SetWidths(cellWidth);
                foreach (DataGridViewColumn column in dataGridView3.Columns)
                {
                    if(column.HeaderText.Equals("Quentity"))
                        pdfTable.AddCell(new Phrase("Qty", boldFont));
                    else
                        pdfTable.AddCell(new Phrase(column.HeaderText, boldFont));
                }
                foreach (DataGridViewRow row in dataGridView3.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        pdfTable.AddCell(new Phrase(row.Cells[0].Value.ToString(), fontH1));
                        pdfTable.AddCell(new Phrase(row.Cells[1].Value.ToString(), fontH1));
                        pdfTable.AddCell(new Phrase(row.Cells[2].Value.ToString(), fontH1));
                        double temp = double.Parse(row.Cells[1].Value.ToString()) * double.Parse(row.Cells[2].Value.ToString());
                        totalAmt += temp;
                        pdfTable.AddCell(new Phrase(temp+"", fontH1));
                    }
                }
                PdfPCell tmwot = new PdfPCell(new Phrase("Total Amount(without taxes) :", fontH1));
                tmwot.Colspan = 3;
                pdfTable.AddCell(tmwot);
                pdfTable.AddCell(new Phrase(totalAmt.ToString(), fontH1));
                
                double cgst = gstrates.Where(x => x.Comodity.Equals("Stove")).FirstOrDefault().CGST;
                double sgst = gstrates.Where(x => x.Comodity.Equals("Stove")).FirstOrDefault().SGST;
                PdfPCell cgstCell = new PdfPCell(new Phrase("CGST :", fontH1));
                cgstCell.Colspan = 3;
                pdfTable.AddCell(cgstCell);
                pdfTable.AddCell(new Phrase((totalAmt * cgst / 100).ToString(), fontH1));

                PdfPCell sgstCell = new PdfPCell(new Phrase("SGST :", fontH1));
                sgstCell.Colspan = 3;
                pdfTable.AddCell(sgstCell);
                pdfTable.AddCell(new Phrase((totalAmt * sgst / 100).ToString(), fontH1));

                PdfPCell tmwt = new PdfPCell(new Phrase("Total Amount(with taxes) :", boldFont));
                tmwt.Colspan = 3;
                pdfTable.AddCell(tmwt);
                pdfTable.AddCell(new Phrase(((100 + cgst + sgst) * totalAmt / 100).ToString(), boldFont));
 
                PdfPCell tc = new PdfPCell(pdfTable);
                tc.Colspan = 2;
                pdfTable3.AddCell(tc);
                pdfTable3.AddCell(tc);
                

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
        }*/
    }
}
