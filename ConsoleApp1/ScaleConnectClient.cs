using HI.label;
using log4net;
using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using NModbus;
using NModbus.Extensions.Enron;
using NModbus.Serial;
using NModbus.Utility;

namespace ConsoleApp1
{
    internal class ScaleConnectClient
    {
        private static readonly ILog logger = LogManager.GetLogger(nameof(ScaleConnectClient));

        private readonly string PortName;

        private readonly SerialPort Port;
        private IModbusSerialMaster Master;

        private volatile bool IsConnected = false;

        public ScaleConnectClient(string portName) { 
            this.PortName = portName;
            this.Port = new SerialPort(portName);
            this.Port.BaudRate = 19200;
            this.Port.DataBits = 8;
            this.Port.Parity = Parity.Even;
            this.Port.StopBits = StopBits.One;
            this.Port.ReadTimeout = 2000;
        }

        public void Connect()
        {
            try
            {
                this.Port.Open();
                ModbusFactory factory = new ModbusFactory();
                Master = factory.CreateRtuMaster(Port);
                this.IsConnected = true;
            }
            catch (Exception ex)
            {
                Task.Delay(5000).ContinueWith((t) =>
                {
                    this.Connect(); // 重新尝试连接
                });
            }
        }

        public async Task<ushort[]> ReadRegisters(byte slaveId, ushort startAddress)
        {
            ushort[] registers = new ushort[10];
            try
            {
                if(IsConnected) registers = await Master.ReadHoldingRegistersAsync(slaveId, startAddress, 10);
                return registers;
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine("Scale connection read registers has error, ex:{}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Scale connection read registers has error, ex:{}");
                this.IsConnected = false;
                await Task.Delay(5000).ContinueWith((t) =>
                {
                    this.Connect(); // 重新尝试连接
                });
            }
            return registers;
        }

        public void Disconnect()
        {
            Port.Close();
        }
    }
}
