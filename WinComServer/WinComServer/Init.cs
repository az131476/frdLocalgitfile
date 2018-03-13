using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHHardWareLib;

namespace WinComServer
{
    partial class Init
    {
        /// <summary>
        /// 初始化仪器控制接口
        /// </summary>
        /// <returns></returns>
        /// 
        public bool InitInterface()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string strPath = baseDir.Substring(0, baseDir.LastIndexOf('\\'));

            strPath += "\\config\\";

            int nReturnValue;
            //初始化仪器控制接口
            DHHardWareLib.DHTestHardWare.axDHTestHardWare.Init(strPath, "chinese", out nReturnValue);
            return nReturnValue == 1 ? true : false;
        }

        /// <summary>
        /// 初始化通道选择列表
        /// </summary>
        void InitChannelCombo()
        {
            this.comboChan.Items.Clear();
            for (int i = 0; i < m_listHardChannel.Count; i++)
            {
                // 跳过不在线通道
                if (!m_listHardChannel[i].m_bOnlineFlag)
                    continue;

                int nGroupID = m_listHardChannel[i].m_nChannelGroupID;
                string strGroupID = String.Format("{0}", nGroupID + 1);
                string strChannelID = String.Format("{0}", m_listHardChannel[i].m_nChannelID + 1);
                string strText = strGroupID + "-" + strChannelID;

                this.comboChan.Items.Add(strText);
            }
            if (this.comboChan.Items.Count > 0)
                this.comboChan.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化采样频率选择列表
        /// </summary>
        void InitFrepCombo()
        {
            this.comboFreq.Items.Clear();

            float fltCurFreq = m_SampleParam.m_fltSampleFrequency;

            int nCount = 0;
            int nCurSel = -1;
            string strFreq;

            RemoveAllEvent();
            foreach (var val in m_listFreq)
            {
                ChanHelp.GetValidFloatString(val, out strFreq);
                comboFreq.Items.Add(strFreq);

                float flt = strFreq.ToFloat();
                if (fltCurFreq == flt)
                {
                    nCurSel = nCount;
                }
                nCount++;
            }
            if (nCurSel >= 0)
            {
                comboFreq.SelectedIndex = nCurSel;
            }
            ResetAllEvent();
        }

        /// <summary>
        /// 初始化通道测点类型
        /// </summary>
        /// <param name="GroupChannelID"></param>
        /// <param name="strMachineIP"></param>
        /// <param name="ChannelStyle"></param>
        /// <param name="ChannelID"></param>
        /// <param name="CellID"></param>
        void InitMeasureType(int GroupChannelID, string strMachineIP, int ChannelStyle, int ChannelID, int CellID)
        {
            comboMeasureType.Items.Clear();

            //获取参数值
            string strCurValue = "";
            axDHTestHardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, Constant.SHOW_CHANNEL_MEASURETYPE, out strCurValue);

            int nCount = 0;
            int nSel = -1;

            RemoveAllEvent();
            foreach (var val in m_listChannelMeasure)
            {
                comboMeasureType.Items.Add(val);
                if (strCurValue == val)
                {
                    nSel = nCount;
                }
                nCount++;
            }
            if (nSel >= 0)
            {
                comboMeasureType.SelectedIndex = nSel;
            }
            ResetAllEvent();
            if (strCurValue == "应变应力")
                InitStrainParam(true);
            else
                InitStrainParam(false);
        }

        /// <summary>
        /// 初始化量程选择列表
        /// </summary>
        /// <param name="GroupChannelID"></param>
        /// <param name="strMachineIP"></param>
        /// <param name="ChannelStyle"></param>
        /// <param name="ChannelID"></param>
        /// <param name="CellID"></param>
        void InitFullValueCombo(int GroupChannelID, string strMachineIP, int ChannelStyle, int ChannelID, int CellID)
        {
            comboFullValue.Items.Clear();
            //获取参数值
            string strCurValue = "";
            axDHTestHardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, Constant.SHOW_CHANNEL_FULLVALUE, out strCurValue);

