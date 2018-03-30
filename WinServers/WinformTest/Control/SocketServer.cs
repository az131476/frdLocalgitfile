using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections;

namespace WinformTest.Control
{
    public class SocketServer
    {

        public static Socket socketwatch = null;
        public static Socket connection = null;
        public IPAddress clientIP;

        static Dictionary<string, Socket> clientConnectionItems = new Dictionary<string, Socket> { };

        public void server()
        {
            socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
            IPAddress address = IPAddress.Parse("127.0.0.1");  
            IPEndPoint point = new IPEndPoint(address, 8098);  
            socketwatch.Bind(point);

            socketwatch.Listen(20);
 
            Thread threadwatch = new Thread(watchconnecting);
 
            threadwatch.IsBackground = true;

            //启动线程     
            threadwatch.Start();

            Log.Debug.Write("socketserver start...");
        }
        //监听客户端发来的请求  
        public void watchconnecting()
        {     
            while (true)
            {
                try
                {
                    connection = socketwatch.Accept();
                }
                catch (Exception ex)
                {    
                    Log.Debug.Write("异常");
                    break;
                }
                try
                { 
                    clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                    int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;
                    string sendmsg = "连接服务端成功！\r\n" + "本地IP:" + clientIP + "，本地端口" + clientPort.ToString();

                    string remoteEndPoint = connection.RemoteEndPoint.ToString();
                    Log.Debug.Write("成功与" + remoteEndPoint + "客户端建立连接！\t\n");
                    clientConnectionItems.Add(remoteEndPoint, connection);
                    IPEndPoint netpoint = connection.RemoteEndPoint as IPEndPoint;
 
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(recv);
                    Thread thread = new Thread(pts);
                    thread.IsBackground = true;
                    //启动线程     
                    thread.Start(connection);
                }
                catch (Exception ex)
                {
                    
                }
            }
        }
        public int sendMsg(string sendmsg)
        {
            try
            {
                byte[] arrSendMsg = Encoding.UTF8.GetBytes(sendmsg);
               // List<ArraySegment<byte>> list = new List<ArraySegment<byte>>();

                return connection.Send(arrSendMsg);
            }
            catch (Exception ex)
            {
                Log.Debug.Write("发送指令失败"+ex.Message);
                return -1;
            }
        }
        /// <summary>
        /// 接收客户端发来的信息，客户端套接字对象
        /// </summary>
        /// <param name="socketclientpara"></param>    
        public void recv(object socketclientpara)
        {
            Socket socketServer = socketclientpara as Socket;

            while (true)
            {    
                byte[] arrServerRecMsg = new byte[1024 * 1024]; 
                try
                {
                    int length = socketServer.Receive(arrServerRecMsg);    
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                    if (strSRecMsg.Substring(0, 4).Equals("$001"))
                    {
                        Log.Debug.Write("客户端参数变动设置");
                        ///<summary>
                        ///<通道参数>
                        ///通道频率：根据仪器获取显示到客户端进行设置 
                        ///通道列表：16个通道，显示格式为1-X，可根据客户端设置
                        /// 测量类型：仪器获取
                        /// 量程：
                        /// <应变应力子参数>
                        /// 应变应力类型：列表选择设置，根据测量类型值而不同
                        /// 桥路方式：选择设置
                        /// 应变计阻值：
                        /// 导线电阻
                        /// 灵敏度系数
                        /// 泊松比
                        /// 弹性模量
                        /// </summary>
                        //new MainHardWare().SetSampleParam();

                    }
                    else if (strSRecMsg.Substring(0, 4).Equals("$101"))
                    {
                        Log.Debug.Write("客户端点击事件");
                        ///<summary>
                        ///启动采样
                        ///平衡
                        ///停止采样
                        ///清零
                        /// </summary>
                        //设置平衡
                        if (strSRecMsg.Substring(4, 7).Equals("balance"))
                        {
                            //执行平衡设置
                        }
                    }
                    else if (strSRecMsg.Equals("sendStart"))
                    {
                        
                    }
                    Log.Debug.Write("客户端:" + socketServer.RemoteEndPoint + ",time:" + GetCurrentTime() + "\r\n" + strSRecMsg + "\r\n\n");

                    socketServer.Send(Encoding.UTF8.GetBytes("jdk"));
                }
                catch (Exception ex)
                {
                    clientConnectionItems.Remove(socketServer.RemoteEndPoint.ToString());

                    Log.Debug.Write("Client Count:" + clientConnectionItems.Count);

                    //提示套接字监听异常  
                    Log.Debug.Write("客户端" + socketServer.RemoteEndPoint + "已经中断连接" + "\r\n" + ex.Message + "\r\n" + ex.StackTrace + "\r\n");
                    //关闭之前accept出来的和客户端进行通信的套接字 
                    socketServer.Close();
                    break;
                }
            }
        }
        ///      
        /// 获取当前系统时间的方法    
        /// 当前时间     
        static DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }
    }
}
