// TestHardWareDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "TestHardWare.h"
#include "TestHardWareDlg.h"
#include "afxdialogex.h"
#include <atlconv.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#endif

// ����Ӧ�ó��򡰹��ڡ��˵���� CAboutDlg �Ի���

class CAboutDlg : public CDialogEx
{
public:
	CAboutDlg();

	// �Ի�������
	enum { IDD = IDD_ABOUTBOX };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV ֧��

	// ʵ��
protected:
	DECLARE_MESSAGE_MAP()
};

CAboutDlg::CAboutDlg() : CDialogEx(CAboutDlg::IDD)
{
}

void CAboutDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CAboutDlg, CDialogEx)
END_MESSAGE_MAP()


// CTestHardWareDlg �Ի���




CTestHardWareDlg::CTestHardWareDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(CTestHardWareDlg::IDD, pParent)
	, m_fltSenseCoef(0)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	m_nInstrument = 1;
	m_bThread = false;
	m_pGetDataThread = NULL;
}

CTestHardWareDlg::~CTestHardWareDlg()
{
	ClearAllGroupChannel();
}

void CTestHardWareDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_BALANCE, m_BtnBalance);
	DDX_Control(pDX, IDC_CLEARZERO, m_BtnClearZero);
	DDX_Control(pDX, IDC_COMBO_FREP, m_ComboFrep);
	DDX_Control(pDX, IDC_COMBO_CHANNEL, m_ComboChannel);
	DDX_Control(pDX, IDC_COMBO_FULLVALUE, m_ComboFullValue);
	DDX_Control(pDX, IDC_EDIT_SENSECOEF, m_SenseCoef);
	DDX_Control(pDX, IDC_COMBO_MEASURETYPE, m_ComboMeasureType);
	DDX_Control(pDX, IDC_COMBO_UPFREQ, m_ComboUpFreq);
	DDX_Control(pDX, IDC_COMBO_INPUTMODE, m_ComboInputMode);
	DDX_Control(pDX, IDC_COMBO_SAMPLEMODE, m_comboSampleMode);
	DDX_Control(pDX, IDC_COMBO_BLOCKSIZE, m_comboBlockSize);
	DDX_Control(pDX, IDC_COMBO_TYPE, m_ComboStrainType);
	DDX_Control(pDX, IDC_COMBO_BRIDGETYPE, m_ComboBridgeType);
	DDX_Control(pDX, IDC_STRAIN_GAUGE, m_StrainGauge);
	DDX_Control(pDX, IDC_STRAIN_SENSECOEF, m_strainSenseCoef);
	DDX_Control(pDX, IDC_STRAIN_POSION, m_strainPosion);
	DDX_Control(pDX, IDC_STRAIN_ELASTICITY, m_strainElasticity);
	DDX_Control(pDX, IDC_STRAIN_LEAD, m_StrainLead);
	DDX_Control(pDX, IDC_COMBO_SIGNLAL_CHANNEL, m_ComboSignalChannel);
	DDX_Control(pDX, IDC_COMBO_SIGNAL_TYPE, m_ComboSignalType);
	DDX_Control(pDX, IDC_COMBO_SIGNAL_RATESTYLE, m_ComboSignalRateType);
	DDX_Control(pDX, IDC_STARTSIGNAL, m_StartSignal);
	DDX_Control(pDX, IDC_STOPSIGNAL, m_StopSignal);
	DDX_Control(pDX, IDC_DHTESTHARDWARE1, m_HardWare);
}

BEGIN_MESSAGE_MAP(CTestHardWareDlg, CDialogEx)
	ON_WM_SYSCOMMAND()
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_CBN_SELCHANGE(IDC_COMBO_FREP, &CTestHardWareDlg::OnCbnSelchangeComboFrep)
	ON_CBN_SELCHANGE(IDC_COMBO_CHANNEL, &CTestHardWareDlg::OnCbnSelchangeComboChannel)
	ON_CBN_SELCHANGE(IDC_COMBO_MEASURETYPE, &CTestHardWareDlg::OnCbnSelchangeComboMeasuretype)
	ON_CBN_SELCHANGE(IDC_COMBO_FULLVALUE, &CTestHardWareDlg::OnCbnSelchangeComboFullvalue)
	ON_CBN_SELCHANGE(IDC_COMBO_UPFREQ, &CTestHardWareDlg::OnCbnSelchangeComboUpfreq)
	ON_CBN_SELCHANGE(IDC_COMBO_INPUTMODE, &CTestHardWareDlg::OnCbnSelchangeComboInputmode)
	ON_EN_CHANGE(IDC_EDIT_SENSECOEF, &CTestHardWareDlg::OnEnChangeEditSensecoef)
	ON_WM_DESTROY()
	ON_BN_CLICKED(IDC_SAMPLESTART, &CTestHardWareDlg::OnBnClickedSamplestart)
	ON_BN_CLICKED(IDC_SAMPLESTOP, &CTestHardWareDlg::OnBnClickedSamplestop)
	ON_CBN_SELCHANGE(IDC_COMBO_SAMPLEMODE, &CTestHardWareDlg::OnCbnSelchangeComboSamplemode)
	ON_CBN_SELCHANGE(IDC_COMBO_BLOCKSIZE, &CTestHardWareDlg::OnCbnSelchangeComboBlocksize)
	ON_BN_CLICKED(IDC_INIT, &CTestHardWareDlg::OnBnClickedInit)
	ON_BN_CLICKED(IDC_BALANCE, &CTestHardWareDlg::OnBnClickedBalance)
	ON_BN_CLICKED(IDC_CLEARZERO, &CTestHardWareDlg::OnBnClickedClearzero)
	ON_BN_CLICKED(IDC_AUTOCHECK, &CTestHardWareDlg::OnBnClickedAutocheck)
	ON_CBN_SELCHANGE(IDC_COMBO_TYPE, &CTestHardWareDlg::OnSelchangeComboType)
	ON_CBN_SELCHANGE(IDC_COMBO_BRIDGETYPE, &CTestHardWareDlg::OnSelchangeComboBridgetype)
	ON_EN_KILLFOCUS(IDC_STRAIN_GAUGE, &CTestHardWareDlg::OnKillfocusStrainGauge)
	ON_EN_KILLFOCUS(IDC_STRAIN_LEAD, &CTestHardWareDlg::OnKillfocusStrainLead)
	ON_EN_KILLFOCUS(IDC_STRAIN_SENSECOEF, &CTestHardWareDlg::OnKillfocusStrainSensecoef)
	ON_EN_KILLFOCUS(IDC_STRAIN_POSION, &CTestHardWareDlg::OnKillfocusStrainPosion)
	ON_EN_KILLFOCUS(IDC_STRAIN_ELASTICITY, &CTestHardWareDlg::OnKillfocusStrainElasticity)
	ON_CBN_SELCHANGE(IDC_COMBO_SIGNAL_TYPE, &CTestHardWareDlg::OnCbnSelchangeComboSignalType)
	ON_BN_CLICKED(IDC_STARTSIGNAL, &CTestHardWareDlg::OnBnClickedStartsignal)
	ON_BN_CLICKED(IDC_STOPSIGNAL, &CTestHardWareDlg::OnBnClickedStopsignal)
	ON_EN_CHANGE(IDC_SIGNAL_EXTEND, &CTestHardWareDlg::OnEnChangeSignalExtend)
	ON_EN_CHANGE(IDC_SIGNAL_PHASE, &CTestHardWareDlg::OnEnChangeSignalPhase)
	ON_EN_CHANGE(IDC_SIGNAL_OCCUPY_SCALE, &CTestHardWareDlg::OnEnChangeSignalOccupyScale)
	ON_EN_CHANGE(IDC_SIGNAL_CYCLE, &CTestHardWareDlg::OnEnChangeSignalCycle)
	ON_EN_CHANGE(IDC_SIGNAL_FREQ, &CTestHardWareDlg::OnEnChangeSignalFreq)
	ON_EN_CHANGE(IDC_SIGNAL_RATE, &CTestHardWareDlg::OnEnChangeSignalRate)
	ON_EN_CHANGE(IDC_SIGNAL_START_FREQ, &CTestHardWareDlg::OnEnChangeSignalStartFreq)
	ON_EN_CHANGE(IDC_SIGNAL_END_FREQ, &CTestHardWareDlg::OnEnChangeSignalEndFreq)
	ON_CBN_SELCHANGE(IDC_COMBO_SIGNAL_RATESTYLE, &CTestHardWareDlg::OnCbnSelchangeComboSignalRatestyle)
	ON_CBN_SELCHANGE(IDC_COMBO_SIGNLAL_CHANNEL, &CTestHardWareDlg::OnCbnSelchangeComboSignlalChannel)
	ON_EN_KILLFOCUS(IDC_EDIT_SENSECOEF, &CTestHardWareDlg::OnEnKillfocusEditSensecoef)
	ON_MESSAGE(CLOSE_SHOWDATA_WINDOW, &CTestHardWareDlg::OnCloseShowdataWindow)
	ON_WM_CLOSE()
	ON_BN_CLICKED(IDC_BTN_SINGLESAMPLE, &CTestHardWareDlg::OnBnClickedBtnSinglesample)
	ON_BN_CLICKED(IDC_CREATE_CABLEALG, &CTestHardWareDlg::OnBnClickedCreateCablealg)
	ON_BN_CLICKED(IDC_DELALL_CABLEALG, &CTestHardWareDlg::OnBnClickedDelallCablealg)
	ON_BN_CLICKED(IDC_RESET_CABLEALG, &CTestHardWareDlg::OnBnClickedResetCablealg)
	ON_BN_CLICKED(IDC_CAL_CABLE, &CTestHardWareDlg::OnBnClickedCalCable)
