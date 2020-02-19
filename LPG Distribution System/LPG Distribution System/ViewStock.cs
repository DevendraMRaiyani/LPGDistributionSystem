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
    public partial class ViewStock : Form
    {
        public ViewStock()
        {
            InitializeComponent();
        }
        
        private void ViewStock_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                dataGridView1.DataSource = client.GetCylinders();
                dataGridView2.DataSource = client.GetStoves();
                var reg= client.GetRegulators(); 
                label3.Text += reg[0].Quentity + " piece(s)";
            }
            
        }
    }
}
