using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPG_Distribution_System.File
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            Location = new System.Drawing.Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            textBox1.PasswordChar = '*';
            textBox2.PasswordChar = '*';
            textBox3.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length >= 8 && textBox3.Text.Length >= 8)
            {
                using (MainService.MainServiceClient client = new MainService.MainServiceClient())
                {
                    string oldpass = client.GetPassword(292029);
                    if (oldpass.Equals(textBox1.Text))
                    {
                        if (textBox2.Text.Equals(textBox3.Text))
                        {
                            var msg = client.SetPassword(textBox2.Text, 292029);
                            if (msg.Equals("OK"))
                            {
                                MessageBox.Show("Successfully Updated Password!!!");
                                textBox1.Text = "";
                                textBox2.Text = "";
                                textBox3.Text = "";
                            }
                            else
                                MessageBox.Show("Something went wrong!!!");
                        }
                        else
                            MessageBox.Show("New Password and Conform New Password are not same!!!");
                    }
                    else
                        MessageBox.Show("Old Password is not correct!!!");
                }
            }
            else
                MessageBox.Show("Password must be of minimum 8 charactors!!!");
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