END_MESSAGE_MAP()


// CTestHardWareDlg ��Ϣ�������

BOOL CTestHardWareDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	// ��������...���˵�����ӵ�ϵͳ�˵��С�

	// IDM_ABOUTBOX ������ϵͳ���Χ�ڡ�
	ASSERT((IDM_ABOUTBOX & 0xFFF0) == IDM_ABOUTBOX);
	ASSERT(IDM_ABOUTBOX < 0xF000);

	CMenu* pSysMenu = GetSystemMenu(FALSE);
	if (pSysMenu != NULL)
	{
		BOOL bNameValid;
		CString strAboutMenu;
		bNameValid = strAboutMenu.LoadString(IDS_ABOUTBOX);
		ASSERT(bNameValid);
		if (!strAboutMenu.IsEmpty())
		{
			pSysMenu->AppendMenu(MF_SEPARATOR);
			pSysMenu->AppendMenu(MF_STRING, IDM_ABOUTBOX, strAboutMenu);
		}
	}

	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��

	m_HardWare.PrepareInit(1);//����3817J��������
	if (!InitInterface())
	{
		AfxMessageBox(LPCTSTR("��ʼ���ӿ�ʧ��!"));
		return FALSE;
	}

	// ��ʼ���ź�Դͨ������
	InitSignalDialog();

	m_DlgShowData.Create(IDD_DLG_SHOWDATA, this);
	m_DlgShowData.ShowWindow(SW_HIDE);
	m_DlgShowData.CenterWindow();

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}

void CTestHardWareDlg::OnSysCommand(UINT nID, LPARAM lParam)
{
	if ((nID & 0xFFF0) == IDM_ABOUTBOX)
	{
		CAboutDlg dlgAbout;
		dlgAbout.DoModal();
	}
	else
	{
		CDialogEx::OnSysCommand(nID, lParam);
	}
}

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void CTestHardWareDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}

	CPaintDC dc(this);
	if(dc == NULL)
		return;
//	DrawGraph(&dc);
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR CTestHardWareDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}

BEGIN_EVENTSINK_MAP(CTestHardWareDlg, CDialogEx)
	ON_EVENT(CTestHardWareDlg, IDC_DHTESTHARDWARE1, 1, CTestHardWareDlg::StatusChangeDhtesthardware1, VTS_I4 VTS_BSTR)
END_EVENTSINK_MAP()

void CTestHardWareDlg::StatusChangeDhtesthardware1(long ID, LPCTSTR strXMLInfo)
{
	TRACE(strXMLInfo);
}

GroupChannel *CTestHardWareDlg::FindGroupChannel(int nChannelGroupID, string strMachineIP)
{
	GroupChannel *pChannelGroup;
	for (int i = 0; i < m_vecGroupChannel.size(); i++)
	{
		int nGroupID = m_vecGroupChannel[i].m_GroupID;
		string strIP = m_vecGroupChannel[i].m_strMachineIP;
		if(nGroupID == nChannelGroupID && strcmp(strIP.data(), strMachineIP.data())==0)
			break;
	}
	return pChannelGroup;
}

/// �Ƿ���鲻��������
bool CTestHardWareDlg::IsGroupNoData(int nGroupID, vector<int> lstNoData)
{
	for (int j = 0; j < lstNoData.size(); j++)
	{
		if (lstNoData[j] == nGroupID)
		{
			return true;
		}
	}

	return false;
}

//��������Ƿ�����
long CTestHardWareDlg::IsConnectMachine()
{
	long lReturnValue;
	m_HardWare.IsConnectMachine(&lReturnValue);
	return lReturnValue;
}

//��ʼ���������ƽӿ�
long CTestHardWareDlg::InitInterface()
{
	TCHAR cPath[MAX_PATH];
	GetModuleFileName(NULL, cPath, MAX_PATH);
	CString  strPath = cPath;
	strPath = strPath.Left(strPath.ReverseFind('\\'));

	strPath += "\\config\\";

	long lReturnValue;
	//��ʼ���������ƽӿ�
	m_HardWare.Init(LPCTSTR(strPath), LPCTSTR("chinese"), &lReturnValue);
	return lReturnValue;
}

//�ͷ��ڴ�
void CTestHardWareDlg::ClearAllGroupChannel()
{
	m_vecGroupChannel.clear();
	m_vecHardChannel.clear();
}

//��ȡ����ͨ������Ϣ
void CTestHardWareDlg::GetAllGroupChannel()
{
	ClearAllGroupChannel();
	int i = 0, j = 0;
	long nGroupCount;
	m_HardWare.GetChannelGroupCount(&nGroupCount);

	long nGroupChannelID, nChannelFirst, nChannelNumber, nDataType;
	string strMachineIP;
	long lReturnValue = 0;
	stuGroupChannel stuGroupChannel;
	for (i = 0; i < nGroupCount; i++)
	{
		BSTR *strValue = new BSTR();

		// ��ȡͨ������Ϣ
		m_HardWare.GetChannelGroup(i, &nGroupChannelID, strValue, &lReturnValue);
		char *pTempData = _com_util::ConvertBSTRToString(*strValue);
		strMachineIP = pTempData;
		delete pTempData;
		stuGroupChannel.m_GroupID = nGroupChannelID;
		stuGroupChannel.m_strMachineIP = strMachineIP;

		// ��ȡĳ̨��������ʼͨ��ID
		m_HardWare.GetChannelFirstID(nGroupChannelID, strMachineIP.data(), &nChannelFirst);
		stuGroupChannel.m_nChannelFirst = nChannelFirst;

		// ��ȡĳ̨�������ܵ�ͨ����
		m_HardWare.GetChannelCount(nGroupChannelID, strMachineIP.data(), &nChannelNumber);
		stuGroupChannel.m_nChannelNumber = nChannelNumber;

		// ��ȡĳ̨��������������
		m_HardWare.GetChannelGroupDataType(nGroupChannelID, strMachineIP.data(), &nDataType);
		stuGroupChannel.m_nDataType = nDataType;

		m_vecGroupChannel.push_back(stuGroupChannel);
		delete strValue;

		long bOnLine = 1;
		long nMeasureType = 0;
		stuHardChannel HardChannel;
		// ͨ����Ϣ
		for (j = 0; j < nChannelNumber; j ++)
		{
			long nChannelID = nChannelFirst + j;

			m_HardWare.IsChannelOnLine(nGroupChannelID, strMachineIP.data(), nChannelID, &bOnLine);
			m_HardWare.GetChannelMeasureType(nGroupChannelID, strMachineIP.data(), nChannelID, &nMeasureType);
			HardChannel.m_nChannelGroupID = nGroupChannelID;
			HardChannel.m_nChannelID = nChannelID;
			HardChannel.m_nMeasureType = nMeasureType;
			HardChannel.m_bOnlineFlag = bOnLine;

			memcpy(HardChannel.m_strMachineIP, strMachineIP.data(), 32);
			m_vecHardChannel.push_back(HardChannel);
		}
	}

	GetBufferIndex();
}

void CTestHardWareDlg::GetBufferIndex()
{
	// �����е����ݰ�������ID���У���С����
	vector<int> vecMachineID;
	for(int i =0 ; i < m_vecGroupChannel.size(); i ++)
	{
		vecMachineID.push_back(m_vecGroupChannel[i].m_GroupID);
	}

	vector<int> sortID;
	for(int i = 0; i < vecMachineID.size(); i++)
	{
		bool bFind = false;
		for(vector<int>::iterator it = sortID.begin(); it != sortID.end(); it++)
		{
			if(*it > vecMachineID[i])
			{
				bFind = true;
				sortID.insert(it, vecMachineID[i]);
				break;
			}
		}

		if(!bFind)
			sortID.push_back(vecMachineID[i]);
	}

	for(int i = 0; i < sortID.size(); i++)
	{
		m_mapBufferIndex[sortID[i]] = i;
	}
}

//��ʼ��ͨ��ѡ���б�
void CTestHardWareDlg::InitChannelCombo()
{
	m_ComboChannel.ResetContent();
	for (int i = 0; i < m_vecHardChannel.size(); i++)
	{
		// ����������ͨ��
		if (!m_vecHardChannel[i].m_bOnlineFlag)
			continue;

		CString strGroupID;
		int nGroupID = m_vecHardChannel[i].m_nChannelGroupID;
		strGroupID.Format("%d", nGroupID+1);

		CString strChannelID;
		strChannelID.Format("%d", m_vecHardChannel[i].m_nChannelID+1);

		CString strText = strGroupID + "-" + strChannelID;

		m_ComboChannel.AddString(strText);
	}
	if (m_vecHardChannel.size() > 0)
		m_ComboChannel.SetCurSel(0);

	// ��ʼ����������
	InitSignalWave();
}

//��ȡ��������Ƶ���б�
void CTestHardWareDlg::GetSampleFreqList()
{
	BSTR *strList = new BSTR();
	//��ȡ����Ƶ�ʿ�ѡ��
	m_HardWare.GetSampleFreqList(2, strList);
	char *pTemData = _com_util::ConvertBSTRToString(*strList);
	string strFrepList = pTemData;
	delete pTemData;
	int nFreqCount = m_GroupChannel.BreakString(strFrepList, m_listFreq, string("|"));

	delete strList;
}

