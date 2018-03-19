using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WinformTest.Control
{
    public class SocketServer
    {
        // 创建一个和客户端通信的套接字
        public static Socket socketwatch = null;
        public static Socket connection = null;
        public IPAddress clientIP;
        //定义一个集合，存储客户端信息
        static Dictionary<string, Socket> clientConnectionItems = new Dictionary<string, Socket> { };

        public void server()
        {
            //定义一个套接字用于监听客户端发来的消息，包含三个参数（IP4寻址协议，流式连接，Tcp协议）  
            socketwatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务端发送信息需要一个IP地址和端口号  
            IPAddress address = IPAddress.Parse("127.0.0.1");
            //将IP地址和端口号绑定到网络节点point上  
            IPEndPoint point = new IPEndPoint(address, 8098);
            //此端口专门用来监听的  

            //监听绑定的网络节点  
            socketwatch.Bind(point);

            //将套接字的监听队列长度限制为20  
            socketwatch.Listen(20);

            //负责监听客户端的线程:创建一个监听线程  
            Thread threadwatch = new Thread(watchconnecting);

            //将窗体线程设置为与后台同步，随着主线程结束而结束  
            threadwatch.IsBackground = true;

            //启动线程     
            threadwatch.Start();

            Log.Debug.Write("socketserver start...");
        }
        //监听客户端发来的请求  
        public void watchconnecting()
        {
            //监听客户端发来的请求     
            while (true)
            {
                try
                {
                    connection = socketwatch.Accept();
                }
                catch (Exception ex)
                {
                    //提示套接字监听异常     
                    Log.Debug.Write("异常");
                    break;
                }
                try
                {
                    //获取客户端的IP和端口号  
                    clientIP = (connection.RemoteEndPoint as IPEndPoint).Address;
                    int clientPort = (connection.RemoteEndPoint as IPEndPoint).Port;

                    //client显示"连接成功的"  
                    string sendmsg = "连接服务端成功！\r\n" + "本地IP:" + clientIP + "，本地端口" + clientPort.ToString();
                    sendMsg(sendmsg);

                    //客户端网络结点号  
                    string remoteEndPoint = connection.RemoteEndPoint.ToString();
                    //显示与客户端连接情况
                    Log.Debug.Write("成功与" + remoteEndPoint + "客户端建立连接！\t\n");
                    //添加客户端信息  
                    clientConnectionItems.Add(remoteEndPoint, connection);

                    //IPEndPoint netpoint = new IPEndPoint(clientIP,clientPort); 
                    IPEndPoint netpoint = connection.RemoteEndPoint as IPEndPoint;

                    //创建一个通信线程      
                    ParameterizedThreadStart pts = new ParameterizedThreadStart(recv);
                    Thread thread = new Thread(pts);
                    //设置为后台线程，随着主线程退出而退出 
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
                //创建一个内存缓冲区，其大小为1024*1024字节  即1M     
                byte[] arrServerRecMsg = new byte[1024 * 1024];
                //将接收到的信息存入到内存缓冲区，并返回其字节数组的长度    
                try
                {
                    int length = socketServer.Receive(arrServerRecMsg);

                    //将机器接受到的字节数组转换为字符串     
                    string strSRecMsg = Encoding.UTF8.GetString(arrServerRecMsg, 0, length);

                    if (strSRecMsg.Substring(0, 4).Equals("$100"))
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
