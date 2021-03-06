﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using System.Threading;
using MySql.Data.MySqlClient;
using ZedGraph;
using System.IO;
using RedisHelper;
using System.Configuration;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Office2007Form
    {
        SocketClient client = new SocketClient();
        bool flg;
        //public PointPairList list = new PointPairList();
        public PointPairList list2 = new PointPairList();
        public PointPairList list3 = new PointPairList();
        RollingPointPairList list = new RollingPointPairList(2000);
        //RollingPointPairList list2 = new RollingPointPairList(2000);
        //RollingPointPairList list3 = new RollingPointPairList(2000);
        LineItem myCurve;
        LineItem myCurve2;
        LineItem myCurve3;
        bool pflg = true;
        public Form1()
        {
            this.EnableGlass = false;
            
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 设置桌面风格等
            initCompoents();
            #endregion
            InitEquip();
            #region 启动Socket与通讯服务建立连接
            Thread td_socket = new Thread(new ThreadStart(socketConnect));
            td_socket.IsBackground = true;
            td_socket.Start();
            #endregion
            #region 请求获取服务器初始化设备参数，并加载界面参数列表并保存
            LoadOrderData();
            #endregion
            
            ZedgraphShapInit();
        }
        private bool InitEquip()
        {
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select DISTINCT machineID,machineIP from all_channelgroup_message  where `online`='1'", con);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxEx4.Items.Add(reader[0] + "_" + reader[1]);
                    comboBoxEx4.SelectedIndex = 0;
                    comboBoxEx6.Items.Add(reader[0] + "_" + reader[1]);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
            return true;
        }
        private void buttonX18_Click(object sender, EventArgs e)
        {
            comboBoxEx6.Items.Clear();
            if (InitEquip())
            {//初始化成功
                tabItem.Visible = true;
                tabItem7.Visible = false;
                comboBoxEx6.SelectedIndex = 0;
                //string equip_ip = comboBoxEx4.SelectedItem.ToString();
                //client.ClientSendMsg("$100"+equip_ip);
            }
        }
        private void socketConnect()
        {
            while (true)
            {
                if (client.socketConnect())
                {
                    //pictureBox1.ImageLocation = @"pic\success.png";
                    //client.ClientSendMsg("SF0");
                    flg = true;
                    break;
                }
                else
                {
                    //pictureBox1.ImageLocation = @"pic\error1.ico";
                    flg = false;
                }
                Thread.Sleep(300);
            }
        }
        private void initCompoents()
        {
            /*设置背景
             */ 
            StyleManager sytleBackGround = new StyleManager();
            sytleBackGround.ManagerStyle = eStyle.Office2010Blue;
            tabItem.Visible = false;
            tabItem1.Visible = false;
            tabItem2.Visible = false;
            tabItem3.Visible = false;
            tabItem4.Visible = false;
            tabItem5.Visible = false;
            tabItem6.Visible = false;
            tabItem7.Text = "初始化";

            /*控件初始化数据
             */
            string[] data1 = new string[] { "1", "2", "3" };
            comboBoxEx1.Items.AddRange(data1);
            labelX2.SymbolColor = Color.Blue;

            ///<summary>
            ///自适应屏幕大小
            /// </summary>

        }
        /// <summary>
        /// Loads default Order data into the
        /// customersDataSet.Orders data table
        /// </summary>
        private void LoadOrderData()
        {
            //client.ClientSendMsg("$001");
        }
        

        private void dataGridViewX1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewButtonXCell cell = dataGridViewX1.CurrentCell as DataGridViewButtonXCell;

            if (cell != null)
            {
                DataGridViewButtonXColumn bc =
                    dataGridViewX1.Columns[e.ColumnIndex] as DataGridViewButtonXColumn;

                if (bc != null)
                {
                    string s = Convert.ToString(cell.Value);

                    switch (bc.Name)
                    {
                        case "通道设定":
                            MessageBox.Show("What a great country " + s + " is!", "",
                                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "Region":
                            cell.Value = string.IsNullOrEmpty(s) ? "Global" : "";
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 波形测试数据
        /// </summary>
        private void LoadData()
        {

            string sql = "SELECT s,x,y,z FROM datatest limit 50";
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
                    

                    list.Add(Double.Parse(s0), double.Parse(x0));
                    list2.Add(double.Parse(s0), double.Parse(y0));
                    list3.Add(double.Parse(s0), double.Parse(z0));

                    this.zedGraphControl1.AxisChange();
                    this.zedGraphControl1.Refresh();
                    this.zedGraphControl2.AxisChange();
                    this.zedGraphControl2.Refresh();
                    this.zedGraphControl3.AxisChange();
                    this.zedGraphControl3.Refresh();
                    Thread.Sleep(100);
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
        ///<summary>
        ///zedgraph init 
        /// </summary>
        public void ZedgraphShapInit()
        {
            #region x
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "波形图";
            myPane.XAxis.Title.Text = "";
            myPane.YAxis.Title.Text = "采集值";
            myPane.XAxis.Type = ZedGraph.AxisType.Linear;
            
            // Add gridlines to the plot, and make them gray
            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.LightGray;
            myPane.YAxis.MajorGrid.Color = Color.LightGray;

            //myPane.XAxis.Scale.MinorStep = 1;//X轴小步长1,也就是小间隔
            //myPane.XAxis.Scale.MajorStep = 5;//X轴大步长为5，也就是显示文字的大间隔	//改变轴的刻度

            //1.禁用右键菜单：
            zedGraphControl1.IsShowContextMenu = false;
            // 2.禁用鼠标滚轴移动：
            zedGraphControl1.IsEnableHPan = true;//横向移动;
            zedGraphControl1.IsEnableVPan = true; //纵向移动;

            //2.禁用鼠标滚轴缩放：

            zedGraphControl1.IsEnableHZoom = true; //横向缩放;
            zedGraphControl1.IsEnableVZoom = true; //纵向缩放;
            zedGraphControl1.ZoomStepFraction = 0.2;
            ///设置鼠标拖动
            zedGraphControl1.PanModifierKeys = Keys.None;//鼠标左键，随意拖动
                                                         // Move the legend location
            myPane.Legend.Position = ZedGraph.LegendPos.Top;
            myCurve = zedGraphControl1.GraphPane.AddCurve("x", list, Color.Blue, SymbolType.None);
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();

            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            #endregion

            #region y
            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "";
            myPane2.XAxis.Title.Text = "";
            myPane2.YAxis.Title.Text = "采集值";
            myPane2.XAxis.Type = ZedGraph.AxisType.Linear;

            // Add gridlines to the plot, and make them gray
            myPane2.XAxis.MajorGrid.IsVisible = true;
            myPane2.YAxis.MajorGrid.IsVisible = true;
            myPane2.XAxis.MajorGrid.Color = Color.LightGray;
            myPane2.YAxis.MajorGrid.Color = Color.LightGray;

            //1.禁用右键菜单：
            zedGraphControl1.IsShowContextMenu = false;
            // 2.禁用鼠标滚轴移动：
            zedGraphControl1.IsEnableHPan = true;//横向移动;
            zedGraphControl1.IsEnableVPan = true; //纵向移动;

            //2.禁用鼠标滚轴缩放：

            zedGraphControl1.IsEnableHZoom = true; //横向缩放;
            zedGraphControl1.IsEnableVZoom = true; //纵向缩放;
            zedGraphControl1.ZoomStepFraction = 0.2;

            // Move the legend location
            myPane2.Legend.Position = ZedGraph.LegendPos.Top;
            myCurve2 = zedGraphControl2.GraphPane.AddCurve("y", list2, Color.Red, SymbolType.None);
            this.zedGraphControl2.AxisChange();
            this.zedGraphControl2.Refresh();

            zedGraphControl2.IsShowPointValues = true;
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            #endregion

            #region z
            GraphPane myPane3 = zedGraphControl3.GraphPane;
            myPane3.Title.Text = "";
            myPane3.XAxis.Title.Text = "";
            myPane3.YAxis.Title.Text = "采集值";
            myPane3.XAxis.Type = ZedGraph.AxisType.Linear;

            // Add gridlines to the plot, and make them gray
            myPane3.XAxis.MajorGrid.IsVisible = true;
            myPane3.YAxis.MajorGrid.IsVisible = true;
            myPane3.XAxis.MajorGrid.Color = Color.LightGray;
            myPane3.YAxis.MajorGrid.Color = Color.LightGray;

            //1.禁用右键菜单：
            zedGraphControl3.IsShowContextMenu = false;
            // 2.禁用鼠标滚轴移动：
            zedGraphControl3.IsEnableHPan = true;//横向移动;
            zedGraphControl3.IsEnableVPan = true; //纵向移动;

            //2.禁用鼠标滚轴缩放：

            zedGraphControl3.IsEnableHZoom = true; //横向缩放;
            zedGraphControl3.IsEnableVZoom = true; //纵向缩放;
            zedGraphControl3.ZoomStepFraction = 0.2;

            // Move the legend location
            myPane3.Legend.Position = ZedGraph.LegendPos.Top;
            myCurve3 = zedGraphControl3.GraphPane.AddCurve("y", list3, Color.Red, SymbolType.None);
            this.zedGraphControl3.AxisChange();
            this.zedGraphControl3.Refresh();

            zedGraphControl3.IsShowPointValues = true;
            zedGraphControl3.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            #endregion
        }
        /// <summary>
        /// 鼠标显示点坐标
        /// </summary>
        /// <param name="control"></param>
        /// <param name="pane"></param>
        /// <param name="curve"></param>
        /// <param name="iPt"></param>
        /// <returns></returns>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            string xypoint = "横坐标:" + pt.X.ToString() + " 纵坐标:" + pt.Y.ToString();
            MessageBox.Show(xypoint);
            return xypoint;
        }
        #region 界面设置
        private void buttonX3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 1;
            tabItem.Text = "波形显示";
        }

        private void buttonX4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 2;
            tabItem.Text = "信号处理";
        }

        private void buttonX5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 4;
            tabItem.Text = "风险信息";
        }

        private void buttonX6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 0;
            tabItem.Text = "参数配置";
            ///load data
            SyncParams();
            InitFrequency();
            comboBoxEx2.Items.Add("连续");
            comboBoxEx2.Items.Add("瞬态");
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 3;
            tabItem.Text = "存储设置";
            comboBoxEx5.Items.Add("连续");
            comboBoxEx5.Items.Add("瞬态");
            comboBoxEx5.SelectedIndex = 0;
        }

        private void buttonX8_Click(object sender, EventArgs e)
        {
            //LoadData();
            Thread td1 = new Thread(xgraph);
            td1.IsBackground = true;
            td1.Start();

            Thread td2 = new Thread(ygraph);
            td2.IsBackground = true;
            td2.Start();

            Thread td3 = new Thread(zgraph);
            td3.IsBackground = true;
            td3.Start();
        }
        private void xgraph()
        {
            while (true)
            {
                MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                //UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=2
                string sql = "select channel_id,`data`,dtime,currTime,id,now() from datatest where UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=1 and channel_id='0' and state = '0' ORDER BY  id asc ";
                MySqlDataReader reader = null;
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string channel_id = reader[0].ToString();
                        string data = reader[1].ToString();
                        string dtime = reader[2].ToString();
                        string currtime = reader[3].ToString();
                        string id = reader[4].ToString();
                        string t1 = reader[5].ToString();
                        //string t2 = reader[6].ToString();
                        if (channel_id.Equals("0"))
                        {
                            list.Add(double.Parse(dtime), double.Parse(data));
                            this.zedGraphControl1.AxisChange();
                            this.zedGraphControl1.Refresh();
                            this.zedGraphControl1.Invalidate();
                            
                            Debug.Write1(dtime+","+data+","+currtime+","+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+","+t1+",");
                            string sql_update = "UPDATE datatest set state='1' where id='"+id+"'";
                            new Data.DBHelper().updateDB(sql_update);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void ygraph() {
            while (true)
            {
                MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                //UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=2
                string sql = "select channel_id,`data`,dtime,currTime,id,now() from datatest where UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=1 and channel_id='1' and state = '0' ORDER BY  id asc ";
                MySqlDataReader reader = null;
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string channel_id = reader[0].ToString();
                        string data = reader[1].ToString();
                        string dtime = reader[2].ToString();
                        string currtime = reader[3].ToString();
                        string id = reader[4].ToString();
                        string t1 = reader[5].ToString();
                        //string t2 = reader[6].ToString();
                        if (channel_id.Equals("1"))
                        {
                            list2.Add(double.Parse(dtime), double.Parse(data));
                            this.zedGraphControl2.AxisChange();
                            this.zedGraphControl2.Refresh();
                            Debug.Write2(dtime + "," + data + "," + currtime + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + t1 + ",");
                            string sql_update = "UPDATE datatest set state='1' where id='" + id + "'";
                            new Data.DBHelper().updateDB(sql_update);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }
        private void zgraph() {
            //z
            while (true)
            {
                MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                //UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=2
                string sql = "select channel_id,`data`,dtime,currTime,id,now() from datatest where UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=1 and channel_id='2' and state = '0' ORDER BY  id asc ";
                MySqlDataReader reader = null;
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string channel_id = reader[0].ToString();
                        string data = reader[1].ToString();
                        string dtime = reader[2].ToString();
                        string currtime = reader[3].ToString();
                        string id = reader[4].ToString();
                        string t1 = reader[5].ToString();
                        //string t2 = reader[6].ToString();
                        if (channel_id.Equals("2"))
                        {
                            list3.Add(double.Parse(dtime), double.Parse(data));
                            this.zedGraphControl3.AxisChange();
                            this.zedGraphControl3.Refresh();
                            Debug.Write3(dtime + "," + data + "," + currtime + "," + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "," + t1 + ",");
                            string sql_update = "UPDATE datatest set state='1' where id='" + id + "'";
                            new Data.DBHelper().updateDB(sql_update);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
        }

        private void buttonX9_Click(object sender, EventArgs e)
        {
            HomeBack();
        }
        private void HomeBack()
        {
            tabControl1.SelectedTabIndex = 5;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

        }

        private void buttonX10_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void buttonX11_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void buttonX12_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void buttonX13_Click(object sender, EventArgs e)
        {
            HomeBack();
        }
        #endregion

        private void pToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pflg = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
        private void SyncParams()
        {
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
            string sql = "select * from all_channelgroup_message";
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridViewX1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
        private void InitFrequency()
        {
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
            string sql = "select frequency_list from all_channelgroup_message where machineIP='192.168.0.102'";
            string freq = "";
            MySqlDataReader reader = null;
            try
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(sql, con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    freq = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }

            //获取采样频率可选项
            //string strFrepList = "";
            List<string> m_listFreq = new List<string>(); //采样频率集
            
            int nFreqCount = BreakString(freq, out m_listFreq, "|"); //个数
            /// <summary>
            /// 初始化采样频率选择列表
            /// </summary>
            this.comboBoxEx1.Items.Clear();

            //float fltCurFreq = sampleParam.m_fltSampleFrequency;

            int nCount = 0;
            int nCurSel = -1;
            string strFreq;

            //RemoveAllEvent();
            foreach (var val in m_listFreq)
            {
                GetValidFloatString(val, out strFreq);//去点号

                comboBoxEx1.Items.Add(strFreq);

                float flt = float.Parse(strFreq);
                //if (fltCurFreq == flt)
                //{
                //    nCurSel = nCount;
                //}
                nCount++;
            }
            if (nCurSel >= 0)
            {
                comboBoxEx1.SelectedIndex = nCurSel;
            }
        }
        //将字符串进行分解。strSeprator中的任何一个字符都作为分隔符。返回分节得到的字符串数目
        private static int BreakString(string strSrc, out List<string> lstDest, string strSeprator)
        {
            //清空列表
            lstDest = new List<string>();
            //个数
            int iCount = 0;

            if (strSeprator.Length == 0)
            {
                lstDest.Add(strSrc);
                iCount = 1;
                return iCount;
            }

            //查找的位置
            int iPos = 0;
            while (iPos < strSrc.Length)
            {
                int iNewPos = strSrc.IndexOf(strSeprator, iPos);
                //当前字符即分隔符
                if (iNewPos == iPos)
                {
                    iPos++;
                }
                //没找到分隔符
                else if (iNewPos == -1)
                {
                    lstDest.Add(strSrc.Substring(iPos, strSrc.Length - iPos));
                    iCount++;
                    iPos = strSrc.Length;
                }
                //其它
                else
                {
                    lstDest.Add(strSrc.Substring(iPos, iNewPos - iPos));
                    iCount++;
                    iPos = iNewPos;
                    iPos++;
                }
            }
            return iCount;
        }
        public static void GetValidFloatString(string strText, out string strFloat)
        {
            strFloat = "";
            if (strText.Contains('.'))
                strFloat = strText.Substring(0, strText.LastIndexOf('.'));
            else
                strFloat = strText;
        }

        private void buttonX16_Click(object sender, EventArgs e)
        {
            string freq = comboBoxEx1.SelectedItem.ToString();
            string sampleModel = comboBoxEx2.SelectedItem.ToString();
            new SocketClient().ClientSendMsg("$002"+freq+"|"+sampleModel);
        }

        private void buttonX17_Click(object sender, EventArgs e)
        {
            list.Clear();
            list2.Clear();
            list3.Clear();
        }

        private void buttonX20_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();

            if (folder.ShowDialog() == DialogResult.OK)
            {
                string p1 = folder.SelectedPath;
                textBoxX1.Text = p1;
            }
        }

        private void buttonX19_Click(object sender, EventArgs e)
        {
            string comboxtext = comboBoxEx6.SelectedItem.ToString();
            string equip_id = comboxtext.Substring(0,comboxtext.IndexOf('_'));
            string equip_ip = comboxtext.Substring(comboxtext.IndexOf('_')+1,comboxtext.Length-comboxtext.IndexOf('_')-1);
            int autoflg = 0;
            if (checkBoxX1.Checked)
            {
                autoflg = 1;
            }
            else { autoflg = 0; }
            textBoxX1.Text = @textBoxX1.Text;

            string sql_insert = @"insert into client_save_set(equip_id,equip_ip,autosave,savemay,path) VALUES('"+equip_id+"','"+equip_ip+"','"+ autoflg + "','"+ comboBoxEx5.SelectedIndex+ "','"+ textBoxX1.Text+ "')";
            string sql_select = @"select * from client_save_set where equip_ip = '"+equip_ip+"'";
            string sql_update = @"update client_save_set set autosave='"+autoflg+"',savemay='"+comboBoxEx5.SelectedIndex+"',path='"+textBoxX1.Text+"' where equip_ip = '"+equip_ip+"'";

            Data.DBHelper db = new Data.DBHelper();
            if (db.exist(sql_select))
            {
                if (db.updateDB(sql_update))
                {
                    MessageBox.Show("保存成功");
                }else
                {
                    MessageBox.Show("保存失败");
                }
            }
            else
            {
                if (db.updateDB(sql_insert))
                {
                    MessageBox.Show("保存成功");
                }
                else
                {
                    MessageBox.Show("保存失败");
                }
            }
        }
    }
}
