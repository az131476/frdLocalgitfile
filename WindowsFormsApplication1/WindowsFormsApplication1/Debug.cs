using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApplication1
{
    class Debug
    {
        public static void Write(string str)    // 记录服务启动  
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
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "   " + str);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + str);
                sw.Close();
                fs.Close();
            }
        }
        public static void Write1(string str)    // 记录服务启动  
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + "d1" + ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
        }
        public static void Write2(string str)    // 记录服务启动  
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + "d2" + ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
        }
        public static void Write3(string str)    // 记录服务启动  
        {
            string path = Environment.CurrentDirectory + "\\log";
            if (!Directory.Exists(path))
            {
                DirectoryInfo director = Directory.CreateDirectory(path);
            }
            string filePath = path + "\\" + "d3" + ".txt";
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(str);
                sw.Close();
                fs.Close();
            }
        }
        //public static string Read(string pth)
        //{
        //    string path = Environment.CurrentDirectory + "\\log";
        //    string filePath = path + "\\" + pth + ".txt";
        //    if (File.Exists(path))
        //    {
        //        FileStream fs = new FileStream(filePath, FileMode.Open);
        //        StreamReader sr = new StreamReader(fs, ASCIIEncoding.Default);
        //        string str = string.Empty;
        //        while (true)
        //        {
        //            str = sr.ReadLine();
        //            if (!string.IsNullOrEmpty(str))
        //            {
        //                return str;
        //            }
        //            else
        //            {
        //                sr.Close();
        //                fs.Close();
        //                break;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return null; 
        //    }
        //}
    }
}
