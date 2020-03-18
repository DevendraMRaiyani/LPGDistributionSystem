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
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
        }

        private async void Start_Load(object sender, EventArgs e)
        {
            int j = await HideMethod();
            int i = await ConnectionEst();
        }

        private async Task<int> ConnectionEst()
        {
            await Task.Delay(1000);
            using (MainService.MainServiceClient mn = new MainService.MainServiceClient())
            {
                try
                {
                    string res = mn.TestConnection();
                    if (!res.Equals("FAIL"))
                    {
                        this.Hide();
                        Login lg = new Login();
                        lg.Show();
                    }    
                }
                catch (Exception ee)
                {
                    MessageBox.Show("Sorry,Enable to connect service!!!");
                    this.Hide();
                }
            }
            return await Task.Run<int>(() =>
            {
                return 0;
            });
        }
        public async Task<int> HideMethod()
        {
            this.Hide();
            return await Task.Run<int>(() =>
            {
                return 0;
            });
        }
    }
}
