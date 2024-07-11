using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Caching;

namespace HI.label
{
    internal class LabelConnectClient
    {

        private static readonly ILog logger = LogManager.GetLogger(nameof(LabelConnectClient));

        private EndPoint endPoint;
        private Socket socket;
        private volatile bool IsConnected = false;

        private LabelMessageDecode messageDecode;

        public LabelConnectClient(EndPoint endPoint)
        {

            this.endPoint = endPoint;
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.messageDecode = new LabelMessageDecode();
            this.messageDecode.Processor += this.MessageProcessor;
        }

        /// <summary>
        ///  connect to label control
        /// </summary>
        /// <returns></returns>
        public void Connect()
        {
            // 设置连接完成时的回调方法
            SocketAsyncEventArgs connectArgs = new SocketAsyncEventArgs();
            connectArgs.RemoteEndPoint = this.endPoint;
            connectArgs.Completed += this.ConnectCallback;
            Console.WriteLine(socket.Connected);
            bool willRaiseEvent = socket.ConnectAsync(connectArgs);
            if (!willRaiseEvent)
            {
                // 如果 ConnectAsync 立即返回 false，说明连接已经完成或者发生了错误
                ConnectCallback(null, connectArgs);
            }
            Console.WriteLine(socket.Connected);
        }

        /// <summary>
        /// connect callback
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectCallback(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                IsConnected = true;
                logger.Debug("Connection successful in callback");
                // 连接成功的处理逻辑
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                byte[] buffer = new byte[1024];
                args.SetBuffer(buffer, 0, buffer.Length);
                args.Completed += this.ReceiveCompletedCallback;
                socket.ReceiveAsync(args);
            }
            else
            {
                logger.Debug($"Connection failed in callback: {e.SocketError}");

                // 连接失败的处理逻辑
                Task.Delay(5000).ContinueWith((t) =>
                {
                    this.messageDecode.clear();
                    this.Connect(); // 重新尝试连接
                });
            }
        }

        public bool Status() {
            return this.IsConnected;
        }

        public void Disconnect()
        {
            socket.Close();
        }


        public void SendMessage(byte[] message)
        {
            Console.WriteLine(BitConverter.ToString(message));
            if (this.IsConnected) socket.Send(message);
            
        }

      
        private void ReceiveCompletedCallback(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                if(e.BytesTransferred > 0) {
                    byte[] receivedData = new byte[e.BytesTransferred];
                    Array.Copy(e.Buffer, e.Offset, receivedData, 0, receivedData.Length);
                    this.messageDecode.Decode(receivedData);
                }

                // 继续异步接收下一条消息
                bool willRaiseEvent = socket.ReceiveAsync(e);
            }
            else
            {
                IsConnected = false;
                // 处理错误或连接关闭的情况
                logger.Debug($"Socket error: {e.SocketError}. Bytes received: {e.BytesTransferred}");
                socket.Disconnect(true);
                //this.messageDecode.clear();
                Task.Delay(5000).ContinueWith((t) =>
                {
                    this.messageDecode.clear();
                    this.Connect(); // 重新尝试连接
                });
            }
        }


        private event EventHandler<byte[]> m_Processor;
        public event EventHandler<byte[]> Processor
        {
            add { m_Processor += value; }
            remove { m_Processor -= value; }
        }

        private void MessageProcessor(object sender, byte[] message)
        {
            logger.Debug($"Received message: {BitConverter.ToString(message)}");
            if (m_Processor?.GetInvocationList().Length > 0)
                m_Processor.Invoke(this, message);
        }

        /// <summary>
        /// LabelMessageDecode :Electronic Lable Message decode
        /// </summary>
        internal class LabelMessageDecode {
        
            private static readonly int ExpectedDataLength = 13;

            private byte[] cache  = new byte[ExpectedDataLength];
            private int cache_index = 0;

            public LabelMessageDecode() { }

            public void Decode(byte[] message) {
                int offset = 0;
                while (true)
                {
                    if (message.Length - offset >= ExpectedDataLength - cache_index) {
                        Array.Copy(message, offset, cache, cache_index, ExpectedDataLength - cache_index);
                        
                        if(m_Processor?.GetInvocationList().Length > 0)
                            m_Processor.Invoke(this, cache);

                        offset += ExpectedDataLength - cache_index;
                        cache_index = 0; 
                    }
                    else
                    {
                        int len = message.Length - offset;
                        if (len > 0)
                        {
                            Array.Copy(message, offset, cache, cache_index, len);
                            cache_index += len;
                        }
                        
                        break;
                    }
                }
            }

            private event EventHandler<byte[]> m_Processor;
            public event EventHandler<byte[]> Processor
            {
                add
                {
                    m_Processor += value;
                }
                remove
                {
                    m_Processor -= value;
                }
            }

            public void clear()
            {
                cache_index = 0;
            }
        }



        public void RequestUpdateCount(int index, int number) {
            byte high = (byte)((number >> 8) & 0xFF);  // 高位字节
            byte low = (byte)(number & 0xFF);          // 低位字节
            int count = 1 + index + 4 + high + low;
            byte[] data = new byte[13] { 0x08, 0x00, 0x00, 0x00, (byte)index, 0x01, (byte)index, 0x04, 0x00, high, low, 0x00, (byte)count };
             this.SendMessage(data);
        }
        public void RequestOpenLight(int index, int number) {
            int count = 1 + index + 1 + number;
            byte[] data = new byte[13] { 0x08, 0x00, 0x00, 0x00, (byte)index, 0x01, (byte)index, 0x01, 0x00, 0x00, (byte)number, 0x00, (byte)count };
            this.SendMessage(data);
        }
        public void RequestCloseLight(int index) {
            int count = 1 + index + 5;
            byte[] data = new byte[13] { 0x08, 0x00, 0x00, 0x00, (byte)index, 0x01, (byte)index, 0x05, 0x00, 0x00, 0x00, 0x00, (byte)count };
            this.SendMessage(data);
        }
        public void RequestStatus(int index) {
            int count = 1 + index + 6;
            byte[] data = new byte[13] { 0x08, 0x00, 0x00, 0x00, (byte)index, 0x01, (byte)index, 0x06, 0x00, 0x00, 0x00, 0x00, (byte)count };
            this.SendMessage(data);
        }


    }
}
