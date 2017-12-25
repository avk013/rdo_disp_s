using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rdo_disp_s
{
    public partial class barj2list : Form
    {
        public barj2list()
        {
            InitializeComponent();
        }
        
        private void barj2list_Load(object sender, EventArgs e)
        {
           Form1 frm = (Form1)this.Owner;
            frm.barj  = File.ReadAllLines(frm.path + "barj.csv");
           listBox1.DataSource = frm.barj;
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {Form1 frm = (Form1)this.Owner;
            int item = listBox1.SelectedIndex;
            if (frm.barj[item] != "выгр.>>")
            {
                frm.barj_out.Add(frm.barj[item]);
                frm.barj[item] = "выгр.>>";
                listBox1.DataSource = null;
                listBox3.DataSource = null;
                listBox1.DataSource = frm.barj;
                listBox3.DataSource = frm.barj_out;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        { Form1 frm = (Form1)this.Owner;
            int item = listBox3.SelectedIndex;
            int endx = Array.IndexOf(frm.barj, "выгр.>>");
            // button2.Text = endx.ToString();
            //barj.Add(barj_out[item]);
            if (endx >= 0)
            {
                frm.barj[endx] = frm.barj_out[item];
                frm.barj_out.RemoveAt(item);
                listBox1.DataSource = null;
                listBox3.DataSource = null;
                listBox1.DataSource = frm.barj;
                listBox3.DataSource = frm.barj_out;
            }
        }

        private void barj2list_FormClosing(object sender, FormClosingEventArgs e)
        {Form1 frm = (Form1)this.Owner;
            frm.listBox1.DataSource = null;
            frm.listBox1.DataSource = frm.barj_out;
        }
    }
}