//��ʼ������Ƶ��ѡ���б�
void CTestHardWareDlg::InitFrepCombo()
{
	m_ComboFrep.ResetContent();

	float fltCurFreq = m_SampleParam.m_fltSampleFrequency;

	int nCount = 0;
	int nCurSel = -1;
	string strFreq;
	for (list<string>::iterator it = m_listFreq.begin(); it != m_listFreq.end(); it++)
	{
		string strText = *it;

		GetValidFloatString(strText, strFreq);
		m_ComboFrep.AddString(strFreq.data());

		float flt = atof(strFreq.data());
		if (fltCurFreq == flt)
		{
			nCurSel = nCount;
		}
		nCount++;
	}
	if (nCurSel >= 0)
	{
		m_ComboFrep.SetCurSel(nCurSel);
	}
}

void CTestHardWareDlg::InitSampleMode()
{
	int nCurMode = m_SampleParam.m_nSampleMode;
	m_comboSampleMode.SetCurSel(nCurMode - 1);
}

void CTestHardWareDlg::InitSampleBlockSize()
{
	int nCurMode = m_comboSampleMode.GetCurSel();
	if (nCurMode == 0)
		m_comboBlockSize.EnableWindow(TRUE);
	else
		m_comboBlockSize.EnableWindow(FALSE);

	int nBlockSize = m_SampleParam.m_nSampleBlockSize;
	CString strBlockSize;
	strBlockSize.Format("%d", nBlockSize);
	m_comboBlockSize.SelectString(0, strBlockSize);
}

void CTestHardWareDlg::OnCbnSelchangeComboFrep()
{
	SetSampleParam();
}

//��ȡ��ǰ��������
long CTestHardWareDlg::GetSampleParam()
{
	float fltSampleFreq;
	long nSampleMode, nTrigMode, nBlockSize, nDelayCount;

	//��ȡ��������
	m_HardWare.GetSampleFreq(&fltSampleFreq);
	m_HardWare.GetSampleMode(&nSampleMode);
	m_HardWare.GetSampleTrigMode(&nTrigMode);
	m_HardWare.GetTrigBlockCount(&nBlockSize);
	m_HardWare.GetTrigDelayCount(&nDelayCount);

	m_SampleParam.m_fltSampleFrequency = fltSampleFreq;
	m_SampleParam.m_nSampleMode = nSampleMode;
	m_SampleParam.m_nSampleTrigMode = nTrigMode;
	m_SampleParam.m_nSampleBlockSize = nBlockSize;
	m_SampleParam.m_nSampleDelayPoints = nDelayCount;
	return 1;
}

//���ò�������
long CTestHardWareDlg::SetSampleParam()
{
	string strSampleParam;
	long lReturnValue;
	CString strText;
	m_ComboFrep.GetLBText(m_ComboFrep.GetCurSel(), strText);
	float fltSampleFrequency = atof(strText);
	m_SampleParam.m_fltSampleFrequency = fltSampleFrequency;

	//���ò�������
	m_HardWare.SetSampleFreq(m_SampleParam.m_fltSampleFrequency, &lReturnValue);
	m_HardWare.SetSampleMode(m_SampleParam.m_nSampleMode, &lReturnValue);
	m_HardWare.SetSampleTrigMode(m_SampleParam.m_nSampleTrigMode, &lReturnValue);
	m_HardWare.SetTrigBlockCount(m_SampleParam.m_nSampleBlockSize, &lReturnValue);
	m_HardWare.SetTrigDelayCount(m_SampleParam.m_nSampleDelayPoints, &lReturnValue);

	return lReturnValue;
}

//��ȡ������ѡ���б�
void CTestHardWareDlg::GetParamSelectValue()
{
	CString strText;
	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return;

	m_ComboChannel.GetLBText(nCurSel, strText);
	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	BSTR *strSelectValue = new BSTR();
	//��ȡ������ѡ���б�
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_FULLVALUE, strSelectValue);
	char *pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strFullValueSelect = pTempData;
	int nFullValueCount = m_GroupChannel.BreakString(strFullValueSelect, m_listFullValue, string("|"));
	delete pTempData;

	//��ȡ����ֵ
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_SENSECOEF, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	m_strSenseCoef = pTempData;
	delete pTempData;

	//��ȡ������ѡ���б�
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_MEASURETYPE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strMeasureTypeSelect = pTempData;
	int nMeasureTypeCount = m_GroupChannel.BreakString(strMeasureTypeSelect, m_listChannelMeasure, string("|"));
	delete pTempData;

	//��ȡ������ѡ���б�
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_UPFREQ, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strUpFreq = pTempData;
	int nUpFreqCount = m_GroupChannel.BreakString(strUpFreq, m_listUpFreq, string("|"));
	delete pTempData;

	//��ȡ������ѡ���б�
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_ACQ_INPUTMODE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strInputMode = pTempData;
	int nInputModeCount = m_GroupChannel.BreakString(strInputMode, m_listInputMode, string("|"));
	delete pTempData;

	delete strSelectValue;
}

//��ȡ����ID
string CTestHardWareDlg::GetMachineIP(int nID)
{
	string strMachineIP;
	for (int i = 0; i < m_vecHardChannel.size(); i++)
	{
		int nGroupID = m_vecHardChannel[i].m_nChannelGroupID;
		if (nGroupID == nID)
		{
			strMachineIP = m_vecHardChannel[i].m_strMachineIP;
		}
	}
	return strMachineIP;
}

//��ȡͨ������
void CTestHardWareDlg::GetChannelParam(int nID, ChannelParam &ChanParam)
{
	for (int i = 0; i < m_vecHardChannel.size(); i++)
	{
		int nChannelID = m_vecHardChannel[i].m_nChannelID;
		if (nChannelID == nID)
		{
			ChanParam.ChannelStyle = m_vecHardChannel[i].m_nChannelStyle;
			ChanParam.ChannelID = nChannelID;
			ChanParam.CellID = m_vecHardChannel[i].m_nCellID;
		}
	}
}

//��ʼ������ѡ���б�
void CTestHardWareDlg::InitFullValueCombo(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID)
{
	m_ComboFullValue.ResetContent();
	BSTR *strValue = new BSTR();
	//��ȡ����ֵ
	m_HardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, SHOW_CHANNEL_FULLVALUE, strValue);

	char *pTmpData = _com_util::ConvertBSTRToString(*strValue);
	string strCurValue = pTmpData;
	delete pTmpData;

	int nCount = 0;
	int nSel = -1;
	for (list<string>::iterator it = m_listFullValue.begin(); it != m_listFullValue.end(); it++)
	{
		string strText = *it;
		m_ComboFullValue.AddString(strText.data());
		if (strcmp(strCurValue.data(), strText.data()) == 0)
		{
			nSel = nCount;
		}
		nCount++;
	}
	if (nSel >= 0)
	{
		m_ComboFullValue.SetCurSel(nSel);
	}
	delete strValue;
}

//��ʼ���������ı���
void CTestHardWareDlg::InitSenseCoefEdit()
{
	m_SenseCoef.Clear();
	m_SenseCoef.SetWindowTextA(m_strSenseCoef.data());
}

//��ʼ��ͨ���������
void CTestHardWareDlg::InitMeasureType(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID)
{
	m_ComboMeasureType.ResetContent();

	BSTR *strValue = new BSTR();
	//��ȡ����ֵ
	m_HardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, SHOW_CHANNEL_MEASURETYPE, strValue);
	char *pTmpData = _com_util::ConvertBSTRToString(*strValue);
	string strCurValue = pTmpData;
	delete pTmpData;

	int nCount = 0;
	int nSel = -1;

	for (list<string>::iterator it = m_listChannelMeasure.begin(); it != m_listChannelMeasure.end(); it++)
	{
		string strText = *it;
		m_ComboMeasureType.AddString(strText.data());
		if (strcmp(strCurValue.data(), strText.data()) == 0)
		{
			nSel = nCount;
		}
		nCount++;
	}
	if (nSel >= 0)
	{
		m_ComboMeasureType.SetCurSel(nSel);
	}

	delete strValue;

	if(strcmp(strCurValue.data(), "Ӧ��Ӧ��") == 0)
		InitStrainParam(true);
	else
		InitStrainParam(false);
}

//��ʼ������Ƶ��
void CTestHardWareDlg::InitUpFreq(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID)
{
	m_ComboUpFreq.ResetContent();

	BSTR *strValue = new BSTR();
	//��ȡ����ֵ
	m_HardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, SHOW_CHANNEL_UPFREQ, strValue);
	char *pTemData = _com_util::ConvertBSTRToString(*strValue);
	string strCurValue = pTemData;
	delete pTemData;

	int nCount = 0;
	int nSel = -1;

	for (list<string>::iterator it = m_listUpFreq.begin(); it != m_listUpFreq.end(); it++)
	{
		string strText = *it;
		m_ComboUpFreq.AddString(strText.data());

		if (strcmp(strCurValue.data(), strText.data()) == 0)
		{
			nSel = nCount;
		}
		nCount++;
	}
	if (nSel >= 0)
	{
		m_ComboUpFreq.SetCurSel(nSel);
	}

	delete strValue;
}

