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
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomerMgntRef.CustomerMgntClient client = new CustomerMgntRef.CustomerMgntClient();
            CustomerMgntRef.Customer c = new CustomerMgntRef.Customer();


            if (textBox2.Text != textBox3.Text)
            {
                label4.Text = "New password & Re-enter new password must be same";
                textBox2.Text = "";
                textBox3.Text = "";


            }
            else
            {
                //query for checking old password is true or not
                if (true)
                {
                    //query for change password
                    Login login = new Login();
                    login.Show();
                }
                else
                {
                    label4.Text = "Old Password is Wrong";
                }
            }
        }
    }
}
