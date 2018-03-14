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
using System.Threading;

namespace WinComServer
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            //服务开启执行代码
            
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\log";
            log("server start....."+path);
            Log.Write("startnn");
            //string path = AppDomain.CurrentDomain + "\\log";
            //log("start"+path);
            //SocketServer server = new SocketServer();
            //server.server();

            ////初始化仪器
            //MainHardWare main = new MainHardWare();
            //if (!main.InitInterface())
            //{
            //    Debug.Write("初始化失败");
            //}
            //else
            //{
            //    Debug.Write("初始化成功");
            //}
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
            //服务结束执行代码
            Log.Write("服务停止：");
        }
        protected override void OnPause()
        {
            //服务暂停执行代码
            base.OnPause();
            Log.Write("服务暂停：");
        }
        protected override void OnContinue()
        {
            //服务恢复执行代码
            base.OnContinue();
            Log.Write("服务继续运行：");
        }
        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            base.OnShutdown();
            Log.Write("系统关闭：");
        }
        public void log(string str)
        {
            string path = @"D:\gitFile\frdLocalgitfile\WinComServer\WinComServer\bin\Debug\log.txt";
            using (FileStream fs = new FileStream(path, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(str);
                }
            }
        }
    }
}
