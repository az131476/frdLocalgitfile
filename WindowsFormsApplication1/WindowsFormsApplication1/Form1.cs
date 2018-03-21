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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Office2007Form
    {
        SocketClient client = new SocketClient();
        PointPairList list = new PointPairList();
        PointPairList list2 = new PointPairList();
        LineItem myCurve;
        LineItem myCurve2;
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
            #region 请求获取服务器初始化设备参数，并加载界面参数列表
            #endregion
            //zedgraph
            ZedgraphShapInit();
        }
        private void socketConnect()
        {
            while(true)
            {
                if (client.socketConnect())
                {
                    Debug.Write("connection success!");
                    break;
                }
                Thread.Sleep(1000);
            }
            
        }
        private void initCompoents()
        {
            /*设置背景
             */ 
            StyleManager sytleBackGround = new StyleManager();
            sytleBackGround.ManagerStyle = eStyle.Office2010Blue;

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

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        /// <summary>
        /// 波形测试数据
        /// </summary>
        private void LoadData()
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
                    Debug.Write("s0:" + s0);
                    Debug.Write("x0:" + x0);

                    list.Add(Double.Parse(s0), double.Parse(x0));
                    list2.Add(double.Parse(s0), double.Parse(y0));
                    this.zedGraphControl1.AxisChange();
                    this.zedGraphControl1.Refresh();
                    this.zedGraphControl2.AxisChange();
                    this.zedGraphControl2.Refresh();

                    Thread.Sleep(200);
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
        private void ZedgraphShapInit()
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
            myCurve = zedGraphControl1.GraphPane.AddCurve("x", list, Color.Blue, SymbolType.Circle);

            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "";
            myPane2.XAxis.Title.Text = "时间";
            myPane2.YAxis.Title.Text = "采集值";
            myPane2.XAxis.Type = ZedGraph.AxisType.Linear;

            // Add gridlines to the plot, and make them gray
            myPane2.XAxis.MajorGrid.IsVisible = true;
            myPane2.YAxis.MajorGrid.IsVisible = true;
            myPane2.XAxis.MajorGrid.Color = Color.LightGray;
            myPane2.YAxis.MajorGrid.Color = Color.LightGray;
            // Move the legend location
            myPane2.Legend.Position = ZedGraph.LegendPos.Top;
            myCurve2 = zedGraphControl2.GraphPane.AddCurve("y", list2, Color.Red, SymbolType.Circle);

            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
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
    }
}
