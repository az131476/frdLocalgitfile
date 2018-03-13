
// TestHardWareDlg.h : ͷ�ļ�
//

#pragma once

#include "Common.h"
#include "Constdefine.h"
#include "GroupChannel.h"
#include "afxwin.h"
#include "dhtesthardware.h"
#include "DlgShowData.h"

const int SHOW_CHANNEL_USE = 3; 	/// ͨ��ʹ�ñ�־
const int SHOW_CHANNEL_MEASURETYPE = 4; /// ͨ����������
const int SHOW_CHANNEL_FULLVALUE = 5; 	/// ��������
const int SHOW_CHANNEL_SENSECOEF = 6; 	/// ������������
const int SHOW_CHANNEL_UPFREQ = 10; 	/// ����Ƶ��
const int SHOW_CHANNEL_DOWNFREQ = 11; 	/// ����Ƶ��
const int SHOW_CHANNEL_ACQ_INPUTMODE = 12; 	/// ���뷽ʽ
const int SHOW_CHANNEL_ANTIFILTER = 14; 	/// �����˲���
//////////////////////////////////////////////////////////////////////////
// Ӧ��Ӧ��
const int SHOW_STRAIN_SHOWTYPE = 27; 	/// Ӧ��Ӧ����ʾ����
const int SHOW_STRAIN_BRIDGETYPE = 28; 	/// ��·��ʽ
const int SHOW_STRAIN_GAUGE = 29; 	/// Ӧ�����ֵ
const int SHOW_STRAIN_LEAD = 30; 	/// ������ֵ
const int SHOW_STRAIN_SENSECOEF = 31; 	/// ������ϵ��
const int SHOW_STRAIN_POSION = 32; 	/// ���ɱ�
const int SHOW_STRAIN_ELASTICITY = 33; 	/// ����ģ��

//////////////////////////////////////////////////////////////////////////

// ��ʽ������
const int SHOW_CHANNEL_BRIDGE_MODE = 34; 	/// ����
const int SHOW_STRAIN_BRIDGEVOLTAGE = 35; 	/// ��ѹ
// ���������
const int SHOW_PT_TYPE = 38; 	/// ����������			
//////////////////////////////////////////////////////////////////////////

// �ȵ�ż����
const int SHOW_THERMO_TYPE = 40; 	/// �ȵ�ż����
const int SHOW_THERMO_COOLTEMPERATURE = 41; 	/// ����¶�

/// <summary>
/// ����������
/// </summary>
const int SHOW_MEASURE_METERAGE = 80;
/// <summary>
/// ��������
/// </summary>
const int SHOW_SOLATETYPE = 81;
/// <summary>
/// �˲�������
/// </summary>
const int SHOW_FILTERTYPE = 82;

const int SAMPLE_ANALOG_DATA = 0;	// ģ��ͨ������
const int SAMPLE_STATIC_DATA = 1;	// ��̬ͨ������

/// <summary>
/// ˲̬��¼
/// </summary>
const int SAMPLE_MODE_INSTANT = 1;
	/// <summary>
	/// ������¼
	/// </summary>
const int SAMPLE_MODE_CONTINUAL = 2;
	/// <summary>
	/// ��������
	/// </summary>
const int SAMPLE_MODE_TRIGCONTINUAL = 3;

// CTestHardWareDlg �Ի���
class CTestHardWareDlg : public CDialogEx
{
// ����
public:
	CTestHardWareDlg(CWnd* pParent = NULL);	// ��׼���캯��

	virtual ~CTestHardWareDlg();
// �Ի�������
	enum { IDD = IDD_TESTHARDWARE_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��


// ʵ��
protected:
	HICON m_hIcon;

	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:

	DECLARE_EVENTSINK_MAP()
	void StatusChangeDhtesthardware1(long ID, LPCTSTR strXMLInfo);

protected:
	struct ChannelParam 
	{
		int ChannelStyle;
		int ChannelID;
		int CellID;
	};

