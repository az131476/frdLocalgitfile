using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Threading;

namespace WaveShape
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }
        #region **测试数据**
        public List<float> x1 = new List<float>();
        public List<float> y1 = new List<float>();
        public List<float> x2 = new List<float>();
        public List<float> y2 = new List<float>();
        public List<float> x3 = new List<float>();
        public List<float> y3 = new List<float>();
        public List<float> x4 = new List<float>();
        public List<float> y4 = new List<float>();

        public List<float> y = new List<float>();
        int current = 200;


        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            ///模拟串口采样显示[周期k]

            //if (int.TryParse(textBox1.Text.ToString(), out current))
            //{
            //    if (current > 100 && current < 300)
            //    {
            //        timer1.Interval = current;
            //    }
            //    else
            //    {
            //        textBox1.Text = "400";
            //    }
            //}
            //else
            //{
            //    textBox1.Text = "200";
            //}
            //x1.Clear();
            //y1.Clear();
            //x2.Clear();
            //y2.Clear();
            //x3.Clear();
            //y3.Clear();
            //x4.Clear();
            //y4.Clear();
            //zGraph1.f_ClearAllPix();
            //zGraph1.f_reXY();
            //zGraph1.f_LoadOnePix(ref x1, ref y1, Color.Red, 2);

            //zGraph1.f_AddPix(ref x3, ref y3, Color.FromArgb(0, 128, 192), 2);
            //zGraph1.f_AddPix(ref x4, ref y4, Color.Yellow, 3);
            //zGraph1.f_Refresh();

            //开始TIMER
            //timerDrawI = 0;
            //timer1.Start();

            LoadDataList();
        }
        private int timerDrawI = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            ///TIME增加数据

            x2.Add(timerDrawI);
            y2.Add((float)Math.Sin(timerDrawI / 10f) * 200);

            timerDrawI++;
            zGraph1.f_Refresh();
            //更新按钮显示，表示为正在采样
            button1.Text += ".";
            if (button1.Text.Length > 22)
            {
                button1.Text = "采样测试 正在采样.";
            }
            //LoadDataList();
        }
        public void LoadDataList()
        {
            string sql = "SELECT s,x,y,z FROM datatest limit 20";
            MySqlConnection con = null;
            MySqlDataReader reader = null;
            try
            {

                con = new MySqlConnection("server=localhost;user id = root;database=frdtest;password=az13132323251..");
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                #region
                while (reader.Read())
                {
                    string s0 = reader[0].ToString();
                    string x0 = reader[1].ToString();
                    string y0 = reader[2].ToString();
                    string z0 = reader[3].ToString();

                    if (s0 != "")
                    {
                        float s = float.Parse(s0);
                        x2.Add(s);
                        Log.Write("s:" + s);
                    }
                    if (x0 != "")
                    {
                        float x = float.Parse(x0);
                        y2.Add(x);
                        Log.Write("x:" + x);
                    }
                    if (y0 != "")
                    {
                        float y = float.Parse(y0);
                    }
                    if (z0 != "")
                    {
                        float z = float.Parse(z0);
                    }
                    zGraph1.f_ClearAllPix();
                    zGraph1.f_reXY();
                    //zGraph1.f_LoadOnePix(ref x2, ref y2, Color.Red, 2);
                    zGraph1.f_AddPix(ref x2, ref y2, Color.Blue, 3);
                    zGraph1.f_Refresh();
                    //Thread.Sleep(200);
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //LoadDataList();

            string sql = "SELECT s,x,y,z FROM datatest";
            MySqlConnection con = null;
            MySqlDataAdapter adapter = null;
            try
            {
                con = new MySqlConnection("server=localhost;user id = root;database=frdtest;password=az13132323251..");
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                DataSet ds = new DataSet();
                adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                Log.Write("启动");
            }
            catch (Exception ex)
            {
                MessageBox.Show("异常：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoadDataList();
        }
    }
}