//��ʼ�����뷽ʽ
void CTestHardWareDlg::InitInputMode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID)
{
	m_ComboInputMode.ResetContent();
	BSTR *strValue = new BSTR();
	//��ȡ����ֵ
	m_HardWare.GetParamValue(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, SHOW_CHANNEL_ACQ_INPUTMODE, strValue);
	char *pTemData = _com_util::ConvertBSTRToString(*strValue);
	string strCurValue = pTemData;
	delete pTemData;

	int nCount = 0;
	int nSel = -1;

	for (list<string>::iterator it = m_listInputMode.begin(); it != m_listInputMode.end(); it++)
	{
		string strText = *it;
		m_ComboInputMode.AddString(strText.data());
		if (strcmp(strCurValue.data(), strText.data()) == 0)
		{
			nSel = nCount;
		}
		nCount++;
	}
	if (nSel >= 0)
	{
		m_ComboInputMode.SetCurSel(nSel);
	}

	delete strValue;
}

void CTestHardWareDlg::OnCbnSelchangeComboChannel()
{
	RefreshAllParam();
}

void CTestHardWareDlg::RefreshAllParam()
{
	GetParamSelectValue();

	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return;

	CString strText;
	m_ComboChannel.GetLBText(nCurSel, strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	InitMeasureType(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
	InitFullValueCombo(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
	InitUpFreq(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
	InitInputMode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);
	InitSenseCoefEdit();
}

void CTestHardWareDlg::OnCbnSelchangeComboMeasuretype()
{
	CString strText;
	m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	long lResult;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	CString strMeasureType;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	//�޸�ͨ������
	lResult = ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_MEASURETYPE, strMeasureType, nCurSel);
	if (!lResult)
	{
		AfxMessageBox(LPCTSTR("���ò���ʧ�ܣ�"));
	}
	GetParamSelectValue();
	InitFullValueCombo(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID);

	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
		InitStrainParam(true);
	else
		InitStrainParam(false);
}

// �������ΪӦ��Ӧ��ʱ��ʼ���Ӳ���
void CTestHardWareDlg::InitStrainParam(bool bEnable)
{
	m_ComboStrainType.EnableWindow(bEnable);
	m_ComboBridgeType.EnableWindow(bEnable);
	m_StrainLead.EnableWindow(bEnable);
	m_StrainGauge.EnableWindow(bEnable);
	m_strainSenseCoef.EnableWindow(bEnable);
	m_strainPosion.EnableWindow(bEnable);
	m_strainElasticity.EnableWindow(bEnable);
	m_SenseCoef.EnableWindow(!bEnable);
	GetDlgItem(IDC_AUTOCHECK)->EnableWindow(bEnable);

	if(bEnable)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);
		BSTR *strSelectValue = new BSTR();
		//��ȡӦ��Ӧ����ʾ���Ϳ�ѡ���б�
		m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_SHOWTYPE, strSelectValue);
		char *pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strMeasureTypeSelect = pTempData;
		int nMeasureTypeCount = m_GroupChannel.BreakString(strMeasureTypeSelect, m_listStrainType, string("|"));
		delete pTempData;

		//��ʼ����ǰӦ��Ӧ������ֵ
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle,  ChanParam.ChannelID,  ChanParam.CellID, SHOW_STRAIN_SHOWTYPE, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strCurValue = pTempData;
		delete pTempData;
		int nCount = 0;
		int nSel = -1;
		m_ComboStrainType.ResetContent();
		for (list<string>::iterator it = m_listStrainType.begin(); it != m_listStrainType.end(); it++)
		{
			string strText = *it;
			m_ComboStrainType.AddString(strText.data());
			if (strcmp(strCurValue.data(), strText.data()) == 0)
			{
				nSel = nCount;
			}
			nCount++;
		}
		if (nSel >= 0)
		{
			m_ComboStrainType.SetCurSel(nSel);
		}

		//��ȡ��·��ʽ��ѡ���б�
		m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_BRIDGETYPE, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strBridgeTypeSelect = pTempData;
		int nBridgeTypeCount = m_GroupChannel.BreakString(strBridgeTypeSelect, m_listBridgeType, string("|"));
		delete pTempData;
		//��ʼ����ǰ��·��ʽ
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle,  ChanParam.ChannelID,  ChanParam.CellID, SHOW_STRAIN_BRIDGETYPE, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		strCurValue = pTempData;
		delete pTempData;
		nCount = 0;
		nSel = -1;
		m_ComboBridgeType.ResetContent();
		for (list<string>::iterator it = m_listBridgeType.begin(); it != m_listBridgeType.end(); it++)
		{
			string strText = *it;
			m_ComboBridgeType.AddString(strText.data());
			if (strcmp(strCurValue.data(), strText.data()) == 0)
			{
				nSel = nCount;
			}
			nCount++;
		}
		if (nSel >= 0)
		{
			m_ComboBridgeType.SetCurSel(nSel);
		}

		//Ӧ�����ֵ
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_GAUGE, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strStrainGauge = pTempData;
		delete pTempData;
		m_StrainGauge.SetWindowTextA(strStrainGauge.data());

		//������ֵ
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_LEAD, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strStrainLead = pTempData;
		delete pTempData;
		m_StrainLead.SetWindowTextA(strStrainLead.data());

		//������ϵ��
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_SENSECOEF, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strStrainSenseCoef= pTempData;
		delete pTempData;
		m_strainSenseCoef.SetWindowTextA(strStrainSenseCoef.data());

		//���ɱ�
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_POSION, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strStrainPosion= pTempData;
		delete pTempData;
		m_strainPosion.SetWindowTextA(strStrainPosion.data());

		//����ģ��
		m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_ELASTICITY, strSelectValue);
		pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
		string strStrainElasticity= pTempData;
		delete pTempData;
		m_strainElasticity.SetWindowTextA(strStrainElasticity.data());

		delete strSelectValue;
	}
}

void CTestHardWareDlg::OnCbnSelchangeComboFullvalue()
{
	CString strText;
	m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	long lResult;
	int nCurSel = m_ComboFullValue.GetCurSel();
	int nTotalCount = m_ComboFullValue.GetCount();
	CString strFullValue;
	m_ComboFullValue.GetLBText(nCurSel, strFullValue);
	//�޸�ͨ������
	lResult = ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_FULLVALUE, strFullValue, nCurSel);
	if (!lResult)
	{
		AfxMessageBox(LPCTSTR("���ò���ʧ�ܣ�"));
	}
}

void CTestHardWareDlg::OnCbnSelchangeComboUpfreq()
{
	CString strText;
	m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	long lResult;
	int nCurSel = m_ComboUpFreq.GetCurSel();
	CString strUpFreq;
	m_ComboUpFreq.GetLBText(nCurSel, strUpFreq);
	//�޸�ͨ������
	lResult = ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_UPFREQ, strUpFreq, nCurSel);
	if (!lResult)
	{
		AfxMessageBox(LPCTSTR("���ò���ʧ�ܣ�"));
	}
}

void CTestHardWareDlg::OnCbnSelchangeComboInputmode()
{
	CString strText;
	m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	long lResult;
	int nCurSel = m_ComboInputMode.GetCurSel();
	CString strInputMode;
	m_ComboInputMode.GetLBText(nCurSel, strInputMode);
	//�޸�ͨ������
	lResult = ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_ACQ_INPUTMODE, strInputMode, nCurSel);
	if (!lResult)
	{
		AfxMessageBox(LPCTSTR("���ò���ʧ�ܣ�"));
	}
}

void CTestHardWareDlg::SetSenseCoef()
{
	CString strText;
	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return;

	m_ComboChannel.GetLBText(nCurSel, strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);\
		lGroupID -= 1;
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	ChannelParam ChanParam;
	GetChannelParam(int(lChannelID), ChanParam);

	long lResult;
	CString strSenseCoef;
	m_SenseCoef.GetWindowText(strSenseCoef);
	//�޸�ͨ������
	lResult = ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_SENSECOEF, strSenseCoef, 0);
	if (!lResult)
	{
		AfxMessageBox(LPCTSTR("���ò���ʧ�ܣ�"));
	}
}

void CTestHardWareDlg::OnEnChangeEditSensecoef()
{
}

void CTestHardWareDlg::OnEnKillfocusEditSensecoef()
{
 	SetSenseCoef();
 	RefreshAllParam();
}

void CTestHardWareDlg::OnDestroy()
{
	//���ٴ���
	m_DlgShowData.DestroyWindow();
	//�˳��ӿ�
	m_HardWare.DHQuit();
	CDialogEx::OnDestroy();
}

void CTestHardWareDlg::OnBnClickedSamplestart()
{
	SetSenseCoef();
	SetSampleParam();
	SetSampleStartDialog();
	long lIsSampling;
	//�Ƿ����ڲɼ�����
	m_HardWare.IsSampling(&lIsSampling);
	if (lIsSampling)
	{
		AfxMessageBox(LPCTSTR("���������У�����ֹͣ����!"));
		return;
	}

	long lSample;
	//��������
	m_HardWare.StartSample(LPCTSTR("DH3817F"), 0, 1024, &lSample);

	//����ȡ���߳�
	m_bThread = true;
	m_pGetDataThread = AfxBeginThread(GetDataThread, this,THREAD_PRIORITY_NORMAL);

	// ���list
	m_DlgShowData.m_List.ResetContent();
	m_DlgShowData.ShowWindow(SW_SHOW);
}

