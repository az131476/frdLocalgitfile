using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using DevComponents.DotNetBar;
using MySql.Data.MySqlClient;

namespace WinformTest
{
    public partial class Form1 : Office2007Form
    {
        public static HardWare hardWare = new HardWare();
        private static List<Control.GroupChannel> m_listGroupChannel = new List<Control.GroupChannel>();
        private static List<Control.HardChannel> m_listHardChannel = new List<Control.HardChannel>();
        private static Dictionary<int, int> m_dirBufferIndex = new Dictionary<int, int>();
        private static List<string> m_listFreq = new List<string>(); //采样频率集
        private static List<string> m_CommboChan = new List<string>();//通道列表
        private Thread sample_thread;
        private bool thread_state = false;
        private List<string> data_list = new List<string>();
        Control.SampleParam sampleParam = new Control.SampleParam();
        Control.SocketServer socket = new Control.SocketServer();
        string time_ns;
        double temp;
        double time_last;
        public Form1()
        {
            InitializeComponent();
            this.EnableGlass = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            #region 启动socket服务,监听客户端
            socket.server();
            #endregion
            #region////初始化仪器
            if (!InitInterface())
            {
                //失败
                //pictureBox1.BackColor = Color.Red;
                Log.Debug.WriteErr("初始化失败");
                return;
            }
            else
            {
                //pictureBox1.BackColor = Color.Blue;
                Log.Debug.Write("初始化成功");
            }
            #endregion
            #region 与所有在线仪器建立连接

            int nReturnValue;
            hardWare.GetHardWare().ReConnectAllMac(out nReturnValue);//连接在DeviceInfo.ini文件的IP的仪器。IsConnectMachine,重新连接所有仪器
            //axDHTestHardWare.WireLessChange(13124007);
            if (!Control.DeviceConnect.ConAllDevice())
            {

                Log.Debug.WriteErr("仪器连接失败");
                //pictureBox2.BackColor = Color.Red;
                return;
            }
            else
            {
                Log.Debug.Write("仪器连接成功");
                //pictureBox2.BackColor = Color.Blue;
            }
            #endregion

            #region 初始化设备参数：获取通道组数据
            //设置默认初始参数
            GetInitParams();
            #endregion
            #region 启动采样
            startSample();
            #endregion
        }
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
               // MessageBox.Show("开始采样");
                
            }
            else
            {
                MessageBox.Show("失败");
            }

            //启动取数线程
            thread_state = true;

