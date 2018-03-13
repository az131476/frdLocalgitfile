#include "StdAfx.h"
#include "SampleParam.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

CSampleParam::CSampleParam(void)
{
	m_fltSampleFrequency = 1000;
	m_nRateCount = 1000;
	m_nPreTrigRateCount = 10;

	m_nSampleClkMode = 0;
	m_nFrequencyMode = 0;

	m_bAnalysisViewAverage = false;

	m_nSampleDelayPoints = 0;

	m_nSampleMode = 1;
	m_nSampleTrigMode = 1;
	m_nSampleTimes = 1;
	m_nSampleBlockSize = 1024;

	m_nSampleChnFirst = 0;
	m_nSampleChnNumber = 16;

	m_bSyncTrans = true;
}

CSampleParam::~CSampleParam(void)
{
}

// 保存布局
void CSampleParam::Save(MSXML2::IXMLDOMElementPtr node) const
{
	char _value[128];
	strcpy_s(_value, 128, "");

	MSXML2::IXMLDOMElementPtr paramnode = node->ownerDocument->createElement(XML_SAMPLEFREQUENCY.data());
	sprintf_s(_value, 128, "%f", m_fltSampleFrequency);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLEMODE.data());
	sprintf_s(_value, 128, "%d", m_nSampleMode);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLEBLOCKSIZE.data());
	sprintf_s(_value, 128, "%d", m_nSampleBlockSize);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLETIMES.data());
	sprintf_s(_value, 128, "%d", m_nSampleTimes);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLETRIGMODE.data());
	sprintf_s(_value, 128, "%d", m_nSampleTrigMode);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLEDELAYPOINTS.data());
	sprintf_s(_value, 128, "%d", m_nSampleDelayPoints);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_RATECOUNT.data());
	sprintf_s(_value, 128, "%d", m_nRateCount);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_ANALYSISVIEWAVERAGE.data());
	sprintf_s(_value, 128, "%d", m_bAnalysisViewAverage);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLECLKMODE.data());
	sprintf_s(_value, 128, "%d", m_nSampleClkMode);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_FREQUENCYMODE.data());
	sprintf_s(_value, 128, "%d", m_nFrequencyMode);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLECHNFIRST.data());
	sprintf_s(_value, 128, "%d", m_nSampleChnFirst);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SAMPLECHNNUMBER.data());
	sprintf_s(_value, 128, "%d", m_nSampleChnNumber);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);

	paramnode = node->ownerDocument->createElement(XML_SYNCTRANS.data());
	sprintf_s(_value, 128, "%d", m_bSyncTrans);			
	paramnode->put_text(_com_util::ConvertStringToBSTR(_value));
	node->appendChild(paramnode);
}

// 加载布局
void CSampleParam::Load(MSXML2::IXMLDOMNodePtr node)
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

void CSampleParam::SetValue(string strName, string strValue)
{
	map<string,int> ::iterator CurItem = _Mapkey.find (strName);

	if(CurItem == _Mapkey.end())
		return;

	switch(CurItem->second)
	{
	case N_SYNCTRANS:
		{
			m_bSyncTrans = atoi(strValue.data());
		}
		break;
	case N_SAMPLECHNFIRST:	// 由硬件决定 
		{
			m_nSampleChnFirst = atoi(strValue.data());
		}
		break;
	case N_SAMPLECHNNUMBER:
		{
			m_nSampleChnNumber = atoi(strValue.data());
		}
		break;
	case N_SAMPLEFREQUENCY:
		{
			m_fltSampleFrequency = atof(strValue.data());
		}
		break;
	case N_SAMPLEMODE:
		{
			m_nSampleMode = atoi(strValue.data());
		}
		break;
	case N_SAMPLEBLOCKSIZE:
		{
			m_nSampleBlockSize = atoi(strValue.data());
		}
		break;
	case N_SAMPLETIMES:
		{
			m_nSampleTimes = atoi(strValue.data());
		}
		break;
	case N_SAMPLETRIGMODE:
		{
			m_nSampleTrigMode = atoi(strValue.data());
		}
		break;
	case N_SAMPLEDELAYPOINTS:
		{
			m_nSampleDelayPoints = atoi(strValue.data());
		}
		break;
	case N_RATECOUNT:
		{
			m_nRateCount = atoi(strValue.data());
		}
		break;
	case N_ANALYSISVIEWAVERAGE:
		{
			m_bAnalysisViewAverage = atoi(strValue.data());
		}
		break;
	case N_SAMPLECLKMODE:
		{
			m_nSampleClkMode = atoi(strValue.data());
		}
		break;
	case N_FREQUENCYMODE:
		{
			m_nFrequencyMode = atoi(strValue.data());
		}
		break;
	}
}

