using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WinComServer
{
    public class MainHardWare
    {
        List<GroupChannel> m_listGroupChannel = new List<GroupChannel>();
        List<HardChannel> m_listHardChannel = new List<HardChannel>();
        Dictionary<int, int> m_dirBufferIndex = new Dictionary<int, int>();
        HardWare hardWare = new HardWare();
        SampleParam m_SampleParam = new SampleParam();
        List<string> m_listFreq = new List<string>();
        List<string> m_listChannelMeasure = new List<string>();
        List<string> m_listFullValue = new List<string>();
        List<string> m_listStrainType = new List<string>();
        List<string> m_listBridgeType = new List<string>();
        bool m_bThread = false;
        static Thread DataThread1;
        event EventHandler<ShowDataEventArgs> RefreshListBox;

        public void DeviceConnect()
        {
            //默认参数
            int nReturnValue;
            hardWare.GetHardWare().ReConnectAllMac(out nReturnValue);//连接在DeviceInfo.ini文件的IP的仪器。IsConnectMachine
            //axDHTestHardWare.WireLessChange(13124007);无线连接
            if (!IsConnectMachine())
            {
                Log.Write("仪器连接未成功");
                return;
            }
            else
            {
                Log.Write("仪器连接成功");
            }
            //EnableAllWindow(true);设置按钮可点击

            GetAllGroupChannel();//获取所有通道组信息
            //InitChannelCombo();初始化通道
            GetSampleFreqList();//获取仪器采样频率列表
            GetSampleParam();//获取当前采样的参数
            //InitFrepCombo(); //初始化采样频率选择列表
            //除通道信息外获取所有参数
            //RefreshAllParam();
        }
        public void GetDeviceData()
        {
            SetSampleParam();//设置参数
            //SetSampleDialog(false);//设置控件可选
            int nIsSampling;
            //是否正在采集数据
            hardWare.GetHardWare().IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                Log.Write("仪器采样中，请先停止采样!");
                return;
            }

            int nSample;
            //启动采样
            //axDHTestHardWare.StartSample("DH3820", 0, 1024, out nSample);
            hardWare.GetHardWare().StartSample("DH5902", 0, 1024, out nSample);
            if (nSample == 1)
            {
                Log.Write("开始采样");
            }
            else
            {
                Log.Write("失败");
            }

            //启动取数线程
            m_bThread = true;

            DataThread1 = new Thread(GetDataThread);
            
            DataThread1.SetApartmentState(ApartmentState.STA);
            DataThread1.IsBackground = true;
            DataThread1.Name = "GetData";
            DataThread1.Start(this);
        }
        // 取数线程
        static void GetDataThread(object o)
        {
            MainHardWare main = (MainHardWare)o;

            string strChannel = "";
            int nSelGroupID, nSelChanID;
            while (main.m_bThread)
            {
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                main.hardWare.GetHardWare().GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);
                if (nReceiveCount <= 0)
                    continue;
                float[] pfltData;
                // 解析所有仪器的数据
                for (int i = 0; i < main.m_listGroupChannel.Count; i++)
                {
                    GroupChannel GroupChannel = main.m_listGroupChannel[i];
                    int nChannelGroupID = GroupChannel.m_GroupID;

                    // 获取每个通道的数据 默认数据类型为float
                    for (int j = 0; j < GroupChannel.m_nChannelNumber; j++)
                    {
                        if (main.m_bThread)
                        {
                            //main.comboChan.Invoke(new Action(() =>
                            //{
                            if (j == 0)
                            {
                                strChannel = "1-1";
                            }
                            else if (j == 1)
                            {
                                strChannel = "1-2";
                            }
                            else if (j == 2)
                            {
                                strChannel = "1-3";
                            }
                            //}));
                        }
                        nSelGroupID = strChannel.Left('-').ToInt() - 1;
                        nSelChanID = strChannel.Right('-').ToInt() - 1;
                        if (nChannelGroupID != nSelGroupID || main.m_listHardChannel[j].m_nChannelID != nSelChanID)
                            continue;
                        pfltData = (float[])oChnData;

                        float[] pChanData = new float[nReceiveCount];
                        for (int nCount = 0; nCount < nReceiveCount; nCount++)
                        {
                            //Log.WriteLog("nCount:"+nCount);
                            pChanData[nCount] = pfltData[i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + nCount];
                        }
                        IntPtr Handle = new IntPtr();
                        //main.Invoke(new Action(() => { Handle = main.Handle; }));
                        //if (Handle != null)
                        //    main.Invoke(new Action(() =>
                        //    {
                                main.RefreshListBox(main, new ShowDataEventArgs(pChanData, nReceiveCount));
                                Log.Write("nTotalDataPos:" + nTotalDataPos);
                        //    }));
                    }
                }
                float fltTime;
                float fltValue;
                int ReturnValue;
                // 获取转速通道1的数据
                //main.axDHTestHardWare.GetSampleStatValue(0, 1, 0, out fltTime, out fltValue, out ReturnValue);

                //TRACE("GetSampleStatValue nReturnValue %d fltTime %f fltValue %f \n", nReturnValue, fltTime, fltValue);
                // 获取GPS的速度信息
                //		pTest->m_HardWare.GetSampleStatValue(nSelGroupID, 1, 0, &fltTime, &fltValue, &nReturnValue);
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 设置采样参数
        /// </summary>
        public void SetSampleParam()
        {
            int nReturnValue;
            string strText = "";
            float fltSampleFrequency = strText.ToFloat();
            m_SampleParam.m_fltSampleFrequency = fltSampleFrequency;

            //设置采样参数
            hardWare.GetHardWare().SetSampleFreq(m_SampleParam.m_fltSampleFrequency, out nReturnValue);
            hardWare.GetHardWare().SetSampleMode(m_SampleParam.m_nSampleMode, out nReturnValue);
            hardWare.GetHardWare().SetSampleTrigMode(m_SampleParam.m_nSampleTrigMode, out nReturnValue);
            hardWare.GetHardWare().SetTrigBlockCount(m_SampleParam.m_nSampleBlockSize, out nReturnValue);
            hardWare.GetHardWare().SetTrigDelayCount(m_SampleParam.m_nSampleDelayPoints, out nReturnValue);
        }
        /// <summary>
        /// 获取当前采样参数
        /// </summary>
        void GetSampleParam()
        {
            float fltSampleFreq;
            int nSampleMode, nTrigMode, nBlockSize, nDelayCount;

            hardWare.GetHardWare().GetSampleFreq(out fltSampleFreq);
            hardWare.GetHardWare().GetSampleMode(out nSampleMode);
            hardWare.GetHardWare().GetSampleTrigMode(out nTrigMode);
            hardWare.GetHardWare().GetTrigBlockCount(out nBlockSize);
            hardWare.GetHardWare().GetTrigDelayCount(out nDelayCount);

            m_SampleParam.m_fltSampleFrequency = fltSampleFreq;
            m_SampleParam.m_nSampleMode = nSampleMode;
            m_SampleParam.m_nSampleTrigMode = nTrigMode;
            m_SampleParam.m_nSampleBlockSize = nBlockSize;
            m_SampleParam.m_nSampleDelayPoints = nDelayCount;
        }
        /// <summary>
        /// 获取仪器采样频率列表
        /// </summary>
        void GetSampleFreqList()
        {
            //获取采样频率可选项
            string strFrepList = "";
            hardWare.GetHardWare().GetSampleFreqList(2, out strFrepList);
            int nFreqCount = ChanHelp.BreakString(strFrepList, out m_listFreq, "|");
        }
        // <summary>
        /// 初始化仪器控制接口
        /// </summary>
        /// <returns></returns>
        /// 
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
        bool IsConnectMachine()
        {
            int nReturnValue;
            hardWare.GetHardWare().IsConnectMachine(out nReturnValue);
            return nReturnValue == 1 ? true : false;
        }
        /// <summary>
        /// 释放内存
        /// </summary>
        void ClearAllGroupChannel()
        {
            m_listGroupChannel.Clear();
            m_listHardChannel.Clear();
        }
        /// <summary>
        /// 获取所有通道组信息
        /// </summary>
        void GetAllGroupChannel()
        {
            ClearAllGroupChannel();
            int i = 0, j = 0;
            int nGroupCount;
            hardWare.GetHardWare().GetChannelGroupCount(out nGroupCount);
            int nGroupChannelID, nChannelFirst, nChannelNumber, nDataType;
            string strMachineIP;
            int nReturnValue = 0;
            for (i = 0; i < nGroupCount; i++)
            {
                GroupChannel stuGroupChannel = new GroupChannel();

                // 获取通道组信息
                hardWare.GetHardWare().GetChannelGroup(i, out nGroupChannelID, out strMachineIP, out nReturnValue);
                stuGroupChannel.m_GroupID = nGroupChannelID;
                stuGroupChannel.m_strMachineIP = strMachineIP;

                // 获取某台仪器的起始通道ID
                hardWare.GetHardWare().GetChannelFirstID(nGroupChannelID, strMachineIP, out nChannelFirst);
                stuGroupChannel.m_nChannelFirst = nChannelFirst;

                // 获取某台仪器的总的通道数
                hardWare.GetHardWare().GetChannelCount(nGroupChannelID, strMachineIP, out nChannelNumber);
                stuGroupChannel.m_nChannelNumber = nChannelNumber;

                // 获取某台仪器的数据类型
                hardWare.GetHardWare().GetChannelGroupDataType(nGroupChannelID, strMachineIP, out nDataType);
                stuGroupChannel.m_nDataType = nDataType;

                m_listGroupChannel.Add(stuGroupChannel);

                int bOnLine = 1, nMeasureType = 0;
                // 通道信息
                for (j = 0; j < nChannelNumber; j++)
                {
                    HardChannel HardChannel = new HardChannel();
                    int nChannelID = nChannelFirst + j;

                    hardWare.GetHardWare().IsChannelOnLine(nGroupChannelID, strMachineIP, nChannelID, out bOnLine);
                    hardWare.GetHardWare().GetChannelMeasureType(nGroupChannelID, strMachineIP, nChannelID, out nMeasureType);
                    HardChannel.m_nChannelGroupID = nGroupChannelID;
                    HardChannel.m_nChannelID = nChannelID;
                    HardChannel.m_nMeasureType = nMeasureType;
                    HardChannel.m_bOnlineFlag = (bOnLine == 1 ? true : false);

                    HardChannel.m_strMachineIP = strMachineIP;
                    m_listHardChannel.Add(HardChannel);
                }
            }
            GetBufferIndex();
        }
        /// <summary>
        /// 仪器中的数据按照仪器ID排列，由小到大
        /// </summary>
        void GetBufferIndex()
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
