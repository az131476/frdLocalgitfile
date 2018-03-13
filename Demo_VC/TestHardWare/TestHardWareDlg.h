
// TestHardWareDlg.h : 头文件
//

#pragma once

#include "Common.h"
#include "Constdefine.h"
#include "GroupChannel.h"
#include "afxwin.h"
#include "dhtesthardware.h"
#include "DlgShowData.h"

const int SHOW_CHANNEL_USE = 3; 	/// 通道使用标志
const int SHOW_CHANNEL_MEASURETYPE = 4; /// 通道测量类型
const int SHOW_CHANNEL_FULLVALUE = 5; 	/// 满度量程
const int SHOW_CHANNEL_SENSECOEF = 6; 	/// 传感器灵敏度
const int SHOW_CHANNEL_UPFREQ = 10; 	/// 上限频率
const int SHOW_CHANNEL_DOWNFREQ = 11; 	/// 下限频率
const int SHOW_CHANNEL_ACQ_INPUTMODE = 12; 	/// 输入方式
const int SHOW_CHANNEL_ANTIFILTER = 14; 	/// 抗混滤波器
//////////////////////////////////////////////////////////////////////////
// 应变应力
const int SHOW_STRAIN_SHOWTYPE = 27; 	/// 应变应力显示类型
const int SHOW_STRAIN_BRIDGETYPE = 28; 	/// 桥路方式
const int SHOW_STRAIN_GAUGE = 29; 	/// 应变计阻值
const int SHOW_STRAIN_LEAD = 30; 	/// 导线阻值
const int SHOW_STRAIN_SENSECOEF = 31; 	/// 灵敏度系数
const int SHOW_STRAIN_POSION = 32; 	/// 泊松比
const int SHOW_STRAIN_ELASTICITY = 33; 	/// 弹性模量

//////////////////////////////////////////////////////////////////////////

// 桥式传感器
const int SHOW_CHANNEL_BRIDGE_MODE = 34; 	/// 供桥
const int SHOW_STRAIN_BRIDGEVOLTAGE = 35; 	/// 桥压
// 铂电阻测温
const int SHOW_PT_TYPE = 38; 	/// 铂电阻类型			
//////////////////////////////////////////////////////////////////////////

// 热电偶测温
const int SHOW_THERMO_TYPE = 40; 	/// 热电偶类型
const int SHOW_THERMO_COOLTEMPERATURE = 41; 	/// 冷端温度

/// <summary>
/// 测量量类型
/// </summary>
const int SHOW_MEASURE_METERAGE = 80;
/// <summary>
/// 隔离类型
/// </summary>
const int SHOW_SOLATETYPE = 81;
/// <summary>
/// 滤波器类型
/// </summary>
const int SHOW_FILTERTYPE = 82;

const int SAMPLE_ANALOG_DATA = 0;	// 模拟通道数据
const int SAMPLE_STATIC_DATA = 1;	// 静态通道数据

/// <summary>
/// 瞬态记录
/// </summary>
const int SAMPLE_MODE_INSTANT = 1;
	/// <summary>
	/// 连续记录
	/// </summary>
const int SAMPLE_MODE_CONTINUAL = 2;
	/// <summary>
	/// 触发连续
	/// </summary>
const int SAMPLE_MODE_TRIGCONTINUAL = 3;

// CTestHardWareDlg 对话框
class CTestHardWareDlg : public CDialogEx
{
// 构造
public:
	CTestHardWareDlg(CWnd* pParent = NULL);	// 标准构造函数

	virtual ~CTestHardWareDlg();
// 对话框数据
	enum { IDD = IDD_TESTHARDWARE_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持


// 实现
protected:
	HICON m_hIcon;

	// 生成的消息映射函数
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