	// �������ݶ�Ӧ������
	map<int, int> m_mapBufferIndex; // key - MachineID, value - nIndex, 
	// ��ȡ�������ݴ���ʱ�������index
	void GetBufferIndex();
	// 
	bool IsGroupNoData(int nGroupID, vector<int> lstNoData);

protected:
	long IsConnectMachine();						//��������Ƿ�����
	long InitInterface();							//��ʼ���������ƽӿ�
	void GetAllGroupChannel();						//��ȡ����ͨ������Ϣ
	void ClearAllGroupChannel();					//�ͷ��ڴ�
	void InitChannelCombo();						//��ʼ��ͨ��ѡ���б�
	void GetSampleFreqList();						//��ȡ��������Ƶ���б�
	long GetSampleParam();							//��ȡ��ǰ��������
	long SetSampleParam();							//���ò�������
	void InitFrepCombo();							//��ʼ������Ƶ��ѡ���б�
	void InitSampleMode();							//��ʼ������ģʽ
	void InitSampleBlockSize();						//˲̬�����£���ʼ���������С
	void GetParamSelectValue();						//��ȡ������ѡ���б�
	string GetMachineIP(int nGroupID);				//��ȡ����ID
	//��ȡָ��������Ϣ
	GroupChannel *FindGroupChannel(int nChannelGroupID, string strMachineIP);
	//��ͨ����Ϣ���ȡ���в���
	void RefreshAllParam();
	//��ȡͨ������
	void GetChannelParam(int nID, ChannelParam &ChanParam);
	//�������ΪӦ�仨ʱ��ʼ���Ӳ���
	void InitStrainParam(bool bEnable);
	//��ʼ������ѡ���б�
	void InitFullValueCombo(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//��ʼ���������ı���
	void InitSenseCoefEdit();
	//��ʼ��ͨ���������
	void InitMeasureType(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//��ʼ������Ƶ��
	void InitUpFreq(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//��ʼ�����뷽ʽ
	void InitInputMode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//����������
	void SetSenseCoef();
	//�������������ý���
	void SetSampleStartDialog();
	//ֹͣ���������ý���
	void SetSampleStopDialog();
	// �����ź�Դͨ����Ϣ�ַ���
	string CreateSignalChannelKeyString();
	// ��������ͨ����Ϣ�ַ���
	string CreateChannelKeyString();
	//�޸Ĳ��������Ͳ�����������
	long ModifyParamAndSendCode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID, long ShowParamID, LPCTSTR strParamValue, long nSelectIndex);


	// �ź�Դͨ����Ϣ
	void InitSignalDialog();
	// �����ź�Դͨ������ ��ʾ��ؿ�����ѡ��
	void ShowWindowForSignalType(int nSignalType);
	// ���������ź�Դ�ؼ�
	void SetAllWindowHide();
	// ���ź�Դ����ʱ���������ź�Դ�ؼ�
	void EnableAllSignalDialog(BOOL bEnable);
	long ModifySignalParam(long ShowParamID, CString strValue, long nSelectIndex);
	// ��ʼ����������
	void InitSignalWave();
	// ��ȡ��ز�������ʾ
	void SetSignalInfo(int nWaveForm);

/*	//��ͼ
	void DrawGraph(CDC *pDC);
	//ȷ��ͼ�δ�С
	void GetGraphAttributes();
	// ���Ʊ���
	void DrawBackground(CDC *pDC);
	// ��������
	void DrawGrid(CDC *pDC);
	// ��������
	void DrawLine(CDC *pDC);
*/
protected:
	Channel_Key m_ChannelKey;
	GroupChannel m_GroupChannel;
	stuSampleParam m_SampleParam;

	vector<Channel_Key> m_vecChannelKey;
	list<string> m_listFreq;
	vector<stuGroupChannel> m_vecGroupChannel;			//ͨ������Ϣ
	vector<stuHardChannel> m_vecHardChannel;			//ͨ����Ϣ

	list<string> m_listFullValue;
	list<string> m_listChannelMeasure;
	list<string> m_listUpFreq;
	list<string> m_listInputMode;
	list<string> m_listStrainType;
	list<string> m_listBridgeType;
	// �ź�Դͨ����������
	list<string> m_listSignalWave;
	string m_strSenseCoef;

	// �����߳̾��
	CWinThread	*m_pGetDataThread;
	bool m_bThread;

	// ��ʾ���ݴ���
	CDlgShowData m_DlgShowData;

protected:
	static UINT GetDataThread(LPVOID pParam);
	static UINT SingleGetDataThread(LPVOID pParam);

protected:
	CRect m_rcClient;
	// ͼ������
	int m_nGraphLeft;		// 
	int m_nGraphRight;		// 
	int m_nGraphTop;		// 
	int m_nGraphBottom;		// 
	int m_nGraphWidth;		// ͼ������Ŀ��
	int m_nGraphHeight;		// ͼ������ĸ߶�
	float m_fltRatioX;		// X�����ϵ��
	float m_fltRatioY;		// Y�����ϵ��
	int m_nGraphBaseLineX;	// X�����λ��
	int m_nGraphBaseLineY;	// Y�����λ��

public:
	int m_nInstrument;
	CComboBox m_comboSampleMode;
	CComboBox m_comboBlockSize;
	CComboBox m_ComboFrep;
	CComboBox m_ComboChannel;
	CButton	m_BtnBalance;
	CButton m_BtnClearZero;

	CComboBox m_ComboFullValue;
	float m_fltSenseCoef;
	CEdit m_SenseCoef;
	CComboBox m_ComboMeasureType;
	CComboBox m_ComboUpFreq;
	CComboBox m_ComboInputMode;

	afx_msg void OnCbnSelchangeComboFrep();
	afx_msg void OnCbnSelchangeComboChannel();
	afx_msg void OnCbnSelchangeComboMeasuretype();
	afx_msg void OnCbnSelchangeComboFullvalue();
	afx_msg void OnCbnSelchangeComboUpfreq();
	afx_msg void OnCbnSelchangeComboInputmode();
	afx_msg void OnEnChangeEditSensecoef();
	afx_msg void OnDestroy();
	afx_msg void OnBnClickedSamplestart();
	afx_msg void OnBnClickedSamplestop();
	afx_msg void OnCbnSelchangeComboSamplemode();
	afx_msg void OnCbnSelchangeComboBlocksize();
	afx_msg void OnBnClickedInit();
	afx_msg void OnBnClickedBalance();
	afx_msg void OnBnClickedClearzero();
	afx_msg void OnBnClickedAutocheck();
	CComboBox m_ComboStrainType;
	CComboBox m_ComboBridgeType;
	CEdit m_StrainGauge;
	CEdit m_strainSenseCoef;
	CEdit m_strainPosion;
	CEdit m_strainElasticity;
	CEdit m_StrainLead;
	afx_msg void OnSelchangeComboType();
	afx_msg void OnSelchangeComboBridgetype();
	afx_msg void OnKillfocusStrainGauge();
	afx_msg void OnKillfocusStrainLead();
	afx_msg void OnKillfocusStrainSensecoef();
	afx_msg void OnKillfocusStrainPosion();
	afx_msg void OnKillfocusStrainElasticity();
	CComboBox m_ComboSignalChannel;
	CComboBox m_ComboSignalType;
	CComboBox m_ComboSignalRateType;
	afx_msg void OnCbnSelchangeComboSignalType();
	CButton m_StartSignal;
	CButton m_StopSignal;
	afx_msg void OnBnClickedStartsignal();
	afx_msg void OnBnClickedStopsignal();
	afx_msg void OnEnChangeSignalExtend();
	afx_msg void OnEnChangeSignalPhase();
	afx_msg void OnEnChangeSignalOccupyScale();
	afx_msg void OnEnChangeSignalCycle();
	afx_msg void OnEnChangeSignalFreq();
	afx_msg void OnEnChangeSignalRate();
	afx_msg void OnEnChangeSignalStartFreq();
	afx_msg void OnEnChangeSignalEndFreq();
	afx_msg void OnCbnSelchangeComboSignalRatestyle();
	afx_msg void OnCbnSelchangeComboSignlalChannel();
	afx_msg void OnEnKillfocusEditSensecoef();

	virtual BOOL PreTranslateMessage(MSG* pMsg);
protected:
	afx_msg LRESULT OnCloseShowdataWindow(WPARAM wParam, LPARAM lParam);
public:
	afx_msg void OnClose();
	afx_msg void OnBnClickedBtnSinglesample();
	CDhtesthardware m_HardWare;
	afx_msg void OnBnClickedCreateCablealg();
	afx_msg void OnBnClickedDelallCablealg();
	afx_msg void OnBnClickedResetCablealg();
	afx_msg void OnBnClickedCalCable();
};
