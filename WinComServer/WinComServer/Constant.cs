using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinComServer
{
    class Constant
    {
        public const int SHOW_CHANNEL_USE = 3; 	/// 通道使用标志
        public const int SHOW_CHANNEL_MEASURETYPE = 4; /// 通道测量类型
        public const int SHOW_CHANNEL_FULLVALUE = 5; 	/// 满度量程
        public const int SHOW_CHANNEL_SENSECOEF = 6; 	/// 传感器灵敏度
        public const int SHOW_CHANNEL_UPFREQ = 10; 	/// 上限频率
        public const int SHOW_CHANNEL_DOWNFREQ = 11; 	/// 下限频率
        public const int SHOW_CHANNEL_ACQ_INPUTMODE = 12; 	/// 输入方式
        public const int SHOW_CHANNEL_ANTIFILTER = 14; 	/// 抗混滤波器
        //////////////////////////////////////////////////////////////////////////
        // 应变应力
        public const int SHOW_STRAIN_SHOWTYPE = 27; 	/// 应变应力显示类型
        public const int SHOW_STRAIN_BRIDGETYPE = 28; 	/// 桥路方式
        public const int SHOW_STRAIN_GAUGE = 29; 	/// 应变计阻值
        public const int SHOW_STRAIN_LEAD = 30; 	/// 导线阻值
        public const int SHOW_STRAIN_SENSECOEF = 31; 	/// 灵敏度系数
        public const int SHOW_STRAIN_POSION = 32; 	/// 泊松比
        public const int SHOW_STRAIN_ELASTICITY = 33; 	/// 弹性模量

        //////////////////////////////////////////////////////////////////////////

        // 桥式传感器
        public const int SHOW_CHANNEL_BRIDGE_MODE = 34; 	/// 供桥
        public const int SHOW_STRAIN_BRIDGEVOLTAGE = 35; 	/// 桥压
        // 铂电阻测温
        public const int SHOW_PT_TYPE = 38; 	/// 铂电阻类型			
        //////////////////////////////////////////////////////////////////////////

        // 热电偶测温
        public const int SHOW_THERMO_TYPE = 40; 	/// 热电偶类型
        public const int SHOW_THERMO_COOLTEMPERATURE = 41; 	/// 冷端温度

        /// <summary>
        /// 测量量类型
        /// </summary>
        public const int SHOW_MEASURE_METERAGE = 80;
        /// <summary>
        /// 隔离类型
        /// </summary>
        public const int SHOW_SOLATETYPE = 81;
        /// <summary>
        /// 滤波器类型
        /// </summary>
        public const int SHOW_FILTERTYPE = 82;

        public const int SAMPLE_ANALOG_DATA = 0;	// 模拟通道数据
        public const int SAMPLE_STATIC_DATA = 1;	// 静态通道数据

        /// <summary>
        /// 瞬态记录
        /// </summary>
        public const int SAMPLE_MODE_INSTANT = 1;
        /// <summary>
        /// 连续记录
        /// </summary>
        public const int SAMPLE_MODE_CONTINUAL = 2;
        /// <summary>
        /// 触发连续
        /// </summary>
        public const int SAMPLE_MODE_TRIGCONTINUAL = 3;
    }
}
