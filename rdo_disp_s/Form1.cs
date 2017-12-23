using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rdo_disp_s
{
    public partial class Form1 : Form
    {public  string path = @"e:\!rdo\";
        public Form1()
        {
            InitializeComponent();
        }
        string sudno= "", reis ="";
      
        string[] port;
        public string[] barj;
        public List<string> barj_out = new List<string> { };
        private void Form1_Load(object sender, EventArgs e)
        {
            date.Text = DateTime.Today.ToString("dd/MM/yyyy");
            // но лучше брать время из интернета
            time.Text= DateTime.Now.ToString("HH:mm");
            //есть ли доступ к интернету
            if (ConnectionAvailable("http://www.google.com") == false)
            {MessageBox.Show("не вижу интернета :(((((");this.Close();};
            init_config();
            textBox2.Text = "судно:"+sudno;
        }

        // процедура проверки есть ли интернет
        public bool ConnectionAvailable(string strServer)
        {
            try
            {
                HttpWebRequest reqFP = (HttpWebRequest)HttpWebRequest.Create(strServer);

                HttpWebResponse rspFP = (HttpWebResponse)reqFP.GetResponse();
                if (HttpStatusCode.OK == rspFP.StatusCode)
                {
                    // HTTP = 200 - Интернет безусловно есть! 
                    rspFP.Close();
                    return true;
                }
                else
                {
                    // сервер вернул отрицательный ответ, возможно что инета нет
                    rspFP.Close();
                    return false;
                }
            }
            catch (WebException)
            {// Ошибка, значит интернета у нас нет. Плачем :'(
                return false;  }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        public void init_config()
        {
            if (File.Exists(@path + "config.cfg"))
            {
                //Format net_if_stat2mail.cfg: servsmtp/n port/n login/n pass/n 2mail
                string[] cfg = File.ReadAllLines(@path + "config.cfg");
                if (cfg.Length >= 1)
                {
                    sudno = cfg[0];

                    try {
                    //    client.Send(mess);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                else MessageBox.Show("Read stricture config file_" + cfg.Length.ToString());
            }
            else MessageBox.Show("Error read config file");
            // считываем в массивы
            port = File.ReadAllLines(@path + "port.csv");
            comboBox3.DataSource = comboBox5.DataSource=comboBox8.DataSource= port;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox5.Enabled = true;comboBox6.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox6.Enabled = true; comboBox5.Enabled = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            comboBox8.Enabled = true; comboBox7.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            comboBox7.Enabled = true; comboBox8.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = barj_out;
            barj2list frm = new barj2list();
            frm.Owner = this;
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radiogramma.Text =
                "РДО ДИСП/РЕКА 01/"+sudno+" 02/"+reis+"";
        }
    }
}
