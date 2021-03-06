﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace WaveShape
{
    class Log
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
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "   " + str);
                sw.Close();
                fs.Close();
            }
            else
            {
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "  " + str);
                sw.Close();
                fs.Close();
            }
        }
    }
}