void CTestHardWareDlg::OnBnClickedSamplestop()
{
	SetSampleStopDialog();

	long lIsSampling;
	//�Ƿ����ڲɼ�����
	m_HardWare.IsSampling(&lIsSampling);
	if (lIsSampling)
	{
		// ֹͣ�����߳�
		m_bThread = false;
		MSG msg;
		DWORD dwRet = 0;
		while(TRUE)
		{
			//ʹ�����߳̿��Դ�����Ϣ������������Ϣ���̷߳��͵���Ϣ��
			dwRet = MsgWaitForMultipleObjects(1,&(m_pGetDataThread->m_hThread),FALSE,10,QS_ALLINPUT);
			switch(dwRet)
			{
			case WAIT_OBJECT_0:
				break;
			case WAIT_OBJECT_0+1://��Ӧmainframe�е����ݴ�����Ϣ
				PeekMessage(&msg,NULL,0,0,PM_REMOVE);
				DispatchMessage(&msg);
				continue;
			case WAIT_TIMEOUT:
				PeekMessage(&msg,NULL,0,0,PM_REMOVE);
				DispatchMessage(&msg);
				continue;
			default:
				break;
			}
			break;
		}

		long lStopSample;
		////ֹͣ����
		m_HardWare.StopSample(&lStopSample);
	}
}

void CTestHardWareDlg::OnCbnSelchangeComboSamplemode()
{
	int nCurSel = m_comboSampleMode.GetCurSel();
	if (nCurSel == 0)
		m_comboBlockSize.EnableWindow(TRUE);
	else
		m_comboBlockSize.EnableWindow(FALSE);

	int nSampleMode = m_comboSampleMode.GetCurSel() + 1;
	m_SampleParam.m_nSampleMode = nSampleMode;
	m_SampleParam.m_nSampleTrigMode = 0;
}

void CTestHardWareDlg::OnCbnSelchangeComboBlocksize()
{
	CString strText;
	m_comboBlockSize.GetLBText(m_comboBlockSize.GetCurSel(), strText);
	int nBlockSize = atoi(strText);
	m_SampleParam.m_nSampleBlockSize = nBlockSize;
}

void CTestHardWareDlg::SetSampleStartDialog()
{
	m_ComboFrep.EnableWindow(FALSE);
	m_comboSampleMode.EnableWindow(FALSE);
	m_comboBlockSize.EnableWindow(FALSE);
	m_ComboChannel.EnableWindow(FALSE);
	m_ComboMeasureType.EnableWindow(FALSE);
	m_ComboFullValue.EnableWindow(FALSE);
	m_ComboUpFreq.EnableWindow(FALSE);
	m_ComboInputMode.EnableWindow(FALSE);
	m_SenseCoef.EnableWindow(FALSE);
	m_BtnBalance.EnableWindow(FALSE);
	m_BtnClearZero.EnableWindow(FALSE);
	CWnd *pWnd = GetDlgItem(IDC_AUTOCHECK);
	pWnd->EnableWindow(FALSE);
}

void CTestHardWareDlg::SetSampleStopDialog()
{
	m_ComboFrep.EnableWindow(TRUE);
	m_comboSampleMode.EnableWindow(TRUE);

	int nCurSel = m_comboSampleMode.GetCurSel();
	if (nCurSel == 0)
		m_comboBlockSize.EnableWindow(TRUE);
	else
		m_comboBlockSize.EnableWindow(FALSE);

	m_ComboChannel.EnableWindow(TRUE);
	m_ComboMeasureType.EnableWindow(TRUE);
	m_ComboFullValue.EnableWindow(TRUE);
	m_ComboUpFreq.EnableWindow(TRUE);
	m_ComboInputMode.EnableWindow(TRUE);
	m_SenseCoef.EnableWindow(TRUE);
	m_BtnBalance.EnableWindow(TRUE);
	m_BtnClearZero.EnableWindow(TRUE);
}

// ��������ͨ����Ϣ�ַ���
string CTestHardWareDlg::CreateChannelKeyString()
{
	string strChannelKeyXML;
	m_vecChannelKey.clear();
	Channel_Key ChannelKey;
	for (int i = 0; i < m_vecHardChannel.size(); i++)
	{
		ChannelKey.m_nGroupID  = m_vecHardChannel[i].m_nChannelGroupID;
		ChannelKey.m_strIP = m_vecHardChannel[i].m_strMachineIP;
		ChannelKey.m_nChannelID = m_vecHardChannel[i].m_nChannelID;
		ChannelKey.m_nChannelStyle = m_vecHardChannel[i].m_nChannelStyle;
		m_vecChannelKey.push_back(ChannelKey);
	}
	// ͨ����Ϣ������XML��ʽ�ַ�����
	strChannelKeyXML = ChannelKey.ConvertToXML(m_vecChannelKey);
	return strChannelKeyXML;
}

void CTestHardWareDlg::OnBnClickedInit()
{
	long lReturnValue;
	m_HardWare.ReConnectAllMac(&lReturnValue);//������DeviceInfo.ini�ļ���IP��������
	if (!IsConnectMachine())
	{
		AfxMessageBox(LPCTSTR("����δ����!"));
		return;
	}

	GetAllGroupChannel();
	InitChannelCombo();

	GetSampleFreqList();
	GetSampleParam();
	InitFrepCombo();
	InitSampleMode();
	InitSampleBlockSize();
	//��ͨ����Ϣ���ȡ���в���
	RefreshAllParam();
}



void CTestHardWareDlg::OnBnClickedBalance()
{
	long lReturnValue;
	m_HardWare.AllChannelBalance(&lReturnValue);
}


void CTestHardWareDlg::OnBnClickedClearzero()
{
	long lReturnValue;
	m_HardWare.AllChannelClearZero(&lReturnValue);
}


void CTestHardWareDlg::OnBnClickedAutocheck()
{
	BSTR *strCheckVal = new BSTR();
	long lReturnValue;
	m_HardWare.MacAutoCheck(strCheckVal,&lReturnValue);
	
	char *pTemData =_com_util::ConvertBSTRToString(*strCheckVal); 
	string strXML = pTemData;
	delete pTemData;

	MSXML2::IXMLDOMDocumentPtr  _Doc;

	try
	{
		HRESULT h = _Doc.CreateInstance (__uuidof(MSXML2::DOMDocument30));
		_Doc->loadXML((_bstr_t)strXML.c_str ());	
	}
	catch(...)
	{
	}
	TRACE(strXML.data());
	delete strCheckVal;
}


void CTestHardWareDlg::OnSelchangeComboType()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//�޸�Ӧ��Ӧ����ʾ����
		CString strStrainType;
		int nCursel = m_ComboStrainType.GetCurSel();
		m_ComboStrainType.GetLBText(nCursel, strStrainType);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_SHOWTYPE, strStrainType, nCurSel);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnSelchangeComboBridgetype()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//�޸���·��ʽ
		CString strBridgeType;
		int nCursel = m_ComboBridgeType.GetCurSel();
		m_ComboBridgeType.GetLBText(m_ComboBridgeType.GetCurSel(), strBridgeType);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_BRIDGETYPE, strBridgeType, nCurSel);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnKillfocusStrainGauge()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//Ӧ�����ֵ
		CString strStrainGuage;
		m_StrainGauge.GetWindowTextA(strStrainGuage);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_GAUGE, strStrainGuage, 0);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnKillfocusStrainLead()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//������ֵ
		CString strStrainLead;
		m_StrainLead.GetWindowTextA(strStrainLead);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_LEAD, strStrainLead, 0);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnKillfocusStrainSensecoef()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//������ϵ��
		CString strStrainSenseCoef;
		m_strainSenseCoef.GetWindowTextA(strStrainSenseCoef);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_SENSECOEF, strStrainSenseCoef, 0);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnKillfocusStrainPosion()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//���ɱ�
		CString strStrainPosion;
		m_strainPosion.GetWindowTextA(strStrainPosion);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_POSION, strStrainPosion, 0);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}


void CTestHardWareDlg::OnKillfocusStrainElasticity()
{
	CString strMeasureType;
	int nCurSel = m_ComboMeasureType.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboMeasureType.GetLBText(nCurSel, strMeasureType);
	if(strcmp(strMeasureType, "Ӧ��Ӧ��") == 0)
	{
		CString strText;
		m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);
		CString strGroupID = strText.Left(strText.ReverseFind('-'));
		long lGroupID = atol(strGroupID);
		lGroupID -= 1;
		CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
		long lChannelID = atol(strChannelID);
		lChannelID -= 1;
		string strMachineIP = GetMachineIP(int(lGroupID));
		ChannelParam ChanParam;
		GetChannelParam(int(lChannelID), ChanParam);

		long lResult;

		//����ģ��
		CString strStrainElastiCity;
		m_strainElasticity.GetWindowTextA(strStrainElastiCity);
		ModifyParamAndSendCode(lGroupID, strMachineIP.data(), ChanParam.ChannelStyle, ChanParam.ChannelID, ChanParam.CellID, SHOW_STRAIN_ELASTICITY, strStrainElastiCity, 0);

		// ˢ�³�ͨ���������н������
		RefreshAllParam();
	}
}

