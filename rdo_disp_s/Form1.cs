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
        string[] port_csv, fraht_csv,cfg;
        string[] actual = new string [10];
        static string[] km = { "-100", "1", "101", "200" };
        static string[] km_action = new string[km.Length], km_add = new string[km.Length];
        string[] othod_port_in, othod_port_out;
        // таблица для Datagrid
        DataTable dt = new DataTable("tab0");
        static int st = 0;
        DataColumn a0 = new DataColumn("баржа/судно", typeof(String));
        DataColumn a1 = new DataColumn("груз", typeof(String));
        DataColumn a2 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a3 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a4 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a5 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a6 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a7 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a8 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a9 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a10 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a11 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a12 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a13 = new DataColumn(st++.ToString(), typeof(String));
        DataColumn a14 = new DataColumn(st++.ToString(), typeof(String));
        
    DataRow dr = null;
    //
    public string[] barj;
        public List<string> barj_out = new List<string> { };
        private void Form1_Load(object sender, EventArgs e)
        {//иниц таблицы
            dt.Columns.AddRange(new DataColumn[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11,a12,a13,a14});
          
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
            {   cfg = File.ReadAllLines(@path + "config.cfg");
                if (cfg.Length >= 1)
                {sudno = cfg[0];}
                else MessageBox.Show("Read stricture config file_" + cfg.Length.ToString());
            }
            else MessageBox.Show("Error read config file");
            // считываем в массивы
            actual= File.ReadAllLines(@path + "actual.cfg");
            init_actual();
            port_csv = File.ReadAllLines(@path + "port.csv");
            Array.Resize(ref othod_port_out, port_csv.Length);
            Array.Resize(ref othod_port_in, port_csv.Length);
            //othod_port_out new string[port_csv.Length];
            fraht_csv = File.ReadAllLines(@path + "frahtovatel.csv");
            //копирование массивов списков
            Array.Copy(km, km_action, km.Length);
            Array.Copy(km, km_add, km.Length);
            Array.Copy(port_csv, othod_port_out, port_csv.Length);
            Array.Copy(port_csv, othod_port_in, port_csv.Length);
            // km.CopyTo(km_action, 0);

            // папки для отправленных и ошибок нет то создаем
            if (!File.Exists(path + @"/out/")) Directory.CreateDirectory(path + @"/out/");
            if (!File.Exists(path + @"/error/")) Directory.CreateDirectory(path + @"/error/");
            // помещаем данные в выпадающие списки           
            comboBox3.DataSource = comboBox5.DataSource=comboBox8.DataSource= port_csv;
            comboBox3.AutoCompleteMode=othod_port_i.AutoCompleteMode=othod_port_o.AutoCompleteMode = AutoCompleteMode.SuggestAppend;//создает поиск по буквам
            comboBox3.AutoCompleteSource = othod_port_i.AutoCompleteSource=othod_port_o.AutoCompleteSource= AutoCompleteSource.ListItems;//создает поиск по буквам
            
            fraht.DataSource = fraht_csv;
            comboBox1.DataSource = km;//км главная
            comboBox7.DataSource = km_action; //км движение
            comboBox6.DataSource = km_add; //км формирование
            othod_port_i.DataSource = othod_port_in;
            othod_port_o.DataSource = othod_port_out;
        }
        public void init_actual()
        {
            text_reis.Text = actual[0];
        }


        private void button1_Click(object sender, EventArgs e)
        {//запускаем форму для движения барж
            listBox1.DataSource = barj_out;
            barj2list frm = new barj2list();
            frm.Owner = this;frm.Show();}

        private void button3_Click(object sender, EventArgs e)
        { send_mail(radiogramma.Text,"");}

        private void button2_Click(object sender, EventArgs e)
        {
            radiogramma.Text =
                "РДО ДИСП/РЕКА"+Environment.NewLine+"01/"+sudno+" 02/"+text_reis.Text+" 03/"+date.Text+" 04/"+time.Text+ " 05/"+comboBox1.Text+
                " 06/"+comboBox2.Text+" 07/"+comboBox3.Text+" 08/"+dateTimePicker1.Text  ;
            button3.Enabled = true;
        }
        public void send_mail(string body, string attach)
        {                                      
                    string smtpHost = cfg[2];//smtp сервер//"smtp.gmail.com";                         
                    int smtpPort = Convert.ToInt16(cfg[3]);//587;//smtp порт                                        
                    string login = cfg[1];//логин
                    string pass = cfg[4];//пароль 
                    SmtpClient client = new SmtpClient(smtpHost, smtpPort);//создаем подключение
                    client.Credentials = new NetworkCredential(login, pass);
                    client.EnableSsl = true;                    
                    string from = cfg[1]; ;//От кого письмо
                    string to = cfg[5]; ;//Кому письмо
                    string subject = "Письмо от "+sudno + DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm"); ;
                    //MessageBox.Show(login+pass);
                    //string body = "";//Текст письма                   
                                     //Вложение для письма Если нужно не одно вложение, для каждого создаем отдельный Attachment                  
                    MailMessage mess = new MailMessage(from, to, subject, body);//Создаем сообщение  
                    if (attach!="") { Attachment attData = new Attachment(@attach);
                    mess.Attachments.Add(attData);//прикрепляем вложение
                    }
                    mess.SubjectEncoding = Encoding.UTF8;//прописываем заголовок 
                    mess.BodyEncoding = Encoding.UTF8;
                    //   mess.Headers["Content-type"] = "text/plain; charset=windows-1251";
                    try { client.Send(mess);
                    File.WriteAllText(path + @"/out/" + DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm")+".txt", radiogramma.Text);
                    radiogramma.Text = "OK!";
                    MessageBox.Show("OK!");
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message);
                    File.WriteAllText(path + @"/error/" + DateTime.Now.ToString("dd-MMMM-yyyy_HH-mm.txt") + ".txt", radiogramma.Text);
                    }}
     private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {comboBox5.Enabled = true;comboBox6.Enabled = false;}
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {comboBox6.Enabled = true; comboBox5.Enabled = false;}
        private void textBox1_TextChanged(object sender, EventArgs e)
        {actual[0] = text_reis.Text;}
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {actual[1] = comboBox1.Text;}

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {File.WriteAllLines(path + "actual.cfg", actual); }//записываем последние актуальные поля

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {File.WriteAllLines(path + "actual.cfg", actual);}//записываем последние актуальные поля

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //string[] tab0Values = null;
            //tab0Values = files[i].Name.Split('_');
            dr = dt.NewRow();

            //for (int ii = 0; ii < 6; ii++) { dr[ii] = tab0Values[ii]; }
            dr[1] = dateTimePicker2.Value.ToString("dd/MM/yyyy");
            dr[2] = othod_port_i.Text;
            dr[3] = othod_port_o.Text;
            dt.Rows.Add(dr);

        dataGridView1.DataSource = dt;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {}
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {comboBox8.Enabled = true; comboBox7.Enabled = false;}

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {comboBox7.Enabled = true; comboBox8.Enabled = false;} } }
