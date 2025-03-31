using System;
using System.IO; // Thêm using IO
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks; // Thêm using Task
using NKHCafe_Admin.Utils; // Assuming Logger is here

namespace NKHCafe_Admin.ServerCore
{
    public class ClientHandler
    {
        private readonly TcpClient _client;
        private readonly NetworkStream _stream;
        private readonly CancellationToken _cancellationToken; // Thêm token để dừng sớm

        public ClientHandler(TcpClient client, CancellationToken token = default) // Nhận token từ Server
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _stream = _client.GetStream();
            _cancellationToken = token;
        }

        // Đổi tên thành HandleClientAsync và dùng async/await
        public async Task HandleClientAsync()
        {
            byte[] buffer = new byte[4096]; // Buffer lớn hơn một chút
            string clientEndPoint = _client.Client?.RemoteEndPoint?.ToString() ?? "Unknown Client"; // Lấy thông tin client an toàn

            Console.WriteLine($"[HANDLER] Started handling client: {clientEndPoint}");
            Logger.Log($"[HANDLER] Started handling client: {clientEndPoint}");

            try
            {
                // Vòng lặp đọc dữ liệu bất đồng bộ
                while (_client.Connected && !_cancellationToken.IsCancellationRequested)
                {
                    int bytesRead = 0;
                    try
                    {
                        // Đọc bất đồng bộ với CancellationToken
                        bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, _cancellationToken).ConfigureAwait(false);
                    }
                    catch (IOException ex) when (ex.InnerException is SocketException se && (se.SocketErrorCode == SocketError.ConnectionReset || se.SocketErrorCode == SocketError.ConnectionAborted))
                    {
                        Console.WriteLine($"[HANDLER] IOException (Connection lost) for {clientEndPoint}: {se.Message}");
                        Logger.Log($"[HANDLER] IOException (Connection lost) for {clientEndPoint}: {se.Message}");
                        break; // Thoát vòng lặp khi mất kết nối
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"[HANDLER] Read operation cancelled for {clientEndPoint}.");
                        Logger.Log($"[HANDLER] Read operation cancelled for {clientEndPoint}.");
                        break; // Thoát khi token bị hủy
                    }


                    if (bytesRead == 0)
                    {
                        // Client đóng kết nối bình thường
                        Console.WriteLine($"[HANDLER] Client {clientEndPoint} disconnected gracefully (0 bytes read).");
                        Logger.Log($"[HANDLER] Client {clientEndPoint} disconnected gracefully.");
                        break;
                    }

                    // Xử lý dữ liệu nhận được
                    string rawMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Logger.Log($"[HANDLER] Raw from {clientEndPoint}: {rawMessage.Trim()}"); // Log raw message đã trim

                    // --- Xử lý message bị tách/nối ---
                    // Cần cơ chế tách message dựa trên delimiter (ví dụ '\n')
                    // Giả sử client gửi message kết thúc bằng '\n'
                    // TODO: Implement proper message framing handler if needed
                    string[] messages = rawMessage.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries); // Tách theo '\n'

                    foreach (string msg in messages)
                    {
                        if (string.IsNullOrWhiteSpace(msg)) continue; // Bỏ qua message rỗng
                        string trimmedMsg = msg.Trim();
                        Logger.Log($"[HANDLER] Processing from {clientEndPoint}: {trimmedMsg}");
                        // Gọi MessageRouter để xử lý từng message đã tách
                        // Truyền _stream để MessageRouter có thể gửi phản hồi
                        await MessageRouter.ProcessMessageAsync(trimmedMsg, _stream, _client).ConfigureAwait(false);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                Console.WriteLine($"[HANDLER] Stream/Client disposed for {clientEndPoint}.");
                Logger.Log($"[HANDLER] Stream/Client disposed for {clientEndPoint}.");
            }
            catch (Exception ex) when (!(ex is OperationCanceledException || ex is IOException)) // Bỏ qua các lỗi đã xử lý
            {
                Console.WriteLine($"[HANDLER ERROR] Unexpected error for {clientEndPoint}: {ex.GetType().Name} - {ex.Message}");
                Logger.Log($"[HANDLER ERROR] Unexpected error for {clientEndPoint}: {ex.Message}");
            }
            finally
            {
                // --- Dọn dẹp ---
                ServerManager.Instance.RemoveClient(_client); // Luôn xóa client khỏi danh sách
                try { _client?.Close(); } catch { /* Ignore */ } // Đóng client an toàn
                Console.WriteLine($"[HANDLER] Finished handling client: {clientEndPoint}");
                Logger.Log($"[HANDLER] Finished handling client: {clientEndPoint}");
            }
        }
    }
}