            sample_thread = new Thread(GetDataThread);
            sample_thread.SetApartmentState(ApartmentState.STA);
            sample_thread.IsBackground = true;
            sample_thread.Name = "GetData";
            sample_thread.Start(this);
        }
        /// <summary>
        /// 设置采样参数
        /// </summary>
        private void SetSampleParam()
        {
            int nReturnValue;
            string strText;
            strText = "20"; //comboBoxEx1.SelectedItem.ToString();  //默认采样频率10
            float fltSampleFrequency = float.Parse(strText);
            sampleParam.m_fltSampleFrequency = fltSampleFrequency;

            //设置采样参数
            hardWare.GetHardWare().SetSampleFreq(sampleParam.m_fltSampleFrequency, out nReturnValue);
            hardWare.GetHardWare().SetSampleMode(sampleParam.m_nSampleMode, out nReturnValue);
            hardWare.GetHardWare().SetSampleTrigMode(sampleParam.m_nSampleTrigMode, out nReturnValue);
            hardWare.GetHardWare().SetTrigBlockCount(sampleParam.m_nSampleBlockSize, out nReturnValue);
            hardWare.GetHardWare().SetTrigDelayCount(sampleParam.m_nSampleDelayPoints, out nReturnValue);
        }
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
        /// 获取所有通道组信息
        /// </summary>
        public  void GetAllGroupMsg()
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
                Control.GroupChannel stuGroupChannel = new Control.GroupChannel();
                // 获取通道组信息
                hardWare.GetHardWare().GetChannelGroup(i, out nGroupChannelID, out strMachineIP, out nReturnValue);
                stuGroupChannel.m_GroupID = nGroupChannelID; //组ID
                stuGroupChannel.m_strMachineIP = strMachineIP;//仪器IP

                textBox2.Clear();
                textBox2.Enabled = false;
                textBox2.Text += nGroupCount+"号机："+strMachineIP+"\r\n";
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
                for (j = 0; j < nChannelNumber; j++)
                {
                    Control.HardChannel HardChannel = new Control.HardChannel();
                    int nChannelID = nChannelFirst + j;

                    hardWare.GetHardWare().IsChannelOnLine(nGroupChannelID, strMachineIP, nChannelID, out bOnLine);
                    hardWare.GetHardWare().GetChannelMeasureType(nGroupChannelID, strMachineIP, nChannelID, out nMeasureType);
                    HardChannel.m_nChannelGroupID = nGroupChannelID;//通道组ID，多个通道为一组  0-3
                    HardChannel.m_nChannelID = nChannelID;//通道ID
                    HardChannel.m_nMeasureType = nMeasureType;//测量类型
                    HardChannel.m_bOnlineFlag = (bOnLine == 1 ? true : false);

                    HardChannel.m_strMachineIP = strMachineIP;
                    m_listHardChannel.Add(HardChannel);

                    if (bOnLine == 0)
                    {
                        textBox1.Clear();
                        textBox1.Enabled = false;
                        textBox1.Text += nChannelID + "号通道" + "\r\n"; 

                        //保存到数据库
                        string sql = "insert into all_channelgroup_message(machineID,`online`, machineIP, groupCount, returnValue, channelFirst, datatype, channelNumber, channelID, channelgroupID, measuretype,flg)";
                        sql += "VALUES('" + nGroupCount + "', '" + bOnLine + "', '" + strMachineIP + "', '" + nGroupCount + "', '" + nReturnValue + "', '" + nChannelFirst + "', '" + nDataType + "', '" + nChannelNumber + "', '" + nChannelID + "', '" + nGroupChannelID + "', '" + nMeasureType + "','N')";
                        Data.OperatData opear = new Data.OperatData();
                        string sqldel = "DELETE from all_channelgroup_message";
                        opear.delete(sqldel);
                        opear.paramsSave(sql);
                    }
                }
            }
            GetBufferIndex();
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        private static void ClearAllGroupChannel()
        {
            m_listGroupChannel.Clear();
            m_listHardChannel.Clear();
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
 
        /// <summary>
        /// 获取当前采样参数
        /// </summary>
        private void GetSampleParam()
        {
            float fltSampleFreq;
            int nSampleMode, nTrigMode, nBlockSize, nDelayCount;

            hardWare.GetHardWare().GetSampleFreq(out fltSampleFreq);
            hardWare.GetHardWare().GetSampleMode(out nSampleMode);
            hardWare.GetHardWare().GetSampleTrigMode(out nTrigMode);
            hardWare.GetHardWare().GetTrigBlockCount(out nBlockSize);
            hardWare.GetHardWare().GetTrigDelayCount(out nDelayCount);

            sampleParam.m_fltSampleFrequency = fltSampleFreq;//频率
            sampleParam.m_nSampleMode = nSampleMode;//采样方式
            sampleParam.m_nSampleTrigMode = nTrigMode;//触发方式(瞬态模式下有效)
            sampleParam.m_nSampleBlockSize = nBlockSize;//采样块大小
            sampleParam.m_nSampleDelayPoints = nDelayCount;//延迟点数(瞬态模式下有效)
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
        /// <summary>
        /// 初始化设备参数
        /// </summary>
        private void GetInitParams()
        {
            ///获取所有通道组信息
            GetAllGroupMsg(); 
            ///通道列表
            GetChannelCombo();
            ///频率列表
            GetSampleFreqList();
            ///采样参数
            GetSampleParam();
        }       
        ///<summary>
        ///初始化通道选择列表
        /// </summary>
        private void GetChannelCombo()
        {
            m_CommboChan.Clear();
            for (int i = 0; i < m_listHardChannel.Count; i++)
            {
                // 跳过不在线通道
                if (!m_listHardChannel[i].m_bOnlineFlag)
                    continue;

                int nGroupID = m_listHardChannel[i].m_nChannelGroupID;
                string strGroupID = String.Format("{0}", nGroupID + 1);
                string strChannelID = String.Format("{0}", m_listHardChannel[i].m_nChannelID + 1);
                string strText = strGroupID + "-" + strChannelID;

                m_CommboChan.Add(strText);
            }
        }
        /// <summary>
        /// 获取仪器采样频率列表
        /// </summary>
        private void GetSampleFreqList()
        {
            //获取采样频率可选项
            string strFrepList = "";
            hardWare.GetHardWare().GetSampleFreqList(2, out strFrepList);
            int nFreqCount = BreakString(strFrepList, out m_listFreq, "|"); //个数
            /// <summary>
            /// 初始化采样频率选择列表
            /// </summary>
            this.comboBoxEx1.Items.Clear();

            float fltCurFreq = sampleParam.m_fltSampleFrequency;

            int nCount = 0;
            int nCurSel = -1;
            string strFreq;

            //RemoveAllEvent();
            foreach (var val in m_listFreq)
            {
                GetValidFloatString(val, out strFreq);//去点号

                comboBoxEx1.Items.Add(strFreq);

                float flt = float.Parse(strFreq);
                if (fltCurFreq == flt)
                {
                    nCurSel = nCount;
                }
                nCount++;
            }
            if (nCurSel >= 0)
            {
                comboBoxEx1.SelectedIndex = nCurSel;
            }
            //ResetAllEvent();
        }
        public static void GetValidFloatString(string strText, out string strFloat)
        {
            strFloat = "";
            if (strText.Contains('.'))
                strFloat = strText.Substring(0, strText.LastIndexOf('.'));
            else
                strFloat = strText;
        }
        ///<summary>
        ///将获取的设备参数提供给客户端
        /// </summary>
        public void SendAllParams()
        {
        }
        ///<summary>
        ///执行更新客户端修改的参数
        /// </summary>
        public void UpdateClientParams()
        {
        }
        /// <summary>
        /// 采样数据
        /// </summary>
        private void GetDataThread(object o)
        {
           
            Form1 main = (Form1)o;
            string strChannel = "";
            int nSelGroupID, nSelChanID;
            while (main.thread_state)
            {
                ///nTotalDataPos-单个通道已采集的总数据量（不含本次接收数据量）
                ///nReceiveCount-每通道数据量
                ///nChnCount-通道数
                ///oChnData-仪器采集的数据
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                hardWare.GetHardWare().GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);
                
                

                if (nReceiveCount <= 0)
                    continue;
                float[] pfltData;
                // 解析所有仪器的数据
                for (int i = 0; i < m_listGroupChannel.Count; i++)
                {
                    Control.GroupChannel GroupChannel = m_listGroupChannel[i];
                    int nChannelGroupID = GroupChannel.m_GroupID;

                    // 获取每个通道的数据 默认数据类型为float
                    for (int j = 0; j < GroupChannel.m_nChannelNumber; j++)
                    {
                        if (main.thread_state)
                        {
                                if (j == 0)
                                {
                                    strChannel = "1-1";//main.comboChan.SelectedItem.ToString();//eg:1-1/1-2/1-3
                                }
                                else if (j == 1)
                                {
                                    strChannel = "1-2";
                                }
                                else if (j == 2)
                                {
                                    strChannel = "1-3";
                                }
                        }
                        nSelGroupID = int.Parse(strChannel.Substring(0, strChannel.LastIndexOf('-')))-1;
                        string m = "";
                        
                        nSelChanID = int.Parse(strChannel.Substring(strChannel.LastIndexOf('-') + 1, strChannel.Length - 1 - strChannel.LastIndexOf('-'))) - 1;
                        if (nChannelGroupID != nSelGroupID || m_listHardChannel[j].m_nChannelID != nSelChanID)
                            continue;
                        pfltData = (float[])oChnData;

                        float[] pChanData = new float[nReceiveCount];
                        for (int nCount = 0; nCount < nReceiveCount; nCount++)
                        {
                            pChanData[nCount] = pfltData[i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + nCount];

                            string strData = String.Format("{0:f3}", pChanData[nCount]);
                            time_ns = String.Format("{0:f4}", ((double)nTotalDataPos / 20));
                            //time_ns = temp+double.Parse(time_ns) * (nCount+1)+"";
                            AnlySampleData(strData,nTotalDataPos, time_ns.ToString(), nChannelGroupID, nSelGroupID, nSelChanID,nCount,nReceiveCount);

                            Log.Debug.WriteErr("channelid:"+nSelChanID+" nreceive:"+nReceiveCount+"time_ns:" +time_ns+" strdata:"+strData+" ntotal:"+nTotalDataPos);
                            
                        }
                        temp = double.Parse(time_ns) ;
                    }
                }
                #region 获取GPS信息
                float fltTime;
                float fltValue;
                int ReturnValue;
                // 获取转速通道1的数据
                //int nMachineID, int nChannelStyle, int nValueType, out float fltTime, out float fltValue, out int ReturnValue
                //hardWare.GetHardWare().GetSampleStatValue(1, 4, 1, out fltTime, out fltValue, out ReturnValue);

                //Log.Debug.Write("ReturnValue:" + ReturnValue);

                //TRACE("GetSampleStatValue nReturnValue %d fltTime %f fltValue %f \n", nReturnValue, fltTime, fltValue);
                // 获取GPS的速度信息
                //		pTest->m_HardWare.GetSampleStatValue(nSelGroupID, 1, 0, &fltTime, &fltValue, &nReturnValue);
                #endregion
                Thread.Sleep(200);
            }
        }
        ///<summary>
        ///处理采样数据
        /// </summary>
        public void AnlySampleData(string data,int nTotalDataPos, string time_ns, int nChannelGroupID,int nSelGroupID,int nSelChanID,int nCount,int nReceiveCount)
        {
            //strData:-164.1896  nTotalDataPos:3158  time_ns:315.0000
            //${123.00,7849,32.5432}
            //仪器ID，通道组ID，通道组对应通道ID

            List<ArraySegment<byte>> list = new List<ArraySegment<byte>>();
            List<ArraySegment<byte>> len_list = new List<ArraySegment<byte>>();
            //
            string d1 = "{" + "d1" + nChannelGroupID.ToString().Length + nChannelGroupID.ToString();
            string d2 = "d2" + nSelGroupID.ToString().Length + nSelGroupID.ToString();
            string d3 = "d3" + nSelChanID.ToString().Length + nSelChanID.ToString();
            string d4 = "d4" + nCount.ToString().Length + nCount.ToString();
            string d5 = "d5" + nReceiveCount.ToString().Length + nReceiveCount.ToString();
            string d6 = "d6" + data.ToString().Length + data.ToString();
            string d7 = "d7" + nTotalDataPos.ToString().Length + nTotalDataPos.ToString();
            string d8 = "d8" + time_ns.Length + time_ns.ToString() + "}";
            string nd = d1 + d3 + d6 + d8;
            string n_len = "L"+nd.Length.ToString()+"N";
            string allData = d1 + d2 + d3 + d4 + d5 + d6 + d7 + d8;
            string currTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string sql = "insert into datatest (channel_id,data,dtime,currTime) values('" + nSelChanID + "','"+data+"','"+time_ns+"','"+currTime+"')";
            new Data.OperatData().paramsSave(sql);

            #region
            /// 二进制存储原始数据
            /// <summary>
            /// </summary>
            string filename = DateTime.Now.ToString("yyyy-MM-dd");
            Log.Biwriter.BinaryFile(filename,allData);
            #endregion

            #region
            Log.Debug.Write(nd);
            SendSampleData(nd);
            #endregion

            #region 

            #endregion
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendSampleData(string data)
        {
            if (socket.clientIP != null)
            {
                socket.sendMsg(data);
            }
            else
            {
                Log.Debug.Write("客户端未连接");
            }
        }
        /// <summary>
        /// 收到SF0请求初始参数
        /// </summary>
        public void SendInitData()
        {

            //sendToclient
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void labelX6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        
    }
}
