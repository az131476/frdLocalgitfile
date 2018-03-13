using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WinComServer
{
    class ActiceXHelper
    {
        /// <summary>
        /// ActiveX相关的操作方法封装
        /// </summary>
        private readonly static string g_registerApp = string.Empty;
        static ActiceXHelper()
        {
            string sysFolder = Environment.GetFolderPath(Environment.SpecialFolder.System);
            sysFolder = sysFolder.Replace("/", @"\");
            if (!sysFolder.EndsWith(@"\"))
                sysFolder += @"\";
            g_registerApp = sysFolder + "regsvr32.exe";
        }

        /// <summary>
        /// 注册ActiveX
        /// </summary>
        /// <param name="activeXFile">待注册的ActiveX文件</param>
        public static void Register(string activeXFile)
        {
            if (!File.Exists(activeXFile))
                return;

            Process regsvr32Process = new Process();
            regsvr32Process.StartInfo.FileName = g_registerApp;
            regsvr32Process.StartInfo.Arguments = " -s " + activeXFile;
            regsvr32Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            regsvr32Process.Start();
            regsvr32Process.WaitForExit();
        }

        /// <summary>
        /// 注销ActiveX
        /// </summary>
        /// <param name="activeXFile">待注销的ActiveX文件</param>
        public static void UnRegister(string activeXFile)
        {
            if (!File.Exists(activeXFile))
                return;

            Process regsvr32Process = new Process();
            regsvr32Process.StartInfo.FileName = g_registerApp;
            regsvr32Process.StartInfo.Arguments = " -u " + activeXFile;
            regsvr32Process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            regsvr32Process.Start();
            regsvr32Process.WaitForExit();
        }
    }
}
