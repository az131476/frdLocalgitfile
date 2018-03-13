#pragma once

#include "SampleParam.h"
#include "Channel.h"

#include <string>
#include <vector>
#include <list>
#include <map>
using namespace std;

#import "bin\msxml4.dll"

class GroupChannel
{
public:
	GroupChannel(void);
	~GroupChannel(void);

public:
	static const string XML_GROUPCHANNEL;
	static const string XML_GROUPCHANNELID;
	static const string XML_MACHINEIP;

	int GetChannelGroupID() const
	{
		return m_GroupID;
	}

	string GetMachineIP() const
	{
		return m_strMachineIP;
	}

	int GetChannelsPerCase() const
	{
		return m_nChannelsPerCase;
	}

	int GetSaveBytes() const 
	{
		return m_nSaveBytes;
	}

	int GetDataType() const
	{
		return m_nDataType;
	}

	int GetADBits() const 
	{
		return m_nADBits;
	}

	int GetADBytes() const 
	{
		return m_nADBytes;
	}

	int GetChannelNumber() const
	{
		return m_nChannelNumber;
	}

	int GetInterfaceType() const 
	{
		return m_nInterfaceType;
	}

	int GetAmpInterfaceType() const
	{
		return m_nAmpInterfaceType;
	}

	const vector<class HardChannel*>* GetAllChannel() const;

	class HardChannel* FindHardChannel(long ChannelStyle, long ChannelID, long CellID) const;

public:
	// ��ȡ��Ϣ
	void LoadXML(MSXML2::IXMLDOMNodePtr node);

	// ����ĳ��ͨ����XML����
	bool SetChannel_XML(int nChannelStyle, int nChannelID, int nCellID, string strXML);

protected:
	void ClearChannel();

	void SetValue(MSXML2::IXMLDOMNodePtr node, string strName, string strValue);

protected:
	/// <summary>
	/// ��ID
	/// </summary>
	int m_GroupID;

	/// <summary>
	/// �����汾,��Ӳ����ȡ�İ汾
	/// </summary>
	string m_Version;

	/// <summary>
	/// ������������5920��5927
	/// </summary>
	int m_nInstrumentType;

	/// <summary>
	/// ���ɽӿ�����, PCI, 1394, USB, COM�������
	/// </summary>
	int m_nInterfaceType;

	/// <summary>
	/// �Ŵ����ӿ�����
	/// </summary>
	int m_nAmpInterfaceType;

	/// <summary>
	/// ����IP
	/// </summary>
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

protected:
	// �ɼ�ʱ�Ĳ�����Ϣ
	CSampleParam m_SampleParam;

public:
	static int BreakString( const string& strSrc, list<string>& lstDest, const string& strSeprator );

private:
	vector<class HardChannel*> m_vecChannel;

public:
	static const string XML_GROUPNAME;
	static const string XML_VERSION;
	static const string XML_INSTRUMENTTYPE;
	static const string XML_INTERFACETYPE;
	static const string XML_AMPINTERFACETYPE;
	static const string XML_CHANNELPERCASE;
	static const string XML_ROTATESPEEDBYTES;
	static const string XML_VOLTAGEBASE;
	static const string XML_ADBITS;
	static const string XML_ADBYTES;
	static const string XML_ADBASE;
	static const string XML_SAVEBYTES;
	static const string XML_DATATYPE;
	static const string XML_CHANNELFIRST;
	static const string XML_CHANNELNUMBER;

	static const int N_GROUPNAME = 0;
	static const int N_VERSION = 1;
	static const int N_INSTRUMENTTYPE = 2;
	static const int N_INTERFACETYPE = 3;
	static const int N_AMPINTERFACETYPE = 4;
	static const int N_MACHINEIP = 5;
	static const int N_CHANNELPERCASE = 6;
	static const int N_ROTATESPEEDBYTES = 7;
	static const int N_VOLTAGEBASE = 8;
	static const int N_ADBITS = 9;
	static const int N_ADBYTES = 10;
	static const int N_ADBASE = 11;
	static const int N_SAVEBYTES = 12;
	static const int N_DATATYPE = 13;
	static const int N_CHANNELFIRST = 14;
	static const int N_CHANNELNUMBER = 15;
	static const int N_GROUPCHANNELID = 16;

	static const int N_SAMPLEPARAM = 17;
	static const int N_HARDCHANNEL = 18;

	static map<string ,int> _Mapkey;
};

