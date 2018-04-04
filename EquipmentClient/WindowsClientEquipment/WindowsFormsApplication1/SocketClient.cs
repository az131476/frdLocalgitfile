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
    public class SocketClient
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
                    byte[] bigBuffer = new byte[128];
                    //
                    int length = socketclient.Receive(bigBuffer);
                    string strRece = Encoding.UTF8.GetString(bigBuffer, 0, length);
                    Debug.Write("strRece:" + strRece);
                    int b_len = length;
                    #region
                    
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