	// 仪器数据对应的索引
	map<int, int> m_mapBufferIndex; // key - MachineID, value - nIndex, 
	// 获取仪器数据处理时，所需的index
	void GetBufferIndex();
	// 
	bool IsGroupNoData(int nGroupID, vector<int> lstNoData);

protected:
	long IsConnectMachine();						//检查仪器是否连接
	long InitInterface();							//初始化仪器控制接口
	void GetAllGroupChannel();						//获取所有通道组信息
	void ClearAllGroupChannel();					//释放内存
	void InitChannelCombo();						//初始化通道选择列表
	void GetSampleFreqList();						//获取仪器采样频率列表
	long GetSampleParam();							//获取当前采样参数
	long SetSampleParam();							//设置采样参数
	void InitFrepCombo();							//初始化采样频率选择列表
	void InitSampleMode();							//初始化采样模式
	void InitSampleBlockSize();						//瞬态采样下，初始化采样块大小
	void GetParamSelectValue();						//获取参数可选项列表
	string GetMachineIP(int nGroupID);				//获取仪器ID
	//获取指定仪器信息
	GroupChannel *FindGroupChannel(int nChannelGroupID, string strMachineIP);
	//除通道信息外获取所有参数
	void RefreshAllParam();
	//获取通道参数
	void GetChannelParam(int nID, ChannelParam &ChanParam);
	//测点类型为应变花时初始化子参数
	void InitStrainParam(bool bEnable);
	//初始化量程选择列表
	void InitFullValueCombo(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//初始化灵敏度文本框
	void InitSenseCoefEdit();
	//初始化通道测点类型
	void InitMeasureType(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//初始化上限频率
	void InitUpFreq(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//初始化输入方式
	void InitInputMode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID);
	//设置灵敏度
	void SetSenseCoef();
	//启动采样后设置界面
	void SetSampleStartDialog();
	//停止采样后设置界面
	void SetSampleStopDialog();
	// 构建信号源通道信息字符串
	string CreateSignalChannelKeyString();
	// 构建数采通道信息字符串
	string CreateChannelKeyString();
	//修改参数并发送参数码至仪器
	long ModifyParamAndSendCode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID, long ShowParamID, LPCTSTR strParamValue, long nSelectIndex);


	// 信号源通道信息
	void InitSignalDialog();
	// 根据信号源通道类型 显示相关可设置选项
	void ShowWindowForSignalType(int nSignalType);
	// 隐藏所有信号源控件
	void SetAllWindowHide();
	// 当信号源工作时禁用所有信号源控件
	void EnableAllSignalDialog(BOOL bEnable);
	long ModifySignalParam(long ShowParamID, CString strValue, long nSelectIndex);
	// 初始化波形类型
	void InitSignalWave();
	// 获取相关参数并显示
	void SetSignalInfo(int nWaveForm);

/*	//绘图
	void DrawGraph(CDC *pDC);
	//确定图形大小
	void GetGraphAttributes();
	// 绘制背景
	void DrawBackground(CDC *pDC);
	// 绘制网格
	void DrawGrid(CDC *pDC);
	// 绘制曲线
	void DrawLine(CDC *pDC);
*/
protected:
	Channel_Key m_ChannelKey;
	GroupChannel m_GroupChannel;
	stuSampleParam m_SampleParam;

	vector<Channel_Key> m_vecChannelKey;
	list<string> m_listFreq;
	vector<stuGroupChannel> m_vecGroupChannel;			//通道组信息
	vector<stuHardChannel> m_vecHardChannel;			//通道信息

	list<string> m_listFullValue;
	list<string> m_listChannelMeasure;
	list<string> m_listUpFreq;
	list<string> m_listInputMode;
	list<string> m_listStrainType;
	list<string> m_listBridgeType;
	// 信号源通道波形类型
	list<string> m_listSignalWave;
	string m_strSenseCoef;

	// 采样线程句柄
	CWinThread	*m_pGetDataThread;
	bool m_bThread;

	// 显示数据窗口
	CDlgShowData m_DlgShowData;

protected:
	static UINT GetDataThread(LPVOID pParam);
	static UINT SingleGetDataThread(LPVOID pParam);

protected:
	CRect m_rcClient;
	// 图形属性
	int m_nGraphLeft;		// 
	int m_nGraphRight;		// 
	int m_nGraphTop;		// 
	int m_nGraphBottom;		// 
	int m_nGraphWidth;		// 图形区域的宽度
	int m_nGraphHeight;		// 图形区域的高度
	float m_fltRatioX;		// X轴比例系数
	float m_fltRatioY;		// Y轴比例系数
	int m_nGraphBaseLineX;	// X轴基线位置
	int m_nGraphBaseLineY;	// Y轴基线位置

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
