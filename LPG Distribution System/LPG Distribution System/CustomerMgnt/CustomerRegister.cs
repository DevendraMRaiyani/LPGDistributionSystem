using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPG_Distribution_System.CustomerMgnt
{
    public partial class CustomerRegister : Form
    {
        public CustomerRegister()
        {
            InitializeComponent();
        }

        private void CustomerRegister_Load(object sender, EventArgs e)
        {
            Location = new System.Drawing.Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);

            using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
            {
                string[] installs = client.GetCustomersTypes().Distinct().ToArray();
                comboBox1.Items.Add("");
                comboBox1.Items.AddRange(installs);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cyType = comboBox1.Text;
            using (CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient())
            {
                var result = client.GetCustomers(cyType);
                label5.Text = result.Count().ToString();
                dataGridView1.DataSource = result;
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
