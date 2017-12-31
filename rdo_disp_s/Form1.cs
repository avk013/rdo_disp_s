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
    public partial class Form1  : Form
    { public string path = @"e:\!rdo\";
        public Form1()
        {
            InitializeComponent();
        }
        string sudno = "" ;
        string[] port_csv, fraht_csv, rukav_csv, gruz_csv,suda_csv, cfg;
        string[] actual = new string [10];
        static string[] km = { "-100", "1", "101", "200" };
        static string[] km_action = new string[km.Length], km_add = new string[km.Length];
        string[] othod_port_in, othod_port_out;
        // таблица для Datagrid
        DataTable dt = new DataTable("tab0");
        static int st = 0;
        DataColumn a0 = new DataColumn("баржа/судно", typeof(String));
        DataColumn a1 = new DataColumn("груз", typeof(String));
        DataColumn a2 = new DataColumn("порт назначения", typeof(String));
        DataColumn a3 = new DataColumn("фрахтователь", typeof(String));
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
        DataColumn a15 = new DataColumn("Место отбкс.", typeof(String));
        DataColumn a16 = new DataColumn("Дата отбкс.", typeof(String));
        DataColumn a17 = new DataColumn("Время отбкс.", typeof(String));


        DataRow dr = null;
    //
    public string[] barj;
        public List<string> barj_out = new List<string> { };
        private void Form1_Load(object sender, EventArgs e)
        {//иниц таблицы
            dt.Columns.AddRange(new DataColumn[] { a0, a1, a2, a3, a4, a5, a6, a7, a8, a9, a10, a11,a12,a13,a14,a15,a16,a17});
          
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
            //port_csv = File.ReadAllLines(@path + "port.csv");
            port_csv = csv2array(@path + "sport.csv", 1);
            rukav_csv = csv2array(@path + "rukav.csv", 1);
            gruz_csv=csv2array(@path + "sgruz.csv", 1);

            Array.Resize(ref othod_port_out, port_csv.Length);
            Array.Resize(ref othod_port_in, port_csv.Length);
            //othod_port_out new string[port_csv.Length];
            fraht_csv = csv2array(@path + "frahtovatel.csv",1);
            suda_csv = csv2array(@path + "ssuda.csv", 2);
            //rukav_csv= File.ReadAllLines(@path + "rukav.csv");

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
            rukav_box.DataSource = rukav_csv;
            //rukav_box.Items.Remove("");
            rukav_box.SelectedItem = "Дунай";
            //rukav_box. = true;

            fraht.DataSource = fraht_csv;
            comboBox11.DataSource = suda_csv;
            comboBox1.DataSource = km;//км главная
            comboBox7.DataSource = km_action; //км движение
            comboBox6.DataSource = km_add; //км формирование
            comboBox13.DataSource = gruz_csv;
            othod_port_i.DataSource = othod_port_in;
            othod_port_o.DataSource = othod_port_out;
            // считываем данные текущих барж в таблицу
            if(File.Exists(path + "barj+.csv")) csv2datagridview(path + "barj+.csv", dataGridView1);
        }
        public void init_actual()
        {
            text_reis.Text = actual[0];
        }


        private void button1_Click(object sender, EventArgs e)
        {//запускаем форму для движения барж
            listBox2.DataSource = barj_out;
            barj2list frm = new barj2list();
            frm.Owner = this;frm.Show();}

        private void button3_Click(object sender, EventArgs e)
        {   send_mail(radiogramma.Text,"");
            for (int i = 0; i <= dt.Rows.Count; i++)
            if (dataGridView1.Rows[i].DefaultCellStyle.BackColor == Color.Red) { dt.Rows[i].Delete();i = -1; }
            writeCSV(dataGridView1, path + "barj+.csv");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            radiogramma.Text =
                "РДО ДИСП/РЕКА"+Environment.NewLine+sudno+"\t"+text_reis.Text+"\t"+date.Text+"\t"+time.Text+ "\t"+comboBox1.Text+
                "\t"+comboBox2.Text+"\t"+comboBox3.Text+"\t"+dateTimePicker1.Text  ;
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

        private void tabPage3_Click(object sender, EventArgs e)
        {
          //  barj = File.ReadAllLines(path + "barj.csv");
//            listBox2.DataSource = barj;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int item = listBox2.SelectedIndex;
            if (barj[item] != "выгр.>>")
            {
                barj_out.Add(barj[item]);
                string barja = barj[item];
                barj[item] = "выгр.>>";
                listBox2.DataSource = null;
                listBox3.DataSource = null;
                listBox2.DataSource = barj;
                listBox3.DataSource = barj_out;
                //+ надо пометить красным с таблице....
                int res = 0;
                for (int i = 0; i < dt.Rows.Count; i++) if (dt.Rows[i][0].ToString()==barja.Trim()){res = i;break;}
                dataGridView1.Rows[res].DefaultCellStyle.BackColor = Color.Red;
                // вносим порт или км
                string port_="";

                dt.Rows[res][15] = port_;
                dt.Rows[res][16] = dateTimePicker3.Text;
                dt.Rows[res][17] = maskedTextBox1.Text;}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int item = listBox3.SelectedIndex;
            int endx = Array.IndexOf(barj, "выгр.>>");
            // button2.Text = endx.ToString();
            //barj.Add(barj_out[item]);
            if (endx >= 0)
            {
                barj[endx] = barj_out[item];
                barj_out.RemoveAt(item);
                listBox2.DataSource = null;
                listBox3.DataSource = null;
                listBox2.DataSource = barj;
                listBox3.DataSource = barj_out;
            }}

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dr = dt.NewRow();
            dr[0] = comboBox11.Text;
            dr[1] = comboBox13.Text;
            dr[2] = othod_port_o.Text;
            dr[3] = fraht.Text;
            dr[4] = dateTimePicker2.Value.ToString("dd/MM/yyyy");
            dr[6] = othod_port_i.Text;
            
            dt.Rows.Add(dr);
            int i = dt.Rows.Count;
        dataGridView1.DataSource = dt;
            dataGridView1.Rows[i-1].DefaultCellStyle.BackColor = Color.Green;
            writeCSV(dataGridView1, path + "barj+.csv");
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {}

        private void button6_Click(object sender, EventArgs e)
        {
            listBox2.DataSource = null;
            for (int i = 0; i < dt.Rows.Count; i++) //barj[i] = dt.Rows[i][0].ToString();
                                                    //barj = listBox2.DataSource;
                                                    //listBox2.DataSource = barj;                
            listBox2.Items.Add(dt.Rows[i][0]);
            barj = (from object item in listBox2.Items select item.ToString()).ToArray<string>();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            radiogramma.Text =
                "РДО ДИСП/ОТХОД" + Environment.NewLine + " " + sudno + " " + text_reis.Text + " " + date.Text + " " + time.Text;
            //+ " " + comboBox1.Text + " " + comboBox2.Text + " " + comboBox3.Text + " " + dateTimePicker1.Text;
for(int i=0;i<dt.Rows.Count;i++) {
                if (dataGridView1.Rows[i].DefaultCellStyle.BackColor == Color.Green)
                { string buks ="";
                  //  if (radioButton1.Checked == true) buks = comboBox5.Text;
                    //if (radioButton2.Checked == true) buks = comboBox6.Text;
                    radiogramma.Text +=Environment.NewLine+ dt.Rows[i][0] + time2.Text+ buks;
                }
            }


            button3.Enabled = true;
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {         
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {comboBox8.Enabled = true; comboBox7.Enabled = false;}

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {comboBox7.Enabled = true; comboBox8.Enabled = false;}
        public void csv2datagridview(string filein, DataGridView gridIn)
        {
            //string path = filein;
            string[] tab0 = File.ReadAllLines(filein, Encoding.UTF8);
                  string[] tab0Values = null;
                  DataRow dr = null;
                  //помещаем файл в виртуальную таблицу
                  for (int i = 0; i < tab0.Length; i++)
                  {
                      if (!String.IsNullOrEmpty(tab0[i]))
                      {
                          tab0Values = tab0[i].Split(';');
                          //создаём новую строку
                          dr = dt.NewRow();

                          for (int j = 0; j < 11; j++)
                          {
                              string valp = tab0Values[j];
                              // string valp = tab0Values[1].ToUpper();

                              // dr[j] = Regex.Replace(valp, " {2,}", " ");
                              dr[j] = valp;
                          }
                          dt.Rows.Add(dr);
                      }
                  }
                  gridIn.DataSource = dt;
            radiogramma.Text = "read";
        }
        public string[] csv2array(string inputFile, int column=0)
        { if (File.Exists(inputFile)){ string[] f = File.ReadAllLines(inputFile, System.Text.Encoding.GetEncoding(1251));//nado UTF8
                string[] outf = new string[f.Length]; //outf[0] = "";
                for (int i = 1; i < f.Length; i++)
                { string[] Line = f[i].Split(';'); outf[i-1] = Line[column]; }
                return outf; } else { File.Create(inputFile); return null; } }
        public void writeCSV(DataGridView gridIn, string outputFile)
        {
            //test to see if the DataGridView has any rows
            if (gridIn.RowCount > 0)
            {
                string value = "";
                DataGridViewRow dr = new DataGridViewRow();
                StreamWriter swOut = new StreamWriter(outputFile);
                //write DataGridView rows to csv
                for (int j = 0; j <= gridIn.Rows.Count - 2; j++)
                {
                    if (j > 0)
                    {
                        swOut.WriteLine();
                    }

                    dr = gridIn.Rows[j];

                    for (int i = 0; i <= gridIn.Columns.Count - 1; i++)
                    {
                        if (i > 0)
                        {
                            swOut.Write(";");
                        }
                        value = dr.Cells[i].Value.ToString();
                        //replace comma's with spaces
                        value = value.Replace(',', ' ');
                        //replace embedded newlines with spaces
                        value = value.Replace(Environment.NewLine, " ");
                        swOut.Write(value);
                    }
                } swOut.Close();}
        }
    } }
