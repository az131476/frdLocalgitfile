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
	// ���沼��
	void Save(MSXML2::IXMLDOMElementPtr node) const;
	// ���ز���
	void Load(MSXML2::IXMLDOMNodePtr node);

protected:
	void SetValue(string strName, string strValue);

public:
	/// <summary>
	/// ����Ƶ��
	/// </summary>
	float m_fltSampleFrequency;

	/// <summary>
	/// ������ʽ
	/// </summary>
	int m_nSampleMode;

	// �������ݵĳ�����
	int m_nRateCount;

	// ˲̬ģʽ�£�FFT������������ݿ��С�����ܳ�����ֵ���Ե�����
	// ���ݿ��С������Լ���ܹ�ѹ����һ����ʾ�����������.
	/// <summary>
	/// �������С
	/// </summary>
	int m_nSampleBlockSize;
	/// <summary>
	/// �������� 0 -- ����ɼ�������
	/// </summary>
	int m_nSampleTimes;
	/// <summary>
	/// ������ʽ(˲̬ģʽ����Ч)
	/// </summary>
	int m_nSampleTrigMode;
	/// <summary>
	/// �ӳٵ���(˲̬ģʽ����Ч)
	/// </summary>
	int m_nSampleDelayPoints;

	/// <summary>
	/// Ԥ����˲̬ģʽ��Ч
	/// </summary>
	BOOL m_bAnalysisViewAverage;

	// ����ʱ��ģʽ
	int m_nSampleClkMode;

	// Ƶ��ģʽ
	int m_nFrequencyMode;	

    // ����ʱʹ�õ���ʼͨ��
	int m_nSampleChnFirst;
    // ����ʱʹ�õ�ͨ����
    int m_nSampleChnNumber;

	/// <summary>
	/// ����ǰ�ĳ�����
	/// </summary>
	int m_nPreTrigRateCount;
	// �ⴥ������ (��ʱ����)
	int m_nExtraTrigType;
	// ����ģʽ
	int m_nIsStepTimeMode;
	// ʱ����
	int m_nStepTimeInterval;

	// �Ƿ�ͬ������
	BOOL m_bSyncTrans;

};