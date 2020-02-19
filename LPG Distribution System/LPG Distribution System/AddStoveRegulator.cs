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
        int roldQty=0;
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
                string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
                comboBox2.Items.AddRange(stoveTypes);
                comboBox1.Items.AddRange(stoveTypes);
                var reg = client.GetRegulators();
                roldQty = reg[0].Quentity;
                textBox6.Text = roldQty.ToString();
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
            string type = comboBox1.Text;
            int quantity = Int32.Parse(textBox6.Text);
            label1.Location.Offset(10, 10);


        }

        
        private void button4_Click(object sender, EventArgs e)
        {

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
        
        private void button2_Click(object sender, EventArgs e)
        {
            int tbQty = int.Parse(textBox6.Text);
            if (roldQty <= tbQty)
            {
                if (tbQty != roldQty)
                {
                    MessageBox.Show("Your new Regulator stock is "+tbQty);
                }
                else
                    MessageBox.Show("You have not changed Regulator stock!!!");
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
            //int tbQty = int.Parse(textBox2.Text);
            if (roldQty > tbQty)
            {
                label17.Text = "You can not reduce quentity of Regulator!!";
            }
            else
            {
                label17.Text = null;
                label16.Text = (tbQty) + " piece(s)";
            }
        }
    }
}
