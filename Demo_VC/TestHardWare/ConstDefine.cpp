#include "StdAfx.h"

#include "Constdefine.h"

#include "Channel.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

const string Channel_Key::XML_CHANNELKEY = "ChannelKey";

const string  Channel_Key::XML_GROUPID = "GroupChannelID";
const string  Channel_Key::XML_CHANNELSTYLE = "ChannelStyle";
const string  Channel_Key::XML_CHANNELID = "ChannelID";
const string  Channel_Key::XML_CELLLID = "CellID";
const string  Channel_Key::XML_MACHINEIP = "MachineIP";
const string  Channel_Key::XML_STATUS = "Status";
const string  Channel_Key::XML_VALUE = "Value";
const string  Channel_Key::XML_VALUE2 = "Value2";
const string  Channel_Key::XML_PATH = "Path";
const string  Channel_Key::XML_VALUE3 = "Value3";
const string  Channel_Key::XML_INFO = "Info";
const string Channel_Key::XML_MACHINEID = "MachineID";
const string Channel_Key::XML_CHNID = "ChnID";
const string  Channel_Key::XML_VALUE4 = "Value4";


pair<string,int> channelkey_pair[] =
{
	pair<string,int>(Channel_Key::XML_GROUPID, Channel_Key::N_GROUPID),
	pair<string,int>(Channel_Key::XML_CHANNELSTYLE, Channel_Key::N_CHANNELSTYLE),
	pair<string,int>(Channel_Key::XML_CHANNELID, Channel_Key::N_CHANNELID),
	pair<string,int>(Channel_Key::XML_CELLLID, Channel_Key::N_CELLLID),
	pair<string,int>(Channel_Key::XML_MACHINEIP, Channel_Key::N_MACHINEIP),
	pair<string,int>(Channel_Key::XML_STATUS, Channel_Key::N_STATUS),
	pair<string,int>(Channel_Key::XML_VALUE, Channel_Key::N_VALUE),
	pair<string,int>(Channel_Key::XML_VALUE2, Channel_Key::N_VALUE2),
	pair<string,int>(Channel_Key::XML_PATH, Channel_Key::N_PATH),
	pair<string,int>(Channel_Key::XML_VALUE3, Channel_Key::N_VALUE3),
	pair<string,int>(Channel_Key::XML_VALUE4, Channel_Key::N_VALUE4),
	pair<string,int>(Channel_Key::XML_INFO, Channel_Key::N_INFO),

	pair<string,int>(Channel_Key::XML_MACHINEID, Channel_Key::N_MACHINEID),
	pair<string,int>(Channel_Key::XML_CHNID, Channel_Key::N_CHNID),
};

map<string,int> Channel_Key::_Mapkey(channelkey_pair, channelkey_pair+sizeof(channelkey_pair)/sizeof(*channelkey_pair));

Channel_Key::Channel_Key()
{
	m_nGroupID = 0;
	m_nChannelStyle = 0;
	m_nChannelID = 0;
	m_nCellID = 0;
	m_strIP = "";
	m_nMachineID = 0;
	m_nMacChnID = 0;
}

// 创建一个key，默认将m_nMachineID与m_nGroupID一致
Channel_Key Channel_Key::CreateKey(int nGroupID, const std::string& ip, int nChannelStyle)
{
	Channel_Key key;
	key.m_nGroupID = nGroupID;
	key.m_nMachineID = nGroupID;
	key.m_strIP = ip;
	key.m_nChannelStyle = nChannelStyle;

	return key;
}

// 转换IP地址
unsigned long Channel_Key::TransIP(const std::string &strIP)
{
	if(strIP == "")
		return 0;

	return inet_addr(strIP.data());
}

std::string Channel_Key::TransIP(unsigned long lIP)
{
	if(lIP == 0)
		return "";

	sockaddr_in sockAddr;
	memset(&sockAddr, 0, sizeof(sockAddr));
	sockAddr.sin_addr.S_un.S_addr = lIP;

	std::string strIP;
	strIP = inet_ntoa(sockAddr.sin_addr);

	return strIP;
}

