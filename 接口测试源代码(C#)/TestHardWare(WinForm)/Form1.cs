using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using TestHardWare_WinForm_.Properties;
using System.IO;

namespace TestHardWare_WinForm_
{
    public partial class TestHardWare : Form
    {
        List<GroupChannel> m_listGroupChannel = new List<GroupChannel>();
        List<HardChannel> m_listHardChannel = new List<HardChannel>();
        Dictionary<int, int> m_dirBufferIndex = new Dictionary<int, int>();
        SampleParam m_SampleParam = new SampleParam();
        List<string> m_listFreq = new List<string>();
        List<string> m_listChannelMeasure = new List<string>();
        List<string> m_listFullValue = new List<string>();
        List<string> m_listStrainType = new List<string>();
        List<string> m_listBridgeType = new List<string>();
        bool m_bThread = false;
        static Thread DataThread;
        event EventHandler<ShowDataEventArgs> RefreshListBox;

        public TestHardWare()
        {
            InitializeComponent();
            try
            {
                this.Icon = Resources.user_group;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("加载异常");
            }

            this.CenterToScreen();
            EnableAllWindow(false);

            if (!InitInterface())
            {
                MessageBox.Show("初始化接口失败!");
            }
            else
            {
                MessageBox.Show("初始化成功");
            }
            RefreshListBox += new EventHandler<ShowDataEventArgs>(main_RefreshListBox);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            int nIsSampling;
            //是否正在采集数据
            axDHTestHardWare.IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                MessageBox.Show("请先停止采样!");
                e.Cancel = true;
            }
            if (!e.Cancel)
                base.OnClosing(e);
        }

        protected override void OnClosed(System.EventArgs e)
        {
            axDHTestHardWare.DHQuit();
        }

        public void EnableAllWindow(bool bEnable)
        {
            buttonStartSample.Enabled = bEnable;
            buttonStopSample.Enabled = bEnable;
            buttonBalance.Enabled = bEnable;
            buttonClear.Enabled = bEnable;
        }

        /// <summary>
        /// 检查仪器是否连接
        /// </summary>
        /// <returns></returns>
        bool IsConnectMachine()
        {
            int nReturnValue;
            axDHTestHardWare.IsConnectMachine(out nReturnValue);
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
            axDHTestHardWare.GetChannelGroupCount(out nGroupCount);
            int nGroupChannelID, nChannelFirst, nChannelNumber, nDataType;
            string strMachineIP;
            int nReturnValue = 0;
            for (i = 0; i < nGroupCount; i++)
            {
                GroupChannel stuGroupChannel = new GroupChannel();

               // 获取通道组信息
                axDHTestHardWare.GetChannelGroup(i, out nGroupChannelID, out strMachineIP, out nReturnValue);
                stuGroupChannel.m_GroupID = nGroupChannelID;
                stuGroupChannel.m_strMachineIP = strMachineIP;

                // 获取某台仪器的起始通道ID
                axDHTestHardWare.GetChannelFirstID(nGroupChannelID, strMachineIP, out nChannelFirst);
                stuGroupChannel.m_nChannelFirst = nChannelFirst;

                // 获取某台仪器的总的通道数
                axDHTestHardWare.GetChannelCount(nGroupChannelID, strMachineIP, out nChannelNumber);
                stuGroupChannel.m_nChannelNumber = nChannelNumber;

                // 获取某台仪器的数据类型
                axDHTestHardWare.GetChannelGroupDataType(nGroupChannelID, strMachineIP, out nDataType);
                stuGroupChannel.m_nDataType = nDataType;

                m_listGroupChannel.Add(stuGroupChannel);

                int bOnLine = 1, nMeasureType = 0;
                // 通道信息
                for (j = 0; j < nChannelNumber; j++)
                {
                    HardChannel HardChannel = new HardChannel();
                    int nChannelID = nChannelFirst + j;

                    axDHTestHardWare.IsChannelOnLine(nGroupChannelID, strMachineIP, nChannelID, out bOnLine);
                    axDHTestHardWare.GetChannelMeasureType(nGroupChannelID, strMachineIP, nChannelID, out nMeasureType);
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

        /// <summary>
        /// 获取仪器采样频率列表
        /// </summary>
        void GetSampleFreqList()
        {
            //获取采样频率可选项
            string strFrepList = "";
            axDHTestHardWare.GetSampleFreqList(2, out strFrepList);
            int nFreqCount = ChanHelp.BreakString(strFrepList, out m_listFreq, "|");
        }

        /// <summary>
        /// 获取当前采样参数
        /// </summary>
        void GetSampleParam()
        {
            float fltSampleFreq;
            int nSampleMode, nTrigMode, nBlockSize, nDelayCount;

            axDHTestHardWare.GetSampleFreq(out fltSampleFreq);
            axDHTestHardWare.GetSampleMode(out nSampleMode);
            axDHTestHardWare.GetSampleTrigMode(out nTrigMode);
            axDHTestHardWare.GetTrigBlockCount(out nBlockSize);
            axDHTestHardWare.GetTrigDelayCount(out nDelayCount);

            m_SampleParam.m_fltSampleFrequency = fltSampleFreq;
            m_SampleParam.m_nSampleMode = nSampleMode;
            m_SampleParam.m_nSampleTrigMode = nTrigMode;
            m_SampleParam.m_nSampleBlockSize = nBlockSize;
            m_SampleParam.m_nSampleDelayPoints = nDelayCount;
        }

        void RefreshAllParam()
        {
            GetParamSelectValue();

            int nCurSel = comboChan.SelectedIndex;
            if (nCurSel < 0)
                return;

            string strText;
            strText = comboChan.SelectedItem.ToString();

            string strGroupID = strText.Left('-');
            int nGroupID = strGroupID.ToInt();
            nGroupID -= 1;
            string strChannelID = strText.Right('-');
            int nChannelID = strChannelID.ToInt();
            nChannelID -= 1;

            string strMachineIP = GetMachineIP(nGroupID);

            stuChannelParam ChanParam = new stuChannelParam();
            GetChannelParam(nChannelID, ref ChanParam);

            InitMeasureType(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
            InitFullValueCombo(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
        }

        /// <summary>
        /// 获取参数可选项列表
        /// </summary>
        void GetParamSelectValue()
        {
            string strText;
            int nCurSel = comboChan.SelectedIndex;
            if (nCurSel < 0)
                return;

            strText = comboChan.SelectedItem.ToString();
            string strGroupID = strText.Left('-');
            int nGroupID = strGroupID.ToInt();
            nGroupID -= 1;
            string strChannelID = strText.Right('-');
            int nChannelID = strChannelID.ToInt();
            nChannelID -= 1;

            string strMachineIP = GetMachineIP(nGroupID);

            stuChannelParam ChanParam = new stuChannelParam();
            GetChannelParam(nChannelID, ref ChanParam);

            //获取参数可选项列表
            string strMeasureTypeSelect = "";
            axDHTestHardWare.GetParamSelectValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_CHANNEL_MEASURETYPE, out strMeasureTypeSelect);
            int nMeasureTypeCount = ChanHelp.BreakString(strMeasureTypeSelect, out m_listChannelMeasure, "|");

            //获取参数可选项列表
            string strFullValueSelect = "";
            axDHTestHardWare.GetParamSelectValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_CHANNEL_FULLVALUE, out strFullValueSelect);
            int nFullValueCount = ChanHelp.BreakString(strFullValueSelect, out m_listFullValue, "|");
}

        /// <summary>
        /// 获取仪器ID
        /// </summary>
        /// <param name="nID"></param>
        /// <returns></returns>
        string GetMachineIP(int nID)
        {
            string strMachineIP = "";
            for (int i = 0; i < m_listHardChannel.Count; i++)
            {
                int nGroupID = m_listHardChannel[i].m_nChannelGroupID;
                if (nGroupID == nID)
                {
                    strMachineIP = m_listHardChannel[i].m_strMachineIP;
                }
            }
            return strMachineIP;
        }

        /// <summary>
        /// 获取通道参数
        /// </summary>
        /// <param name="nID"></param>
        /// <param name="ChanParam"></param>
        void GetChannelParam(int nID, ref stuChannelParam ChanParam)
        {
            for (int i = 0; i < m_listHardChannel.Count; i++)
            {
                int nChannelID = m_listHardChannel[i].m_nChannelID;
                if (nChannelID == nID)
                {
                    ChanParam.ChannelStyle = m_listHardChannel[i].m_nChannelStyle;
                    ChanParam.ChannelID = nChannelID;
                    ChanParam.CellID = m_listHardChannel[i].m_nCellID;
                }
            }
        }

        /// <summary>
        /// 设置采样参数
        /// </summary>
        void SetSampleParam()
        {
            int nReturnValue;
            string strText;
            strText = comboFreq.SelectedItem.ToString();
            float fltSampleFrequency = strText.ToFloat();
            m_SampleParam.m_fltSampleFrequency = fltSampleFrequency;

            //设置采样参数
            axDHTestHardWare.SetSampleFreq(m_SampleParam.m_fltSampleFrequency, out nReturnValue);
            axDHTestHardWare.SetSampleMode(m_SampleParam.m_nSampleMode, out nReturnValue);
            axDHTestHardWare.SetSampleTrigMode(m_SampleParam.m_nSampleTrigMode, out nReturnValue);
            axDHTestHardWare.SetTrigBlockCount(m_SampleParam.m_nSampleBlockSize, out nReturnValue);
            axDHTestHardWare.SetTrigDelayCount(m_SampleParam.m_nSampleDelayPoints, out nReturnValue);
        }

        //修改参数并发送参数码至仪器
        bool ModifyParamAndSendCode(int GroupChannelID, string strMachineIP, int ChannelStyle, int ChannelID, int CellID, int ShowParamID, string strParamValue, int nSelectIndex)
        {
            int nModifyReturn, nSendCodeReturn;
            axDHTestHardWare.ModifyParam(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, ShowParamID, strParamValue, nSelectIndex, out nModifyReturn);
            // 修改数采通道参数后,lSendCodeReturn值为0，但是发码已成功，不影响修改参数
            axDHTestHardWare.SendChannelParamCode_Single(GroupChannelID, strMachineIP,
                ChannelID, out nSendCodeReturn);
            return nModifyReturn == 1 ? true : false;
        }

        void UpdateHardAndRefresh(int nParamID)
        {
            string strMeasureType;
            int nCursel = comboMeasureType.SelectedIndex;
            if (nCursel < 0)
                return;
            strMeasureType = comboMeasureType.SelectedItem.ToString();
            if (strMeasureType == "应变应力")
            {
                string strText;
                strText = comboChan.SelectedItem.ToString();
                string strGroupID = strText.Left('-');
                int nGroupID = strGroupID.ToInt();
                nGroupID -= 1;
                string strChannelID = strText.Right('-');
                int nChannelID = strChannelID.ToInt();
                nChannelID -= 1;
                string strMachineIP = GetMachineIP(nGroupID);
                stuChannelParam ChanParam = new stuChannelParam();
                GetChannelParam(nChannelID, ref ChanParam);

                switch (nParamID)
                {
                    case Constant.SHOW_STRAIN_SHOWTYPE:
                        //修改应变应力显示类型
                        string strStrainType;
                        strStrainType = comboStrainType.SelectedItem.ToString();
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_SHOWTYPE, strStrainType, comboStrainType.SelectedIndex);
                        break;
                    case Constant.SHOW_STRAIN_BRIDGETYPE:
                        //修改桥路方式
                        string strBridgeType;
                        strBridgeType = comboBrigeType.SelectedItem.ToString();
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_BRIDGETYPE, strBridgeType, comboBrigeType.SelectedIndex);
                        break;
                    case Constant.SHOW_STRAIN_GAUGE:
                        //应变计阻值
                        string strStrainGuage = textStrainGauge.Text;
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_GAUGE, strStrainGuage, 0);
                        break;
                    case Constant.SHOW_STRAIN_LEAD:
                        //导线阻值
                        string strStrainLead = textStrainLead.Text;
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_LEAD, strStrainLead, 0);
                        break;
                    case Constant.SHOW_STRAIN_SENSECOEF:
                        //灵敏度系数
                        string strStrainSenseCoef = textStrainSenseCoef.Text;
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_SENSECOEF, strStrainSenseCoef, 0);
                        break;
                    case Constant.SHOW_STRAIN_POSION:
                        //泊松比
                        string strStrainPosion = textStrainPosion.Text;
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_POSION, strStrainPosion, 0);
                        break;
                    case Constant.SHOW_STRAIN_ELASTICITY:
                        //弹性模量
                        string strStrainElastiCity = textStrainElast.Text;
                        ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_ELASTICITY, strStrainElastiCity, 0);
                        break;
                }

                // 刷新除通道参数所有界面参数
                RefreshAllParam();
            }
        }

        void SetSampleDialog(bool bEnable)
        {
            comboFreq.Enabled = bEnable;
            comboChan.Enabled = bEnable;
            comboMeasureType.Enabled = bEnable;
            comboFullValue.Enabled = bEnable;
            buttonBalance.Enabled = bEnable;
            buttonClear.Enabled = bEnable;

            if (comboMeasureType.SelectedItem.ToString() == "应变应力")
            {
                comboStrainType.Enabled = bEnable;
                comboBrigeType.Enabled = bEnable;
                textStrainLead.Enabled = bEnable;
                textStrainGauge.Enabled = bEnable;
                textStrainSenseCoef.Enabled = bEnable;
                textStrainPosion.Enabled = bEnable;
                textStrainElast.Enabled = bEnable;
            }
        }

        // 取数线程
        static void GetDataThread(object o)
        {
            TestHardWare main = (TestHardWare)o;
            string strChannel = "";
            int nSelGroupID, nSelChanID;
            while (main.m_bThread)
            {
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                main.axDHTestHardWare.GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);
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
                            main.comboChan.Invoke(new Action(() =>
                            {
                                strChannel = "1-1";//main.comboChan.SelectedItem.ToString();
                            }));
                        }
                        nSelGroupID = strChannel.Left('-').ToInt() - 1;
                        nSelChanID = strChannel.Right('-').ToInt() - 1;
                        if (nChannelGroupID != nSelGroupID || main.m_listHardChannel[j].m_nChannelID != nSelChanID)
                            continue;
                        pfltData = (float[])oChnData;

                        float[] pChanData = new float[nReceiveCount];
                        for (int nCount = 0; nCount < nReceiveCount; nCount++)
                        {
                            pChanData[nCount] = pfltData[i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + nCount];
                        }

                        IntPtr Handle = new IntPtr();
                        main.Invoke(new Action(() => { Handle = main.Handle; }));
                        if (Handle != null)
                            main.Invoke(new Action(() => 
                            {
                                main.RefreshListBox(main, new ShowDataEventArgs(pChanData, nReceiveCount));
                                int nDataCount = nReceiveCount;
                                for (int ii = 0; ii < nDataCount; ii++)
                                {
                                    //if (i % 30 == 0)
                                    //{
                                    //    main.listBox1.Items.Add(strText);
                                    //    strText = "";
                                    //}
                                    string strData = String.Format("{0:f3}   ", pfltData[ii]);

                                    Log.WriteLog1("ch1:" + strData+"time:"+ii*0.1+"ii:"+ii);

                                }
                            }));
                    }
                }
                Thread.Sleep(200);
            }
        }
        static void GetDataThread1(object o)
        {
            TestHardWare main = (TestHardWare)o;
            string strChannel = "";
            int nSelGroupID, nSelChanID;
            while (main.m_bThread)
            {
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                main.axDHTestHardWare.GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);
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
                            main.comboChan.Invoke(new Action(() => 
                            {
                                strChannel = "1-2";//main.comboChan.SelectedItem.ToString();
                            }));
                        }
                        nSelGroupID = strChannel.Left('-').ToInt() - 1;
                        nSelChanID = strChannel.Right('-').ToInt() - 1;
                        if (nChannelGroupID != nSelGroupID || main.m_listHardChannel[j].m_nChannelID != nSelChanID)
                            continue;
                        pfltData = (float[])oChnData;

                        float[] pChanData = new float[nReceiveCount];
                        for (int nCount = 0; nCount < nReceiveCount; nCount++)
                        {
                            pChanData[nCount] = pfltData[i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + nCount];
                        }

                        IntPtr Handle = new IntPtr();
                        main.Invoke(new Action(() => { Handle = main.Handle; }));
                        if (Handle != null)
                            main.Invoke(new Action(() => 
                            {
                                //main.RefreshListBox(main, new ShowDataEventArgs(pChanData, nReceiveCount));
                                int nDataCount = nReceiveCount;
                                for (int ii = 0; ii < nDataCount; ii++)
                                {
                                    //if (i % 30 == 0)
                                    //{
                                    //    main.listBox1.Items.Add(strText);
                                    //    strText = "";
                                    //}
                                    string strData = String.Format("{0:f3}   ", pfltData[ii]);

                                    Log.WriteLog2("ch2:"+strData+"time:"+ii*0.1+"ii:"+ii);

                                }
                            }));
                    }
                }
                Thread.Sleep(200);
            }
        }
        static void GetDataThread2(object o)
        {
            TestHardWare main = (TestHardWare)o;
            string strChannel = "";
            int nSelGroupID, nSelChanID;
            while (main.m_bThread)
            {
                int nTotalDataPos, nReceiveCount, nChnCount, nReturnValue;
                object oChnData;
                main.axDHTestHardWare.GetAllChnDataEx(out oChnData, out nTotalDataPos, out nReceiveCount, out nChnCount, out nReturnValue);
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
                            main.comboChan.Invoke(new Action(() => 
                            {
                                strChannel = "1-3";//main.comboChan.SelectedItem.ToString();
                            }));
                        }
                        nSelGroupID = strChannel.Left('-').ToInt() - 1;
                        nSelChanID = strChannel.Right('-').ToInt() - 1;
                        if (nChannelGroupID != nSelGroupID || main.m_listHardChannel[j].m_nChannelID != nSelChanID)
                            continue;
                        pfltData = (float[])oChnData;

                        float[] pChanData = new float[nReceiveCount];
                        for (int nCount = 0; nCount < nReceiveCount; nCount++)
                        {
                            pChanData[nCount] = pfltData[i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + nCount];
                        }

                        IntPtr Handle = new IntPtr();
                        main.Invoke(new Action(() => { Handle = main.Handle; }));
                        if (Handle != null)
                            main.Invoke(new Action(() => 
                            {
                                //main.RefreshListBox(main, new ShowDataEventArgs(pChanData, nReceiveCount));
                                int nDataCount = nReceiveCount;
                                for (int ii = 0; ii < nDataCount; ii++)
                                {
                                    //if (i % 30 == 0)
                                    //{
                                    //    main.listBox1.Items.Add(strText);
                                    //    strText = "";
                                    //}
                                    string strData = String.Format("{0:f3}   ", pfltData[i]);

                                    Log.WriteLog3("ch3:"+strData+"time:"+ii*0.1+"ii:"+ii);
                                    
                                }
                            }));
                    }
                }
                Thread.Sleep(200);
            }
        }
        static void GetDataThreadCallBackToDir()
        {
            TestHardWare main = new TestHardWare();
            string strTestName="test_"+DateTime.Now.ToString("yyyy-MM-ddHHmmss"), strDestDir= @"D:\文件夹\DLL\招商局交通科研设计研究院检测中心_DH5902新版软件二次开发接口(陈源)\DHDAS新版软件网络仪器二次开发接口_V6.11.32\接口测试源代码\测试程序\bin\Debug\saveCallBackFile", strMacIP="192.168.0.102";
            int bDelLinuxData=0;
            int ReturnValue;
            //strTestName–回收工程名称，支持多个工程回收，用”|”隔开
            //strDestDir–回收后保存目录
            //strMacIP–回收仪器IP
            //bDelLinuxData–是否删除下位机数据0—不删除, 1—删除
            //ReturnValue –返回值– 0 回收失败  1 –回收成功
            main.axDHTestHardWare.CallbackDataToDir(strTestName, strDestDir, strMacIP, bDelLinuxData, out ReturnValue);
            if (ReturnValue == 1)
            {
                Log.WriteLog("回收成功");
            }
            else
            {
                Log.WriteLog("回收失败");            }
        }
        static void main_RefreshListBox(object sender, ShowDataEventArgs e)
        {
            TestHardWare main = (TestHardWare)sender;
            int nDataCount = e.nReceiveCount;
            float[] pfltData = e.pfltDats;
            string strText = "", strData = "";
            for (int i = 0; i < nDataCount; i++)
            {
                //if (i % 30 == 0)
                //{
                //    main.listBox1.Items.Add(strText);
                //    strText = "";
                //}
                strData = String.Format("{0:f3}   ", pfltData[i]);
                strText += strData;
                Log.WriteLog("ch1:"+strData);
                string index = main.comboChan.SelectedItem.ToString();
            }
            main.listBox1.Items.Add(strText);
            int nCount = main.listBox1.Items.Count;
            main.listBox1.SelectedIndex = nCount - 1;
        }

        private void RemoveAllEvent()
        {
            comboFreq.SelectedIndexChanged -= comboFreq_SelectedIndexChanged;
            comboChan.SelectedIndexChanged -= comboChan_SelectedIndexChanged;
            comboMeasureType.SelectedIndexChanged -= comboMeasureType_SelectedIndexChanged;
            comboFullValue.SelectedIndexChanged -= comboFullValue_SelectedIndexChanged;
            comboStrainType.SelectedIndexChanged -= comboStrainType_SelectedIndexChanged;
            comboBrigeType.SelectedIndexChanged -= comboBrigeType_SelectedIndexChanged;
            textStrainGauge.Leave -= textStrainGauge_Leave;
            textStrainLead.Leave -= textStrainLead_Leave;
            textStrainSenseCoef.Leave -= textStrainSenseCoef_Leave;
            textStrainPosion.Leave -= textStrainPosion_Leave;
            textStrainElast.Leave -= textStrainElast_Leave;
        }

        private void ResetAllEvent()
        {
            comboFreq.SelectedIndexChanged += comboFreq_SelectedIndexChanged;
            comboChan.SelectedIndexChanged += comboChan_SelectedIndexChanged;
            comboMeasureType.SelectedIndexChanged += comboMeasureType_SelectedIndexChanged;
            comboFullValue.SelectedIndexChanged += comboFullValue_SelectedIndexChanged;
            comboStrainType.SelectedIndexChanged += comboStrainType_SelectedIndexChanged;
            comboBrigeType.SelectedIndexChanged += comboBrigeType_SelectedIndexChanged;
            textStrainGauge.Leave += textStrainGauge_Leave;
            textStrainLead.Leave += textStrainLead_Leave;
            textStrainSenseCoef.Leave += textStrainSenseCoef_Leave;
            textStrainPosion.Leave += textStrainPosion_Leave;
            textStrainElast.Leave += textStrainElast_Leave;
        }

        private void TestHardWare_Load(object sender, EventArgs e)
        {
            //HardWare m_hardWare = new HardWare();
            //int nResultValue = 0;
            //m_hardWare.GetHardWare().Init(@"D:\文件夹\DLL\招商局交通科研设计研究院检测中心_DH5902新版软件二次开发接口(陈源)\DHDAS新版软件网络仪器二次开发接口_V6.11.32\接口测试源代码\测试程序\bin\Debug\config\", "chinese", out nResultValue);
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
 