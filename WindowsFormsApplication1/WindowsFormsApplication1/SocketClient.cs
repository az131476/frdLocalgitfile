﻿using System;
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
        //创建 1个客户端套接字 和1个负责监听服务端请求的线程  
        Thread threadclient = null;
        Socket socketclient = null;
        bool flg = false;
        public bool socketConnect()
        {
            //定义一个套接字监听  
            socketclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //获取文本框中的IP地址  
            IPAddress address = IPAddress.Parse("127.0.0.1");

            //将获取的IP地址和端口号绑定在网络节点上  
            IPEndPoint point = new IPEndPoint(address, 8098);

            try
            {
                //客户端套接字连接到网络节点上，用的是Connect  
                socketclient.Connect(point);
            }
            catch (Exception)
            {
                Debug.Write("连接失败");
                return false;
            }

            threadclient = new Thread(recv);
            threadclient.IsBackground = true;
            threadclient.Start();
            return true;
        }

        // 接收服务端发来信息的方法    
        public void recv()
        {
            int x = 0;
            //持续监听服务端发来的消息 
            while (true)
            {
                try
                {
                    //定义一个1M的内存缓冲区，用于临时性存储接收到的消息  
                    byte[] arrRecvmsg = new byte[512];

                    //将客户端套接字接收到的数据存入内存缓冲区，并获取长度  
                    int length = socketclient.Receive(arrRecvmsg);

                    //将套接字获取到的字符数组转换字符串  
                    string strRevMsg = Encoding.UTF8.GetString(arrRecvmsg, 0, length);

                    //$SampleData{0,0,0,0,2,-0.327,0,0.0000}
                    //
                    if (strRevMsg.Substring(0, 11).Equals("$SampleData"))
                    {
                        string mstrRevMsg = strRevMsg.Substring("$SampleData".Length, strRevMsg.Length - "$SampleData".Length);
                        //int receiveCount = int.Parse(strRevMsg.Substring(8, 1));
                        //int channel = int.Parse(strRevMsg.Substring(4, 1));
                        //float d_y = float.Parse(strRevMsg.Substring("{0,0,0,0,2,".Length,"-0.327".Length));
                        //float d_s = float.Parse(strRevMsg.Substring("{0,0,0,0,2,-0.327,0,".Length, strRevMsg.Length-"{ 0,0,0,0,2,-0.327,0,".Length));

                        Debug.Write("  " + mstrRevMsg);
                        //Debug.Write("receiveCount:"+receiveCount+"  channel:"+channel+" d_s:"+d_s+"  d_y"+d_y);
                    }
                    //if (x == 1)
                    //{
                    //    //this.txtDebugInfo.AppendText("服务器:" + GetCurrentTime() + "\r\n" + strRevMsg + "\r\n\n");
                    //    Debug.Write("服务器:" + GetCurrentTime() + "\r\n" + strRevMsg + "\r\n\n");

                    //}
                    //else
                    //{
                    //    //this.txtDebugInfo.AppendText(strRevMsg + "\r\n\n");
                    //    Debug.Write("mm:"+strRevMsg + "\r\n\n");
                    //    //ClientSendMsg("sendStart");
                    //    x = 1;
                    //}
                }
                catch (Exception ex)
                {
                    Debug.Write("远程服务器已经中断连接" + "\r\n");
                    break;
                }
            }
        }

        //获取当前系统时间  
        DateTime GetCurrentTime()
        {
            DateTime currentTime = new DateTime();
            currentTime = DateTime.Now;
            return currentTime;
        }

        //发送字符信息到服务端的方法  
        public void ClientSendMsg(string sendMsg)
        {
            //将输入的内容字符串转换为机器可以识别的字节数组     
            byte[] arrClientSendMsg = Encoding.UTF8.GetBytes(sendMsg);
            //调用客户端套接字发送字节数组     
            socketclient.Send(arrClientSendMsg);    
            Debug.Write("打印： " + GetCurrentTime() + "\r\n" + sendMsg + "\r\n\n");
            //this.txtDebugInfo.AppendText("hello...." + ": " + GetCurrentTime() + "\r\n" + sendMsg + "\r\n\n");
        }
    }
}
