using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NKHCafe_Admin.ServerCore
{
    // Singleton class to manage server-wide client list and broadcasting
    public sealed class ServerManager
    {
        private static readonly ServerManager _instance = new ServerManager();
        private readonly List<TcpClient> _connectedClients = new List<TcpClient>();
        public Server ServerInstance { get; set; } // Reference to the Server instance

        private ServerManager() { } // Private constructor for singleton

        public static ServerManager Instance => _instance;

        public void AddClient(TcpClient client)
        {
            lock (_connectedClients)
            {
                _connectedClients.Add(client);
            }
        }

        public void RemoveClient(TcpClient client)
        {
            lock (_connectedClients)
            {
                _connectedClients.Remove(client);
            }
        }

        public void ClearClients()
        {
            lock (_connectedClients)
            {
                _connectedClients.Clear();
            }
        }


        public void BroadcastMessageToAllClients(string message, NetworkStream excludeStream = null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);

            lock (_connectedClients)
            {
                foreach (TcpClient client in _connectedClients)
                {
                    if (client.Connected)
                    {
                        try
                        {
                            NetworkStream stream = client.GetStream();
                            if (stream != excludeStream) // Optional: Exclude sender's stream from broadcast
                            {
                                stream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        catch
                        {
                            // Handle client send error, maybe remove client from list in real app
                        }
                    }
                }
            }
            // No need to append to server log here, UI form will handle display
        }
    }
}