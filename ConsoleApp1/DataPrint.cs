using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class DataPrint
    {
        public void PrintHexString(byte[] bytes) {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b);
                Console.Write($"{b:X2} "); // 将每个字节格式化为两位16进制数并输出
            }
            BitConverter.ToString(bytes);
        }

    }
}
