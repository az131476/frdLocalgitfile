using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
