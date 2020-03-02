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
    }
}
