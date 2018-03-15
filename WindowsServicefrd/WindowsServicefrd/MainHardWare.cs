using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServicefrd
{
    public class MainHardWare
    {

        public static bool InitInterface()
        {
            HardWare main = new HardWare();
            int nReturnValue=0;
            main.BeginInvoke(new Action(() => 
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string strPath = baseDir.Substring(0, baseDir.LastIndexOf('\\'));//D:\work\1-dhmonitor\bin\debug\Config\
                strPath += "\\config\\";

                ////初始化仪器控制接口
                main.GetHardWare().Init(baseDir, "chinese", out nReturnValue);
                
            }));
            return nReturnValue == 1 ? true : false;
        }
    }
}
