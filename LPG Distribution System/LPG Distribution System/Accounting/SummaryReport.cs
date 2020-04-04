using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;

namespace LPG_Distribution_System.Accounting
{
    public partial class SummaryReport : Form
    {
        public SummaryReport()
        {
            InitializeComponent();
        }

        private void SummaryReport_Load(object sender, EventArgs e)
        {
            Location = new System.Drawing.Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime from = dateTimePicker1.Value.Date;
            DateTime to = dateTimePicker2.Value.Date;
            using (AccountingRef.AccountingClient client = new AccountingRef.AccountingClient())
            {
                var result = client.GetTxCylinders(from, to, "");
                var groups = result.Where(x => x.CashMemoNo == 0).AsEnumerable().GroupBy(g => g.CylinderDetails).Select(g => new { CylinderDetails = g.Key, Price = Math.Round(g.Sum(s => s.Price) / g.Count(), 2), Quentity = g.Sum(s => s.Quentity), Total = g.Sum(s => s.Total) });
                dataGridView1.DataSource = groups.ToList();
                groups = result.Where(x => x.CashMemoNo != 0).AsEnumerable().GroupBy(g => g.CylinderDetails).Select(g => new { CylinderDetails = g.Key, Price = Math.Round(g.Sum(s => s.Price) / g.Count(), 2), Quentity = g.Sum(s => s.Quentity), Total = g.Sum(s => s.Total) });
                dataGridView3.DataSource = groups.ToList();

                var result1 = client.GetTxStoveRegulators(from, to, "");
                var groups1 = result1.Where(x => x.CashMemoNo == 0).AsEnumerable().GroupBy(g => g.Details).Select(g => new { Details = g.Key, Price = Math.Round(g.Sum(s => s.Price) / g.Count(), 2), Quentity = g.Sum(s => s.Quentity), Total = g.Sum(s => s.Total) });
                dataGridView2.DataSource = groups1.ToList();
                groups1 = result1.Where(x => x.CashMemoNo != 0).AsEnumerable().GroupBy(g => g.Details).Select(g => new { Details = g.Key, Price = Math.Round(g.Sum(s => s.Price) / g.Count(), 2), Quentity = g.Sum(s => s.Quentity), Total = g.Sum(s => s.Total) });
                dataGridView4.DataSource = groups1.ToList();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            /*Microsoft.Office.Interop.Excel.Application xcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook wb = xcelApp.Application.Workbooks.Add(Type.Missing);
            //Microsoft.Office.Interop.Excel.Worksheet ws1 = wb.ActiveSheet;
            //ws1.Name = "WS1";

            Microsoft.Office.Interop.Excel.Sheets worksheets = wb.Worksheets;

            var xlNewSheet = (Microsoft.Office.Interop.Excel.Worksheet)worksheets.Add(worksheets[1], Type.Missing, Type.Missing, Type.Missing);
            xlNewSheet.Name = "new1sheet";

            for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            {
                xlNewSheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    xlNewSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                }
            }
            xlNewSheet.Columns.AutoFit();

            Microsoft.Office.Interop.Excel.Worksheet ws2 = wb.ActiveSheet;
            ws2.Name = "WS2";
            for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
            {
                ws2.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    ws2.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                }
            }

            ws2.Columns.AutoFit();*/

            // creating Excel Application
            Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            // creating new WorkBook within Excel application
            Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            // creating new Excelsheet in workbook
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

            Sheets xlSheets = null;
            Worksheet xlNewSheet = null;


            xlSheets = workbook.Sheets as Sheets;

            if (dataGridView1.Rows.Count > 0)
            {
                //Select the sheet
                worksheet = workbook.Worksheets[1];
                //Rename the sheet
                worksheet.Name = "Cylinder Purchased";
                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i].EntireRow.Font.Bold = true;
                    worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
                worksheet.Columns.AutoFit();
            }

            if (dataGridView2.Rows.Count > 0)
            {
                // The first argument below inserts the new worksheet as the first one
                xlNewSheet = (Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlNewSheet.Name = "Stove&Regulator Purchased";
                for (int i = 1; i < dataGridView2.Columns.Count + 1; i++)
                {
                    xlNewSheet.Cells[1, i].EntireRow.Font.Bold = true;
                    xlNewSheet.Cells[1, i] = dataGridView2.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView2.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView2.Columns.Count; j++)
                    {
                        xlNewSheet.Cells[i + 2, j + 1] = dataGridView2.Rows[i].Cells[j].Value.ToString();
                    }
                }

                xlNewSheet.Columns.AutoFit();
            }

            if (dataGridView3.Rows.Count > 0)
            {
                // The first argument below inserts the new worksheet as the first one
                xlNewSheet = (Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlNewSheet.Name = "Cylinder Sold";
                for (int i = 1; i < dataGridView3.Columns.Count + 1; i++)
                {
                    xlNewSheet.Cells[1, i].EntireRow.Font.Bold = true;
                    xlNewSheet.Cells[1, i] = dataGridView3.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView3.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView3.Columns.Count; j++)
                    {
                        xlNewSheet.Cells[i + 2, j + 1] = dataGridView3.Rows[i].Cells[j].Value.ToString();
                    }
                }

                xlNewSheet.Columns.AutoFit();
            }

            if (dataGridView4.Rows.Count > 0)
            {
                // The first argument below inserts the new worksheet as the first one
                xlNewSheet = (Worksheet)xlSheets.Add(xlSheets[1], Type.Missing, Type.Missing, Type.Missing);
                xlNewSheet.Name = "Stove&Regulator Sold";
                for (int i = 1; i < dataGridView4.Columns.Count + 1; i++)
                {
                    xlNewSheet.Cells[1, i].EntireRow.Font.Bold = true;
                    xlNewSheet.Cells[1, i] = dataGridView4.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView4.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView4.Columns.Count; j++)
                    {
                        xlNewSheet.Cells[i + 2, j + 1] = dataGridView4.Rows[i].Cells[j].Value.ToString();
                    }
                }

                xlNewSheet.Columns.AutoFit();
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\";
            sfd.RestoreDirectory = true;
            sfd.FileName = "*.xlsx";
            sfd.DefaultExt = "xlsx";
            sfd.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string path = sfd.FileName;
                workbook.SaveAs(path, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                workbook.Close(Type.Missing, Type.Missing, Type.Missing);
                app.UserControl = true;
                app.Quit();
                MessageBox.Show("Report is exported successfully!!!");
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
