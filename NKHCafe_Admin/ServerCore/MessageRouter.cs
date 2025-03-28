using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;

namespace NKHCafe_Admin.ServerCore
{
    public static class MessageRouter
    {
        public static void ProcessMessage(string message, NetworkStream stream)
        {
            if (message.StartsWith("[CHAT]"))
            {
                string content = message.Substring(6);
                Console.WriteLine("[CHAT] " + content);

                // Gửi phản hồi lại client
                string response = "[SERVER]: Đã nhận tin nhắn của bạn.";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }
            else if (message.StartsWith("[LOGIN]"))
            {
                // Xử lý đăng nhập
            }
            else if (message.StartsWith("[ORDER]"))
            {
                // Xử lý đặt món
            }
        }
    }
}
