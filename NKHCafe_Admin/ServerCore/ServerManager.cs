using System; // Thêm using System
using System.Collections.Generic;
using System.IO; // Thêm using IO
using System.Linq; // Thêm using Linq
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks; // Thêm using Task
using NKHCafe_Admin.Utils; // Assuming Logger is here
using System.Windows.Forms;
using System.Data.SqlClient; // Nhớ thêm dòng này ở đầu file

namespace NKHCafe_Admin.ServerCore
{
    public sealed class ServerManager
    {
        private static readonly ServerManager _instance = new ServerManager();
        // Dùng Dictionary để có thể lưu thêm thông tin client nếu cần
        private readonly Dictionary<TcpClient, ClientInfo> _connectedClients = new Dictionary<TcpClient, ClientInfo>();
        private readonly object _lock = new object(); // Lock object riêng
        internal Form MainForm;
      

        public Server ServerInstance { get; set; }

        // Class để lưu thông tin client (ví dụ)
        public class ClientInfo
        {
            public int? IDTaiKhoan { get; set; }
            public int? IDMay { get; set; }
            public string TenDangNhap { get; set; }
            // Thêm các thông tin khác nếu cần
        }


        private ServerManager() { }

        public static ServerManager Instance => _instance;

        public void AddClient(TcpClient client)
        {
            lock (_lock)
            {
                if (!_connectedClients.ContainsKey(client))
                {
                    _connectedClients.Add(client, new ClientInfo()); // Thêm client với info rỗng ban đầu
                    Logger.Log($"[MANAGER] Client added. Total: {_connectedClients.Count}");
                }
            }
        }

        public void RegisterClient(TcpClient client, int idTaiKhoan, int idMay)
        {
            string tenDangNhap = GetTenNguoiDungFromDatabase(idTaiKhoan); // Lấy tên từ DB

            var info = new ClientInfo
            {
                IDTaiKhoan = idTaiKhoan,
                IDMay = idMay,
                TenDangNhap = tenDangNhap
            };

            lock (_lock)
            {
                _connectedClients[client] = info;
            }

            Logger.Log($"[SERVER MANAGER] Registered client: {tenDangNhap} (ID: {idTaiKhoan}) - Máy: {idMay}");
        }
        public string GetClientName(TcpClient client)
        {
            lock (_lock)
            {
                if (_connectedClients.TryGetValue(client, out var info))
                {
                    return info.TenDangNhap ?? $"User_{info.IDTaiKhoan}";
                }
            }
            return $"Client_{client.Client?.RemoteEndPoint}";
        }
        private string GetTenNguoiDungFromDatabase(int idTaiKhoan)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=LAPTOP-5V6TA3CH\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True"))
            {
                conn.Open();
                string query = "SELECT TenNguoiDung FROM TaiKhoan WHERE Id = @id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idTaiKhoan);
                    var result = cmd.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        // TODO: Thêm hàm để liên kết thông tin khi nhận CLIENT_CONNECT
        public void AssociateClientInfo(TcpClient client, string idTaiKhoanStr, string idMayStr)
        {
            lock (_lock)
            {
                if (_connectedClients.TryGetValue(client, out ClientInfo info))
                {
                    if (int.TryParse(idTaiKhoanStr, out int idTK)) info.IDTaiKhoan = idTK;
                    if (int.TryParse(idMayStr, out int idMay)) info.IDMay = idMay;
                    // TODO: Lấy tên đăng nhập từ DB dựa vào idTK nếu cần và gán vào info.TenDangNhap
                    Logger.Log($"[MANAGER] Associated info for client: TK={info.IDTaiKhoan}, May={info.IDMay}");
                }
            }
        }

        public void InvokeUI(Action action)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(action);
            }
            else
            {
                action();
            }
        }


        public void RemoveClient(TcpClient client)
        {
            lock (_lock)
            {
                if (_connectedClients.Remove(client))
                {
                    Logger.Log($"[MANAGER] Client removed. Total: {_connectedClients.Count}");
                }
            }
            // Đóng client ở đây để đảm bảo nó được đóng ngay cả khi Handler gặp lỗi trước đó
            try { client?.Close(); } catch { /* ignore */ }
        }

        

