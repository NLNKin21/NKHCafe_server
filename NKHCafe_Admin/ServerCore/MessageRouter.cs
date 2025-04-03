using System;
using System.Collections.Generic;
using System.IO; // Thêm using IO
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks; // Thêm using Task
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native.WebClientUIControl;
using Newtonsoft.Json;
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
                    if (dataParts.Length >= 3)
                    {
                        string idTaiKhoanStr = dataParts[0];
                        string idMayStr = dataParts[1];
                        string amountStr = dataParts[2];

                        Logger.Log($"[ROUTER] Deposit request: TK={idTaiKhoanStr}, May={idMayStr}, Amount={amountStr}");

                        // Ghi nhận yêu cầu vào danh sách
                        var yeuCau = new YeuCauNapTien
                        {
                            IdTaiKhoan = int.Parse(idTaiKhoanStr),
                            IdMay = int.Parse(idMayStr),
                            SoTien = decimal.Parse(amountStr)
                        };

                        NapTienManager.ThemYeuCauMoi(yeuCau);

                        await SendResponseAsync(stream, "DEPOSIT_ACK", "OK", "Yêu cầu nạp tiền đã được ghi nhận.");
                    }
                    else
                    {
                        Logger.Log($"[ROUTER WARNING] Invalid REQUEST_DEPOSIT message: {message}");
                    }
                    break;

                case "HOADONDOAN":
                    if (dataParts.Length >= 1)
                    {
                        string jsonData = dataParts[0];

                        try
                        {
                            var hoaDon = JsonConvert.DeserializeObject<HoaDonDoAnRequest>(jsonData);

                            Logger.Log($"[ROUTER] Đã nhận HOADONDOAN từ TK={hoaDon.IdTaiKhoan}, May={hoaDon.IdMay}, SL Mon={hoaDon.ChiTiet.Count}");

                            foreach (var mon in hoaDon.ChiTiet)
                            {
                                Logger.Log($"[ROUTER] -> Mon: ID={mon.IDMon}, Ten={mon.TenMon}, SL={mon.SoLuong}, Gia={mon.DonGia}");
                            }

                            // ✅ Thêm vào danh sách hiển thị (hoặc lưu DB tùy ý)
                            lock (HoaDonManager.DanhSachHoaDon)
                            {
                                HoaDonManager.DanhSachHoaDon.Add(hoaDon);
                            }

                            await SendResponseAsync(stream, "ORDER_CONFIRMATION", "OK", "Đã nhận hóa đơn đồ ăn.");
                        }
                        catch (Exception ex)
                        {
                            Logger.Log($"[ROUTER ERROR] Lỗi xử lý HOADONDOAN: {ex.Message}");
                            await SendResponseAsync(stream, "ORDER_CONFIRMATION", "ERROR", "Lỗi xử lý dữ liệu.");
                        }
                    }
                    else
                    {
                        Logger.Log($"[ROUTER WARNING] HOADONDOAN thiếu dữ liệu: {message}");
                        await SendResponseAsync(stream, "ORDER_CONFIRMATION", "ERROR", "Thiếu dữ liệu HOADONDOAN.");
                    }
                    break;


                case "GET_HOADON_LIST":
                    Logger.Log($"[ROUTER] Định tuyến lệnh: GET_HOADON_LIST (Chế độ nhận danh sách).");
                    // Giả định client gửi: "GET_HOADON_LIST|[{json_obj1}, {json_obj2}, ...]"
                    // dataParts[0] sẽ là chuỗi JSON chứa một MẢNG các đối tượng HoaDonDoAnRequest.
                    if (dataParts.Length >= 1)
                    {
                        string jsonData = dataParts[0];
                        try
                        {
                            // 1. Deserialize JSON nhận được thành List<HoaDonDoAnRequest>
                            List<HoaDonDoAnRequest> danhSachHoaDonTuClient = JsonConvert.DeserializeObject<List<HoaDonDoAnRequest>>(jsonData);

                            if (danhSachHoaDonTuClient != null)
                            {
                                // 2. Cập nhật vào HoaDonManager của server một cách an toàn luồng
                                lock (HoaDonManager.DanhSachHoaDon)
                                {
                                    // Xóa danh sách cũ
                                    HoaDonManager.DanhSachHoaDon.Clear();
                                    // Thêm tất cả các hóa đơn từ client vào danh sách mới (đã rỗng)
                                    HoaDonManager.DanhSachHoaDon.AddRange(danhSachHoaDonTuClient);
                                }

                                int count = danhSachHoaDonTuClient.Count;
                                Logger.Log($"[ROUTER] Đã cập nhật HoaDonManager bằng {count} hóa đơn từ client.");

                                // 3. Gửi phản hồi xác nhận thành công cho client
                                await SendResponseAsync(stream, "GET_HOADON_LIST_ACK", "OK", $"Đã nhận và cập nhật {count} hóa đơn.");
                            }
                            else
                            {
                                // Trường hợp JSON hợp lệ nhưng là 'null' hoặc không deserialize được thành List
                                Logger.Log("[ROUTER WARNING] Dữ liệu JSON hóa đơn nhận được không thể deserialize thành danh sách hoặc là null.");
                                await SendResponseAsync(stream, "GET_HOADON_LIST_ACK", "ERROR", "Dữ liệu hóa đơn không hợp lệ (không phải danh sách hoặc null).");
                            }
                        }
                        catch (JsonException jsonEx)
                        {
                            // Lỗi nếu JSON client gửi lên không đúng định dạng List<HoaDonDoAnRequest>
                            Logger.Log($"[ROUTER ERROR] Lỗi deserialize JSON cho GET_HOADON_LIST: {jsonEx.Message}. Data: {jsonData}");
                            await SendResponseAsync(stream, "GET_HOADON_LIST_ACK", "ERROR", "Lỗi định dạng dữ liệu JSON danh sách hóa đơn.");
                        }
                        catch (Exception ex)
                        {
                            // Các lỗi khác trong quá trình xử lý
                            Logger.Log($"[ROUTER ERROR] Lỗi xử lý GET_HOADON_LIST (nhận danh sách): {ex.Message}");
                            await SendResponseAsync(stream, "GET_HOADON_LIST_ACK", "ERROR", "Lỗi máy chủ khi xử lý cập nhật danh sách hóa đơn.");
                        }
                    }
                    else
                    {
                        // Client chỉ gửi "GET_HOADON_LIST" mà không có dữ liệu JSON kèm theo
                        Logger.Log($"[ROUTER WARNING] GET_HOADON_LIST không có dữ liệu JSON kèm theo.");
                        await SendResponseAsync(stream, "GET_HOADON_LIST_ACK", "ERROR", "Thiếu dữ liệu JSON cho danh sách hóa đơn.");
                    }
                    break;

                // Nếu bạn VẪN cần chức năng Admin LẤY danh sách hóa đơn TỪ SERVER, hãy dùng lệnh mới
                case "REQUEST_SERVER_HOADON_LIST": // Lệnh mới ví dụ
                    Logger.Log($"[ROUTER] Định tuyến lệnh: REQUEST_SERVER_HOADON_LIST.");
                    try
                    {
                        string hoaDonJson;
                        int count;
                        // Đọc danh sách một cách an toàn luồng
                        lock (HoaDonManager.DanhSachHoaDon)
                        {
                            // Serialize danh sách hiện tại trên server (List<HoaDonDoAnRequest>)
                            hoaDonJson = JsonConvert.SerializeObject(HoaDonManager.DanhSachHoaDon);
                            count = HoaDonManager.DanhSachHoaDon.Count;
                        }
                        // Gửi dữ liệu dạng COMMAND|DATA (hoặc theo protocol của bạn)
                        string responseData = $"SERVER_HOADON_LIST_DATA|{hoaDonJson}"; // Tên lệnh phản hồi mới
                        byte[] dataToSend = Encoding.UTF8.GetBytes(responseData);
                        await stream.WriteAsync(dataToSend, 0, dataToSend.Length);

                        Logger.Log($"[ROUTER] Đã gửi danh sách {count} hóa đơn (từ server) cho admin.");
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"[ROUTER ERROR] Lỗi gửi danh sách hóa đơn từ server: {ex.Message}");
                        // Gửi thông báo lỗi
                        await SendResponseAsync(stream, "SERVER_HOADON_LIST_ERROR", "ERROR", "Không thể gửi danh sách hóa đơn từ server.");
                    }
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