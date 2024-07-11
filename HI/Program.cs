using HI.label;
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
        static void Main1(string[] args)
        {
            ScaleConnectClient scaleConnect = new ScaleConnectClient("COM1");
            scaleConnect.Connect();
            ushort[] data = scaleConnect.ReadRegisters(1, 185).Result;
            Console.WriteLine(string.Join(", ", data));

            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9989);
            LabelConnectClient client = new LabelConnectClient(endPoint);
            client.Connect();
            while (true)
            {
                if (client.Status())
                {
                    client.RequestUpdateCount(1, 22);
                    client.RequestOpenLight(1, 22);
                    client.RequestCloseLight(1);
                    client.RequestStatus(1);
                }
                Thread.Sleep(5000); // 可以适当地添加延迟，避免过度消耗 CPU
            }
        }


    }
}
