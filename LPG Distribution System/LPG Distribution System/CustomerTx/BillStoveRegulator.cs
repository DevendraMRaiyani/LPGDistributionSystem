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
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridView3.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
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
                var gstrates = txClient.GetGSTRates();
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
