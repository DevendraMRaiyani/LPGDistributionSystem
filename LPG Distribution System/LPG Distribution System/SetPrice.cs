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
    public partial class SetPrice : Form
    {
        List<StockMgntRef.Cylinder> cylinders = null;
        List<StockMgntRef.Stove> stoves = null;
        StockMgntRef.Stove selectedStove = null;
        StockMgntRef.Cylinder selectedCy = null;
        public SetPrice()
        {
            InitializeComponent();
        }


        private void SetPrice_Load(object sender, EventArgs e)
        {
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                cylinders = client.GetCylinders().ToList();
                string[] cyTypes = cylinders.Where(x => x.Price != 0).Select(x => x.CylinderType).ToArray();
                comboBox1.Items.AddRange(cyTypes);

                stoves = client.GetStoves().ToList();
                string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
                comboBox2.Items.AddRange(stoveTypes);

                var reg = client.GetRegulators();
                textBox3.Text = reg.Price.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double imageWidth, cPrice = 0;
            if (double.TryParse(textBox1.Text, out imageWidth))
            {
                cPrice = imageWidth;
            }
            if (cPrice > 0)
            {
                StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
                var msg = client.SetCylPrice(selectedCy.CylinderType, cPrice);
                if (msg.Equals("OK"))
                    MessageBox.Show("Successfully Updated Price of Product '" + selectedCy.CylinderType + "' . New price is " + cPrice + " Rs.");
                cylinders = client.GetCylinders().ToList();
            }
            else
            {
                textBox2.Text = cPrice.ToString();
                MessageBox.Show("Invalid Price!!!");
            }
            textBox1.Text = "";
            comboBox1.Items.Clear();
            string[] cyTypes = cylinders.Where(x => x.Price != 0).Select(x => x.CylinderType).ToArray();
            comboBox1.Items.AddRange(cyTypes);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double imageWidth, sPrice = 0;
            if (double.TryParse(textBox2.Text, out imageWidth))
            {
                sPrice = imageWidth;
            }
            if (sPrice > 0)
            {
                StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
                var msg = client.SetStovePrice(selectedStove.Product, sPrice);
                if (msg.Equals("OK"))
                    MessageBox.Show("Successfully Updated Price of Product '"+selectedStove.Product +"' . New price is " + sPrice + " Rs.");
                stoves = client.GetStoves().ToList();
            }
            else
            {
                textBox2.Text = sPrice.ToString();
                MessageBox.Show("Invalid Price!!!");
            }
            textBox2.Text = "";
            comboBox2.Items.Clear();
            string[] stoveTypes = stoves.Where(x => x.Price != 0).Select(x => x.Product).ToArray();
            comboBox2.Items.AddRange(stoveTypes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double imageWidth,rPrice=0;
            if (double.TryParse(textBox3.Text, out imageWidth))
            {
                rPrice = imageWidth;
            }
            if (rPrice > 0)
            {
                StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();
                var msg = client.SetRegPrice(rPrice);
                if(msg.Equals("OK"))
                    MessageBox.Show("Successfully Updated Price of Regulators. New price is "+rPrice+" Rs.");
            }
            else
            {
                textBox3.Text = rPrice.ToString();
                MessageBox.Show("Invalid Price!!!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCy = cylinders.Where(x => x.CylinderType == comboBox1.Text).FirstOrDefault();
            if(selectedCy!=null)
                textBox1.Text = selectedCy.Price.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedStove = stoves.Where(x => x.Product == comboBox2.Text).FirstOrDefault();
            if (selectedStove != null)
                textBox2.Text = selectedStove.Price.ToString();
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
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
