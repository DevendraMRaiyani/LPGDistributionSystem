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
    public partial class AddCylinder : Form
    {
        List<StockMgntRef.Cylinder> cylinders = null;
        int oldQty=0, newQty=0,emptyQty=0;
        public AddCylinder()
        {
            InitializeComponent();
        }

        private void AddCylinder_Load(object sender, EventArgs e)
        {
            Location = new Point(-7, 50);
            int w = SystemInformation.VirtualScreen.Width + 14;
            int h = SystemInformation.VirtualScreen.Height - 43;
            Size = new Size(w, h);
            dataGridView1.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 10);
            using (StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient())
            {
                dataGridView1.DataSource = client.GetCylinders();
                cylinders = client.GetCylinders().ToList();
               // cylinders1 = client.GetCylinders().ToList();
                string[] cyTypes = cylinders.Where(x=>x.Price!=0).Select(x => x.CylinderType).ToArray();
                comboBox1.Items.AddRange(cyTypes);
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedtype = cylinders.Where(x=>x.CylinderType==comboBox1.Text).Select(x => x.Quentity);
            string result = string.Join(",", selectedtype);
            oldQty = int.Parse(result);
            textBox1.Text = result;
                
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            string selectedtype = string.Join(",", cylinders.Where(x => x.CylinderType == comboBox1.Text).Select(x => x.CylinderType));
            string[] spearator = { "(F)" };
            string selectedEType = string.Join(",", selectedtype.Split(spearator, StringSplitOptions.RemoveEmptyEntries))+"(E)";
            string streqty = string.Join(",", cylinders.Where(x => x.CylinderType == selectedEType).Select(x => x.Quentity));
            int EQty = int.Parse(streqty);
            string result = string.Join(",", cylinders.Where(x => x.CylinderType == comboBox1.Text).Select(x => x.Quentity));
            int loldQty = int.Parse(result);
            int imageWidth,lnewQty=0;
            if (Int32.TryParse(textBox1.Text, out imageWidth))
            {
                lnewQty = imageWidth;
            }

            if (oldQty > lnewQty)
            {
                //dataGridView1.DataSource = null;
                dataGridView1.DataSource = cylinders;
                label3.Text = "You cant Remove Cylinders " + oldQty + " " + newQty + " " + EQty;
            }
            else
            {
                if ((lnewQty - oldQty) > EQty)
                {
                    //dataGridView1.DataSource = null;
                    dataGridView1.DataSource = cylinders;
                    label3.Text = "You have not enough empty quentity!!";
                }
                else
                {
                    //cylinders.Where(x => x.CylinderType == selectedtype).ToList().ForEach(s => s.Quentity = newQty);
                    //cylinders.Where(x => x.CylinderType == selectedEType).ToList().ForEach(s => s.Quentity = EQty - (newQty - oldQty));
                    //dataGridView1.DataSource = cylinders;
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        if ((string)dr.Cells[0].Value==selectedEType)
                        {
                            dr.Cells[1].Value = EQty - (lnewQty - oldQty);
                        }
                        if ((string)dr.Cells[0].Value == selectedtype)
                        {
                            dr.Cells[1].Value = lnewQty;
                            break;
                        }
                        //pl.Quentity = (int)dr.Cells[2].Value;
                    }
                    label3.Text = "ok " + " " + lnewQty +" "+ oldQty + " " + " " + EQty; 
                }
            }
            lnewQty = 0;
            loldQty = 0;
            EQty = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StockMgntRef.StockMgntClient client = new StockMgntRef.StockMgntClient();

            List<StockMgntRef.Cylinder> cylist = new List<StockMgntRef.Cylinder>();
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                //Create object of your list type pl
                StockMgntRef.Cylinder pl = new StockMgntRef.Cylinder();
                pl.CylinderType = (string)dr.Cells[0].Value;
                pl.Quentity = (int)dr.Cells[1].Value;
//                pl.Property3 = dr.Cells[3].Value;

                //Add pl to your List  
                cylist.Add(pl);
            }

            label3.Text=client.SetCylinders(cylist.ToArray());
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
