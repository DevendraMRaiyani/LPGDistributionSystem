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
    public partial class DistributorInfo : Form
    {
        public DistributorInfo()
        {
            InitializeComponent();
        }

        private void DistributorInfo_Load(object sender, EventArgs e)
        {
            Location = new System.Drawing.Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            using (MainService.MainServiceClient client = new MainService.MainServiceClient())
            {
                MainService.DistributorUser user = client.GetDistributor();
                textBox2.Text = user.AgencyName ;
                textBox12.Text = user.DistributorCode.ToString() ;
                textBox16.Text = user.DistributorName ;
                textBox3.Text = user.PANnumber ;
                textBox14.Text = user.GSTIN ;
                textBox13.Text = user.Address ;
                textBox4.Text = user.City ;
                textBox15.Text = user.Taluka ;
                textBox5.Text = user.District ;
                textBox6.Text = user.State ;
                textBox7.Text = user.LicenceIssueDate.ToString() ;
                textBox8.Text = user.LicenceExpiryDate.ToString() ;
                textBox9.Text = user.ContectNo ;
                textBox10.Text = user.Email;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
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
