using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Windows.Forms;
using System.Threading; // Thêm using
using System.Threading.Tasks; // Thêm using
using NKHCafe_Admin.ServerCore; // Namespace của lớp Server
using NKHCafe_Admin.Utils;     // Namespace của Config, Logger
using NKHCafe_Admin.Models;
namespace NKHCafe_Admin.Forms
{
    internal static class Program
    {
        private static Server cafeServer; // Biến lưu đối tượng Server
        private static CancellationTokenSource serverCts; // Token để dừng Server

        [STAThread]
        static void Main() // Không cần async Task ở đây vì Application.Run sẽ giữ thread chính
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Khởi tạo Logger
            Logger.Initialize();
            Logger.Log("Application starting...");
            
            // --- Khởi động Server trên một Task nền ---
            cafeServer = new Server();
            serverCts = new CancellationTokenSource();
            string ip = Config.ServerIP; // Lấy từ Config (mặc định 127.0.0.1)
            int port = Config.ServerPort; // Lấy từ Config (mặc định 8888)

            Logger.Log($"Attempting to start server on {ip}:{port}...");
            // Chạy StartAsync trên một luồng khác để không block UI thread
            Task serverTask = Task.Run(() => cafeServer.StartAsync(ip, port, serverCts.Token));

            // Kiểm tra nhanh xem server có lỗi khởi động nghiêm trọng không (ví dụ: port bị chiếm)
            // Chờ một chút để StartAsync có thời gian chạy và bắt lỗi SocketException
            Task.Delay(500).Wait(); // Chờ 0.5 giây (điều chỉnh nếu cần)
            // Nếu serverTask đã hoàn thành (do lỗi nghiêm trọng), có thể thông báo và thoát
            if (serverTask.IsFaulted || serverTask.IsCanceled || serverTask.IsCompleted) // IsCompleted bao gồm cả Faulted và Canceled
            {
                Logger.Log("[FATAL] Server failed to start properly. Check previous logs.");
                MessageBox.Show($"Không thể khởi động Server trên cổng {port}.\nCổng có thể đang được sử dụng hoặc lỗi cấu hình.\nVui lòng kiểm tra log và thử lại.",
                                "Lỗi Khởi Động Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Có thể không chạy Application.Run nếu server là bắt buộc
                // return;
            }
            else
            {
                Logger.Log("Server starting in background...");
            }
            // -----------------------------------------

            // Đăng ký sự kiện ApplicationExit để dừng Server khi ứng dụng đóng
            Application.ApplicationExit += Application_ApplicationExit;


            // Chạy form đăng nhập (UI chính)
            try
            {
                Application.Run(new frmDangNhap());
            }
            finally
            {
                // Đảm bảo server được dừng nếu Application.Run kết thúc bất thường
                StopServer();
                Logger.Log("Application exited.");
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            // Dừng Server khi ứng dụng thoát
            Logger.Log("Application exiting, stopping server...");
            StopServer();
        }

        private static void StopServer()
        {
            // Gửi yêu cầu dừng và chờ một chút (không nên chờ quá lâu trên UI thread)
            serverCts?.Cancel();
            cafeServer?.Stop();
            // Có thể thêm Task.Wait(timeout) cho serverTask nếu cần đợi hoàn tất dọn dẹp
        }
    }
}