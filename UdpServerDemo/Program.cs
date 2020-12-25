using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace UdpServerDemo {
    class Program
    {
        private static string CLIENT_IP = string.Empty;
        static UdpClient udpServer;
        static void Main(string[] args)
        {
            CLIENT_IP = "192.168.13.141";
            udpServer = new UdpClient(61000);       // 当前服务器使用的端口
            udpServer.Connect(CLIENT_IP, 50000); // 与客户端建立连接
            Console.WriteLine("服务端已经开启......");

            #region 开启线程保持通讯

            var t1 = new Thread(ReciveMsg);
            t1.Start();
            var t2 = new Thread(SendMsg);
            t2.Start();

            #endregion
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        static void ReciveMsg()
        {

            var remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(CLIENT_IP), 50000); // 远程端点，即发送消息方的端点
            while (true)
            {
                byte[] receiveBytes = udpServer.Receive(ref remoteIpEndPoint); // 接收消息，得到数据报
                string returnData = Encoding.UTF8.GetString(receiveBytes);     // 解析字节数组，得到原消息
                Console.WriteLine($"{remoteIpEndPoint.Address}:{remoteIpEndPoint.Port}，" + returnData.ToString());
            }

        }

        /// <summary>
        /// 发送消息
        /// </summary>
        static void SendMsg()
        {
            while (true)
            {
                var msg = Console.ReadLine().ToString();                // 获取控制台字符串
                byte[] sendBytes = Encoding.UTF8.GetBytes(msg); // 将消息编码成字符串数组
                udpServer.Send(sendBytes, sendBytes.Length);      // 发送数据报
            }
        }
    }
}