string Channel_Key::ConvertToXML(const vector<Channel_Key> &vecChnKey)
{
	MSXML2::IXMLDOMDocumentPtr _Doc;
	MSXML2::IXMLDOMNodePtr root;
	MSXML2::IXMLDOMNodePtr node;
	try
	{
		HRESULT h = _Doc.CreateInstance(__uuidof(MSXML2::DOMDocument30));

		root = _Doc->createNode(1, "Root", "");

		_Doc->appendChild (root);
	}
	catch(...)
	{
		return "";
	}

	for(vector<Channel_Key>::const_iterator itFirst = vecChnKey.begin(); itFirst != vecChnKey.end(); itFirst++)
	{
		node = root->ownerDocument->createElement(Channel_Key::XML_CHANNELKEY.data());
		itFirst->Save(node);

		root->appendChild(node);
	}

	string strXML = root->Getxml();

	return strXML;
}

void Channel_Key::ConvertXMLToC(string strXML, vector<Channel_Key> &vecChnKey)
{
	vecChnKey.clear();
	if(strXML == "")
		return;

	MSXML2::IXMLDOMDocumentPtr  _Doc;

	try
	{
		HRESULT h = _Doc.CreateInstance (__uuidof(MSXML2::DOMDocument30));
		_Doc->loadXML((_bstr_t)strXML.c_str ());	
	}
	catch(...)
	{
		return;
	}

	MSXML2::IXMLDOMNodePtr node = _Doc->selectSingleNode("Root");
	if(node == NULL)
		return;

	MSXML2::IXMLDOMNodeListPtr lstnode = node->selectNodes(Channel_Key::XML_CHANNELKEY.data());

	if(lstnode == NULL)
		return;

	vecChnKey.assign(lstnode->Getlength(), Channel_Key());

	for(int i = 0;i < lstnode->Getlength(); i++)
	{
		vecChnKey[i].Load(lstnode->Getitem(i));
	}
}

// 保存布局
void Channel_Key::Save(MSXML2::IXMLDOMElementPtr node) const
{
	char _value[128];
	strcpy_s(_value, 128, "");

	_bstr_t xmlText;

	MSXML2::IXMLDOMElementPtr paramnode = node->ownerDocument->createElement(XML_GROUPID.data());
	sprintf_s(_value, 128, "%d", m_nGroupID);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_CHANNELSTYLE.data());
	sprintf_s(_value, 128, "%d", m_nChannelStyle);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_CHANNELID.data());
	sprintf_s(_value, 128, "%d", m_nChannelID);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_CELLLID.data());
	sprintf_s(_value, 128, "%d", m_nCellID);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_MACHINEIP.data());
	strcpy_s(_value, 128, m_strIP.data());			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_MACHINEID.data());
	sprintf_s(_value, 128, "%d", m_nMachineID);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_CHNID.data());
	sprintf_s(_value, 128, "%d", m_nMacChnID);
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);
}

// 加载布局
void Channel_Key::Load(MSXML2::IXMLDOMNodePtr node)
{
	MSXML2::IXMLDOMNodeListPtr pNodeList = node->childNodes;	
	MSXML2::IXMLDOMNodePtr item;

	string strKey;
	string strValue;
	for(int i = 0; i < pNodeList->Getlength(); i++)
	{		
		item = pNodeList->Getitem(i);

		strKey = item->GetnodeName();
		strValue = item->Gettext();

		SetValue(strKey, strValue);					
	}
}

void Channel_Key::SetValue(const string& strName, const string& strValue)
{
	map<string,int> ::iterator CurItem = _Mapkey.find (strName);

	if(CurItem == _Mapkey.end())
		return;

	switch(CurItem->second)
	{
	case N_MACHINEIP:
		{
			m_strIP = strValue;
		}
		break;
	case N_GROUPID:
		{
			m_nGroupID = atoi(strValue.data());
		}
		break;
	case N_CHANNELSTYLE:
		{
			m_nChannelStyle = atoi(strValue.data());
		}
		break;
	case N_CHANNELID:
		{
			m_nChannelID = atoi(strValue.data());
		}
		break;
	case N_CELLLID:
		{
			m_nCellID = atoi(strValue.data());
		}
		break;
	case N_MACHINEID:
		{
			m_nMachineID = atoi(strValue.data());
		}
		break;
	case N_CHNID:
		{
			m_nMacChnID = atoi(strValue.data());
		}
		break;
	}
}

Channel_Key_Value::Channel_Key_Value()
{
	m_bStatus = TRUE;

	m_fltValue = 0;
	m_fltValue2 = 0;
	m_nValue3 = 0;
	m_fltValue4 = 0;
}

