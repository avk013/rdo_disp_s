using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rdo_disp_s
{
    public partial class Form1 : Form
    { public string path = @"e:\!rdo\";
        public Form1()
        {
            InitializeComponent();
        }
        string sudno = "", reis = "";

        
        string[] port, cfg;
        static string[] km = { "-100", "1", "101", "200" };
        string[] km_action = new string[km.Length];
        string[] km_add = new string[km.Length];
        public string[] barj;
        public List<string> barj_out = new List<string> { };
        private void Form1_Load(object sender, EventArgs e)
        {
            Array.Copy(km, km_action,km.Length);
            Array.Copy(km, km_add, km.Length);
            // km.CopyTo(km_action, 0);
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
                //string[] 
                    cfg = File.ReadAllLines(@path + "config.cfg");
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
            comboBox1.DataSource = km;
            comboBox7.DataSource = km_action;
            comboBox6.DataSource = km_add;
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

        private void button3_Click(object sender, EventArgs e)
        {
            send_mail(radiogramma.Text,"");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radiogramma.Text =
                "РДО ДИСП/РЕКА 01/"+sudno+" 02/"+reis+"";
            button3.Enabled = true;
        }
        public void send_mail(string body, string attach)
        {
            //if (File.Exists(@path + "config.cfg"))
            {
                //Format net_if_stat2mail.cfg: servsmtp/n port/n login/n pass/n 2mail
                //string[] cfg = File.ReadAllLines(@path + "net_if_stat2mail.cfg");
              //  if (cfg.Length == 5)
                {
                    //smtp сервер
                    string smtpHost = cfg[2];//"smtp.gmail.com";                         
                    int smtpPort = Convert.ToInt16(cfg[3]);//587;//smtp порт                                        
                    string login = cfg[1];//логин
                    string pass = cfg[4];//пароль 
                    SmtpClient client = new SmtpClient(smtpHost, smtpPort);//создаем подключение
                    client.Credentials = new NetworkCredential(login, pass);
                    client.EnableSsl = true;                    
                    string from = cfg[1]; ;//От кого письмо
                    string to = cfg[5]; ;//Кому письмо
                    string subject = "Письмо от local" + DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm"); ;
                    MessageBox.Show(login+pass);
                    //string body = "";//Текст письма                   
                                     //Вложение для письма Если нужно не одно вложение, для каждого создаем отдельный Attachment
                    
                    MailMessage mess = new MailMessage(from, to, subject, body);//Создаем сообщение  
                    if (attach!="") { Attachment attData = new Attachment(@attach);
                    mess.Attachments.Add(attData);//прикрепляем вложение
                    }
                    mess.SubjectEncoding = Encoding.UTF8;//прописываем заголовок 
                    mess.BodyEncoding = Encoding.UTF8;
                    //   mess.Headers["Content-type"] = "text/plain; charset=windows-1251";
                    try { client.Send(mess); }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
                //else MessageBox.Show("Read stricture config file_" + cfg.Length.ToString());
            }
            //else MessageBox.Show("Error read config file");
        }
    }
}