// ȡ���߳�
UINT CTestHardWareDlg::GetDataThread(LPVOID pParam)
{
	CTestHardWareDlg *pTest = (CTestHardWareDlg *)pParam;
	CString strChannel;
	int nSelGroupID, nSelChanID;

	long nBufferSize = 1024*1024;		// 1Mb�ڴ� ��ֹ�ڴ治����ȡ��������
	long *BufferPoint = new long[nBufferSize];
	while (pTest->m_bThread)
	{
		long nTotalDataPos, nReceiveCount,nChnCount, lReturnValue;
		pTest->m_HardWare.GetAllChnData(nBufferSize, (long)BufferPoint, &nTotalDataPos, &nReceiveCount, &nChnCount, &lReturnValue);
		if (nReceiveCount <= 0)
			continue;

		float *pfltData = new float[nReceiveCount];
		// ������������������
		for (int i = 0; i < pTest->m_vecGroupChannel.size(); i++)
		{
			stuGroupChannel GroupChannel = pTest->m_vecGroupChannel[i];
			int nChannelGroupID = GroupChannel.m_GroupID;
			
			// ��ȡÿ��ͨ�������� Ĭ����������Ϊfloat
			for (int j = 0; j < GroupChannel.m_nChannelNumber; j++)
			{
				float *pValue = (float*)BufferPoint;

				if(pTest->m_bThread)
					pTest->m_ComboChannel.GetLBText(pTest->m_ComboChannel.GetCurSel(), strChannel);
				nSelGroupID = atoi(strChannel.Left(strChannel.ReverseFind('-')))-1;
				nSelChanID = atoi(strChannel.Mid(strChannel.ReverseFind('-')+1))-1;

				if (nChannelGroupID != nSelGroupID || pTest->m_vecHardChannel[j].m_nChannelID != nSelChanID)
					continue;

				//TRACE("GroupID:%d_%d ", nChannelGroupID, j);
				for (int k = 0; k < nReceiveCount; k++)
				{
					int nSeekPos = i * GroupChannel.m_nChannelNumber * nReceiveCount + j * nReceiveCount + k;

					//pfltData[k] = pValue[j*nReceiveCount + k];
					pfltData[k] = pValue[nSeekPos];

					//TRACE("%.3f ", pfltData[k]);
				}
				//TRACE("\n");

				if (pTest->m_hWnd != NULL)
					pTest->m_DlgShowData.SendMessage(SHOW_SAMPLE_DATA, (WPARAM)nReceiveCount, (LPARAM)pfltData);
// 				float fltValue = pfltData[nReceiveCount - 1];
// 				CString strText;
// 				strText.Format("%f", fltValue);
// 				if(pTest->m_bThread)
// 				{
// 					CEdit *pEdit = (CEdit *)(pTest->GetDlgItem(IDC_DATASHOW));
// 					pEdit->SetWindowText(strText);
// 				}
			}
		}
		delete[] pfltData;

		float fltTime;
		float fltValue;
		long nReturnValue;
		// ��ȡת��ͨ��1������
		pTest->m_HardWare.GetSampleStatValue(nSelGroupID, 1, 0, &fltTime, &fltValue, &nReturnValue);

		TRACE("GetSampleStatValue nReturnValue %d fltTime %f fltValue %f \n", nReturnValue, fltTime, fltValue);
		// ��ȡGPS���ٶ���Ϣ
		//		pTest->m_HardWare.GetSampleStatValue(nSelGroupID, 1, 0, &fltTime, &fltValue, &nReturnValue);

		Sleep(200);
	}
	delete[] BufferPoint;
	return 0;
}

//�޸Ĳ��������Ͳ�����������
long CTestHardWareDlg::ModifyParamAndSendCode(long GroupChannelID, LPCTSTR strMachineIP, long ChannelStyle, long ChannelID, long CellID, long ShowParamID, LPCTSTR strParamValue, long nSelectIndex)
{
	long lModifyReturn, lSendCodeReturn;
	m_HardWare.ModifyParam(GroupChannelID, strMachineIP, ChannelStyle, ChannelID, CellID, ShowParamID, strParamValue, nSelectIndex, &lModifyReturn);
	string strXMLChn = CreateChannelKeyString();
	// �޸�����ͨ��������,lSendCodeReturnֵΪ0�����Ƿ����ѳɹ�����Ӱ���޸Ĳ���
	m_HardWare.SendChannelParamCode_Single(GroupChannelID, strMachineIP, ChannelID, &lSendCodeReturn);
	return lModifyReturn;
}

//////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////
// �ź�Դͨ����Ϣ
void CTestHardWareDlg::InitSignalDialog()
{
	long lReturnValue;
	m_HardWare.IsSignalSourceHandle(&lReturnValue);
	if (lReturnValue)
		m_StartSignal.EnableWindow(TRUE);
	else
		m_StartSignal.EnableWindow(FALSE);

	m_HardWare.IsSignalSourceStart(&lReturnValue);
	if (lReturnValue)
		EnableAllSignalDialog(FALSE);
	else
		EnableAllSignalDialog(TRUE);

	// ��ʼ���Ի���
	m_ComboSignalChannel.SetCurSel(0);
	// ��ʼ����������
	InitSignalWave();
	// 
	int nCurSel = m_ComboSignalType.GetCurSel();
	if (nCurSel < 0)
		return;
	SetSignalInfo(nCurSel);
}

// ��ʼ����������
void CTestHardWareDlg::InitSignalWave()
{
	CString strText;
	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboChannel.GetLBText(nCurSel, strText);
	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	string strMachineIP = GetMachineIP(int(lGroupID));

	long lSignalChanID = m_ComboSignalChannel.GetCurSel();
	long lResult;
	BSTR *strSelectValue = new BSTR();
	// ��ȡ�������Ϳ�ѡ���б� 
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_WAVE_FORM, strSelectValue);
	char *pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strWaveTypeSelect = pTempData;
	int nWaveCount = m_GroupChannel.BreakString(strWaveTypeSelect, m_listSignalWave, string("|"));
	delete pTempData;

	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_WAVE_FORM, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strWaveType = pTempData;
	delete pTempData;

	m_ComboSignalType.ResetContent();
	int nWaveSel = -1;
	for (list<string>::iterator it = m_listSignalWave.begin(); it != m_listSignalWave.end(); it++)
	{
		nWaveSel++;
		string strText = *it;
		m_ComboSignalType.AddString(strText.data());
		if (strText == strWaveType)
		{
			m_ComboSignalType.SetCurSel(nWaveSel);
			ShowWindowForSignalType(nWaveSel);
			SetSignalInfo(nWaveSel);
		}
	}
	delete strSelectValue;
}

// ��ȡ��ز�������ʾ
// nWaveForm::��������
void CTestHardWareDlg::SetSignalInfo(int nWaveForm)
{
	CString strText;
	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return;
	m_ComboChannel.GetLBText(nCurSel, strText);
	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	string strMachineIP = GetMachineIP(int(lGroupID));

	CWnd *pWnd;
	long lSignalChanID = m_ComboSignalChannel.GetCurSel();
	long lResult;
	BSTR *strSelectValue = new BSTR();
	// ��ֵ
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_LEVEL, strSelectValue);
	char *pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strExtend = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_EXTEND);
	pWnd->SetWindowTextA(strExtend.data());
	delete pTempData;

	// ��λ
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_PHASE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strPhase = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_PHASE);
	pWnd->SetWindowTextA(strPhase.data());
	delete pTempData;

	// ռ�ձ�
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_OCCUPY_SCALE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strScale = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_OCCUPY_SCALE);
	pWnd->SetWindowTextA(strScale.data());
	delete pTempData;

	// ����
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_CYCLE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strCycle = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_CYCLE);
	pWnd->SetWindowTextA(strCycle.data());
	delete pTempData;

	// Ƶ��
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_START_FREQ, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strFreq = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_FREQ);
	pWnd->SetWindowTextA(strFreq.data());
	delete pTempData;

	// ɨƵ�ٶ�
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_SAOVALUE, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strRate = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_RATE);
	pWnd->SetWindowTextA(strRate.data());
	delete pTempData;

	// ��ʼƵ��
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_START_FREQ, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strStartFreq = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_START_FREQ);
	pWnd->SetWindowTextA(strStartFreq.data());
	delete pTempData;

	// ����Ƶ��
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_END_FREQ, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strEndFreq = pTempData;
	pWnd = GetDlgItem(IDC_SIGNAL_END_FREQ);
	pWnd->SetWindowTextA(strEndFreq.data());
	delete pTempData;

	// ɨƵ��ʽ
	list<string> listRateStyle;
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1,���ǵ��˴���ʼͨ�����ǻ����е�һ��ͨ����ʹ��m_vecHardChannel[0].m_nChannelID������
	// ��������е�һͨ�������ڣ�������ͨ��״�����ź�Դͨ��ID�����κ�״��Ӱ�죬��Ȼ������������ͬ����Ϊ4*16��4*16+1
	m_HardWare.GetParamSelectValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_WIDTHUNIT, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strRateTypeSelect = pTempData;
	int nInputCount = m_GroupChannel.BreakString(strRateTypeSelect, listRateStyle, string("|"));
	delete pTempData;

	m_HardWare.GetParamValue(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, SHOW_GENERATOR_WIDTHUNIT, strSelectValue);
	pTempData = _com_util::ConvertBSTRToString(*strSelectValue);
	string strRateType = pTempData;
	delete pTempData;

	int nRateTypeSel = -1;
	m_ComboSignalRateType.ResetContent();
	for (list<string>::iterator it = listRateStyle.begin(); it != listRateStyle.end(); it++)
	{
		nRateTypeSel++;
		string strText = *it;
		m_ComboSignalRateType.AddString(strText.data());
		if (strText == strRateType)
			m_ComboSignalRateType.SetCurSel(nRateTypeSel);
	}

	delete strSelectValue;
}

