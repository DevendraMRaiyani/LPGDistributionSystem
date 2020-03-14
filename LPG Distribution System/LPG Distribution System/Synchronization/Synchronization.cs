using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPG_Distribution_System.Synchronization
{
    public partial class Synchronization : Form
    {
        public Synchronization()
        {
            InitializeComponent();
        }

        private void Synchronization_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            dataGridView2.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);

            SynchroRef.SynchronizationClient client = new SynchroRef.SynchronizationClient();
            List<SynchroRef.Synchro> synchros = client.GetRecords().ToList();
            label3.Text = label3.Text +" "+ DateTime.Now.ToShortDateString();
            label4.Text = label4.Text + " "+ synchros.Where(x => x.TableName.Equals("Synchroes")).FirstOrDefault().Operation.Substring(0, 10).ToString();
            var groups = synchros.Where(x => x.TableName != "Synchroes").AsEnumerable().GroupBy(g => g.TableName).Select(g => new { TableName = g.Key, Records = g.Count() }).ToList();
            dataGridView2.DataSource = groups.ToList();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            int i = await Btn();
            int j = await Lb5();
            int k = await Lb6();
            int l = await Syn();
        }
        public async Task<int> Btn()
        {
            button3.Enabled = false;
            return await Task.Run<int>(() =>
            {
                return 0;
            });
        }
        public async Task<int> Lb5()
        {
            await Task.Delay(50);
            label5.Text = "Please Wait upto 30 secs!!!";
            return await Task.Run<int>(() =>
            {
                return 0;
            });
        }
        public async Task<int> Lb6()
        {
            await Task.Delay(100);
            label6.Text = "Proccessing...";
            return await Task.Run<int>(() =>
            {
                return 0;
            });
        }
        public async Task<int> Syn()
        {
            await Task.Delay(100);
            try
            {
                SynchroRef.SynchronizationClient client = new SynchroRef.SynchronizationClient();
                if (client.SynchroToCentralDB().Equals("OK"))
                {
                    MessageBox.Show("Successfully Synchroed to Central Database!!!");
                    List<SynchroRef.Synchro> synchros = client.GetRecords().ToList();
                    var groups1 = synchros.Where(x => x.TableName != "Synchroes").AsEnumerable().GroupBy(g => g.TableName).Select(g => new { TableName = g.Key, Records = g.Count() }).ToList();
                    dataGridView2.DataSource = groups1;
                    label4.Text = null;
                    MessageBox.Show("Your Last Synchro Date changed to "+ synchros.Where(x => x.TableName.Equals("Synchroes")).FirstOrDefault().Operation.Substring(0, 10).ToString());
                    label4.Text = "Last Synchro Date : " + synchros.Where(x => x.TableName.Equals("Synchroes")).FirstOrDefault().Operation.Substring(0, 10).ToString();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

            button3.Enabled = true;
            label5.Text = "";
            label6.Text = "";
            return await Task.Run<int>(() =>
            {
                return 0;
            });
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
