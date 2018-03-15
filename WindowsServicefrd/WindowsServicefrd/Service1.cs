using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsServicefrd
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Debug.Write("windows server start...");

            SocketServer server = new SocketServer();
            server.server();

            ////初始化仪器
            bool state = true;
            if (!MainHardWare.InitInterface())
            {
                Debug.Write("初始化失败");
            }
            else
            {
                Debug.Write("初始化成功");
            }
            //连接设备
            //main.DeviceConnect();
            ///<summary>
            ///采集仪器数据
            ///存储初始数据
            ///发送数据给客户端、其他
            ///</summary>
            //main.GetDeviceData();
        }

        protected override void OnStop()
        {
            Debug.Write("windows server stop");
        }
        protected override void OnContinue()
        {
            base.OnContinue();
        }

    }
}
