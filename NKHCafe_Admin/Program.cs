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
        private static Server cafeServer; // Đối tượng Server
        private static CancellationTokenSource serverCts; // Token để dừng Server
        private static Task serverTask; // Giữ task để theo dõi trạng thái

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Khởi tạo Logger
            Logger.Initialize();
            Logger.Log("Application starting...");

            // --- Khởi động Server ---
            StartServer();

            // Đăng ký sự kiện ApplicationExit để dừng Server khi ứng dụng đóng
            Application.ApplicationExit += Application_ApplicationExit;

            // Chạy form đăng nhập
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

        private static void StartServer()
        {
            cafeServer = new Server();
            serverCts = new CancellationTokenSource();
            string ip = Config.ServerIP;
            int port = Config.ServerPort;

            Logger.Log($"Attempting to start server on {ip}:{port}...");

            serverTask = Task.Run(async () =>
            {
                try
                {
                    await cafeServer.StartAsync(ip, port, serverCts.Token);
                }
                catch (Exception ex)
                {
                    Logger.Log($"[ERROR] Server failed to start: {ex.Message}");
                    throw; // Để IsFaulted bắt được
                }
            });

            // Chờ 500ms để xem có lỗi socket ngay lúc đầu không
            Task.Delay(500).Wait();

            if (serverTask.IsFaulted)
            {
                Logger.Log("[FATAL] Server failed to start properly. Check logs for detail.");
                MessageBox.Show($"Không thể khởi động Server trên cổng {port}.\nCổng có thể đang được sử dụng hoặc lỗi cấu hình.\nVui lòng kiểm tra log và thử lại.",
                                "Lỗi Khởi Động Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Nếu muốn, có thể return hoặc thoát hẳn
                // Environment.Exit(1);
            }
            else
            {
                Logger.Log("Server is starting in background...");
            }
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            Logger.Log("Application exiting, stopping server...");
            StopServer();
        }

        private static void StopServer()
        {
            try
            {
                serverCts?.Cancel();
                cafeServer?.Stop();

                // Đợi serverTask hoàn tất dọn dẹp nếu cần
                if (serverTask != null && !serverTask.IsCompleted)
                {
                    if (!serverTask.Wait(2000)) // Đợi tối đa 2 giây
                    {
                        Logger.Log("Warning: Server task did not stop gracefully in time.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log("[ERROR] Error when stopping server: " + ex.Message);
            }
        }
    }
}