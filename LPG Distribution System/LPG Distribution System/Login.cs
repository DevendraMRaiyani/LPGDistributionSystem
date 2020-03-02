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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangePassword changePassword = new ChangePassword();
            changePassword.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient();
            CustomerMgntRef.Customer c = new CustomerMgntRef.Customer();
            //query for searching username and password... if found then inside if  or else wrong password !!
            if(textBox1.Text=="user1" && textBox2.Text=="user1")
            {
                home h = new home();
                h.Show();
            }
            else
            {
                
                label3.Text = "Username or Password is Wrong !!";
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
