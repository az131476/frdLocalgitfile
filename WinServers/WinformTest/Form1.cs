using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformTest
{
    public partial class Form1 : Form
    {
        public static HardWare hardWare = new HardWare();
        private static List<Control.GroupChannel> m_listGroupChannel = new List<Control.GroupChannel>();
        private static List<Control.HardChannel> m_listHardChannel = new List<Control.HardChannel>();
        private static Dictionary<int, int> m_dirBufferIndex = new Dictionary<int, int>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            #region 启动socket服务,监听客户端
            Control.SocketServer.server();
            #endregion
            #region////初始化仪器
            if (!InitInterface())
            {
                MessageBox.Show("初始化失败");
                Log.Debug.Write("初始化失败");
                return;
            }
            else
            {
                MessageBox.Show("初始化成功");
                Log.Debug.Write("初始化成功");
            }
            #endregion
            #region 与所有在线仪器建立连接

            int nReturnValue;
            hardWare.GetHardWare().ReConnectAllMac(out nReturnValue);//连接在DeviceInfo.ini文件的IP的仪器。IsConnectMachine
            //axDHTestHardWare.WireLessChange(13124007);
            if (!Control.DeviceConnect.ConAllDevice())
            {
                MessageBox.Show("仪器未连接!");
                return;
            }
            else
            {
                MessageBox.Show("连接成功");
            }
            #endregion
            #region 初始化设备参数：获取通道组所有数据
            GetAllGroupMsg();
            #endregion

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
        public static void GetAllGroupMsg()
        {
            ClearAllGroupChannel();
            int i = 0, j = 0;
            int nGroupCount;//Form1.GroupCount();//通道组数量-即仪器连接的数量
            hardWare.GetHardWare().GetChannelGroupCount(out nGroupCount);
            Log.Debug.Write("nGroupCount1:" + nGroupCount);
            int nGroupChannelID, nChannelFirst, nChannelNumber, nDataType;
            string strMachineIP;
            int nReturnValue = 0;
            for (i = 0; i < nGroupCount; i++)
            {
                Control.GroupChannel stuGroupChannel = new Control.GroupChannel();
                Log.Debug.Write("nGroupCount-for:" + nGroupCount);
                // 获取通道组信息
                hardWare.GetHardWare().GetChannelGroup(i, out nGroupChannelID, out strMachineIP, out nReturnValue);
                stuGroupChannel.m_GroupID = nGroupChannelID; //组ID
                stuGroupChannel.m_strMachineIP = strMachineIP;//仪器IP
                Log.Debug.Write("nGroupChannelID:" + nGroupChannelID); //0
                Log.Debug.Write("strMachineIP:" + strMachineIP);
                Log.Debug.Write("nReturnValue:" + nReturnValue);

                // 获取某台仪器的起始通道ID 0-15
                hardWare.GetHardWare().GetChannelFirstID(nGroupChannelID, strMachineIP, out nChannelFirst);
                stuGroupChannel.m_nChannelFirst = nChannelFirst;//0
                Log.Debug.Write("nChannelFirst:" + nChannelFirst);//0

                // 获取某台仪器的总的通道数
                hardWare.GetHardWare().GetChannelCount(nGroupChannelID, strMachineIP, out nChannelNumber);
                stuGroupChannel.m_nChannelNumber = nChannelNumber;
                Log.Debug.Write("nChannelNumber:" + nChannelNumber);//16

                // 获取某台仪器的数据类型
                hardWare.GetHardWare().GetChannelGroupDataType(nGroupChannelID, strMachineIP, out nDataType);
                stuGroupChannel.m_nDataType = nDataType; //0
                Log.Debug.Write("nDataType:" + nDataType);

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

                    Log.Debug.Write("nGroupChannelID:" + nGroupChannelID);//0-0
                    Log.Debug.Write("nChannelID:" + nChannelID);//0-15
                    Log.Debug.Write("bOnLine:" + bOnLine);//1
                    Log.Debug.Write("nMeasureType:" + nMeasureType);//0
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
        }
}
