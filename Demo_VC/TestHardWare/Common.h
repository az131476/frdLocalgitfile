
#include <string>
using namespace std;
#include <atlstr.h>

#define SHOW_SAMPLE_DATA		WM_USER+1
#define CLOSE_SHOWDATA_WINDOW	WM_USER+2

void GetValidFloatString(string strText, string &strFloat);

// �ź�Դͨ��
/// ��������
const int SHOW_GENERATOR_WAVE_FORM = 43;
/// ���ε�ѹ(��ֵ)
const int SHOW_GENERATOR_LEVEL = 44;
/// (��ʼ)Ƶ��
const int SHOW_GENERATOR_START_FREQ = 45;
/// ��λ
const int SHOW_GENERATOR_PHASE = 46;
/// ����Ƶ��
const int SHOW_GENERATOR_END_FREQ = 47;
/// ����ʱ��
const int SHOW_GENERATOR_UPTIME = 48;
/// �½�ʱ��
const int SHOW_GENERATOR_DOWNTIME = 49;
/// ռ�ձ�
const int SHOW_GENERATOR_OCCUPY_SCALE = 50;
/// ����
const int SHOW_GENERATOR_CYCLE = 51;
/// �ļ�·��
const int SHOW_GENERATOR_FILEPATH = 52;
/// ϵ��
const int SHOW_GENERATOR_COIEF = 53;
/// ɨƵʱ��
const int SHOW_GENERATOR_USETIME = 54;
/// ɨƵֵ(ɨƵ�ٶ�)
const int SHOW_GENERATOR_SAOVALUE = 55;
/// ɨƵ��ʽ
const int SHOW_GENERATOR_WIDTHUNIT = 56;

/// �Ƿ�����ʵʱ����Ƶ��
const int SHOW_GENERATOR_ALLOWCHANGEFREQ = 57;

/// �ź�ԴƵ������
const int SHOW_GENERATOR_LOWFREQ = 58;

/// �ź�ԴƵ������
const int SHOW_GENERATOR_HIGHFREQ = 59;

// CommonStruct
struct stuSampleParam
{
	/// ����Ƶ��
	float m_fltSampleFrequency;
	/// ������ʽ
	int m_nSampleMode;
	// �������ݵĳ�����
	int m_nRateCount;
	// ˲̬ģʽ�£�FFT������������ݿ��С�����ܳ�����ֵ���Ե�����
	// ���ݿ��С������Լ���ܹ�ѹ����һ����ʾ�����������.
	/// �������С
	int m_nSampleBlockSize;
	/// �������� 0 -- ����ɼ�������
	int m_nSampleTimes;
	/// ������ʽ(˲̬ģʽ����Ч)
	int m_nSampleTrigMode;
	/// �ӳٵ���(˲̬ģʽ����Ч)
	int m_nSampleDelayPoints;
	/// Ԥ����˲̬ģʽ��Ч
	BOOL m_bAnalysisViewAverage;
	// ����ʱ��ģʽ
	int m_nSampleClkMode;
	// Ƶ��ģʽ
	int m_nFrequencyMode;	

	/// ����ǰ�ĳ�����
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

struct stuGroupChannel 
{
	/// ��ID
	int m_GroupID;
	/// �����汾,��Ӳ����ȡ�İ汾
	string m_Version;
	/// ������������5920��5927
	int m_nInstrumentType;
	/// ���ɽӿ�����, PCI, 1394, USB, COM�������
	int m_nInterfaceType;
	/// �Ŵ����ӿ�����
	int m_nAmpInterfaceType;
	/// ����IP
	string m_strMachineIP;
	// ����������
	string m_strMacNumber;
	// ÿ̨�������Ƶ�ͨ����
	int m_nChannelsPerCase;
	// 
	int m_nADBits;		// ADλ 12��14��16��24
	int m_nADBytes;	// AD�ֽ��� 2 ��4
	int m_nADBase;		// ADBase 
	int m_nSaveBytes;	// �������ݵ��ֽ��� 2, 4
	int m_nDataType;	// �������ݵ����� short��int��float

	float m_fltVoltageBase;

	// ʵ�ʶ�ȡ�Ĳɼ�����ʼͨ��
	int m_nChannelFirst;
	// ʵ�ʶ�ȡ�Ĳɼ���ͨ����
	int m_nChannelNumber;
	// �����Ƶ�ʲɼ�ģʽ
	int m_nAllowFreqMode;

	stuSampleParam m_SampleParam;
};

struct stuHardChannel
{
	/// ��������, Ӧ�䡢��ѹ�����...
	int m_nMeasureType;
	/// ͨ�����,ģ��ͨ������ת��ͨ����CANͨ����GPSͨ����
	int m_nChannelStyle;
	/// ͨ����ID�����ͨ��Ϊһ��
	int m_nChannelGroupID;
	// ����IP
	char m_strMachineIP[32];
	/// ͨ��ID
	int m_nChannelID;
	/// ��ԪID
	int m_nCellID;
	/// ���߱�־����Ӳ����⣬�û���������
	BOOL m_bOnlineFlag;

	stuHardChannel::stuHardChannel()
	{
		m_nMeasureType = 0;
		m_nChannelStyle = 0;
		m_nChannelGroupID = 0;
		strcpy_s(m_strMachineIP, "");
		m_nChannelID = 0;
		m_nCellID = 0;
		m_bOnlineFlag = TRUE;
	}
};