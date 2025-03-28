using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace NKHCafe_Admin.ServerCore
{
    public class Server
    {
        private TcpListener _listener;
        private bool _isRunning;

        public void Start(string ip, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
            _listener.Start();
            _isRunning = true;

            Console.WriteLine($"[SERVER] Đang lắng nghe tại {ip}:{port}");

            while (_isRunning)
            {
                TcpClient client = _listener.AcceptTcpClient();
                Console.WriteLine("[SERVER] Client kết nối!");

                ClientHandler handler = new ClientHandler(client);
                Thread clientThread = new Thread(handler.HandleClient);
                clientThread.Start();
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }
    }
}