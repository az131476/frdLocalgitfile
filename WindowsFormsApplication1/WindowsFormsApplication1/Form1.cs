using System;
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
        public PointPairList list = new PointPairList();
        public PointPairList list2 = new PointPairList();
        public PointPairList list3 = new PointPairList();
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
            #region 启动Socket与通讯服务建立连接
            Thread td_socket = new Thread(new ThreadStart(socketConnect));
            td_socket.IsBackground = true;
            td_socket.Start();
            #endregion
            #region 请求获取服务器初始化设备参数，并加载界面参数列表并保存

            #endregion
            if (!flg)
            {
                tabItem7.Visible = false;
                tabItem.Visible = true;
            }
            ZedgraphShapInit();
            string key = "Users";
            //RedisBase.Core.FlushAll();
            //RedisBase.Core.AddItemToList(key, "cuiyanwei");
            //RedisBase.Core.AddItemToList(key, "xiaoming");
            //RedisBase.Core.Add<string>("mykey", "123456");
            //RedisString.Set("mykey1", "abcdef");
            //string str3 = RedisString.Get("mykey1");
            //Debug.Write("str3:" + str3);
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
            myCurve2 = zedGraphControl2.GraphPane.AddCurve("y", list2, Color.Red, SymbolType.Circle);
            this.zedGraphControl2.AxisChange();
            this.zedGraphControl2.Refresh();

            zedGraphControl2.IsShowPointValues = true;
            zedGraphControl2.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

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
            ///设置鼠标拖动
            zedGraphControl3.PanModifierKeys = Keys.None;//鼠标左键，随意拖动
                                                         // Move the legend location
            myPane3.Legend.Position = ZedGraph.LegendPos.Top;
            myCurve3 = zedGraphControl3.GraphPane.AddCurve("z", list3, Color.Blue, SymbolType.Circle);
            this.zedGraphControl3.AxisChange();
            this.zedGraphControl3.Refresh();
            zedGraphControl3.IsShowPointValues = true;
            zedGraphControl3.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
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
        }

        private void buttonX7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTabIndex = 3;
            tabItem.Text = "存储设置";
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

            //Thread td3 = new Thread(zgraph);
            //td3.IsBackground = true;
            //td3.Start();
        }
        private void xgraph()
        {
            while (true)
            {
                MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["myStr"].ToString());
                //UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=2
                string sql = "select channel_id,`data`,dtime,currTime,id,MICROSECOND(DATE_FORMAT(NOW(),'%Y-%m-%d %T.%f')),MICROSECOND(DATE_FORMAT(currTime,'%Y-%m-%d %T.%f')) from datatest where MICROSECOND(DATE_FORMAT(NOW(),'%Y-%m-%d %T.%f'))-MICROSECOND(DATE_FORMAT(currTime,'%Y-%m-%d %T.%f'))<2000 and channel_id='0' and state = '0' ORDER BY  id asc limit 1";
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
                        string t2 = reader[6].ToString();
                        if (channel_id.Equals("0"))
                        {
                            list.Add(double.Parse(dtime), double.Parse(data));
                            this.zedGraphControl1.AxisChange();
                            this.zedGraphControl1.Refresh();
                            Debug.Write1(dtime+","+data+","+currtime+","+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+","+t1+","+t2);
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
                string sql = "select channel_id,`data`,dtime,currTime from datatest ";// where UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)=2 ORDER BY  id asc";
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

                        if (channel_id.Equals("0"))
                        {
                            list2.Add(double.Parse(dtime), double.Parse(data));
                            this.zedGraphControl2.AxisChange();
                            this.zedGraphControl2.Refresh();
                        }

                        Thread.Sleep(100);
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
            string path3 = AppDomain.CurrentDomain.BaseDirectory + "\\log";
            string filePath3 = path3 + "\\" + "d3" + ".txt";
            if (File.Exists(filePath3))
            {
                FileStream fs = new FileStream(filePath3, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                //FileStream fs = new FileStream(filePath3, FileMode.Open);
                StreamReader sr = new StreamReader(fs, ASCIIEncoding.Default);
                string str = string.Empty;
                while (true)
                {
                    str = sr.ReadLine();
                    if (!string.IsNullOrEmpty(str))
                    {
                        double s = double.Parse(str.Substring(0, str.IndexOf(',')));
                        int len = str.IndexOf(',');
                        str = str.Substring(len + 1, str.Length - len - 1);
                        double y = double.Parse(str.Substring(0, str.Length));
                        list3.Add(s, y);
                        this.zedGraphControl3.AxisChange();
                        this.zedGraphControl3.Refresh();
                    }
                    else
                    {
                        sr.Close();
                        fs.Close();
                        break;
                    }
                    Thread.Sleep(100);
                }
            }
            else
            {

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
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
            this.zedGraphControl2.AxisChange();
            this.zedGraphControl2.Refresh();
            this.zedGraphControl3.AxisChange();
            this.zedGraphControl3.Refresh();
        }
    }
}
