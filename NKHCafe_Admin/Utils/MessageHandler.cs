using System.Globalization; // Cần cho CultureInfo nếu bạn dùng hàm liên quan đến số thập phân
using System.Linq;          // Cần cho Skip, ToArray
using System.Text;          // Cần cho Encoding

// Đảm bảo namespace này khớp với cấu trúc thư mục của bạn
namespace NKHCafe_Admin.Network // Hoặc NKHCafe_Admin.Utils nếu bạn chọn thư mục Utils
{
    /// <summary>
    /// Cung cấp các phương thức tĩnh để tạo và phân tích các chuỗi message phía Server.
    /// </summary>
    public static class MessageHandler
    {
        /// <summary>
        /// Ký tự dùng để phân tách các phần trong message. Phải giống với Client!
        /// </summary>
        public const char DELIMITER = '|';

        /// <summary>
        /// Phân tích message nhận được từ Client.
        /// </summary>
        /// <param name="request">Chuỗi message từ Client.</param>
        /// <param name="command">OUT: Lệnh chính (đã chuẩn hóa chữ hoa).</param>
        /// <param name="dataParts">OUT: Mảng chứa các phần dữ liệu đi kèm.</param>
        /// <returns>True nếu phân tích thành công, False nếu message không hợp lệ.</returns>
        public static bool ParseClientRequest(string request, out string command, out string[] dataParts)
        {
            command = null;
            dataParts = System.Array.Empty<string>(); // Khởi tạo mảng rỗng

            if (string.IsNullOrWhiteSpace(request))
            {
                // Không cần log ở đây vì nơi gọi sẽ log lỗi nếu cần
                return false;
            }

            string[] parts = request.Split(DELIMITER);

            if (parts.Length == 0 || string.IsNullOrWhiteSpace(parts[0]))
            {
                return false; // Không có command hợp lệ
            }

            command = parts[0].Trim().ToUpperInvariant(); // Lấy command và chuẩn hóa
            dataParts = parts.Skip(1).ToArray();         // Lấy phần dữ liệu còn lại

            return true; // Phân tích thành công
        }

        /// <summary>
        /// Tạo chuỗi message phản hồi chuẩn cho Client.
        /// </summary>
        /// <param name="messageType">Loại message (ví dụ: BALANCE_UPDATE, ORDER_CONFIRMATION).</param>
        /// <param name="status">Trạng thái (ví dụ: OK, ERROR).</param>
        /// <param name="message">Nội dung thông điệp chi tiết.</param>
        /// <returns>Chuỗi message đã được định dạng.</returns>
        public static string CreateResponseMessage(string messageType, string status, string message)
        {
            // Đảm bảo các phần không chứa dấu phân cách hoặc xử lý chúng nếu cần
            // Ví dụ đơn giản:
            messageType = messageType?.Replace(DELIMITER, ' ') ?? "RESPONSE";
            status = status?.Replace(DELIMITER, ' ') ?? "INFO";
            message = message?.Replace(DELIMITER, ' ') ?? "";

            return $"{messageType}{DELIMITER}{status}{DELIMITER}{message}";
        }

        /// <summary>
        /// Tạo message cập nhật số dư gửi cho Client.
        /// </summary>
        /// <param name="newBalance">Số dư mới.</param>
        /// <returns>Chuỗi message BALANCE_UPDATE.</returns>
        public static string CreateBalanceUpdateMessage(decimal newBalance)
        {
            // Dùng InvariantCulture để đảm bảo dấu '.' thập phân
            return $"BALANCE_UPDATE{DELIMITER}{newBalance.ToString(CultureInfo.InvariantCulture)}";
        }

        /// <summary>
        /// Tạo message xác nhận đặt món gửi cho Client.
        /// </summary>
        /// <param name="confirmationText">Nội dung xác nhận.</param>
        /// <returns>Chuỗi message ORDER_CONFIRMATION.</returns>
        public static string CreateOrderConfirmationMessage(string confirmationText)
        {
            return CreateResponseMessage("ORDER_CONFIRMATION", "OK", confirmationText);
            // Hoặc format đơn giản hơn nếu client chỉ cần nội dung:
            // return $"ORDER_CONFIRMATION{DELIMITER}{confirmationText?.Replace(DELIMITER, ' ') ?? ""}";
        }

        /// <summary>
        /// Tạo message chat để broadcast (thường có người gửi).
        /// </summary>
        /// <param name="sender">Tên người gửi.</param>
        /// <param name="content">Nội dung chat.</param>
        /// <returns>Chuỗi message CHAT.</returns>
        public static string CreateChatMessageForBroadcast(string sender, string content)
        {
            sender = sender?.Replace(DELIMITER, ' ') ?? "Unknown";
            content = content?.Replace(DELIMITER, ' ') ?? "";
            return $"CHAT{DELIMITER}{sender}{DELIMITER}{content}";
        }

        /// <summary>
        /// Tạo message yêu cầu Client đăng xuất.
        /// </summary>
        /// <param name="reason">Lý do đăng xuất.</param>
        /// <returns>Chuỗi message FORCE_LOGOUT.</returns>
        public static string CreateForceLogoutMessage(string reason = "Bạn đã bị đăng xuất.")
        {
            return $"FORCE_LOGOUT{DELIMITER}{reason?.Replace(DELIMITER, ' ') ?? ""}";
        }

        /// <summary>
        /// Tạo message thông báo chung từ Server gửi cho Client.
        /// </summary>
        /// <param name="message">Nội dung thông báo.</param>
        /// <returns>Chuỗi message SERVER_MESSAGE.</returns>
        public static string CreateServerMessage(string message)
        {
            return $"SERVER_MESSAGE{DELIMITER}{message?.Replace(DELIMITER, ' ') ?? ""}";
        }

        // --- Có thể thêm các hàm tạo message cụ thể khác nếu Server cần ---

    }
}