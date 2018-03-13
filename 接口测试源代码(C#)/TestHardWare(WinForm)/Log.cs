using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TestHardWare_WinForm_
{
    class Log
    {
        public static void WriteLog(string context)
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"   "+context);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+"  "+context);
                sw.Close();
                fs.Close();
            }
        }
        public static void WriteLog1(string context)
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd")+"-1" + ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ "   "+ context);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + context);
                sw.Close();
                fs.Close();
            }
        }
        public static void WriteLog2(string context)
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") +"-2"+ ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +"  "+ context);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + context);
                sw.Close();
                fs.Close();
            }
        }
        public static void WriteLog3(string context)
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + DateTime.Now.ToString("yyyy-MM-dd") +"-3"+ ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   "+context);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + context);
                sw.Close();
                fs.Close();
            }
        }
    }
}
