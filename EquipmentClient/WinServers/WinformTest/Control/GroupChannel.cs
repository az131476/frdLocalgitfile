using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
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
}
