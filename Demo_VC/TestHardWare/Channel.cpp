#include "StdAfx.h"
#include "Channel.h"

#include <list>

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

const string HardChannel::XML_CHANNELSTYLE = "ChannelStyle";
const string HardChannel::XML_CHANNELGROUPID = "ChannelGroupID";
const string HardChannel::XML_CHANNELID = "ChannelID";
const string HardChannel::XML_CELLID = "CellID";
const string HardChannel::XML_MACHINEIP = "MachineIP";
const string HardChannel::XML_CHANNELTYPE = "ChannelType";
const string HardChannel::XML_MEASUERTYPE = "MeasureType";
const string HardChannel::XML_METERAGETYPE = "MeterageType";

const string HardChannel::XML_MACHINEID = "MachineID";
const string HardChannel::XML_MACCHNID = "MacChnID";
const string HardChannel::XML_ONLINEFLAG = "OnLineFlag";
const string HardChannel::XML_CHANUSEDFLAG = "ChanUsedFlag";

const string HardChannel::XML_FULLVALUE = "FullValue";
const string HardChannel::XML_VOLTAGEBASE = "VoltageBase";
const string HardChannel::XML_ADBASE = "ADBase";
const string HardChannel::XML_FULLVOLTAGE = "FullVoltage";
const string HardChannel::XML_TRANSFACTOR = "TransFactor";
const string HardChannel::XML_ADTOMVCOEF = "ADtoMVCoef";
const string HardChannel::XML_ADTOEUCOEF = "ADtoEUCoef";

pair<string,int> channel_pair[] =
{
	pair<string,int>(HardChannel::XML_CHANNELSTYLE, HardChannel::N_CHANNELSTYLE),
	pair<string,int>(HardChannel::XML_CHANNELGROUPID, HardChannel::N_GROUPID),
	pair<string,int>(HardChannel::XML_CHANNELID, HardChannel::N_CHANNELID),
	pair<string,int>(HardChannel::XML_CELLID, HardChannel::N_CELLID),
	pair<string,int>(HardChannel::XML_CHANNELTYPE, HardChannel::N_CHANNELTYPE),
	pair<string,int>(HardChannel::XML_MEASUERTYPE, HardChannel::N_MEASUERTYPE),
	pair<string,int>(HardChannel::XML_METERAGETYPE, HardChannel::N_GROUPTYPE),

	pair<string,int>(HardChannel::XML_MACHINEID, HardChannel::N_MACHINEID),
	pair<string,int>(HardChannel::XML_MACCHNID, HardChannel::N_MACCHNID),
	pair<string,int>(HardChannel::XML_ONLINEFLAG, HardChannel::N_ONLINEFLAG),
	pair<string,int>(HardChannel::XML_CHANUSEDFLAG, HardChannel::N_CHANUSEDFLAG),

	pair<string,int>(HardChannel::XML_FULLVALUE, HardChannel::N_FULLVALUE),
	pair<string,int>(HardChannel::XML_VOLTAGEBASE, HardChannel::N_VOLTAGEBASE),
	pair<string,int>(HardChannel::XML_ADBASE, HardChannel::N_ADBASE),
	pair<string,int>(HardChannel::XML_FULLVOLTAGE, HardChannel::N_FULLVOLTAGE),
	pair<string,int>(HardChannel::XML_TRANSFACTOR, HardChannel::N_TRANSFACTOR),
	pair<string,int>(HardChannel::XML_ADTOMVCOEF, HardChannel::N_ADTOMVCOEF),
	pair<string,int>(HardChannel::XML_ADTOEUCOEF, HardChannel::N_ADTOEUCOEF),
	pair<string,int>(HardChannel::XML_MACHINEIP, HardChannel::N_MACHINEIP),
};

map<string,int> HardChannel::_Mapkey(channel_pair, channel_pair+sizeof(channel_pair)/sizeof(*channel_pair));

const string HardChannel::XML_HARDCHANNEL = "HardChannel";


HardChannel::HardChannel(void)
{
	m_nMeasureType = 0;
	m_nChannelStyle = 0;
	m_nChannelGroupID = 0;
	m_nChannelID = 0;
	m_nCellID = 0;
	strcpy_s(m_strMachineIP, "");
}


HardChannel::~HardChannel(void)
{
}

bool HardChannel::IsCommonChn() const
{
	bool bAnalog = true;

	switch (m_nMeasureType)
	{
		case MEASURE_TYPE_ROTATE_SPEED:
		case MEASURE_TYPE_SIGNAL:
		case MEASURE_PULSE_COUNTER:
		case MEASUER_TYPE_GPS:
		case MEASUER_TYPE_CAN:
			bAnalog = false;
			break;
	}

	return bAnalog;
}

// 是否为信号源通道
bool HardChannel::IsSignalChn() const
{
	if(m_nMeasureType == MEASURE_TYPE_SIGNAL)
		return true;

	return false;
}

bool HardChannel::IsPulseChn() const
{
	if (m_nMeasureType == MEASURE_PULSE_COUNTER)
		return true;

	return false;
}

	// 是否为转速通道
bool HardChannel::IsRotateChn() const
{
	if (m_nMeasureType == MEASURE_TYPE_ROTATE_SPEED)
	{
		return true;
	}

	return false;
}

// 是否为测温通道
bool HardChannel::IsMeasureTemperatureChn() const
{
	bool bTemp = false;
	switch(m_nMeasureType)
	{
		case (int)MEASURE_TYPE_TEMPERATURE_PT:
		case (int)MEASURE_TYPE_TEMPERATURE_THERMO:
			bTemp = true;
			break;
	}

	return bTemp;
}

// 加载布局
void HardChannel::Load(MSXML2::IXMLDOMNodePtr node)
{
	MSXML2::IXMLDOMNodeListPtr pNodeList = node->childNodes;	
	MSXML2::IXMLDOMNodePtr item;
	string strKey;
	string strValue;

	item = node->attributes->getNamedItem(XML_MEASUERTYPE.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		int nMeasureType = 0;
		nMeasureType = atoi(strValue.data());

		m_nMeasureType = nMeasureType;
	}

	item = node->attributes->getNamedItem(XML_CHANNELSTYLE.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		m_nChannelStyle = atoi(strValue.data());
	}

	item = node->attributes->getNamedItem(XML_CHANNELGROUPID.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		m_nChannelGroupID = atoi(strValue.data());
	}

	item = node->attributes->getNamedItem(XML_MACHINEIP.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		strcpy_s(m_strMachineIP, 32, strValue.data());
	}

	item = node->attributes->getNamedItem(XML_CHANNELID.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		m_nChannelID = atoi(strValue.data());
	}

	item = node->attributes->getNamedItem(XML_CELLID.data());
	if(item != NULL)
	{
		strValue = item->Gettext();

		m_nCellID = atoi(strValue.data());
	}

	for(int i = 0; i < pNodeList->Getlength(); i++)
	{		
		item = pNodeList->Getitem(i);

		strKey = item->GetnodeName();
		strValue = item->Gettext();				
	}
}