            int nCount = 0;
            int nSel = -1;

            RemoveAllEvent();
            foreach (var val in m_listFullValue)
            {
                comboFullValue.Items.Add(val);
                if (strCurValue == val)
                {
                    nSel = nCount;
                }
                nCount++;
            }
            if (nSel >= 0)
            {
                comboFullValue.SelectedIndex = nSel;
            }
            ResetAllEvent();
        }

        /// <summary>
        /// 测点类型为应变应力时初始化子参数
        /// </summary>
        /// <param name="bEnable"></param>
        void InitStrainParam(bool bEnable)
        {
            comboStrainType.Enabled = bEnable;
            comboBrigeType.Enabled = bEnable;
            textStrainLead.Enabled = bEnable;
            textStrainGauge.Enabled = bEnable;
            textStrainSenseCoef.Enabled = bEnable;
            textStrainPosion.Enabled = bEnable;
            textStrainElast.Enabled = bEnable;

            RemoveAllEvent();

            if (bEnable)
            {
                string strText = comboChan.SelectedItem.ToString();
                string strGroupID = strText.Left('-');
                int nGroupID = strGroupID.ToInt();
                nGroupID -= 1;
                string strChannelID = strText.Right('-');
                int nChannelID = strChannelID.ToInt();
                nChannelID -= 1;
                string strMachineIP = GetMachineIP(nGroupID);
                stuChannelParam ChanParam = new stuChannelParam();
                GetChannelParam(nChannelID, ref ChanParam);
                //获取应变应力显示类型可选项列表
                string strMeasureTypeSelect = "";
                axDHTestHardWare.GetParamSelectValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_SHOWTYPE, out strMeasureTypeSelect);
                int nMeasureTypeCount = ChanHelp.BreakString(strMeasureTypeSelect, out m_listStrainType, "|");

                //初始化当前应变应力参数值
                string strCurValue = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_SHOWTYPE, out strCurValue);
                int nCount = 0;
                int nSel = -1;
                comboStrainType.Items.Clear();
                foreach (var val in m_listStrainType)
                {
                    comboStrainType.Items.Add(val);
                    if (strCurValue == val)
                    {
                        nSel = nCount;
                    }
                    nCount++;
                }
                if (nSel >= 0)
                {
                    comboStrainType.SelectedIndex = nSel;
                }

                //获取桥路方式可选项列表
                string strBridgeTypeSelect = "";
                axDHTestHardWare.GetParamSelectValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_BRIDGETYPE, out strBridgeTypeSelect);
                int nBridgeTypeCount = ChanHelp.BreakString(strBridgeTypeSelect, out m_listBridgeType, "|");
                //初始化当前桥路方式
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_BRIDGETYPE, out strCurValue);
                nCount = 0;
                nSel = -1;
                comboBrigeType.Items.Clear();
                foreach (var val in m_listBridgeType)
                {
                    comboBrigeType.Items.Add(val);
                    if (strCurValue == val)
                    {
                        nSel = nCount;
                    }
                    nCount++;
                }
                if (nSel >= 0)
                {
                    comboBrigeType.SelectedIndex = nSel;
                }

                //应变计阻值
                string strStrainGauge = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_GAUGE, out strStrainGauge);
                textStrainGauge.Text = strStrainGauge;

                //导线阻值
                string strStrainLead = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_LEAD, out strStrainLead);
                textStrainLead.Text = strStrainLead;

                //灵敏度系数
                string strStrainSenseCoef = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_SENSECOEF, out strStrainSenseCoef);
                textStrainSenseCoef.Text = strStrainSenseCoef;

                //泊松比
                string strStrainPosion = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_POSION, out strStrainPosion);
                textStrainPosion.Text = strStrainPosion;

                //弹性模量
                string strStrainElasticity = "";
                axDHTestHardWare.GetParamValue(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_STRAIN_ELASTICITY, out strStrainElasticity);
                textStrainElast.Text = strStrainElasticity;

            }
            ResetAllEvent();
        }
    }
}
