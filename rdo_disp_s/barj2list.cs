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
        //    frm.barj  = File.ReadAllLines(frm.path + "barj.csv");
          // listBox1.DataSource = frm.barj;
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {Form1 frm = (Form1)this.Owner;
        
        }

        private void button2_Click(object sender, EventArgs e)
        { Form1 frm = (Form1)this.Owner;
            
        }

        private void barj2list_FormClosing(object sender, FormClosingEventArgs e)
        {Form1 frm = (Form1)this.Owner;
         //   frm.listBox1.DataSource = null;
//            frm.listBox1.DataSource = frm.barj_out;
        }
    }
}
