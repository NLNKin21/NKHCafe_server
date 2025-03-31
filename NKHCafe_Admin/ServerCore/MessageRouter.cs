using System;
using System.IO; // Thêm using IO
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks; // Thêm using Task
using System.Windows.Forms;
using NKHCafe_Admin.DAO;
using NKHCafe_Admin.Forms;
using NKHCafe_Admin.Models;
using NKHCafe_Admin.Network;
using NKHCafe_Admin.Utils; // Assuming Logger is here


namespace NKHCafe_Admin.ServerCore
{
    public static class MessageRouter
    {
        // Đổi thành ProcessMessageAsync
        public static async Task ProcessMessageAsync(string message, NetworkStream stream, TcpClient client)
        {
            // Phân tích message chuẩn hóa hơn (dùng MessageHandler nếu có hoặc tương tự)
            // Tạm dùng Split để minh họa
            string[] parts = message.Split('|');
            if (parts.Length == 0 || string.IsNullOrEmpty(parts[0]))
            {
                Logger.Log($"[ROUTER] Invalid message format: {message}");
                await SendResponseAsync(stream, "INVALID_FORMAT", "ERROR", "Invalid message format.");
                return;
            }

            string command = parts[0].Trim().ToUpperInvariant(); // Lấy command, chuẩn hóa
            string[] dataParts = parts.Skip(1).ToArray();     // Lấy dữ liệu

            Logger.Log($"[ROUTER] Routing command: {command} with {dataParts.Length} data parts.");

            // --- Xử lý các lệnh ---
            switch (command)
            {
                case "CLIENT_CONNECT":
                    if (dataParts.Length >= 2 &&
                        int.TryParse(dataParts[0], out int idTaiKhoan) &&
                        int.TryParse(dataParts[1], out int idMay))
                    {
                        Logger.Log($"[ROUTER] Client {client.Client.RemoteEndPoint} identified as ID: {idTaiKhoan}, Máy: {idMay}");

                        try
                        {
                            // ✅ Gọi DAO mở máy (cập nhật trạng thái hoạt động)
                            bool capNhatThanhCong = MayTramDAO.MoMay(idTaiKhoan, idMay);

                            if (capNhatThanhCong)
                            {
                                Logger.Log($"[ROUTER] MoMay thành công cho máy {idMay}");

                                // ✅ Lưu thông tin client vào ServerManager để sử dụng sau (ví dụ trong CHAT)
                                ServerManager.Instance.RegisterClient(client, idTaiKhoan, idMay);

                                // ✅ Gọi lại LoadMayTram() trên UI thread nếu cần cập nhật giao diện
                                ServerManager.Instance.InvokeUI(() =>
                                {
                                    if (ServerManager.Instance.MainForm is frmMain mainForm)
                                    {
                                        mainForm.LoadMayTram();
                                        Logger.Log("[ROUTER] LoadMayTram đã được gọi sau CLIENT_CONNECT.");
                                    }
                                });

                                await SendResponseAsync(stream, "CLIENT_CONNECT_ACK", "OK", "Đăng nhập thành công.");
                            }
                            else
                            {
                                Logger.Log($"[ROUTER] MoMay thất bại hoặc máy đã được sử dụng.");
                                await SendResponseAsync(stream, "CLIENT_CONNECT_ACK", "ERROR", "Không thể mở máy. Có thể máy đang được sử dụng.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"[ROUTER ERROR] Lỗi khi xử lý CLIENT_CONNECT: {ex.Message}");
                            await SendResponseAsync(stream, "CLIENT_CONNECT_ACK", "ERROR", "Lỗi hệ thống khi mở máy.");
                        }
                    }
                    else
                    {
                        Logger.Log($"[ROUTER WARNING] CLIENT_CONNECT thiếu hoặc sai tham số: {message}");
                        await SendResponseAsync(stream, "CLIENT_CONNECT_ACK", "ERROR", "Thiếu hoặc sai định dạng tham số.");
                    }
                    break;

                case "CHAT":
                    if (dataParts.Length > 0)
                    {
                        string chatContent = dataParts[0]; // Nội dung chat

                        // ✅ Lấy tên người gửi từ ServerManager (nếu đã lưu qua CLIENT_CONNECT)
                        string senderInfo = ServerManager.Instance.GetClientName(client);
                        if (string.IsNullOrEmpty(senderInfo))
                        {
                            senderInfo = $"Client_{client.Client.RemoteEndPoint}";
                        }

                        Logger.Log($"[ROUTER] Chat message from {senderInfo}: {chatContent}");

                        // ✅ Format đúng: CHAT|Sender|Message
                        string broadcastMessage = $"CHAT|{senderInfo}|{chatContent}";
                        
                        // Gửi đến tất cả client khác (trừ người gửi)
                        ServerManager.Instance.BroadcastMessageToAllClients(broadcastMessage, stream);
                        AppendToChatLogServer(broadcastMessage);
                    }
                    else
                    {
                        Logger.Log($"[ROUTER WARNING] Received empty CHAT message: {message}");
                        await SendResponseAsync(stream, "CHAT_ACK", "ERROR", "Nội dung chat rỗng.");
                    }
                    break;

                case "REQUEST_DEPOSIT":
                    // Ví dụ: REQUEST_DEPOSIT|idTaiKhoan|idMay|amount
                    if (dataParts.Length >= 3)
                    {
                        string idTaiKhoanStr = dataParts[0];
                        string idMayStr = dataParts[1];
                        string amountStr = dataParts[2];
                        Logger.Log($"[ROUTER] Deposit request: TK={idTaiKhoanStr}, May={idMayStr}, Amount={amountStr}");
                        // TODO:
                        // 1. Ghi nhận yêu cầu này vào DB hoặc danh sách chờ duyệt.
                        // 2. Thông báo cho giao diện Admin (qua event hoặc callback) để hiển thị yêu cầu.
                        // 3. KHÔNG cập nhật số dư ngay.
                        // 4. Có thể gửi phản hồi tạm thời cho client.
                        await SendResponseAsync(stream, "DEPOSIT_ACK", "OK", "Yêu cầu nạp tiền đã được ghi nhận.");
                    }
                    else { Logger.Log($"[ROUTER WARNING] Invalid REQUEST_DEPOSIT message: {message}"); }
                    break;

                case "ORDER_REQUEST":
                    // Ví dụ: ORDER_REQUEST|idTaiKhoan|idMay|idMon|soLuong
                    if (dataParts.Length >= 4)
                    {
                        string idTaiKhoanStr = dataParts[0];
                        string idMayStr = dataParts[1];
                        string idMonStr = dataParts[2];
                        string soLuongStr = dataParts[3];
                        Logger.Log($"[ROUTER] Order request: TK={idTaiKhoanStr}, May={idMayStr}, Mon={idMonStr}, SL={soLuongStr}");
                        // TODO:
                        // 1. Validate thông tin (parse int, check tồn tại món,...)
                        // 2. Tìm/Tạo HoaDon tương ứng trong DB (Trạng thái 'DangCho').
                        // 3. Thêm ChiTietHoaDon vào DB.
                        // 4. Tính toán lại tổng tiền HoaDon (nếu cần).
                        // 5. Gửi thông báo cho Admin (ghi vào DB ThongBao hoặc cách khác).
                        // 6. Gửi phản hồi xác nhận cho Client.
                        await SendResponseAsync(stream, "ORDER_CONFIRMATION", "OK", $"Đã nhận yêu cầu đặt món ID {idMonStr}.");
                    }
                    else { Logger.Log($"[ROUTER WARNING] Invalid ORDER_REQUEST message: {message}"); }
                    break;

                case "LOGOUT_REQUEST":
                    // Ví dụ: LOGOUT_REQUEST|idTaiKhoan|idMay
                    if (dataParts.Length >= 2)
                    {
                        Logger.Log($"[ROUTER] Logout request received from TK={dataParts[0]}, May={dataParts[1]}. Client will be disconnected.");
                        // Không cần làm gì nhiều ở đây vì ClientHandler sẽ tự đóng kết nối khi client ngắt.
                        // ServerManager đã có remove client trong finally của ClientHandler.
                    }
                    break;

                // Thêm các case khác...

                default:
                    Logger.Log($"[ROUTER] Unknown command: {command} from {client.Client.RemoteEndPoint}");
                    await SendResponseAsync(stream, "UNKNOWN_COMMAND", "ERROR", $"Lệnh không xác định: {command}");
                    break;
            }
        }
        public static void AppendToChatLogServer(string text)
        {
            var chatForm = FormManager.ChatFormInstance;

            if (chatForm == null)
            {
                Logger.Log("❌ ChatFormInstance is NULL");
                return;
            }

            if (chatForm.IsDisposed)
            {
                Logger.Log("❌ ChatFormInstance is DISPOSED");
                return;
            }

            Logger.Log("✅ AppendToChatLogServer is writing: " + text);

            chatForm.AppendToChatLog($"[Server] {text}");
        }

        // Đổi thành SendResponseAsync
        private static async Task SendResponseAsync(NetworkStream stream, string messageType, string status, string message)
        {
            // Luôn kiểm tra stream trước khi ghi
            if (stream == null || !stream.CanWrite)
            {
                Logger.Log($"[ROUTER WARNING] Cannot send response '{messageType}': Stream is not writable.");
                return;
            }
            try
            {
                // Thêm ký tự kết thúc dòng '\n' để client dễ dàng đọc
                string response = $"{messageType}{MessageHandler.DELIMITER}{status}{MessageHandler.DELIMITER}{message}\n";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                // Gửi bất đồng bộ
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length).ConfigureAwait(false);
                Logger.Log($"[ROUTER] Sent response: {response.Trim()}");
            }
            catch (IOException ex)
            {
                Logger.Log($"[ROUTER ERROR] IOException sending response '{messageType}': {ex.Message}");
                // Lỗi IO khi gửi thường là mất kết nối, không cần làm gì thêm ở đây
            }
            catch (ObjectDisposedException)
            {
                Logger.Log($"[ROUTER WARNING] Stream disposed while trying to send response '{messageType}'.");
            }
            catch (Exception ex)
            {
                Logger.Log($"[ROUTER ERROR] Unexpected error sending response '{messageType}': {ex.Message}");
            }
        }
    }
}