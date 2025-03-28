using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using NKHCafe_Admin.Utils;

namespace NKHCafe_Admin.ServerCore
{
    public class ClientHandler
    {
        private TcpClient _client;

        public ClientHandler(TcpClient client)
        {
            _client = client;
        }

        public void HandleClient()
        {
            NetworkStream stream = _client.GetStream();
            byte[] buffer = new byte[4096];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Logger.Log($"[CLIENT] {message}");

                    // Gửi message xử lý
                    MessageRouter.ProcessMessage(message, stream);
                }
                catch (Exception ex)
                {
                    Logger.Log("[ERROR] " + ex.Message);
                    break;
                }
            }

            Logger.Log("[SERVER] Client ngắt kết nối.");
            _client.Close();
        }
    }
}