const string CSampleParam::XML_SAMPLEPARAM = "SampleParam";

const string CSampleParam::XML_SAMPLEFREQUENCY = "SampleFrequency";
const string CSampleParam::XML_SAMPLEMODE = "SampleMode";
const string CSampleParam::XML_SAMPLEBLOCKSIZE = "SampleBlockSize";
const string CSampleParam::XML_SAMPLETIMES = "SampleTimes";
const string CSampleParam::XML_SAMPLETRIGMODE = "SampleTrigMode";
const string CSampleParam::XML_SAMPLEDELAYPOINTS = "SampleDelayPoints";
const string CSampleParam::XML_RATECOUNT = "RateCount";
const string CSampleParam::XML_ANALYSISVIEWAVERAGE = "AnalysisViewAverage";
const string CSampleParam::XML_SAMPLECLKMODE = "SampleClkMode";
const string CSampleParam::XML_FREQUENCYMODE = "FrequencyMode";
const string CSampleParam::XML_SAMPLECHNFIRST = "SampleChnFirst";
const string CSampleParam::XML_SAMPLECHNNUMBER = "SampleChnNumber";
const string CSampleParam::XML_SYNCTRANS = "SyncTrans";

pair<string,int> XML_samp[] =
{
	pair<string,int>(CSampleParam:: XML_SAMPLEFREQUENCY, CSampleParam::N_SAMPLEFREQUENCY),
	pair<string,int>(CSampleParam:: XML_SAMPLEMODE, CSampleParam::N_SAMPLEMODE),
	pair<string,int>(CSampleParam:: XML_SAMPLEBLOCKSIZE, CSampleParam::N_SAMPLEBLOCKSIZE),
	pair<string,int>(CSampleParam:: XML_SAMPLETIMES, CSampleParam::N_SAMPLETIMES),
	pair<string,int>(CSampleParam:: XML_SAMPLETRIGMODE, CSampleParam::N_SAMPLETRIGMODE),
	pair<string,int>(CSampleParam:: XML_SAMPLEDELAYPOINTS, CSampleParam::N_SAMPLEDELAYPOINTS),
	pair<string,int>(CSampleParam:: XML_RATECOUNT, CSampleParam::N_RATECOUNT),
	pair<string,int>(CSampleParam:: XML_ANALYSISVIEWAVERAGE, CSampleParam::N_ANALYSISVIEWAVERAGE),
	pair<string,int>(CSampleParam:: XML_SAMPLECLKMODE, CSampleParam::N_SAMPLECLKMODE),
	pair<string,int>(CSampleParam:: XML_FREQUENCYMODE, CSampleParam::N_FREQUENCYMODE),
	pair<string,int>(CSampleParam:: XML_SAMPLECHNFIRST, CSampleParam::N_SAMPLECHNFIRST),
	pair<string,int>(CSampleParam:: XML_SAMPLECHNNUMBER, CSampleParam::N_SAMPLECHNNUMBER),
	pair<string,int>(CSampleParam:: XML_SYNCTRANS, CSampleParam::N_SYNCTRANS),
};

map<string,int> CSampleParam::_Mapkey(XML_samp, XML_samp+sizeof(XML_samp)/sizeof(*XML_samp));