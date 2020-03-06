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

namespace LPG_Distribution_System.Accounting
{
    public partial class CylinderReport : Form
    {
        public CylinderReport()
        {
            InitializeComponent();
        }

        private void CylinderReport_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);

            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                string[] installs = client.GetCylinders().Where(x=>x.CylinderType.EndsWith("(F)")).Select(x=>x.CylinderType).ToArray();
                comboBox1.Items.Add("");
                comboBox1.Items.AddRange(installs);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            DateTime from = dateTimePicker1.Value;
            DateTime to = dateTimePicker2.Value;
            string cyType = comboBox1.Text;
            using (AccountingRef.AccountingClient client = new AccountingRef.AccountingClient())
            {
                var result = client.GetTxCylinders(from, to, cyType);

                if (!checkBox1.Checked)
                    result = result.Where(x => x.CashMemoNo == 0).ToArray();
                if (!checkBox2.Checked)
                    result = result.Where(x => x.CashMemoNo != 0).ToArray();
                label5.Text = result.Count().ToString();
                result = result.OrderByDescending(x=>x.TxDate).ToArray();
                dataGridView1.DataSource = result;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook wb = xcelApp.Application.Workbooks.Add(Type.Missing);

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    xcelApp.Cells[1, i].EntireRow.Font.Bold = true;
                    xcelApp.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        xcelApp.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString() ;
                    }
                }
               
                xcelApp.Columns.AutoFit();
                
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = @"C:\";
                sfd.RestoreDirectory = true;
                sfd.FileName = "*.xlsx";
                sfd.DefaultExt = "xlsx";
                sfd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    string path = sfd.FileName;
                    wb.SaveAs(path);
                    wb.Close();
                    xcelApp.Quit();
                    MessageBox.Show("Report is exported successfully!!!");
                }
                
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
