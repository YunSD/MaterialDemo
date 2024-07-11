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
            ScaleConnectClient scaleConnect = new ScaleConnectClient("COM3");
            scaleConnect.Connect();
            ushort[] data = scaleConnect.ReadRegisters((byte)4, (ushort)148).Result;
            
            Console.WriteLine(string.Join(", ", data));
            Console.WriteLine(ModbusUtility.GetUInt32(data[0], data[1]));
            Console.WriteLine(ModbusUtility.GetUInt32(data[2], data[3]));

            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9989);
            LabelConnectClient client = new LabelConnectClient(endPoint);
            //client.Connect();
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
