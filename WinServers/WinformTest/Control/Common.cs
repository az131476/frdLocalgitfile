using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinformTest.Control
{
    public static class Common
    {
        //将字符串进行分解。strSeprator中的任何一个字符都作为分隔符。返回分节得到的字符串数目
        public static int BreakString(string strSrc, out List<string> lstDest, string strSeprator)
        {
            //清空列表
            lstDest = new List<string>();
            //个数
            int iCount = 0;

            if (strSeprator.Length == 0)
            {
                lstDest.Add(strSrc);
                iCount = 1;
                return iCount;
            }

            //查找的位置
            int iPos = 0;
            while (iPos < strSrc.Length)
            {
                int iNewPos = strSrc.IndexOf(strSeprator, iPos);
                //当前字符即分隔符
                if (iNewPos == iPos)
                {
                    iPos++;
                }
                //没找到分隔符
                else if (iNewPos == -1)
                {
                    lstDest.Add(strSrc.Substring(iPos, strSrc.Length - iPos));
                    iCount++;
                    iPos = strSrc.Length;
                }
                //其它
                else
                {
                    lstDest.Add(strSrc.Substring(iPos, iNewPos - iPos));
                    iCount++;
                    iPos = iNewPos;
                    iPos++;
                }
            }
            return iCount;
        }

        public static float ToFloat(this string value)
        {
            float n;
            float.TryParse(value, out n);

            return n;
        }

        public static int ToInt(this string value)
        {
            int n;
            int.TryParse(value, out n);

            return n;
        }

        public static string Left(this string value, char c)
        {
            string strVal = value.Substring(0, value.LastIndexOf(c));
            return strVal;
        }

        public static string Right(this string value, char c)
        {
            string strVal = value.Substring(value.LastIndexOf(c) + 1, value.Length - 1 - value.LastIndexOf(c));
            return strVal;
        }

        public static void GetValidFloatString(string strText, out string strFloat)
        {
            strFloat = "";
            if (strText.Contains('.'))
                strFloat = strText.Substring(0, strText.LastIndexOf('.'));
            else
                strFloat = strText;
        }
    }
}
