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
    public partial class AddCylinder : Form
    {
        List<StockMgntRef.Cylinder> cylinders = null;
        int fQty = 0, eQty = 0;
        string selectedEType="", selectedFType="";
        public AddCylinder()
        {
            InitializeComponent();
        }

        private void AddCylinder_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                dataGridView1.DataSource = client.GetCylinders();
                cylinders = client.GetCylinders().ToList();
                string[] cyTypes = cylinders.Where(x=>x.Price!=0).Select(x => x.CylinderType).ToArray();
                comboBox1.Items.AddRange(cyTypes);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedtype = cylinders.Where(x => x.CylinderType == comboBox1.Text).Select(x => x.Quentity);
            string result = string.Join(",", selectedtype);
            fQty= int.Parse(result);

            selectedFType = string.Join(",", cylinders.Where(x => x.CylinderType == comboBox1.Text).Select(x => x.CylinderType));
            string[] spearator = { "(F)" };
            selectedEType = string.Join(",", selectedFType.Split(spearator, StringSplitOptions.RemoveEmptyEntries)) + "(E)";
            string streqty = string.Join(",", cylinders.Where(x => x.CylinderType == selectedEType).Select(x => x.Quentity));
            eQty = int.Parse(streqty);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            int imageWidth,tbQty = 0;
            if (Int32.TryParse(textBox1.Text, out imageWidth))
            {
                tbQty = imageWidth;
            }
            if (tbQty > eQty)
            {
                label3.Text = "You have not suficient empty cylinder stock!!";
                dataGridView1.DataSource = cylinders;
                button1.Enabled = false;
            }
            else
            {
                label3.Text = " ";
                button1.Enabled = true;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if ((string)dr.Cells[0].Value == selectedEType)
                    {
                        dr.Cells[1].Value = eQty-tbQty;
                    }
                    if ((string)dr.Cells[0].Value == selectedFType)
                    {
                        dr.Cells[1].Value = fQty+tbQty;
                        break;
                    }
                    //pl.Quentity = (int)dr.Cells[2].Value;
                }
            }
            //label3.Text = "You cant Remove Cylinders " + fQty + " " + eQty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
            TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
            var fr = client.SetFCylinders(selectedFType, int.Parse(textBox1.Text));
            var er = client.SetECylinders(selectedEType, int.Parse(textBox1.Text));
            var msg1 = client1.AddCylenderTx(comboBox1.Text, int.Parse(textBox1.Text));
            if (fr.Equals("OKF") && er.Equals("OKE") && msg1.Equals("OK"))
                MessageBox.Show("Stock is Updated");
            cylinders = client.GetCylinders().ToList();
            dataGridView1.DataSource = cylinders;
            textBox1.Text = "";
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
