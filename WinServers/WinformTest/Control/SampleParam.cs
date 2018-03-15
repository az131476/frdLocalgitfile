using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
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
    }
}
