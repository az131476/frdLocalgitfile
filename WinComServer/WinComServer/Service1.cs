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
            System.Timers.Timer t = new System.Timers.Timer(1000); //这里的1000指的是Timer的时间间隔为1000毫秒
            t.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Tick); //Timer_Click是到达时间的时候执行事件的函数
            t.AutoReset = true; //设置是执行一次（false）还是一直执行(true)
            t.Enabled = true; //是否执行System.Timers.Timer.Elapsed事件

            Log("服务启动：");
            Log("正在继续");
        }
        protected override void OnStop()
        {
            //服务结束执行代码
            Log("服务停止：");
        }
        protected override void OnPause()
        {
            //服务暂停执行代码
            base.OnPause();
            Log("服务暂停：");
        }
        protected override void OnContinue()
        {
            //服务恢复执行代码
            base.OnContinue();
            Log("服务继续运行：");
        }
        protected override void OnShutdown()
        {
            //系统即将关闭执行代码
            base.OnShutdown();
            Log("系统关闭：");
        }
        void Log(string str)    // 记录服务启动  
        {  
            string path = @"D:\gitFile\frdLocalgitfile\WinComServer\WinComServer\bin\Debug\log.txt";
            using (FileStream fs = new FileStream(path,FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(str);
                }    
            }  
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Process[] localByName = Process.GetProcessesByName("exe");
            if (!IsExistProcess("exe")) //如果得到的进程数是0, 那么说明程序未启动，需要启动程序
            {
                Process.Start("exe"); //启动程序的路径
            }
            else
            {
                //如果程序已经启动，则执行这一部分代码
                Log("已经启动");
            }
        }
        private bool IsExistProcess(string processName)
        {
            Process[] MyProcesses = Process.GetProcesses();
            foreach (Process MyProcess in MyProcesses)
            {
                if (MyProcess.ProcessName.CompareTo(processName) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
