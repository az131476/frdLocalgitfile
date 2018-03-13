#include "StdAfx.h"
#include "GroupChannel.h"

#include "Channel.h"

#include "ConstDefine.h"

#include <list>
using std::list;

#include <comutil.h>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

const string GroupChannel::XML_GROUPCHANNEL = "GroupChannel";
const string GroupChannel::XML_GROUPCHANNELID = "GroupChannelID";

const string GroupChannel::XML_GROUPNAME = "name";
const string GroupChannel::XML_VERSION = "Version";
const string GroupChannel::XML_INSTRUMENTTYPE = "InstrumentType";
const string GroupChannel::XML_INTERFACETYPE = "InterfaceType";
const string GroupChannel::XML_AMPINTERFACETYPE = "AmpInterfaceType";
const string GroupChannel::XML_MACHINEIP = "MachineIP";
const string GroupChannel::XML_CHANNELPERCASE = "ChannelsPerCase";
const string GroupChannel::XML_ROTATESPEEDBYTES = "RotateSpeedBytes";
const string GroupChannel::XML_VOLTAGEBASE = "VoltageBase";
const string GroupChannel::XML_ADBITS = "ADBits";
const string GroupChannel::XML_ADBYTES = "ADBytes";
const string GroupChannel::XML_ADBASE = "ADBase";
const string GroupChannel::XML_SAVEBYTES = "SaveBytes";
const string GroupChannel::XML_DATATYPE = "DataType";
const string GroupChannel::XML_CHANNELFIRST = "ChannelFirst";
const string GroupChannel::XML_CHANNELNUMBER = "ChannelNumber";

pair<string,int> xml_groupchannel[] =
{ 
	pair<string,int>(GroupChannel::XML_GROUPCHANNELID, GroupChannel::N_GROUPCHANNELID),
	pair<string,int>(GroupChannel::XML_GROUPNAME, GroupChannel::N_GROUPNAME),
	pair<string,int>(GroupChannel::XML_VERSION, GroupChannel::N_VERSION),
	pair<string,int>(GroupChannel::XML_INSTRUMENTTYPE, GroupChannel::N_INSTRUMENTTYPE),
	pair<string,int>(GroupChannel::XML_INTERFACETYPE, GroupChannel::N_INTERFACETYPE),
	pair<string,int>(GroupChannel::XML_AMPINTERFACETYPE, GroupChannel::N_AMPINTERFACETYPE),
	pair<string,int>(GroupChannel::XML_MACHINEIP, GroupChannel::N_MACHINEIP),
	pair<string,int>(GroupChannel::XML_CHANNELPERCASE, GroupChannel::N_CHANNELPERCASE),
	pair<string,int>(GroupChannel::XML_ROTATESPEEDBYTES, GroupChannel::N_ROTATESPEEDBYTES),
	pair<string,int>(GroupChannel::XML_VOLTAGEBASE, GroupChannel::N_VOLTAGEBASE),
	pair<string,int>(GroupChannel::XML_ADBITS, GroupChannel::N_ADBITS),
	pair<string,int>(GroupChannel::XML_ADBYTES, GroupChannel::N_ADBYTES),
	pair<string,int>(GroupChannel::XML_ADBASE, GroupChannel::N_ADBASE),
	pair<string,int>(GroupChannel::XML_SAVEBYTES, GroupChannel::N_SAVEBYTES),
	pair<string,int>(GroupChannel::XML_DATATYPE, GroupChannel::N_DATATYPE),
	pair<string,int>(GroupChannel::XML_CHANNELFIRST, GroupChannel::N_CHANNELFIRST),
	pair<string,int>(GroupChannel::XML_CHANNELNUMBER, GroupChannel::N_CHANNELNUMBER),
	pair<string,int>(CSampleParam::XML_SAMPLEPARAM, GroupChannel::N_SAMPLEPARAM),
	pair<string,int>(HardChannel::XML_HARDCHANNEL, GroupChannel::N_HARDCHANNEL),
};

map<string ,int> GroupChannel::_Mapkey(xml_groupchannel,xml_groupchannel+sizeof(xml_groupchannel)/sizeof(*xml_groupchannel));

GroupChannel::GroupChannel(void)
{
	m_GroupID = 0;
	m_Version = "";
	m_nInstrumentType = 0;
	m_nInterfaceType = 0;
	m_nAmpInterfaceType = 0;
	m_strMachineIP = "";
	m_nChannelsPerCase = 16;

	m_nADBits = 16;		// AD位 12、14、16、24
	m_nADBytes = 2;	// AD字节数 2 、4
	m_nADBase = 32768;		// ADBase 
	m_nSaveBytes = sizeof(short);	// 保存数据的字节数 sizeof(short) sizeof(int) sizeof(float)
	m_nDataType = 0;	// 保存数据的类型 short、int、float

	m_nChannelFirst = 0;
	m_nChannelNumber = 16;
}

GroupChannel::~GroupChannel(void)
{
	ClearChannel();
}

