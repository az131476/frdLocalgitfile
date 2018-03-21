using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
    class DeviceConnect
    {
        public static HardWare hardWare = new HardWare();
        /// <summary>
        /// 检查仪器是否连接
        /// </summary>
        /// <returns></returns>
        public static bool ConAllDevice()
        {
            int nReturnValue;
            hardWare.GetHardWare().IsConnectMachine(out nReturnValue);
            return nReturnValue == 1 ? true : false;


            //GetAllGroupChannel();
            //InitChannelCombo();
            //GetSampleFreqList();
            //GetSampleParam();
            //InitFrepCombo();
            ////除通道信息外获取所有参数
            //RefreshAllParam();
        }
    }
}
