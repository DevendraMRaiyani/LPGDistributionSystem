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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string unm = textBox1.Text, pass = textBox2.Text;
            using (MainService.MainServiceClient client = new MainService.MainServiceClient())
            {
                MainService.DistributorUser msg = client.Login(unm, pass);
                if (msg != null)
                {
                    this.Hide();
                    home h = new home();
                    h.Show();
                }
                else
                    label3.Text = "Invalid Username / Password!!!";
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
