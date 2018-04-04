using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;

namespace WinformTest.Log
{
    public static class Biwriter
    {
        public static int  BinaryFile(string name,string content)
        {
            try
            {
                string sFileName = AppDomain.CurrentDomain.BaseDirectory + "\\blog";
                if (!Directory.Exists(sFileName))
                {
                    DirectoryInfo director = Directory.CreateDirectory(sFileName);
                }
                FileStream fs = new FileStream(sFileName+"\\"+name, FileMode.OpenOrCreate);
                BinaryWriter binWriter = new BinaryWriter(fs);
                byte[] buffer = HexToByte(content);
                if (buffer == null)
                {
                    return 0;
                }
                binWriter.Write(buffer, 0, buffer.Length);

                binWriter.Close();
                fs.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #region 将文件保存到本地  
        /// <summary>  
        /// 将文件保存到本地  
        /// </summary>  
        /// <param name="psContent">文件的二进制数据字符串</param>  
        /// <param name="psFileName">文件名称，必须带后缀</param>  
        public static void SaveFile(string psContent, string psFileName)
        {
            
            string sFileName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + '\\' + "blog";
            if (!Directory.Exists(sFileName))
            {
                DirectoryInfo director = Directory.CreateDirectory(sFileName);
            }
            FileStream fs = new FileStream(sFileName+"\\"+psFileName, FileMode.Append);
            BinaryWriter binWriter = new BinaryWriter(fs);
            binWriter.Write(bianma(psContent));
            binWriter.Flush();

            binWriter.Close();
            fs.Close();
        }
        public static string bianma(string s)
      {
          byte[] data = Encoding.Unicode.GetBytes(s);
          StringBuilder result = new StringBuilder(data.Length * 8);
 
         foreach (byte b in data)
         {
             result.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
         }
         return result.ToString();
     }
        /// <summary>
        /// 将二进制转成字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string jiema(string s)
        {
            System.Text.RegularExpressions.CaptureCollection cs =
                System.Text.RegularExpressions.Regex.Match(s, @"([01]{8})+").Groups[1].Captures;
            byte[] data = new byte[cs.Count];
            for (int i = 0; i < cs.Count; i++)
            {
                data[i] = Convert.ToByte(cs[i].Value, 2);
            }
            return Encoding.Unicode.GetString(data, 0, data.Length);
        }
        #endregion
        private static byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            try
            {
                for (int i = 0; i < returnBytes.Length; i++)
                {
                    //returnBytes[i] = Convert.ToByte(hexString.Substring(0, 2));
                    byte bt;
                    if (byte.TryParse(hexString.Substring(i * 2, 2), out bt))
                    {
                        returnBytes[i] = bt;
                    }
                    else
                    {
                        //
                    }
                    
                }
                    return returnBytes;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
