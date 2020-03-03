using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime from = dateTimePicker1.Value;
            DateTime to = dateTimePicker2.Value;
            
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
                groups1 = result1.Where(x => x.CashMemoNo != 0).AsEnumerable().GroupBy(g => g.Details).Select(g => new { Details = g.Key, Price = Math.Round(g.Sum(s=>s.Price)/g.Count(),2),Quentity = g.Sum(s=>s.Quentity), Total = g.Sum(s=>s.Total) });
                dataGridView4.DataSource = groups1.ToList();
               
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