// 读取信息
void GroupChannel::LoadXML(MSXML2::IXMLDOMNodePtr node)
{
	ClearChannel();

	MSXML2::IXMLDOMNodeListPtr pNodeList = node->childNodes;	
	MSXML2::IXMLDOMNodePtr item;

	string strKey;
	string strValue;
	for(int i = 0; i < pNodeList->Getlength(); i++)
	{		
		item = pNodeList->Getitem(i);

		strKey = item->GetnodeName();
		strValue = item->Gettext();

		SetValue(item, strKey, strValue);					
	}
}

void GroupChannel::SetValue(MSXML2::IXMLDOMNodePtr node, string strName, string strValue)
{
	map<string,int> ::iterator CurItem = _Mapkey.find (strName);

	if(CurItem == _Mapkey.end())
		return;

	switch(CurItem->second)
	{
	case N_VERSION: // 无需设置，由当前仪器状态控制
		{
			m_Version = strValue;
		}
		break;
	case N_INSTRUMENTTYPE:
		{
			m_nInstrumentType = atoi(strValue.data());
		}
		break;
	case N_INTERFACETYPE:
		{
			m_nInterfaceType = atoi(strValue.data());
		}
		break;
	case N_AMPINTERFACETYPE:
		{
			m_nAmpInterfaceType = atoi(strValue.data());
		}
		break;
	case N_MACHINEIP:
		{
			m_strMachineIP = strValue;
		}
		break;
	case N_CHANNELPERCASE:
		{
			m_nChannelsPerCase = atoi(strValue.data());
		}
		break;
	case N_ROTATESPEEDBYTES:
		{

		}
		break;
	case N_VOLTAGEBASE:
		{
			m_fltVoltageBase = atof(strValue.data());
		}
		break;
	case N_ADBITS:
		{
			m_nADBits = atoi(strValue.data());
		}
		break;
	case N_ADBYTES:
		{
			m_nADBytes = atoi(strValue.data());
		}
		break;
	case N_ADBASE:
		{
			m_nADBase = atoi(strValue.data());
		}
		break;
	case N_SAVEBYTES:
		{
			m_nSaveBytes = atoi(strValue.data());
		}
		break;
	case N_DATATYPE:
		{
			m_nDataType = atoi(strValue.data());
		}
		break;
	case N_CHANNELFIRST:
		{
			m_nChannelFirst = atoi(strValue.data());
		}
		break;
	case N_CHANNELNUMBER:
		{
			m_nChannelNumber = atoi(strValue.data());
		}
		break;
	case N_GROUPCHANNELID:
		{
			m_GroupID = atoi(strValue.data());
		}
		break;
	case N_SAMPLEPARAM:
		{
			CSampleParam tmpParam = m_SampleParam;

			tmpParam.Load(node);

			m_SampleParam = tmpParam;
		}
		break;
	case N_HARDCHANNEL:
		{
			HardChannel * pHardChannel = new HardChannel();
			pHardChannel->Load(node);
			m_vecChannel.push_back(pHardChannel);
		}
		break;
	}
}

// 设置某个通道的XML参数
bool GroupChannel::SetChannel_XML(int nChannelStyle, int nChannelID, int nCellID, string strXML)
{
	return false;
}

void GroupChannel::ClearChannel()
{
	for(unsigned int i = 0; i < m_vecChannel.size(); i++)
	{
		delete m_vecChannel[i];
	}

	m_vecChannel.clear();
}

//将字符串进行分解。strSeprator中的任何一个字符都作为分隔符。返回分节得到的字符串数目
int GroupChannel::BreakString( const string& strSrc, list<string>& lstDest, const string& strSeprator )
{
	//清空列表
	lstDest.clear();
	//个数
	int iCount = 0;

	if( strSeprator.length() == 0 )
	{
		lstDest.push_back( strSrc );
		iCount = 1;
		return iCount;
	}
	
	//查找的位置
	std::string::size_type iPos=0;
	while( iPos < strSrc.length() )
	{
		std::string::size_type iNewPos = strSrc.find_first_of( strSeprator, iPos );
		//当前字符即分隔符
		if( iNewPos == iPos )
		{
			iPos++;
		}
		//没找到分隔符
		else if( iNewPos == std::string::npos )
		{
			lstDest.push_back( strSrc.substr(iPos, strSrc.length()-iPos) );
			iCount++;
			iPos = strSrc.length();
		}
		//其它
		else
		{
			lstDest.push_back( strSrc.substr(iPos, iNewPos-iPos) );
			iCount++;
			iPos = iNewPos;
			iPos++;
		}
	}
	
	return iCount;
}

const vector<class HardChannel*>* GroupChannel::GetAllChannel() const
{
	return &m_vecChannel;
}

class HardChannel* GroupChannel::FindHardChannel(long ChannelStyle, long ChannelID, long CellID) const
{
	for(int i = 0; i < m_vecChannel.size(); i++)
	{
		if(m_vecChannel[i]->m_nChannelStyle == ChannelStyle && \
			m_vecChannel[i]->m_nChannelID == ChannelID && \
			m_vecChannel[i]->m_nCellID == CellID)
			return m_vecChannel[i];
	}

	return NULL;
}