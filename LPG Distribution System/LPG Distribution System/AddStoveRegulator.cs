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
    public partial class AddStoveRegulator : Form
    {
        List<StockMgntRef.Stove> stoves = null;
        StockMgntRef.Stove selectedStove = null;
        int roldQty=0;
        int soldQty = 0;
        public AddStoveRegulator()
        {
            InitializeComponent();
        }
        private void AddStoveandRegulator_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                dataGridView1.DataSource = client.GetStoves();
                stoves = client.GetStoves().ToList();
                if (stoves.Count>=10)
                {
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                    textBox5.Enabled = false;
                    button3.Enabled = false;
                }
                string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
                comboBox2.Items.AddRange(stoveTypes);
                comboBox1.Items.AddRange(stoveTypes);

                var reg = client.GetRegulators();
                roldQty = reg.Quentity;
                //textBox6.Text = roldQty.ToString();
                label16.Text = roldQty + " piece(s)";
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

        private void button1_Click(object sender, EventArgs e)
        {
            StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
            TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
            int total = int.Parse(textBox1.Text) + soldQty;
            var msg = client.SetStoves(comboBox1.Text,total);
            var msg1 = client1.AddStoveTx(comboBox1.Text, int.Parse(textBox1.Text));
            if (msg.Equals("OK") && msg1.Equals("OK"))
                MessageBox.Show("Your new stock for product '"+comboBox1.Text+"' is "+total);
            textBox1.Text = "";
            stoves = client.GetStoves().ToList();
            comboBox1.DataSource = stoves;
            dataGridView1.DataSource = stoves;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int tbQty = int.Parse(textBox6.Text);
            if (tbQty != roldQty)
            {
                StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
                TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
                var msg1 = client1.AddRegulatorTx(tbQty);
                var msg = client.SetRegulators(tbQty + roldQty);
                if (msg.Equals("OK") && msg1.Equals("OK"))
                    MessageBox.Show("Your new Regulator stock is " + (tbQty + roldQty));
                else
                {
                    MessageBox.Show("Some error occured!!");
                }
            }
            else
                MessageBox.Show("You have not changed Regulator stock!!!");
            textBox6.Text = null;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            StockMgntRef.Stove stove = new StockMgntRef.Stove();
            stove.Product = textBox3.Text;
            stove.Price = double.Parse(textBox5.Text);
            stove.Quentity = int.Parse(textBox4.Text);
            StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
            TransactionMgnt.TransactionMgntClient client1 = new TransactionMgnt.TransactionMgntClient();
            var msg = client.AddStove(stove);
            var msg1 = client1.AddStoveTx(textBox3.Text, int.Parse(textBox4.Text));
            if (msg.Equals("OK") && msg1.Equals("OK"))
                MessageBox.Show("Successfully added new product.");
            dataGridView1.DataSource = client.GetStoves();
            stoves = client.GetStoves().ToList();
            string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(stoveTypes);
            comboBox1.Items.AddRange(stoveTypes);
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
            var msg = client.RemoveStove(comboBox2.Text);
            if (msg.Equals("OK"))
                MessageBox.Show("Successfully removed product.");

            dataGridView1.DataSource = client.GetStoves();
            stoves = client.GetStoves().ToList();
            string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(stoveTypes);
            comboBox1.Items.AddRange(stoveTypes);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox6_KeyUp(object sender, KeyEventArgs e)
        {
            int imageWidth, tbQty = 0;
            if (Int32.TryParse(textBox6.Text, out imageWidth))
            {
                tbQty = imageWidth;
            }
            if (tbQty+ roldQty>499)
            {
                label16.Text = ""+roldQty + " piece(s)";
                label17.Text = "You can not have stock of regulator more than 500 pieces!!";
                button2.Enabled = false;
            }
            else
            {
                label17.Text = null;
                button2.Enabled = true;
                label16.Text = (tbQty + roldQty) + " piece(s)";
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            int imageWidth, newQty = 0;
            if (Int32.TryParse(textBox1.Text, out imageWidth))
            {
                newQty = imageWidth;
            }
            int total = newQty + soldQty;
            if (total > 100)
            {
                textBox1.Text = "";
                MessageBox.Show("Stock for a product must not be more than 100 pieces!!!");
                dataGridView1.DataSource = stoves;
            }
            else
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if ((string)dr.Cells[0].Value == selectedStove.Product)
                    {
                        dr.Cells[1].Value = total;
                    }
                }
            }
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedStove = stoves.Where(x => x.Product == comboBox1.Text).FirstOrDefault();
            if(selectedStove!=null)
                soldQty = selectedStove.Quentity;
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