// 保存布局
void Channel_Key_Value::Save(MSXML2::IXMLDOMElementPtr node) const
{
	Channel_Key::Save(node);

	char _value[MAX_PATH];
	strcpy_s(_value, MAX_PATH, "");

	_bstr_t xmlText;

	MSXML2::IXMLDOMElementPtr paramnode = node->ownerDocument->createElement(XML_STATUS.data());
	sprintf_s(_value, MAX_PATH, "%d", m_bStatus);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_VALUE.data());
	sprintf_s(_value, MAX_PATH, "%f", m_fltValue);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_VALUE2.data());
	sprintf_s(_value, MAX_PATH, "%f", m_fltValue2);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_PATH.data());
	strcpy_s(_value, MAX_PATH, m_strPath.data());			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_VALUE3.data());
	sprintf_s(_value, MAX_PATH, "%I64d", m_nValue3);	

	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_INFO.data());
	strcpy_s(_value, MAX_PATH, m_strInfo.data());			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_VALUE4.data());
	sprintf_s(_value, MAX_PATH, "%f", m_fltValue4);			
	xmlText.Attach(_com_util::ConvertStringToBSTR(_value));
	paramnode->put_text(xmlText);
	node->appendChild(paramnode);
}

// 加载布局
void Channel_Key_Value::Load(MSXML2::IXMLDOMNodePtr node)
{
	Channel_Key::Load(node);

	MSXML2::IXMLDOMNodePtr paramnode = node->selectSingleNode(XML_STATUS.data());

	if(paramnode != NULL)
	{
		string strValue = paramnode->Gettext();

		m_bStatus = atoi(strValue.data());
	}

	paramnode = node->selectSingleNode(XML_VALUE.data());

	if(paramnode != NULL)
	{
		string strValue = paramnode->Gettext();

		m_fltValue = (float)atof(strValue.data());
	}

	paramnode = node->selectSingleNode(XML_VALUE2.data());

	if(paramnode != NULL)
	{
		string strValue = paramnode->Gettext();

		m_fltValue2 = (float)atof(strValue.data());
	}

	paramnode = node->selectSingleNode(XML_PATH.data());
	if(paramnode != NULL)
	{
		m_strPath = paramnode->Gettext();
	}

	paramnode = node->selectSingleNode(XML_INFO.data());
	if(paramnode != NULL)
	{
		m_strInfo = paramnode->Gettext();
	}

	paramnode = node->selectSingleNode(XML_VALUE3.data());

	if(paramnode != NULL)
	{
		string strValue = paramnode->Gettext();

		m_nValue3 = _atoi64(strValue.data());
	}

	paramnode = node->selectSingleNode(XML_VALUE4.data());

	if(paramnode != NULL)
	{
		string strValue = paramnode->Gettext();

		m_fltValue4 = (float)atof(strValue.data());
	}
}


void Channel_Key_Value::ConvertXMLToValue(string strXML, vector<Channel_Key_Value> &vecChnKey)
{
	MSXML2::IXMLDOMDocumentPtr  _Doc;

	try
	{
		HRESULT h = _Doc.CreateInstance (__uuidof(MSXML2::DOMDocument30));
		_Doc->loadXML((_bstr_t)strXML.c_str ());	
	}
	catch(...)
	{
		return;
	}

	MSXML2::IXMLDOMNodePtr node = _Doc->selectSingleNode("Root");
	if(node == NULL)
		return;

	MSXML2::IXMLDOMNodeListPtr lstnode = node->selectNodes(Channel_Key::XML_CHANNELKEY.data());

	if(lstnode == NULL)
		return;

	vecChnKey.assign(lstnode->Getlength(), Channel_Key_Value());

	for(int i = 0;i < lstnode->Getlength(); i++)
	{
		vecChnKey[i].Load(lstnode->Getitem(i));
	}
}

string Channel_Key_Value::ConverValueToXML(const vector<Channel_Key_Value> &vecChnKey)
{
	MSXML2::IXMLDOMDocumentPtr _Doc;
	MSXML2::IXMLDOMNodePtr root;
	MSXML2::IXMLDOMNodePtr node;
	try
	{
		HRESULT h = _Doc.CreateInstance(__uuidof(MSXML2::DOMDocument30));

		root = _Doc->createNode(1, "Root", "");

		_Doc->appendChild (root);
	}
	catch(...)
	{
		return "";
	}

	for(vector<Channel_Key_Value>::const_iterator itFind = vecChnKey.begin(); itFind != vecChnKey.end(); itFind++)
	{
		node = root->ownerDocument->createElement(Channel_Key_Value::XML_CHANNELKEY.data());
		itFind->Save(node);

		root->appendChild(node);
	}

	string strXML = root->Getxml();

	return strXML;
}