        // Ngắt kết nối tất cả client khi server dừng
        public void DisconnectAllClients()
        {
            List<TcpClient> clientsToDisconnect;
            lock (_lock)
            {
                clientsToDisconnect = _connectedClients.Keys.ToList(); // Tạo bản sao danh sách key
                _connectedClients.Clear(); // Xóa ngay lập tức
            }

            Logger.Log($"[MANAGER] Disconnecting all {clientsToDisconnect.Count} clients...");
            foreach (var client in clientsToDisconnect)
            {
                try { client?.Close(); } catch { /* ignore */ }
            }
            Logger.Log($"[MANAGER] All clients disconnected.");
        }

        public void BroadcastAdminMessage(string messageContent)
        {
            string sender = "Server"; // Hoặc tên admin nếu có
            string formattedMessage = $"CHAT|{sender}|{messageContent}";
            
            BroadcastMessageToAllClients(formattedMessage);
            Logger.Log($"[ADMIN] Broadcasted message: {formattedMessage}");
        }

        public void BroadcastMessageToAllClients(string message, NetworkStream excludeStream = null)
        {
            if (string.IsNullOrWhiteSpace(message)) return;

            // ✅ Đảm bảo kết thúc bằng newline để client dễ phân tích
            if (!message.EndsWith("\n")) message += "\n";

            byte[] buffer = Encoding.UTF8.GetBytes(message);

            List<KeyValuePair<TcpClient, ClientInfo>> clientsSnapshot;

            // ✅ Snapshot client list để tránh giữ lock lâu
            lock (_lock)
            {
                clientsSnapshot = _connectedClients.ToList();
            }

            Logger.Log($"[MANAGER] Broadcasting '{message.Trim()}' to {clientsSnapshot.Count} clients"
                + (excludeStream != null ? " (excluding sender)." : "."));

            foreach (var kvp in clientsSnapshot)
            {
                TcpClient client = kvp.Key;

                // ✅ Kiểm tra kết nối
                if (client == null || !client.Connected) continue;

                try
                {
                    NetworkStream stream = client.GetStream();
                    if (stream == null || stream == excludeStream || !stream.CanWrite) continue;

                    // ✅ Gửi bất đồng bộ, không chờ (fire-and-forget)
                    _ = stream.WriteAsync(buffer, 0, buffer.Length);
                }
                catch (IOException ex)
                {
                    Logger.Log($"[MANAGER ERROR] IOException when broadcasting to {client.Client?.RemoteEndPoint}: {ex.Message}");
                    RemoveClientSafe(client);
                }
                catch (ObjectDisposedException)
                {
                    Logger.Log($"[MANAGER WARNING] Disposed stream when broadcasting to {client.Client?.RemoteEndPoint}. Removing.");
                    RemoveClientSafe(client);
                }
                catch (Exception ex)
                {
                    Logger.Log($"[MANAGER ERROR] Unexpected error when broadcasting to {client.Client?.RemoteEndPoint}: {ex.Message}");
                    RemoveClientSafe(client);
                }
            }
        }
        private void RemoveClientSafe(TcpClient client)
        {
            lock (_lock)
            {
                if (_connectedClients.ContainsKey(client))
                {
                    _connectedClients.Remove(client);
                    Logger.Log($"[MANAGER] Client {client.Client?.RemoteEndPoint} removed from list.");
                }
            }

            try
            {
                client?.Close();
            }
            catch { /* Ignore close errors */ }
        }

        // TODO: Thêm hàm gửi tin nhắn đến một client cụ thể (dựa vào ID tài khoản, ID máy,...)
        public async Task SendMessageToClientAsync(int targetIdTaiKhoan, string message)
        {
            TcpClient targetClient = null;
            lock (_lock)
            {
                targetClient = _connectedClients.FirstOrDefault(kvp => kvp.Value.IDTaiKhoan == targetIdTaiKhoan).Key;
            }

            if (targetClient != null && targetClient.Connected)
            {
                if (!message.EndsWith("\n")) message += "\n";
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                try
                {
                    await targetClient.GetStream().WriteAsync(buffer, 0, buffer.Length);
                    Logger.Log($"[MANAGER] Sent direct message to TK={targetIdTaiKhoan}: {message.Trim()}");
                }
                catch (Exception ex)
                {
                    Logger.Log($"[MANAGER ERROR] Failed sending direct message to TK={targetIdTaiKhoan}: {ex.Message}");
                    RemoveClient(targetClient); // Xóa nếu gửi lỗi
                }
            }
            else
            {
                Logger.Log($"[MANAGER WARNING] Could not find connected client for TK={targetIdTaiKhoan} to send direct message.");
            }
        }

    }
}