// �����ź�Դͨ������ ��ʾ��ؿ�����ѡ��
void CTestHardWareDlg::ShowWindowForSignalType(int nSignalType)
{
	CWnd *pExtend = GetDlgItem(IDC_SIGNAL_EXTEND);
	CWnd *pPhase = GetDlgItem(IDC_SIGNAL_PHASE);
	CWnd *pScale = GetDlgItem(IDC_SIGNAL_OCCUPY_SCALE);
	CWnd *pCycle= GetDlgItem(IDC_SIGNAL_CYCLE);
	CWnd *pFreq = GetDlgItem(IDC_SIGNAL_FREQ);
	CWnd *pRate = GetDlgItem(IDC_SIGNAL_RATE);
	CWnd *pStartFreq = GetDlgItem(IDC_SIGNAL_START_FREQ);
	CWnd *pEndFreq = GetDlgItem(IDC_SIGNAL_END_FREQ);

	CWnd *pStaticExtend = GetDlgItem(IDC_STATIC_EXTEND);
	CWnd *pStaticPhase = GetDlgItem(IDC_STATIC_PHASE);
	CWnd *pStaticScale = GetDlgItem(IDC_STATIC_SCALE);
	CWnd *pStaticCycle= GetDlgItem(IDC_STATIC_CYCLE);
	CWnd *pStaticFreq = GetDlgItem(IDC_STATIC_FREQ);
	CWnd *pStaticRate = GetDlgItem(IDC_STATIC_RATE);
	CWnd *pStaticStartFreq = GetDlgItem(IDC_STATIC_STARTFREQ);
	CWnd *pStaticEndFreq = GetDlgItem(IDC_STATIC_ENDFREQ);
	CWnd *pStaticRateType = GetDlgItem(IDC_STATIC_RATESTYLE);
	switch (nSignalType)
	{
	case 0:			// ��Ƶ���ǲ�
	case 1:			// ���Ҷ�Ƶ
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pPhase->ShowWindow(SW_SHOW);
		pFreq->ShowWindow(SW_SHOW);

		pStaticExtend->ShowWindow(SW_SHOW);
		pStaticPhase->ShowWindow(SW_SHOW);
		pStaticFreq->ShowWindow(SW_SHOW);
		break;
	case 2:			// ����ɨƵ
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pPhase->ShowWindow(SW_SHOW);
		pRate->ShowWindow(SW_SHOW);
		pStartFreq->ShowWindow(SW_SHOW);
		pEndFreq->ShowWindow(SW_SHOW);
		m_ComboSignalRateType.ShowWindow(SW_SHOW);
		m_ComboSignalRateType.SetCurSel(0);

		pStaticExtend->ShowWindow(SW_SHOW);
		pStaticPhase->ShowWindow(SW_SHOW);
		pStaticRate->ShowWindow(SW_SHOW);
		pStaticStartFreq->ShowWindow(SW_SHOW);
		pStaticEndFreq->ShowWindow(SW_SHOW);
		pStaticRateType->ShowWindow(SW_SHOW);
		break;
	case 3:			// ������
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pPhase->ShowWindow(SW_SHOW);
		pCycle->ShowWindow(SW_SHOW);
		pFreq->ShowWindow(SW_SHOW);

		pStaticExtend->ShowWindow(SW_SHOW);
		pStaticPhase->ShowWindow(SW_SHOW);
		pStaticCycle->ShowWindow(SW_SHOW);
		pStaticFreq->ShowWindow(SW_SHOW);
		break;
	case 4:			// ⧷����
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pScale->ShowWindow(SW_SHOW);
		pCycle->ShowWindow(SW_SHOW);

		pStaticExtend->ShowWindow(SW_SHOW);
		pStaticScale->ShowWindow(SW_SHOW);
		pStaticCycle->ShowWindow(SW_SHOW);
	break;
	case 5:			// ����
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pPhase->ShowWindow(SW_SHOW);
		pScale->ShowWindow(SW_SHOW);
		pCycle->ShowWindow(SW_SHOW);

		pStaticExtend->ShowWindow(SW_SHOW);
		pStaticPhase->ShowWindow(SW_SHOW);
		pStaticScale->ShowWindow(SW_SHOW);
		pStaticCycle->ShowWindow(SW_SHOW);
		break;
	case 6:			// ���
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pStaticExtend->ShowWindow(SW_SHOW);
		break;
	case 7:			// ֱ��
		SetAllWindowHide();
		pExtend->ShowWindow(SW_SHOW);
		pStaticExtend->ShowWindow(SW_SHOW);
		break;
	}
}

// ���������ź�Դ�ؼ�
void CTestHardWareDlg::SetAllWindowHide()
{
	CWnd *pWnd;
	for (int i = 0; i < 9; i++)
	{
		pWnd = GetDlgItem(IDC_SIGNAL_EXTEND+i);
		pWnd->ShowWindow(SW_HIDE);

		pWnd = GetDlgItem(IDC_STATIC_EXTEND+i);
		pWnd->ShowWindow(SW_HIDE);
	}
}

// ���ź�Դ����ʱ���������ź�Դ�ؼ�
void CTestHardWareDlg::EnableAllSignalDialog(BOOL bEnable)
{
	m_ComboSignalChannel.EnableWindow(bEnable);
	m_ComboSignalType.EnableWindow(bEnable);
	CWnd *pWnd;
	for (int i = 0; i < 9; i++)
	{
		pWnd = GetDlgItem(IDC_SIGNAL_EXTEND+i);
		pWnd->EnableWindow(bEnable);
	}
}

void CTestHardWareDlg::OnBnClickedStartsignal()
{
	long lReturnValue;
	m_HardWare.IsSignalSourceStart(&lReturnValue);
	if (!lReturnValue)
		m_HardWare.StartSignalSource();
}

void CTestHardWareDlg::OnBnClickedStopsignal()
{
	long lReturnValue;
	m_HardWare.IsSignalSourceStart(&lReturnValue);
	if (lReturnValue)
		m_HardWare.StopSignalSource();
}

// �ź�Դͨ��
void CTestHardWareDlg::OnCbnSelchangeComboSignlalChannel()
{
	int nCurSel = m_ComboSignalType.GetCurSel();
	SetSignalInfo(nCurSel);
}

// ��������
void CTestHardWareDlg::OnCbnSelchangeComboSignalType()
{
	int nCurSel = m_ComboSignalType.GetCurSel();
	ShowWindowForSignalType(nCurSel);

	if (nCurSel < 0)
		return;
	CString strValue;
	m_ComboSignalType.GetLBText(nCurSel, strValue);

	long lReturnValue;
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_WAVE_FORM, strValue, nCurSel);

	SetSignalInfo(nCurSel);
}

// ��ֵ
void CTestHardWareDlg::OnEnChangeSignalExtend()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_EXTEND);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_LEVEL, strValue, 0);
}

// ��λ
void CTestHardWareDlg::OnEnChangeSignalPhase()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_PHASE);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_PHASE, strValue, 0);
}

// ռ�ձ�
void CTestHardWareDlg::OnEnChangeSignalOccupyScale()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_OCCUPY_SCALE);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_OCCUPY_SCALE, strValue, 0);
}

// ����
void CTestHardWareDlg::OnEnChangeSignalCycle()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_CYCLE);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_CYCLE, strValue, 0);
}

// Ƶ��
void CTestHardWareDlg::OnEnChangeSignalFreq()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_FREQ);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_START_FREQ, strValue, 0);
}

// ɨƵ�ٶ�
void CTestHardWareDlg::OnEnChangeSignalRate()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_RATE);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_SAOVALUE, strValue, 0);
}

// ��ʼƵ��
void CTestHardWareDlg::OnEnChangeSignalStartFreq()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_START_FREQ);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_START_FREQ, strValue, 0);
}

// ����Ƶ��
void CTestHardWareDlg::OnEnChangeSignalEndFreq()
{
	long lReturnValue;
	CWnd *pWnd = GetDlgItem(IDC_SIGNAL_END_FREQ);
	CString strValue;
	pWnd->GetWindowTextA(strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_END_FREQ, strValue, 0);
}

// ɨƵ��ʽ
void CTestHardWareDlg::OnCbnSelchangeComboSignalRatestyle()
{
	long lReturnValue;
	int nCurSel = m_ComboSignalType.GetCurSel();
	if (nCurSel < 0)
		return;
	CString strValue;
	m_ComboSignalType.GetLBText(nCurSel, strValue);
	lReturnValue = ModifySignalParam(SHOW_GENERATOR_WIDTHUNIT, strValue, nCurSel);
	CWnd *pWnd = GetDlgItem(IDC_STATIC_RATE);
	if (nCurSel == 0)
		pWnd->SetWindowTextA("ɨƵ�ٶ�(Hz/s):");
	else
		pWnd->SetWindowTextA("ɨƵ�ٶ�(oct/min):");
}

long CTestHardWareDlg::ModifySignalParam(long ShowParamID, CString strValue, long nSelectIndex)
{
	long lReturnValue = 0;
	// ��ȡGroupID, strMachineIP
	CString strText;
	int nCurSel = m_ComboChannel.GetCurSel();
	if (nCurSel < 0)
		return lReturnValue;
	m_ComboChannel.GetLBText(nCurSel, strText);
	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;

	string strMachineIP = GetMachineIP(int(lGroupID));

	long lSignalChanID = m_ComboSignalChannel.GetCurSel();
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1
	m_HardWare.ModifyParam(lGroupID, strMachineIP.data(), EXTRASIGNAL_CHANNEL_STYLE, m_vecHardChannel[0].m_nChannelID, 0, ShowParamID, strValue, nSelectIndex, &lReturnValue);
	string strXMLChn = CreateSignalChannelKeyString();
	m_HardWare.SendChannelParamCode(strXMLChn.data(), &lReturnValue);
	return lReturnValue;
}

