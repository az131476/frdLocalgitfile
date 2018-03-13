#pragma once

#include <string>
#include <map>
#include <list>
using namespace std;

#import "bin\msxml4.dll"

/// <summary>
/// 通道参数定义
/// </summary>
class HardChannel
{
public:
	HardChannel();
	~HardChannel();

public:
	static const string XML_HARDCHANNEL;
	static const string XML_CHANNELSTYLE;
	static const string XML_CHANNELGROUPID;
	static const string XML_MACHINEIP;
	static const string XML_CHANNELID;
	static const string XML_CELLID;
	static const string XML_CHANNELTYPE;
	static const string XML_MEASUERTYPE;
	static const string XML_METERAGETYPE;

	static const string XML_MACHINEID;
	static const string XML_MACCHNID;
	static const string XML_ONLINEFLAG;
	static const string XML_CHANUSEDFLAG;
	static const string XML_FULLVALUE;
	static const string XML_VOLTAGEBASE;
	static const string XML_ADBASE;
	static const string XML_FULLVOLTAGE;
	static const string XML_TRANSFACTOR;
	static const string XML_ADTOMVCOEF;
	static const string XML_ADTOEUCOEF;

	static const int N_CHANNELSTYLE = 0;
	static const int N_GROUPID = 1;
	static const int N_CHANNELID = 2;
	static const int N_CELLID = 3;
	static const int N_CHANNELTYPE = 4;
	static const int N_MEASUERTYPE = 5;
	static const int N_GROUPTYPE = 6;

	static const int N_MACHINEID = 8;
	static const int N_MACCHNID = 9;

	static const int N_ONLINEFLAG = 11;
	static const int N_CHANUSEDFLAG = 12;

	static const int N_FULLVALUE = 22;
	static const int N_VOLTAGEBASE = 23;
	static const int N_ADBASE = 24;
	static const int N_FULLVOLTAGE = 25;
	static const int N_TRANSFACTOR = 26;
	static const int N_ADTOMVCOEF = 27;
	static const int N_ADTOEUCOEF = 29;

	static const int N_MACHINEIP = 37;

	static map<string,int> _Mapkey;

public:
	/// <summary>
	/// 测量类型, 应变、电压、电荷...
	/// </summary>
	int m_nMeasureType;

	/// <summary>
	/// 通道风格,模拟通道、外转速通道、CAN通道、GPS通道等
	/// </summary>
	int m_nChannelStyle;

	/// <summary>
	/// 通道组ID，多个通道为一组
	/// </summary>
	int m_nChannelGroupID;

	// 仪器IP
	char m_strMachineIP[32];

	/// <summary>
	/// 通道ID
	/// </summary>
	int m_nChannelID;

	/// <summary>
	/// 单元ID
	/// </summary>
	int m_nCellID;

public:
	// 加载布局
	void Load(MSXML2::IXMLDOMNodePtr node);

public:
	// 是否为数采普通通道
	bool IsCommonChn() const;
	// 是否为信号源通道
	bool IsSignalChn() const;
	// 是否为控制卡信号源
	bool IsExtraSignalChn() const;
	// 是否为脉冲计数通道
	bool IsPulseChn() const;
	// 是否为转速通道
	bool IsRotateChn() const;
	// 是否为控制卡转速
	bool IsExtraRotateChn() const;
	// 计算系数
	void CalcChannelCoiefs();

	// 是否为测温通道
	bool IsMeasureTemperatureChn() const;
	// 电学量到物理量的转换
	float VoltageToPhysicsValue(float dbVoltageVal) const;
};

// 测量类型
enum MEASURE_TYPE
{
	MEASURE_TYPE_INTERNAL_DA,					// 内输入数采		// CChanGeneralParam
	MEASURE_TYPE_EXTERNAL_DA,					// 外输入数采		// CChanGeneralParam
	MEASURE_TYPE_STRAINMETER,					// 应变应力			// CStrainChanParam 补偿通道？？
	MEASURE_TYPE_SENSOR_PE,						// 电荷传感器		// CChanGeneralParam
	MEASURE_TYPE_SENSOR_BT,						// 桥式传感器		// CChanGeneralParam
	MEASURE_TYPE_TEMPERATURE_PT,				// 铂电阻测温		// CPtTemperatureChnParam
	MEASURE_TYPE_TEMPERATURE_THERMO,			// 热电偶测温		// CThermoTemperatureChnParam

	MEASURE_TYPE_ROTATE_SPEED,					// 转速测量			// CTachoChnParam
	MEASURE_TYPE_SIGNAL,						// 信号发生器		// CWaveChanParam

	MEASURE_TYPE_DIF_DC,	                        // 差动直流
	MEASURE_TYPE_SIN_DC,                           // 单端直流
	MEASURE_TYPE_SIN_AC,                           // 单端交流
	MEASURE_TYPE_ICP_INTER,                        // 内置ICP
	MEASURE_TYPE_ICP_OUTER,                        // 外置ICP
	MEASURE_TYPE_GND,                              // 输入端短路

	MEASURE_TYPE_CURRENT,							// 电流测试			// CChanGeneralParam

	MEASURE_TWIST_STRAIN,			               // 应变式扭矩测量
	MEASURE_PULSE_COUNTER,			               // 脉冲计数器测量
	MEASURE_PRESSURE_TEST,			               // 压力测试
	MEASURE_SPECIAL_TEST,			               // 特殊测试
	MEASURE_HEAT_FLOW, 			                   // 热流测试

	MEASURE_TYPE_LASERDISTANCE,						//激光测距
	MEASURE_TYPE_EXACTATOV,							//动挠度

	MEASUER_TYPE_GPS = 30,							// GPS测量
	MEASUER_TYPE_CAN = 31,							// CAN测量
};

// 通道风格 
enum CHANNEL_STYLE
{
	ANALOG_CHANNEL_STYLE,			// 模拟通道
	EXTRATACHO_CHANNEL_STYLE,		// 控制卡转速通道
	EXTRASIGNAL_CHANNEL_STYLE,		// 控制卡信号源通道
	CAN_CHANNEL_STYLE,				// CAN通道
	GPS_CHANNEL_STYLE,				// GPS通道
	EXTRAPLUS_CHANNEL_STYLE,		// 控制卡计数器通道
	AUDIO_CHANNEL_STYLE,			// 音频通道
	VIDEO_CHANNEL_STYLE,			// 视频通道
};
