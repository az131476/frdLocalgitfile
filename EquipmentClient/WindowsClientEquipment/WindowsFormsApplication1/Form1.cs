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
using WinformTest;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Office2007Form
    {
        #region equipment params
        private static List<WinformTest.Control.GroupChannel> m_listGroupChannel = new List<WinformTest.Control.GroupChannel>();
        private static List<WinformTest.Control.HardChannel> m_listHardChannel = new List<WinformTest.Control.HardChannel>();
        private static Dictionary<int, int> m_dirBufferIndex = new Dictionary<int, int>();
        private static List<string> m_listFreq = new List<string>(); //采样频率集
        private static List<string> m_CommboChan = new List<string>();//通道列表
        private Thread sample_thread;
        private bool thread_state = false;
        private List<string> data_list = new List<string>();
        WinformTest.Control.SampleParam sampleParam = new WinformTest.Control.SampleParam();
        DataTable dtx = new DataTable();
        DataTable dty = new DataTable();
        DataTable dtz = new DataTable();
        bool threadFlg = false;
        string freq = "20";
        
        #endregion
        HardWare hardWare = new HardWare();
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
            #endregion
            InitToClient();
            ZedgraphShapInit();
        }
        #region 仪器部分
        /// <summary>
        /// 连接仪器
        /// </summary>
        public void InitToClient()
        {
            #region////初始化仪器
            if (!InitInterface())
            {
                //失败
                //pictureBox1.BackColor = Color.Red;
                Debug.Write("初始化失败");
                return;
            }
            else
            {
                //pictureBox1.BackColor = Color.Blue;
                Debug.Write("初始化成功");
            }
            #endregion
            #region 与所有在线仪器建立连接

            int nReturnValue;
            hardWare.GetHardWare().ReConnectAllMac(out nReturnValue);//连接在DeviceInfo.ini文件的IP的仪器。IsConnectMachine,重新连接所有仪器
            //axDHTestHardWare.WireLessChange(13124007);
            if (!ConAllDevice())
            {

                Debug.Write("仪器连接失败");
                //pictureBox2.BackColor = Color.Red;
                return;
            }
            else
            {
                Debug.Write("仪器连接成功");
                //pictureBox2.BackColor = Color.Blue;
            }
            #endregion
            #region 初始化设备参数：获取通道组数据
            //设置默认初始参数
            GetAllGroupMsg();
            #endregion
            #region 启动采样
            startSample();
            #endregion
        }
        #region 加载通道组信息
        public void GetAllGroupMsg()
        {
            ClearAllGroupChannel();
            int i = 0, j = 0;
            int nGroupCount;//Form1.GroupCount();//通道组数量-即仪器连接的数量
            hardWare.GetHardWare().GetChannelGroupCount(out nGroupCount);
            int nGroupChannelID, nChannelFirst, nChannelNumber, nDataType;
            string strMachineIP;
            int nReturnValue = 0;
            for (i = 0; i < nGroupCount; i++)
            {
                //获取当前采样参数
                float fltSampleFreq;
                int nSampleMode, nTrigMode, nBlockSize, nDelayCount;

                hardWare.GetHardWare().GetSampleFreq(out fltSampleFreq);
                hardWare.GetHardWare().GetSampleMode(out nSampleMode);
                hardWare.GetHardWare().GetSampleTrigMode(out nTrigMode);
                hardWare.GetHardWare().GetTrigBlockCount(out nBlockSize);
                hardWare.GetHardWare().GetTrigDelayCount(out nDelayCount);

                //获取采样频率可选项
                string strFrepList = "";
                //1-瞬态、2-连续
                int SampleMode = 2;
                hardWare.GetHardWare().GetSampleFreqList(SampleMode, out strFrepList);

                WinformTest.Control.GroupChannel stuGroupChannel = new WinformTest.Control.GroupChannel();
                // 获取通道组信息
                hardWare.GetHardWare().GetChannelGroup(i, out nGroupChannelID, out strMachineIP, out nReturnValue);
                stuGroupChannel.m_GroupID = nGroupChannelID; //组ID
                stuGroupChannel.m_strMachineIP = strMachineIP;//仪器IP

                // 获取某台仪器的起始通道ID 0-15
                hardWare.GetHardWare().GetChannelFirstID(nGroupChannelID, strMachineIP, out nChannelFirst);
                stuGroupChannel.m_nChannelFirst = nChannelFirst;//0

                // 获取某台仪器的总的通道数
                hardWare.GetHardWare().GetChannelCount(nGroupChannelID, strMachineIP, out nChannelNumber);
                stuGroupChannel.m_nChannelNumber = nChannelNumber;

                // 获取某台仪器的数据类型
                hardWare.GetHardWare().GetChannelGroupDataType(nGroupChannelID, strMachineIP, out nDataType);
                stuGroupChannel.m_nDataType = nDataType; //0

                m_listGroupChannel.Add(stuGroupChannel);

                int bOnLine = 1, nMeasureType = 0;
                // 通道信息
                string channel_id = "";
                for (j = 0; j < nChannelNumber; j++)
                {
                    WinformTest.Control.HardChannel HardChannel = new WinformTest.Control.HardChannel();
                    int nChannelID = nChannelFirst + j;

                    hardWare.GetHardWare().IsChannelOnLine(nGroupChannelID, strMachineIP, nChannelID, out bOnLine);
                    hardWare.GetHardWare().GetChannelMeasureType(nGroupChannelID, strMachineIP, nChannelID, out nMeasureType);
                    HardChannel.m_nChannelGroupID = nGroupChannelID;
                    HardChannel.m_nChannelID = nChannelID;//通道ID
                    HardChannel.m_nMeasureType = nMeasureType;//测量类型
                    HardChannel.m_bOnlineFlag = (bOnLine == 1 ? true : false);

                    HardChannel.m_strMachineIP = strMachineIP;
                    m_listHardChannel.Add(HardChannel);

                    ///通道列表
                    //GetChannelCombo();//界面显示
                    ///频率列表
                    //GetSampleFreqList();//界面显示
                    ///采样参数
                    //GetSampleParam();

                    if (bOnLine == 1)
                    {
                        channel_id += nChannelID + "\r\n";

                        //nGroupCount   equip_id
                        //nReturnValue  0/1
                        //strMachineIP  ip
                        //nGroupChannelID group_id
                        //nChannelFirst 0
                        //nChannelNumber
                        //保存到数据库
                        string sql = "insert into all_channelgroup_message(machineID,`online`, machineIP, groupCount, returnValue, channelFirst, datatype, channelNumber, channelID, channelgroupID, measuretype,flg,curr_frequency,frequency_list)";
                        sql += "VALUES('" + nGroupCount + "', '" + bOnLine + "', '" + strMachineIP + "', '" + nGroupCount + "', '" + nReturnValue + "', '" + nChannelFirst + "', '" + nDataType + "', '" + nChannelNumber + "', '" + nChannelID + "', '" + nGroupChannelID + "', '" + nMeasureType + "','N','" + fltSampleFreq + "','" + strFrepList + "')";
                        WinformTest.Data.OperatData opear = new WinformTest.Data.OperatData();
                        opear.paramsSave(sql);
                    }
                }
            }
            GetBufferIndex();
        }
        private static void ClearAllGroupChannel()
        {
            m_listGroupChannel.Clear();
            m_listHardChannel.Clear();
            WinformTest.Data.OperatData opear = new WinformTest.Data.OperatData();
            string sqldel = "DELETE from all_channelgroup_message";
            opear.delete(sqldel);
        }
        /// <summary>
        /// 仪器中的数据按照仪器ID排列，由小到大
        /// </summary>
        private static void GetBufferIndex()
        {
            List<int> listMachineID = new List<int>();
            for (int i = 0; i < m_listGroupChannel.Count; i++)
            {
                listMachineID.Add(m_listGroupChannel[i].m_GroupID);
            }

            List<int> sortID = new List<int>();
            for (int i = 0; i < listMachineID.Count; i++)
            {
                bool bFind = false;

                foreach (var nVal in sortID)
                {
                    if (nVal > listMachineID[i])
                    {
                        bFind = true;
                        sortID.Add(listMachineID[i]);
                        break;
                    }
                }
                if (!bFind)
                    sortID.Add(listMachineID[i]);
            }

            m_dirBufferIndex.Clear();
            for (int i = 0; i < sortID.Count; i++)
            {
                if (m_dirBufferIndex.ContainsKey(sortID[i]))
                    m_dirBufferIndex[sortID[i]] = i;
                else
                    m_dirBufferIndex.Add(sortID[i], i);
            }
        }

        #endregion
        #region 采样
        private void startSample()
        {
            SetSampleParam();
            //SetSampleDialog(false);
            int nIsSampling;
            //是否正在采集数据
            hardWare.GetHardWare().IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                MessageBox.Show("仪器采样中，请先停止采样!");
                return;
            }

            int nSample;
            //启动采样
            //axDHTestHardWare.StartSample("DH3820", 0, 1024, out nSample);
            hardWare.GetHardWare().StartSample("DH5902", 0, 1024, out nSample);
            if (nSample == 1)
            {
                WinformTest.Log.Debug.Write("开始采样");

            }
            else
            {
                MessageBox.Show("失败");
            }

            //启动取数线程
            threadFlg = true;

            sample_thread = new Thread(new ThreadStart(GetDataThread));
            sample_thread.SetApartmentState(ApartmentState.STA);
            sample_thread.IsBackground = true;
            sample_thread.Name = "GetData";
            sample_thread.Start();
        }
        /// <summary>
        /// 设置采样参数
        /// </summary>
        private void SetSampleParam()
        {
            int nReturnValue;
            string strText;
            strText = freq;//comboBoxEx1.SelectedItem.ToString(); //comboBoxEx1.SelectedItem.ToString();  //默认采样频率10
            float fltSampleFrequency = float.Parse(strText);
            sampleParam.m_fltSampleFrequency = fltSampleFrequency;

            //设置采样参数
            hardWare.GetHardWare().SetSampleFreq(sampleParam.m_fltSampleFrequency, out nReturnValue);
            hardWare.GetHardWare().SetSampleMode(sampleParam.m_nSampleMode, out nReturnValue);
            hardWare.GetHardWare().SetSampleTrigMode(sampleParam.m_nSampleTrigMode, out nReturnValue);
            hardWare.GetHardWare().SetTrigBlockCount(sampleParam.m_nSampleBlockSize, out nReturnValue);
            hardWare.GetHardWare().SetTrigDelayCount(sampleParam.m_nSampleDelayPoints, out nReturnValue);
        }
        private void GetDataThread()
        {
            ///仪器数据
            dtx.Columns.Add("datas");
            dtx.Columns.Add("times");
            dtx.Columns.Add("currtime");
            dtx.Columns.Add("id");

            dty.Columns.Add("datas");
            dty.Columns.Add("times");
            dty.Columns.Add("currtime");
            dty.Columns.Add("id");

            dtz.Columns.Add("datas");
            dtz.Columns.Add("times");
            dtz.Columns.Add("currtime");
            dtz.Columns.Add("id");
            //一台仪器
            Thread equip1_td = new Thread(GetEquipmentData1);
            equip1_td.IsBackground = true;
            equip1_td.Start();
        }
        private void GetEquipmentData1()
        {
            // 查询所有仪器
            for (int i = 0; i < m_listGroupChannel.Count; i++)
            {
                WinformTest.Control.GroupChannel GroupChannel = m_listGroupChannel[i];
                int nChannelGroupID = GroupChannel.m_GroupID;
                string equip_ip1 = ConfigurationManager.AppSettings["equip_ip1"].ToString();
                //string equip_ip2 = ConfigurationManager.AppSettings["equip_ip2"].ToString();

                if (GroupChannel.m_strMachineIP.Equals(equip_ip1))
                {
                    ///three channel
                    Thread x = new Thread(GetX);
                    x.IsBackground = true;
                    x.Start();
                    Thread y = new Thread(GetY);
                    y.IsBackground = true;
                    y.Start();

                    Thread z = new Thread(GetZ);
                    z.IsBackground = true;
                    z.Start();
                }
            }
        }
        public void GetX()
        {
            // 获取每个通道的数据 默认数据类型为float
            string time_ns = "";
            double temp = 0;
            int count = 1;
            while (true)
            {
                int nSelGroupID, nSelChanID;
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                hardWare.GetHardWare().GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);

                if (nReceiveCount <= 0)
                    continue;
                float[] pfltData;
                if (threadFlg)
                {
                    nSelChanID = 0;
                }
                pfltData = (float[])oChnData;

                float[] pChanData = new float[nReceiveCount];
                for (int nCount = 0; nCount < nReceiveCount; nCount++)
                {
                    pChanData[nCount] = pfltData[nCount];

                    string strData = String.Format("{0:f3}", pChanData[nCount]);
                    time_ns = String.Format("{0:f4}", ((double)nTotalDataPos / int.Parse(freq)));
                    //string time_ns2 = temp+(double.Parse(time_ns)/nReceiveCount) * (nCount+1)+"";
                    double tem = ((double.Parse(time_ns) - temp) / nReceiveCount);

                    double time_ns3 = temp + tem * (nCount + 1);
                    string time_ns4 = String.Format("{0:f4}", time_ns3);

                    DataRow dr = dtx.NewRow();
                    dr["datas"] = strData;
                    dr["times"] = time_ns4;
                    dr["currtime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["id"] = count;
                    dtx.Rows.Add(dr);
                    //AnlySampleData(strData, nTotalDataPos, time_ns4.ToString(), 0, 0, nSelChanID, nCount, nReceiveCount, nSelChanID);
                    //Log.Debug.WriteErr("channelid:" + nSelChanID + " nreceive:" + nReceiveCount + "time_ns:" + time_ns + " times_ns3:" + time_ns4 + " strdata:" + strData + " ntotal:" + nTotalDataPos);
                    count++;
                    Debug.Write("count1:" + count);
                }
                temp = double.Parse(time_ns);
                Thread.Sleep(100);
            }
        }
        public void GetY()
        {
            // 获取每个通道的数据 默认数据类型为float
            string time_ns = "";
            double temp = 0;
            int count = 1;
            while (true)
            {
                int nSelGroupID, nSelChanID;
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                hardWare.GetHardWare().GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);

                if (nReceiveCount <= 0)
                    continue;
                float[] pfltData;
                if (threadFlg)
                {
                    nSelChanID = 1;
                }
                pfltData = (float[])oChnData;

                float[] pChanData = new float[nReceiveCount];
                for (int nCount = 0; nCount < nReceiveCount; nCount++)
                {
                    pChanData[nCount] = pfltData[nCount];

                    string strData = String.Format("{0:f3}", pChanData[nCount]);
                    time_ns = String.Format("{0:f4}", ((double)nTotalDataPos / int.Parse(freq)));
                    //string time_ns2 = temp+(double.Parse(time_ns)/nReceiveCount) * (nCount+1)+"";
                    double tem = ((double.Parse(time_ns) - temp) / nReceiveCount);

                    double time_ns3 = temp + tem * (nCount + 1);
                    string time_ns4 = String.Format("{0:f4}", time_ns3);

                    DataRow dr = dty.NewRow();
                    dr["datas"] = strData;
                    dr["times"] = time_ns4;
                    dr["currtime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["id"] = count;
                    dty.Rows.Add(dr);
                    //AnlySampleData(strData, nTotalDataPos, time_ns4.ToString(), 0, 0, nSelChanID, nCount, nReceiveCount, nSelChanID);
                    //Log.Debug.WriteErr("channelid:" + nSelChanID + " nreceive:" + nReceiveCount + "time_ns:" + time_ns + " times_ns3:" + time_ns4 + " strdata:" + strData + " ntotal:" + nTotalDataPos);
                    count++;
                    Debug.Write2("count2:" + count);
                }
                temp = double.Parse(time_ns);
                Thread.Sleep(100);
            }
        }
        public void GetZ()
        {
            // 获取每个通道的数据 默认数据类型为float
            string time_ns = "";
            double temp = 0;
            int count = 1;
            while (true)
            {
                int nSelGroupID, nSelChanID;
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                hardWare.GetHardWare().GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);

                if (nReceiveCount <= 0)
                    continue;
                float[] pfltData;
                if (threadFlg)
                {
                    nSelChanID = 2;
                }
                pfltData = (float[])oChnData;

                float[] pChanData = new float[nReceiveCount];
                for (int nCount = 0; nCount < nReceiveCount; nCount++)
                {
                    pChanData[nCount] = pfltData[nCount];

                    string strData = String.Format("{0:f3}", pChanData[nCount]);
                    time_ns = String.Format("{0:f4}", ((double)nTotalDataPos / int.Parse(freq)));
                    //string time_ns2 = temp+(double.Parse(time_ns)/nReceiveCount) * (nCount+1)+"";
                    double tem = ((double.Parse(time_ns) - temp) / nReceiveCount);

                    double time_ns3 = temp + tem * (nCount + 1);
                    string time_ns4 = String.Format("{0:f4}", time_ns3);

                    DataRow dr = dtz.NewRow();
                    dr["datas"] = strData;
                    dr["times"] = time_ns4;
                    dr["currtime"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dr["id"] = count;
                    dtz.Rows.Add(dr);
                    //AnlySampleData(strData, nTotalDataPos, time_ns4.ToString(), 0, 0, nSelChanID, nCount, nReceiveCount, nSelChanID);
                    //Log.Debug.WriteErr("channelid:" + nSelChanID + " nreceive:" + nReceiveCount + "time_ns:" + time_ns + " times_ns3:" + time_ns4 + " strdata:" + strData + " ntotal:" + nTotalDataPos);
                    count++;
                    Debug.Write3("count2:" + count);
                }
                temp = double.Parse(time_ns);
                Thread.Sleep(100);

            }
        }
        ///<summary>
        ///处理采样数据
        /// </summary>
        public void AnlySampleData(string data, int nTotalDataPos, string time_ns, int nChannelGroupID, int nSelGroupID, int nSelChanID, int nCount, int nReceiveCount, int chanid)
        {
            //strData:-164.1896  nTotalDataPos:3158  time_ns:315.0000
            //${123.00,7849,32.5432}
            //仪器ID，通道组ID，通道组对应通道ID

            List<ArraySegment<byte>> list = new List<ArraySegment<byte>>();
            List<ArraySegment<byte>> len_list = new List<ArraySegment<byte>>();
            //
            string d1 = "d1" + nChannelGroupID.ToString().Length + nChannelGroupID.ToString();
            string d2 = "d2" + nSelGroupID.ToString().Length + nSelGroupID.ToString();
            string d3 = "d3" + nSelChanID.ToString().Length + nSelChanID.ToString();
            string d4 = "d4" + nCount.ToString().Length + nCount.ToString();
            string d5 = "d5" + nReceiveCount.ToString().Length + nReceiveCount.ToString();
            string d6 = "d6" + data.ToString().Length + data.ToString();
            string d7 = "d7" + nTotalDataPos.ToString().Length + nTotalDataPos.ToString();
            string d8 = "d8" + time_ns.Length + time_ns.ToString();
            string nd = d1 + d3 + d6 + d8;
            string n_len = "L" + nd.Length.ToString() + "N";
            string allData = d1 + d2 + d3 + d4 + d5 + d6 + d7 + d8;
            string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into datatest (channel_id,data,dtime,currTime) values('" + nSelChanID + "','" + data + "','" + time_ns + "','" + currTime + "')";
            new WinformTest.Data.OperatData().paramsSave(sql);
            string sql_del = "DELETE from datatest where UNIX_TIMESTAMP(NOW())-UNIX_TIMESTAMP(currTime)>60 and channel_id='" + chanid + "'";
            new WinformTest.Data.OperatData().paramsSave(sql_del);

            #region
            /// 二进制存储原始数据
            /// <summary>
            /// </summary>
            string filename = DateTime.Now.ToString("yyyy-MM-dd") + "_" + nChannelGroupID + nSelChanID + ".txt";
            WinformTest.Log.Biwriter.SaveFile(data, filename);
            #endregion
            ///转发给算法部分分析处理
            //new WinformTest.Arithmetic.AirthmeticData().ArithmeticDeal(nChannelGroupID, nSelGroupID, nSelChanID, time_ns, data);
            #region
            //Log.Debug.Write(allData);
            //SendSampleData(nd);
            #endregion

            #region 

            #endregion
        }
        #endregion
        #endregion
        public bool InitInterface()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string strPath = baseDir.Substring(0, baseDir.LastIndexOf('\\'));//D:\work\1-dhmonitor\bin\debug\Config\
            strPath += "\\config\\";
            int nReturnValue;
            //初始化仪器控制接口
            hardWare.GetHardWare().Init(strPath, "chinese", out nReturnValue);

            return nReturnValue == 1 ? true : false;
        }
        /// <summary>
        /// 检查仪器是否连接
        /// </summary>
        /// <returns></returns>
        public bool ConAllDevice()
        {
            int nReturnValue;
            hardWare.GetHardWare().IsConnectMachine(out nReturnValue);
            return nReturnValue == 1 ? true : false;
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
            LoadGraphData();
        }
        private void LoadGraphData()
        {
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
            myCurve3 = zedGraphControl3.GraphPane.AddCurve("z", list3, Color.Red, SymbolType.None);
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
        private void xgraph()
        {
            int id = 0;
            int curid = 0;
            while (true)
            {
                for (int i = curid; i < dtx.Rows.Count; i++)
                {
                    if (threadFlg)
                    {
                        string data = dtx.Rows[i]["datas"].ToString();
                        string time = dtx.Rows[i]["times"].ToString();
                        id = int.Parse(dtx.Rows[i]["id"].ToString());
                        list.Add(double.Parse(time), double.Parse(data));
                        this.zedGraphControl1.AxisChange();
                        this.zedGraphControl1.Refresh();
                        Debug.Write1("id:" + id + " curid:" + curid);
                        //dt.Rows[i-1].Delete();
                        if (list.Count > 2000)
                        {
                            list.RemoveAt(0);
                        }
                    }
                }
                curid = id;
                Thread.Sleep(1000);
            }
        }
        private void ygraph() {
            int id = 0;
            int curid = 0;
            while (true)
            {
                for (int i = curid; i < dty.Rows.Count; i++)
                {
                    if (threadFlg)
                    {
                        string data = dty.Rows[i]["datas"].ToString();
                        string time = dty.Rows[i]["times"].ToString();
                        id = int.Parse(dty.Rows[i]["id"].ToString());
                        list2.Add(double.Parse(time), double.Parse(data));
                        this.zedGraphControl2.AxisChange();
                        this.zedGraphControl2.Refresh();
                        //dt.Rows[i-1].Delete();
                        Debug.Write2("id:" + id + " curid:" + curid);
                        if (list2.Count > 2000)
                        {
                            list2.RemoveAt(0);
                        }
                    }
                }
                curid = id;
                Thread.Sleep(1000);
            }
        }
        private void zgraph() {
            int id = 0;
            int curid = 0;
            while (true)
            {
                for (int i = curid; i < dtz.Rows.Count; i++)
                {
                    if (threadFlg)
                    {
                        string data = dtz.Rows[i]["datas"].ToString();
                        string time = dtz.Rows[i]["times"].ToString();
                        id = int.Parse(dtz.Rows[i]["id"].ToString());
                        list3.Add(double.Parse(time), double.Parse(data));
                        this.zedGraphControl2.AxisChange();
                        this.zedGraphControl2.Refresh();
                        //dt.Rows[i-1].Delete();
                        Debug.Write3("id:" + id + " curid:" + curid);
                        if (list3.Count > 2000)
                        {
                            list3.RemoveAt(0);
                        }
                    }
                }
                curid = id;
                Thread.Sleep(1000);
            }
        }
        private void HomeBack()
        {
            tabControl1.SelectedTabIndex = 5;
        }

        private void buttonX2_Click(object sender, EventArgs e)
        {

        }
        private void buttonX11_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void buttonX12_Click(object sender, EventArgs e)
        {
            HomeBack();
        }
        #endregion

        private void pToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //暂停波形绘制
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
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pflg = true;
        }

        private void backToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void clearxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list.Clear();
        }

        private void clearyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list2.Clear();
        }

        private void clearzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            list3.Clear();
        }

        private void backToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void backToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void backToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            HomeBack();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //停止采样
            int nIsSampling;
            //是否正在采集数据
            hardWare.GetHardWare().IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                // 停止采样线程
                threadFlg = false;
                dtx.Clear();
                list.Clear();
                int nStopSample;
                ////停止采样
                hardWare.GetHardWare().StopSample(out nStopSample);
            }
            Thread.Sleep(400);
            hardWare.GetHardWare().IsSampling(out nIsSampling);
            if (nIsSampling == 0)
            {
                MessageBox.Show("已停止");
            }
        }
        private void playToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            InitToClient();
            LoadGraphData();
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //以当前参数采样
            freq = comboBoxEx1.SelectedItem.ToString();
            string sampleModel = comboBoxEx2.SelectedItem.ToString();
        }

        private void balanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
            int nIsSampling;
            hardWare.GetHardWare().IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                MessageBox.Show("请先停止采样");
            }
            else
            {///采样停止时通道平衡
                int nReturnValue;
                hardWare.GetHardWare().AllChannelBalance(out nReturnValue);
                if (nReturnValue == 1)
                {
                    MessageBox.Show("所有通道平衡");
                }
                else { MessageBox.Show("平衡失败"); }
            }
            
        }
    }
}