// �����ź�Դͨ����Ϣ�ַ���
string CTestHardWareDlg::CreateSignalChannelKeyString()
{
	// ChannelIDΪ���ź�Դ��һͨ��=����ţ�ÿ������ͨ����������	�ź�Դ�ڶ�ͨ��=����ţ�ÿ������ͨ������+1
	// �˴�ChannelID�����ҵ��������磺�����ǣ��Ż����ڹ��������ź�Դ��һͨ���ɣ�Ϊ��*�������ڶ�ͨ���ɣ�Ϊ��*����+1
	string strChannelKeyXML;
	m_vecChannelKey.clear();
	Channel_Key ChannelKey;
	ChannelKey.m_nGroupID  = m_vecHardChannel[0].m_nChannelGroupID;
	ChannelKey.m_strIP = m_vecHardChannel[0].m_strMachineIP;
	ChannelKey.m_nChannelID = m_vecHardChannel[0].m_nChannelID;
	// ͨ�����������ó��ź�Դͨ������
	ChannelKey.m_nChannelStyle = EXTRASIGNAL_CHANNEL_STYLE;
	m_vecChannelKey.push_back(ChannelKey);

	ChannelKey.m_nGroupID  = m_vecHardChannel[1].m_nChannelGroupID;
	ChannelKey.m_strIP = m_vecHardChannel[1].m_strMachineIP;
	ChannelKey.m_nChannelID = 65;
	// ͨ�����������ó��ź�Դͨ������
	ChannelKey.m_nChannelStyle = EXTRASIGNAL_CHANNEL_STYLE;
	m_vecChannelKey.push_back(ChannelKey);
	// ͨ����Ϣ������XML��ʽ�ַ�����
	strChannelKeyXML = ChannelKey.ConvertToXML(m_vecChannelKey);
	return strChannelKeyXML;
}

BOOL CTestHardWareDlg::PreTranslateMessage(MSG* pMsg)
{
	if (pMsg->message == WM_KEYDOWN && pMsg->wParam == VK_RETURN)
	{
		if (GetFocus() == GetDlgItem(IDC_EDIT_SENSECOEF))
		{
			SetSenseCoef();
			RefreshAllParam();
		}
		return TRUE;
	}
	return CDialogEx::PreTranslateMessage(pMsg);
}

afx_msg LRESULT CTestHardWareDlg::OnCloseShowdataWindow(WPARAM wParam, LPARAM lParam)
{
	m_DlgShowData.ShowWindow(SW_HIDE);
	return 0;
}

void CTestHardWareDlg::OnClose()
{
	OnBnClickedStopsignal();
	OnBnClickedSamplestop();

	CDialogEx::OnClose();
}


void CTestHardWareDlg::OnBnClickedBtnSinglesample()
{
	SetSenseCoef();
	SetSampleParam();
	SetSampleStartDialog();
	long lIsSampling;
	//�Ƿ����ڲɼ�����
	m_HardWare.IsSampling(&lIsSampling);
	if (lIsSampling)
	{
		AfxMessageBox(LPCTSTR("���������У�����ֹͣ����!"));
		return;
	}

	CString strText;
	m_ComboChannel.GetLBText(m_ComboChannel.GetCurSel(), strText);

	CString strGroupID = strText.Left(strText.ReverseFind('-'));
	long lGroupID = atol(strGroupID);
	lGroupID -= 1;
	string strMachineIP = GetMachineIP(int(lGroupID));

	//�ر�����ͨ��
	for(int i=0; i< m_vecHardChannel.size(); i++)
	{
		ChannelParam ChanParam;
		GetChannelParam(i, ChanParam);
		ModifyParamAndSendCode(lGroupID,strMachineIP.data(),ChanParam.ChannelStyle,ChanParam.ChannelID, ChanParam.CellID, SHOW_CHANNEL_USE, "��", 1);
	}
	//����ѡ���ͨ��
	CString strChannelID = strText.Mid(strText.ReverseFind('-')+1);
	long lChannelID = atol(strChannelID);
	lChannelID -= 1;

	ChannelParam SamplingChnParam;
	GetChannelParam(lChannelID, SamplingChnParam);
	ModifyParamAndSendCode(lGroupID,strMachineIP.data(),SamplingChnParam.ChannelStyle,SamplingChnParam.ChannelID, SamplingChnParam.CellID, SHOW_CHANNEL_USE, "��", 0);


	long lSample;
	//��������
	m_HardWare.StartSample(LPCTSTR("Test"), 0, 1024, &lSample);

	//����ȡ���߳�
	m_bThread = true;
	m_pGetDataThread = AfxBeginThread(SingleGetDataThread, this,THREAD_PRIORITY_NORMAL);

	// ���list
	m_DlgShowData.m_List.ResetContent();
	m_DlgShowData.ShowWindow(SW_SHOW);	
}


UINT CTestHardWareDlg::SingleGetDataThread(LPVOID pParam)
{
	CTestHardWareDlg *pTest = (CTestHardWareDlg *)pParam;
	CString strChannel;
	int nSelGroupID, nSelChanID;
	pTest->m_ComboChannel.GetLBText(pTest->m_ComboChannel.GetCurSel(), strChannel);
	nSelGroupID = atoi(strChannel.Left(strChannel.ReverseFind('-')))-1;
	nSelChanID = atoi(strChannel.Mid(strChannel.ReverseFind('-')+1))-1;

	VARIANT varData;
	::VariantInit(&varData);
	while (pTest->m_bThread)
	{
		long nTotalDataPos, nReceiveCount,nChnCount, lReturnValue;
		VARIANT varSelectChn;
		varSelectChn.vt = VT_I4;
		varSelectChn.intVal = nSelChanID;
		pTest->m_HardWare.GetSelectedChnData(nSelGroupID, varSelectChn,&varData, &nTotalDataPos, &nReceiveCount, &lReturnValue);
		if (nReceiveCount <= 0)
			continue;

		float *pfltData = new float[nReceiveCount];
		if(varData.vt == (VT_ARRAY|VT_R4) )
		{
			float *pData = NULL;
			::SafeArrayAccessData(varData.parray,(void **)&pData);	
			int nElementCount = varData.parray->rgsabound[0].cElements;
			ASSERT(nElementCount == nReceiveCount);
			for(int i=0; i< nElementCount; i++)
				pfltData[i] = pData[i];
			::SafeArrayUnaccessData(varData.parray);

			if (pTest->m_hWnd != NULL)
				pTest->m_DlgShowData.SendMessage(SHOW_SAMPLE_DATA, (WPARAM)nReceiveCount, (LPARAM)pfltData);
		}
		delete[] pfltData;
	
		Sleep(200);
	}
	return 0;
}

//���������㷨
void CTestHardWareDlg::OnBnClickedCreateCablealg()
{
	// �㷨����
	long index = 0;

	// ����������,�ָ�
	CString strCableParam = "", strTemp = "";

	int nTimeBlockSize = m_SampleParam.m_nSampleBlockSize;	//���ݿ��С
	strTemp.Format("%d,", nTimeBlockSize);
	strCableParam += strTemp;

	float fltSampleFreq = m_SampleParam.m_fltSampleFrequency;	//����Ƶ��Hz
	strTemp.Format("%f,", fltSampleFreq);
	strCableParam += strTemp;

	float fltCableLength = 1;	//����
	strTemp.Format("%f,", fltCableLength);
	strCableParam += strTemp;

	float fltElas = 2;	//��ģ
	strTemp.Format("%f,", fltElas);
	strCableParam += strTemp;

	float fltCableDensity = 1;	//�ܶ�
	strTemp.Format("%f,", fltCableDensity);
	strCableParam += strTemp;

	float fltCableDia = 0.1;	//ֱ��
	strTemp.Format("%f,", fltCableDia);
	strCableParam += strTemp;

	int nCableType = 0;	//������  0-���š�1-б���š�2-������������3-�����ŵ���
	strTemp.Format("%d", nCableType);
	strCableParam += strTemp;

	// �����㷨�ɹ�����ֵΪ1
	long lReturnValue = 0;
	m_HardWare.CreateCableAlg(index, strCableParam.GetBuffer(), &lReturnValue);

	strCableParam.ReleaseBuffer();
}

//ɾ�����������㷨
void CTestHardWareDlg::OnBnClickedDelallCablealg()
{
	// ɾ���㷨�ɹ�����ֵΪ1
	long lReturnValue = 0;
	m_HardWare.DelAllCableAlg(&lReturnValue);
}

//���������㷨
void CTestHardWareDlg::OnBnClickedResetCablealg()
{
	// �㷨����
	long index = 0;

	// ���������㷨�ɹ�����ֵΪ1
	long lReturnValue = 0;
	m_HardWare.ResetCableAlg(index, &lReturnValue);
}

//��������
void CTestHardWareDlg::OnBnClickedCalCable()
{
	// �㷨����
	long index = 0;

	// ԭʼ��������|�ָ�
	CString strData = "", strTemp = "";

	int nTimeBlockSize = m_SampleParam.m_nSampleBlockSize;	//���ݿ��С
	float *pData = new float[nTimeBlockSize];
	for (int i = 0; i < nTimeBlockSize; i++)
	{
		pData[i] = i + 1;

		strTemp.Format("%f|", pData[i]);
		strData += strTemp;
	}
	delete[] pData; pData = NULL;

	// ����ֵ
	float fltCableData = 0;

	// ���������㷨�ɹ�����ֵΪ1
	long lReturnValue = 0;
	m_HardWare.CalCable(index, strData.GetBuffer(), &fltCableData, &lReturnValue);

	strData.ReleaseBuffer();
}
