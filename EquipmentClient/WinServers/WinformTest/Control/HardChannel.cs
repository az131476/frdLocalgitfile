using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
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
}
