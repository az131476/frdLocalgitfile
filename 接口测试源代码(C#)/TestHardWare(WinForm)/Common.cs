using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestHardWare_WinForm_
{
    public static class ChanHelp
    {
        //将字符串进行分解。strSeprator中的任何一个字符都作为分隔符。返回分节得到的字符串数目
        public static int BreakString(string strSrc, out List<string> lstDest, string strSeprator)
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

        public static float ToFloat(this string value)
        {
            float n;
            float.TryParse(value, out n);

            return n;
        }

        public static int ToInt(this string value)
        {
            int n;
            int.TryParse(value, out n);

            return n;
        }

        public static string Left(this string value, char c)
        {
            string strVal = value.Substring(0, value.LastIndexOf(c));
            return strVal;
        }

        public static string Right(this string value, char c)
        {
            string strVal = value.Substring(value.LastIndexOf(c)+1, value.Length - 1 - value.LastIndexOf(c));
            return strVal;
        }

        public static void GetValidFloatString(string strText, out string strFloat)
        {
            strFloat = "";
            if (strText.Contains('.'))
                strFloat = strText.Substring(0, strText.LastIndexOf('.'));
            else
                strFloat = strText;
        }
    }

    // CommonStruct
    public class SampleParam
    {
        /// 采样频率
        public float m_fltSampleFrequency;
        /// 采样方式
        public int m_nSampleMode;
        // 整个数据的抽点比例
        public int m_nRateCount;
        // 瞬态模式下，FFT分析的最大数据块大小将不能超出此值，以点数计
        // 数据块大小，用来约束能够压缩在一屏显示的最大数据量.
        /// 采样块大小
        public int m_nSampleBlockSize;
        /// 采样次数 0 -- 代表采集无限制
        public int m_nSampleTimes;
        /// 触发方式(瞬态模式下有效)
        public int m_nSampleTrigMode;
        /// 延迟点数(瞬态模式下有效)
        public int m_nSampleDelayPoints;
        /// 预览，瞬态模式有效
        public bool m_bAnalysisViewAverage;
        // 采样时钟模式
        public int m_nSampleClkMode;
        // 频率模式
        public int m_nFrequencyMode;

        /// 触发前的抽点比例
        public int m_nPreTrigRateCount;
        // 外触发类型 (暂时无用)
        public int m_nExtraTrigType;
        // 采样模式
        public int m_nIsStepTimeMode;
        // 时间间隔
        public int m_nStepTimeInterval;
        // 是否同步传输
        public bool m_bSyncTrans;
    };

    public class GroupChannel
    {
        /// 组ID
        public int m_GroupID;
        /// 仪器版本,从硬件读取的版本
        public string m_Version;
        /// 仪器类型例如5920、5927
        public int m_nInstrumentType;
        /// 数采接口类型, PCI, 1394, USB, COM，网络等
        public int m_nInterfaceType;
        /// 放大器接口类型
        public int m_nAmpInterfaceType;
        /// 仪器IP
        public string m_strMachineIP;
        // 网卡特征码
        public string m_strMacNumber;
        // 每台仪器控制的通道数
        public int m_nChannelsPerCase;
        // 
        public int m_nADBits;		// AD位 12、14、16、24
        public int m_nADBytes;	// AD字节数 2 、4
        public int m_nADBase;		// ADBase 
        public int m_nSaveBytes;	// 保存数据的字节数 2, 4
        public int m_nDataType;	// 保存数据的类型 short、int、float

        public float m_fltVoltageBase;

        // 实际读取的采集的起始通道
        public int m_nChannelFirst;
        // 实际读取的采集的通道数
        public int m_nChannelNumber;
        // 允许的频率采集模式
        public int m_nAllowFreqMode;

        SampleParam m_SampleParam;
    }

    public class HardChannel
    {
        /// 测量类型, 应变、电压、电荷...
        public int m_nMeasureType;
        /// 通道风格,模拟通道、外转速通道、CAN通道、GPS通道等
        public int m_nChannelStyle;
        /// 通道组ID，多个通道为一组
        public int m_nChannelGroupID;
        // 仪器IP
        public string m_strMachineIP;
        /// 通道ID
        public int m_nChannelID;
        /// 单元ID
        public int m_nCellID;
        /// 在线标志，由硬件检测，用户不可设置
        public bool m_bOnlineFlag;

        public HardChannel()
        {
            m_nMeasureType = 0;
            m_nChannelStyle = 0;
            m_nChannelGroupID = 0;
            m_strMachineIP = "";
            m_nChannelID = 0;
            m_nCellID = 0;
            m_bOnlineFlag = true;
        }
    }

    public struct stuChannelParam
    {
        public int ChannelStyle;
        public int ChannelID;
        public int CellID;
    }

    public class ShowDataEventArgs : EventArgs
    {
        public float[] pfltDats { get; set; }
        public int nReceiveCount { get; set; }

        public ShowDataEventArgs(float[] pfltDats, int nReceiveCount)
        {
            this.pfltDats = pfltDats;
            this.nReceiveCount = nReceiveCount;
        }
    }

}
