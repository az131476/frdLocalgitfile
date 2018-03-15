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

namespace WindowsFormsApplication1
{
    public partial class Form1 : Office2007Form
    {
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
            SocketClient client = new SocketClient();
            client.socketConnect();
            #endregion
            #region 请求获取服务器初始化设备参数，并加载界面参数列表

            #endregion

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
    }
}
