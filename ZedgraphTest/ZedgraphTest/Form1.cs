using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;

namespace ZedgraphTest
{
    public partial class Form1 : Form
    {
        Random ran = new Random();
        PointPairList list = new PointPairList();
        PointPairList list2 = new PointPairList();
        LineItem myCurve;
        LineItem myCurve2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            SingleShapInit();
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
        }
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,CurveItem curve, int iPt)
        {
            PointPair pt = curve[iPt];
            string xypoint = "横坐标:" + pt.X.ToString() + " 纵坐标:" + pt.Y.ToString();
            MessageBox.Show(xypoint);
            return xypoint;
        }
        /// <summary>
        /// 单个波形
        /// </summary>
        private void SingleShapInit()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "波形图";
            myPane.XAxis.Title.Text = "时间";
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
            myCurve = zedGraphControl1.GraphPane.AddCurve("x",list, Color.Blue, SymbolType.Circle);

            GraphPane myPane2 = zedGraphControl2.GraphPane;
            myPane2.Title.Text = "波形图";
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
        }
        /// <summary>
        /// 多个波形绘制
        /// </summary>
        private void MasterPaneInit()
        {
            MasterPane myPane = zedGraphControl1.MasterPane;
            myPane.Title.Text = "masterPane";
            
            //myPane.XAxis.Title.Text = "时间";
            //myPane.YAxis.Title.Text = "采集值";
            //myPane.XAxis.Type = ZedGraph.AxisType.Linear;
            myPane.Fill = new Fill(Color.White, Color.MediumSlateBlue, 45.0F);

            myPane.Title.FontSpec.FontColor = Color.Green;
            myPane.Margin.Right = 20;
            myPane.Margin.Top = 20;


            // Enable the masterpane legend
            //myPane.Legend.IsVisible = true;
            //myPane.Legend.Position = LegendPos.Top;
            

            for (int j = 0; j < 1; j++)
            {
                GraphPane pane = new GraphPane();
                pane.Title.Text = "one";
                pane.XAxis.Title.Text = "X";
                pane.YAxis.Title.Text = "Y";
                pane.Margin.Top = 30;
                pane.Margin.Bottom = 10;
                pane.Margin.Right = 10;
                myCurve = pane.AddCurve("two", list, Color.DarkGreen, SymbolType.Square);
                myPane.Add(pane);
            }
        }
        private void LoadData()
        {
            string sql = "SELECT s,x,y,z FROM datatest limit 100";
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
                    Debug.Write("s0:"+s0);
                    Debug.Write("x0:"+x0);

                    list.Add(Double.Parse(s0),double.Parse(x0));
                    list2.Add(double.Parse(s0),double.Parse(y0));
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
        /// <summary>
        /// 加载单个波形
        /// </summary>
        private void ZedGraphSet()
        {
            this.zedGraphControl1.GraphPane.Title.Text = "波形图";
            this.zedGraphControl1.GraphPane.XAxis.Title.Text = "时间";
            this.zedGraphControl1.GraphPane.YAxis.Title.Text = "采集值";
            this.zedGraphControl1.GraphPane.XAxis.Type = ZedGraph.AxisType.Ordinal;

            //for (int i = 0; i <= 100; i++)
            //{
            //    double x = (double)new XDate(DateTime.Now.AddSeconds(-(100 - i)));
            //    double y = ran.NextDouble();
            //    list.Add(x, y);
            //}
            DateTime dt = DateTime.Now;

            myCurve = zedGraphControl1.GraphPane.AddCurve("My Curve", list, Color.DarkGreen, SymbolType.Circle);

            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            zedGraphControl1.GraphPane.XAxis.Scale.MaxAuto = true;
            double x = ran.NextDouble();
            double y = Math.Sin(x);
            list.Add(x, y);
            this.zedGraphControl1.AxisChange();
            this.zedGraphControl1.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void zedGraphControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
