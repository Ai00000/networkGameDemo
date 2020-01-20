using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace _Scripts
{
    
    
    public static class NetManager
    {
        /// <summary>
        /// Socket实例
        /// </summary>
        public static Socket Socket { get; set; }
        /// <summary>
        /// 接收缓冲区
        /// </summary>
        public static byte[] ReadBuff { private get; set; }=new byte[1024];
        
        /// <summary>
        /// 处理委托
        /// </summary>
        /// <param name="str"></param>
        public delegate void MsgListener(string str);
        
        /// <summary>
        /// 监听字典
        /// </summary>
        public static Dictionary<string,MsgListener> Listeners { get; set; }=new Dictionary<string, MsgListener>();
        
        /// <summary>
        /// 信息队列
        /// </summary>
        public static Queue<string> MsgList { get; set; }=new Queue<string>();
        
        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="msgName">信息名</param>
        /// <param name="listener">绑定的委托</param>
        public static void AddListener(string msgName, MsgListener listener)
        {
            Listeners[msgName] = listener;
        }
        
        /// <summary>
        /// 获得描述
        /// </summary>
        /// <returns></returns>
        public static string GetDesc()
        {
            if (Socket==null)
            {
                return "";
            }

            if (!Socket.Connected)
            {
                return "";
            }

            return Socket.LocalEndPoint.ToString();
        }
        
        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口号</param>
        public static void Connect(string ip, int port)
        {
            Socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            Socket.Connect(ip,port);
            Socket.BeginReceive(ReadBuff, 0, 1024, SocketFlags.None,callback: ReceiveCallBack, Socket);
        }
        
        /// <summary>
        /// 接收回调
        /// </summary>
        /// <param name="ar">异步结果参数</param>
        private static void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var count = socket.EndReceive(ar);
                var recvStr = Encoding.UTF8.GetString(ReadBuff, 0, count);
                MsgList.Enqueue(recvStr);    //向消息队列添加消息
                //继续监听接收
                socket.BeginReceive(ReadBuff, 0, ReadBuff.Length, SocketFlags.None, ReceiveCallBack, socket);
            }
            catch (SocketException e)
            {
                Debug.Log("Socket 接收错误："+e.Message);
            }
        }
        
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sendStr">消息体</param>
        public static void Send(string sendStr)
        {
            if (Socket==null)
            {
                return;
            }

            if (!Socket.Connected)
            {
                return;
            }

            var sendBytes = Encoding.UTF8.GetBytes(sendStr);
            Socket.Send(sendBytes);
        }
        
        /// <summary>
        /// 持续监听处理
        /// </summary>
        public static void Update()
        {
            if (MsgList.Count<=0)
            {
                return;
            }
            
            //解析数据
            var msgStr = MsgList.Dequeue();    //获得消息队列的首个元素并出队
            var data = msgStr.Split('|');
            var msgName = data[0];
            var msgArgs = data[1];
            
            //回调监听
            if (Listeners.ContainsKey(msgName))
            {
                Listeners[msgName](msgArgs);
            }


        }

    }
}