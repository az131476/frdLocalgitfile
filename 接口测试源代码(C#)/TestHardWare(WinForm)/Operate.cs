using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TestHardWare_WinForm_
{
    partial class TestHardWare
    {
        private void buttonInit_Click(object sender, EventArgs e)
        {
            int nReturnValue;
            axDHTestHardWare.ReConnectAllMac(out nReturnValue);//连接在DeviceInfo.ini文件的IP的仪器。IsConnectMachine
            //axDHTestHardWare.WireLessChange(13124007);
            if (!IsConnectMachine())
            {
                MessageBox.Show("仪器未连接!");
                return;
            }
            else
            {
                MessageBox.Show("连接成功");
            }
            EnableAllWindow(true);

            GetAllGroupChannel();
            InitChannelCombo();
            GetSampleFreqList();
            GetSampleParam();
            InitFrepCombo();
            //除通道信息外获取所有参数
            RefreshAllParam();
        }

        private void comboFreq_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSampleParam();
        }

        private void comboChan_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshAllParam();
        }

        private void comboMeasureType_SelectedIndexChanged(object sender, EventArgs e)
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

            int nCurSel = comboMeasureType.SelectedIndex;
            string strMeasureType = comboMeasureType.SelectedItem.ToString();
            //修改通道参数
            bool bResult = ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_CHANNEL_MEASURETYPE, strMeasureType, nCurSel);
            if (!bResult)
            {
                MessageBox.Show("设置参数失败！");
            }
            GetParamSelectValue();
            InitFullValueCombo(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);

            if (strMeasureType == "应变应力")
                InitStrainParam(true);
            else
                InitStrainParam(false);
        }


        private void comboFullValue_SelectedIndexChanged(object sender, EventArgs e)
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

            int nCurSel = comboFullValue.SelectedIndex;
            int nTotalCount = comboFullValue.Items.Count;
            string strFullValue;
            strFullValue = comboFullValue.SelectedItem.ToString();
            //修改通道参数
            bool bResult = ModifyParamAndSendCode(nGroupID, strMachineIP, ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, Constant.SHOW_CHANNEL_FULLVALUE, strFullValue, nCurSel);
            if (!bResult)
            {
                MessageBox.Show("设置参数失败！");
            }
        }
        
        private void comboStrainType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_SHOWTYPE);
        }

        private void comboBrigeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_BRIDGETYPE);
        }

        private void textStrainGauge_Leave(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_GAUGE);
        }

        private void textStrainLead_Leave(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_LEAD);
        }

        private void textStrainSenseCoef_Leave(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_SENSECOEF);
        }

        private void textStrainPosion_Leave(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_POSION);
        }

        private void textStrainElast_Leave(object sender, EventArgs e)
        {
            UpdateHardAndRefresh(Constant.SHOW_STRAIN_ELASTICITY);
        }

        private void buttonStartSample_Click(object sender, EventArgs e)
        {
            SetSampleParam();
            SetSampleDialog(false);
            int nIsSampling;
            //是否正在采集数据
            axDHTestHardWare.IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                MessageBox.Show("仪器采样中，请先停止采样!");
                return;
            }

            int nSample;
            //启动采样
            //axDHTestHardWare.StartSample("DH3820", 0, 1024, out nSample);
            axDHTestHardWare.StartSample("DH5902", 0, 1024, out nSample);
            if (nSample == 1)
            {
                MessageBox.Show("开始采样");
            }
            else
            {
                MessageBox.Show("失败");
            }

            //启动取数线程
            m_bThread = true;
            DataThread = new Thread(GetDataThread);
            // 清空list
            listBox1.Items.Clear();
            DataThread.SetApartmentState(ApartmentState.STA);
            DataThread.IsBackground = true;
            DataThread.Name = "GetData";
            DataThread.Start(this);

            GetDataThreadCallBackToDir();
            //m_bThread = true;
            //DataThread = new Thread(GetDataThread1);
            //// 清空list
            //listBox1.Items.Clear();
            //DataThread.SetApartmentState(ApartmentState.STA);
            //DataThread.IsBackground = true;
            //DataThread.Name = "GetData";
            //DataThread.Start(this);

            //m_bThread = true;
            //DataThread = new Thread(GetDataThread2);
            //// 清空list
            //listBox1.Items.Clear();
            //DataThread.SetApartmentState(ApartmentState.STA);
            //DataThread.IsBackground = true;
            //DataThread.Name = "GetData";
            //DataThread.Start(this);
        }

        private void buttonStopSample_Click(object sender, EventArgs e)
        {
            SetSampleDialog(true);

            int nIsSampling;
            //是否正在采集数据
            axDHTestHardWare.IsSampling(out nIsSampling);
            if (nIsSampling == 1)
            {
                // 停止采样线程
                m_bThread = false;
                int nStopSample;
                ////停止采样
                axDHTestHardWare.StopSample(out nStopSample);
            }
        }

        private void buttonBalance_Click(object sender, EventArgs e)
        {
            int nReturnValue;
            axDHTestHardWare.AllChannelBalance(out nReturnValue);
            if (nReturnValue == 1)
            {
                MessageBox.Show("所有通道平衡");
            }
            else { MessageBox.Show("平衡失败"); }
            
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            int nReturnValue;
            axDHTestHardWare.AllChannelClearZero(out nReturnValue);
            if (nReturnValue == 1)
            {
                MessageBox.Show("所有通道已清零");
            }
            else
            {
                MessageBox.Show("清零异常");
            }
        }
    }
}
