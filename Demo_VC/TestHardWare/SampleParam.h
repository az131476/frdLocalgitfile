#pragma once

#include <string>
#include <map>
using namespace std;

#import "bin\msxml4.dll"

class CSampleParam
{
public:
	CSampleParam(void);
	~CSampleParam(void);

public:
	static const string XML_SAMPLEPARAM;

	static const string XML_SAMPLEFREQUENCY;
	static const string XML_SAMPLEMODE;
	static const string XML_SAMPLEBLOCKSIZE;
	static const string XML_SAMPLETIMES;
	static const string XML_SAMPLETRIGMODE;
	static const string XML_SAMPLEDELAYPOINTS;
	static const string XML_RATECOUNT;
	static const string XML_ANALYSISVIEWAVERAGE;
	static const string XML_SAMPLECLKMODE;
	static const string XML_FREQUENCYMODE;
	static const string XML_SAMPLECHNFIRST;
	static const string XML_SAMPLECHNNUMBER;
	static const string XML_SYNCTRANS;

	static const int N_SAMPLEFREQUENCY = 0;
	static const int N_SAMPLEMODE = 1;
	static const int N_SAMPLEBLOCKSIZE = 2;
	static const int N_SAMPLETIMES = 3;
	static const int N_SAMPLETRIGMODE = 4;
	static const int N_SAMPLEDELAYPOINTS = 5;
	static const int N_RATECOUNT = 6;
	static const int N_ANALYSISVIEWAVERAGE = 7;
	static const int N_SAMPLECLKMODE = 8;
	static const int N_FREQUENCYMODE = 9;
	static const int N_SAMPLECHNFIRST = 10;
	static const int N_SAMPLECHNNUMBER = 11;
	static const int N_SYNCTRANS = 12;

	static map<string ,int> _Mapkey;

public:
	// 保存布局
	void Save(MSXML2::IXMLDOMElementPtr node) const;
	// 加载布局
	void Load(MSXML2::IXMLDOMNodePtr node);

protected:
	void SetValue(string strName, string strValue);

public:
	/// <summary>
	/// 采样频率
	/// </summary>
	float m_fltSampleFrequency;

	/// <summary>
	/// 采样方式
	/// </summary>
	int m_nSampleMode;

	// 整个数据的抽点比例
	int m_nRateCount;

	// 瞬态模式下，FFT分析的最大数据块大小将不能超出此值，以点数计
	// 数据块大小，用来约束能够压缩在一屏显示的最大数据量.
	/// <summary>
	/// 采样块大小
	/// </summary>
	int m_nSampleBlockSize;
	/// <summary>
	/// 采样次数 0 -- 代表采集无限制
	/// </summary>
	int m_nSampleTimes;
	/// <summary>
	/// 触发方式(瞬态模式下有效)
	/// </summary>
	int m_nSampleTrigMode;
	/// <summary>
	/// 延迟点数(瞬态模式下有效)
	/// </summary>
	int m_nSampleDelayPoints;

	/// <summary>
	/// 预览，瞬态模式有效
	/// </summary>
	BOOL m_bAnalysisViewAverage;

	// 采样时钟模式
	int m_nSampleClkMode;

	// 频率模式
	int m_nFrequencyMode;	

    // 采样时使用的起始通道
	int m_nSampleChnFirst;
    // 采样时使用的通道数
    int m_nSampleChnNumber;

	/// <summary>
	/// 触发前的抽点比例
	/// </summary>
	int m_nPreTrigRateCount;
	// 外触发类型 (暂时无用)
	int m_nExtraTrigType;
	// 采样模式
	int m_nIsStepTimeMode;
	// 时间间隔
	int m_nStepTimeInterval;

	// 是否同步传输
	BOOL m_bSyncTrans;

};