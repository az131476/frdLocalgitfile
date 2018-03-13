
#pragma once

#include <vector>
#include <string>
#include <map>

using namespace std;

const int DATA_TYPE_SHORT = 0;	// short
const int DATA_TYPE_FLOAT = 1;	// float
const int DATA_TYPE_INT	 = 2;	// int

#import "bin\msxml4.dll"

class Channel_Key
{
public:
	Channel_Key();

public:
	string  m_strIP;
	int		m_nGroupID;
	int		m_nChannelStyle;
	int		m_nChannelID;
	int		m_nCellID;
	/// 仪器
	int m_nMachineID;
	/// 通道ID
	int m_nMacChnID;

public:
	static string ConvertToXML(const vector<Channel_Key> &vecChnKey);
	static void ConvertXMLToC(string strXML, vector<Channel_Key> &vecChnKey);
	static std::string TransIP(unsigned long lIP);
	static unsigned long TransIP(const std::string &strIP);
	// 创建一个key，默认将m_nMachineID与m_nGroupID一致
	static Channel_Key CreateKey(int nGroupID, const std::string& ip = "", int nChannelStyle = 0);

public:
	// 保存布局
	virtual void Save(MSXML2::IXMLDOMElementPtr node) const;
	// 加载布局
	virtual void Load(MSXML2::IXMLDOMNodePtr node);

protected:
	void SetValue(const string& strName, const string& strValue);

public:
	static const string XML_CHANNELKEY;

	static const string  XML_GROUPID;
	static const string  XML_CHANNELSTYLE;
	static const string  XML_CHANNELID;
	static const string  XML_CELLLID;
	static const string XML_MACHINEIP;
	static const string XML_STATUS;
	static const string XML_VALUE;
	static const string XML_PATH;
	static const string XML_VALUE2;
	static const string XML_VALUE3;
	static const string XML_VALUE4;
	static const string XML_INFO;
	static const string XML_MACHINEID;
	static const string XML_CHNID;

	static const int  N_GROUPID = 0;
	static const int  N_CHANNELSTYLE = 1;
	static const int  N_CHANNELID = 2;
	static const int  N_CELLLID = 3;
	static const int N_MACHINEIP = 4;
	static const int N_STATUS = 5;
	static const int N_VALUE = 6;
	static const int N_PATH = 7;
	static const int N_VALUE2 = 8;
	static const int N_VALUE3 = 9;
	static const int N_INFO = 10;
	static const int N_MACHINEID = 11;
	static const int N_CHNID = 12;
	static const int N_VALUE4 = 13;

	static map<string,int> _Mapkey;

};


//通道对应的值
class Channel_Key_Value : public Channel_Key
{
public:
	Channel_Key_Value();

public:
	// 保存布局
	virtual void Save(MSXML2::IXMLDOMElementPtr node) const;
	// 加载布局
	virtual void Load(MSXML2::IXMLDOMNodePtr node);

public:
	static string ConverValueToXML(const vector<Channel_Key_Value> &vecChnKey);
	static void ConvertXMLToValue(string strXML, vector<Channel_Key_Value> &vecChnKey);

public:
	// 是否正确，是否获取到
	int m_bStatus;
	LONGLONG m_nValue3;
	string m_strPath;
	string m_strInfo;
	float m_fltValue;
	float m_fltValue2;
	float m_fltValue4;
};