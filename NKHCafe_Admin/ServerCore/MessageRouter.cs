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
            if (message.StartsWith("[CHAT]|"))
            {
                string content = message.Substring(7); // Correct substring index to remove "[CHAT]|"
                Console.WriteLine($"[CHAT] Received from client: {content}");

                // Broadcast chat message to all clients including sender (optional, depends on chat app design)
                ServerManager.Instance.BroadcastMessageToAllClients($"Client: {content}", stream);
            }
            else if (message.StartsWith("[LOGIN]|"))
            {
                // Xử lý đăng nhập (chưa triển khai)
                string content = message.Substring(7);
                Console.WriteLine($"[LOGIN] Request received: {content}");
                // ... Login logic ...
                SendResponse(stream, "LOGIN_RESPONSE", "OK", "Login feature not implemented yet.");
            }
            else if (message.StartsWith("[ORDER]|"))
            {
                // Xử lý đặt món (chưa triển khai)
                string content = message.Substring(7);
                Console.WriteLine($"[ORDER] Request received: {content}");
                // ... Order logic ...
                SendResponse(stream, "ORDER_RESPONSE", "ERROR", "Order feature not implemented yet.");
            }
            else
            {
                Console.WriteLine($"[UNKNOWN MESSAGE] Received: {message}");
                SendResponse(stream, "UNKNOWN_RESPONSE", "ERROR", "Unknown message type.");
            }
        }

        private static void SendResponse(NetworkStream stream, string messageType, string status, string message)
        {
            string response = $"[{messageType}]|{status}|{message}";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            stream.Write(responseBytes, 0, responseBytes.Length);
        }
    }
}