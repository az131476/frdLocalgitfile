using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication1
{
    class SocketClient
    {
        Thread threadclient = null;
        Socket socketclient = null;
        public string d6;
        public bool socketConnect()
        {
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(address, 8098);
            try
            {
                socketclient.Connect(point);
            }
            catch (Exception)
            {
                return false;
            }
            threadclient = new Thread(recv);
            threadclient.IsBackground = true;
            threadclient.Start();
            return true;
        }
        public void recv()
        {
            int x = 0;
            while (true)
            {
                try
                {
                    #region
                    byte[] bigBuffer = new byte[512];
                    //
                    int length = socketclient.Receive(bigBuffer);
                    string strRece = Encoding.UTF8.GetString(bigBuffer, 0, length);
                    //Debug.Write("strRece:" + strRece);
                    int b_len = length;
                    #region
                    //"{d110d310d66-0.348d88460.6000}{d110d310d66-0.347d88460.6000}{d110d311d66-0.270d88460.6000}{d110d311d66-0.268d88460.6000}{d110d312d66-0.174d88460.6000}{d110d312d66-0.174d88460.6000}{d110d310d66-0.342d88460.8000}{d110d310d66-0.346d88460.8000}{d110d311d66-0.273d88460.8000}{d110d311d66-0.269d88460.8000}{d110d312d66-0.175d88460.8000}{d110d312d66-0.175d88460.8000}{d110d310d66-0.348d88461.0000}{d110d310d66-0.346d88461.0000}{d110d311d66-0.269d88461.0000}
                    for (int i=0;i<b_len;i++)
                    {
                        if (strRece.Substring(0, 3).Equals("{d1"))
                        {
                            strRece = strRece.Substring(3, strRece.Length - 3);
                            string len_d1 = strRece.Substring(0, 1);
                            string d1 = strRece.Substring(1, 1);
                            strRece = strRece.Substring(2,strRece.Length-2 );
                            if (strRece.Substring(0, 2).Equals("d3"))
                            {
                                strRece = strRece.Substring(2, strRece.Length - 2);
                                int len_d2 = int.Parse(strRece.Substring(0, 1));
                                string d2 = strRece.Substring(1, 1);
                                strRece = strRece.Substring(2, strRece.Length - 2);
                                if (strRece.Substring(0, 2).Equals("d6"))
                                {
                                    strRece = strRece.Substring(2, strRece.Length - 2);
                                    int len_d6 = int.Parse(strRece.Substring(0, 1));  
                                    d6 = strRece.Substring(1, len_d6);
                                    strRece = strRece.Substring(1+len_d6+1-1, strRece.Length - 1-len_d6);
                                    if (strRece.Substring(0, 2).Equals("d8"))
                                    { //{d110d312d66-0.191 d8 12 1006442.4126}
                                        strRece = strRece.Substring(2, strRece.Length - 2);
                                        int len_d8;
                                        string d8;
                                        if (strRece.Substring(2, 1).Equals("}"))
                                        {
                                            len_d8 = int.Parse(strRece.Substring(0, 1));
                                            d8 = strRece.Substring(1, len_d8);
                                            strRece = strRece.Substring(1 + len_d8 + 1 - 1, strRece.Length - 1 - len_d8);
                                            strRece = strRece.Substring(1, strRece.Length - 1);
                                        }
                                        else
                                        {
                                            len_d8 = int.Parse(strRece.Substring(0,2));
                                            d8 = strRece.Substring(2,len_d8);
                                            strRece = strRece.Substring(2+len_d8+1-1,strRece.Length-2-len_d8);
                                            strRece = strRece.Substring(1,strRece.Length-1);
                                        }
                                        
                                        b_len = strRece.Length;
                                        double equip_id = double.Parse(d1);
                                        double channel_id = double.Parse(d2);
                                        double data = double.Parse(d6);
                                        double times = double.Parse(d8);
                                        Form1 f = new Form1();
                                        if (channel_id == 0)
                                        {

                                            Debug.Write1(times+","+data);
                                        }
                                        if (channel_id==1)
                                        {
                                            Debug.Write2(times + "," + data);
                                        }
                                        if (channel_id==2)
                                        {
                                            Debug.Write3(times + "," + data);
                                        }
                                        //Debug.Write("d1:"+d1+" d2:"+d2+" d6:"+d6+" d8:"+d8);
                                    }
                                }
                            }
                        }
                    }
                    
                    #endregion

                    #endregion
                }
                catch (Exception ex)
                {
                    Debug.Write("远程服务器已经中断连接-"+ex.Message);
                    break;
                }
                Thread.Sleep(100);
            }
        }
        //获取当前系统时间  
        DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
        public void ClientSendMsg(string sendMsg)
        {
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);   
            socketclient.Send(arrClientSendMsg);    
        }
    }
}
