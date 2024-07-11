using HI.label;
using NModbus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ScaleConnectClient scaleConnect = new ScaleConnectClient("COM3");
            //scaleConnect.Connect();
            //ushort[] data = scaleConnect.ReadRegisters((byte)4, (ushort)148).Result;

            //Console.WriteLine(string.Join(", ", data));
            //Console.WriteLine(ModbusUtility.GetUInt32(data[0], data[1]));
            //Console.WriteLine(ModbusUtility.GetUInt32(data[2], data[3]));
            string utf8String = "阿3"; // UTF-8 编码的字符串
            char[] chars = utf8String.ToCharArray();
            // 将 UTF-8 编码的字符串转换为 GB2312 编码的字节数组
            byte[] gb2312Bytes = Encoding.GetEncoding("GB2312").GetBytes(utf8String);


            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.1.253"), 1030);
            LabelConnectClient client = new LabelConnectClient(endPoint);
            client.Connect();
            Thread.Sleep(500);
            client.RequestTextContent(1, "大撒大撒", "的撒旦", "发发发发");
            client.RequestTextContent(3, "打赏v阿萨大大", "方法", "大苏打的");
            while (true)
            {
                if (client.Status())
                {
                    client.RequestUpdateCount(1, 11);
                    Thread.Sleep(500);
                    client.RequestUpdateCount(3, 22);
                    Thread.Sleep(500);
                    client.RequestOpenLight(1, 11);
                    Thread.Sleep(500);
                    client.RequestOpenLight(3, 22);
                    Thread.Sleep(500);
                    client.RequestCloseLight(1);
                    Thread.Sleep(500);
                    client.RequestCloseLight(3);
                    Thread.Sleep(500);
                    client.RequestStatus(1);
                    Thread.Sleep(500);
                    client.RequestStatus(3);
                }
                Thread.Sleep(5000); // 可以适当地添加延迟，避免过度消耗 CPU
            }
        }


    }
}
