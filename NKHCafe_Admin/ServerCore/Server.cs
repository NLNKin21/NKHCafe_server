using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks; // Thêm using Task
using NKHCafe_Admin.Utils; // Assuming Logger is here

namespace NKHCafe_Admin.ServerCore
{
    public class Server
    {
        private TcpListener _listener;
        private CancellationTokenSource _cancellationTokenSource; // Để dừng lắng nghe async
        private bool _isRunning = false;

       

        // Nên dùng Task để Start không block thread gọi nó
        public async Task StartAsync(string ip, int port, CancellationToken cancellationToken = default)
        {
            // Dùng IPAddress.Any để linh hoạt hơn
            IPAddress ipAddress = IPAddress.Any; // Lắng nghe trên tất cả IP
            // Nếu bắt buộc chỉ dùng IP cụ thể:
            // if (!IPAddress.TryParse(ip, out ipAddress)) {
            //     ipAddress = IPAddress.Any; // Fallback
            //     Console.WriteLine($"[SERVER WARNING] Invalid IP '{ip}', falling back to IPAddress.Any.");
            // }

            _listener = new TcpListener(ipAddress, port);
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken); // Liên kết token ngoài nếu có

            try
            {
                _listener = new TcpListener(IPAddress.Any, 8888);

                // ✅ Cho phép tái sử dụng địa chỉ để tránh lỗi khi khởi động lại
                _listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                
                _listener.Start();
                _isRunning = true;
                ServerManager.Instance.ServerInstance = this;
                Console.WriteLine($"[SERVER] Đang lắng nghe tại {ipAddress}:{port}");
                Logger.Log($"[SERVER] Started listening on {ipAddress}:{port}");

                // Vòng lặp chấp nhận client bất đồng bộ
                while (_isRunning && !_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    try
                    {
                        // Chấp nhận client bất đồng bộ
                        TcpClient client = await _listener.AcceptTcpClientAsync().ConfigureAwait(false);
                        // ConfigureAwait(false) để tránh quay lại context ban đầu nếu không cần thiết

                        Console.WriteLine($"[SERVER] Client connected: {client.Client.RemoteEndPoint}");
                        Logger.Log($"[SERVER] Client connected: {client.Client.RemoteEndPoint}");

                        ServerManager.Instance.AddClient(client);

                        // Khởi chạy xử lý client trên một Task khác (hiệu quả hơn Thread)
                        ClientHandler handler = new ClientHandler(client);
                        // Không cần await ở đây, để vòng lặp chấp nhận tiếp tục
                        _ = Task.Run(handler.HandleClientAsync, _cancellationTokenSource.Token); // Chạy task xử lý client

                    }
                    catch (ObjectDisposedException)
                    {
                        // Listener đã bị stop, thoát vòng lặp bình thường
                        Console.WriteLine("[SERVER] Listener stopped, exiting accept loop.");
                        break;
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("[SERVER] Accept operation cancelled.");
                        break;
                    }
                    catch (Exception ex) when (!(ex is ObjectDisposedException))
                    {
                        Console.WriteLine($"[SERVER ERROR] Accepting client failed: {ex.Message}");
                        Logger.Log($"[SERVER ERROR] Accepting client failed: {ex.Message}");
                        // Cân nhắc delay ngắn trước khi thử lại
                        await Task.Delay(100, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"[SERVER FATAL] Failed to start listener on {ipAddress}:{port}. Port may be in use. Error: {ex.Message}");
                Logger.Log($"[SERVER FATAL] Failed to start listener on {ipAddress}:{port}. Error: {ex.Message}");
                _isRunning = false; // Đảm bảo trạng thái đúng
            }
            finally
            {
                StopInternal(); // Dọn dẹp khi vòng lặp kết thúc
            }
            Console.WriteLine("[SERVER] Listener loop finished.");
            Logger.Log("[SERVER] Listener loop finished.");
        }

        public void Stop()
        {
            Console.WriteLine("[SERVER] Stop requested.");
            StopInternal();
        }

        private void StopInternal()
        {
            if (!_isRunning) return; // Đã dừng rồi

            _isRunning = false;
            try
            {
                _cancellationTokenSource?.Cancel();

                if (_listener != null)
                {
                    _listener.Stop();
                    _listener.Server.Close(); // Đảm bảo socket được đóng
                    _listener = null;
                    Console.WriteLine("[SERVER] Listener stopped.");
                    Logger.Log("[SERVER] Listener stopped.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SERVER WARNING] Exception during StopInternal: {ex.Message}");
                Logger.Log($"[SERVER WARNING] Exception during StopInternal: {ex.Message}");
            }
            finally
            {
                ServerManager.Instance.DisconnectAllClients(); // Ngắt kết nối và xóa client
                ServerManager.Instance.ServerInstance = null;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _listener = null; // Giải phóng listener
            }
        }

        // Broadcast không đổi nhiều, chỉ đảm bảo ServerManager làm đúng
        public void BroadcastMessage(string message, NetworkStream excludeStream = null)
        {
            ServerManager.Instance.BroadcastMessageToAllClients(message, excludeStream);
        }
    }
}