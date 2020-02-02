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
            label3.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text=="admin"  && textBox2.Text=="admin")
            {
                home hm = new home();
                hm.Show();
            }
            else
            {
                label3.Text = "Username or Password is incorrect.";
            }
        }
    }
}
