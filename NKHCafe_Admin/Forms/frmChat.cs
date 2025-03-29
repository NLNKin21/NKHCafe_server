using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NKHCafe_Admin.ServerCore;
using NKHCafe_Admin.Utils;

namespace NKHCafe_Admin.Forms
{
    public partial class frmChat : System.Windows.Forms.Form
    {
        private Server _server;
        private const int ServerPort = 12345; // Default server port
        private const string DefaultServerIP = "127.0.0.1"; // Or use IPAddress.Any.ToString() for all IPs

        public frmChat()
        {
            InitializeComponent();
            _server = new Server();
        }

        private void frmChatServer_Load(object sender, EventArgs e)
        {
            txtServerPort.Text = ServerPort.ToString();
            lblStatus.Text = "Trạng thái: Chưa khởi động";
            btnStartServer.Text = "Khởi động Server";
            txtServerIP.Text = DefaultServerIP; // Set default IP
        }

        private async void btnStartServer_Click(object sender, EventArgs e) // Đánh dấu btnStartServer_Click là async
        {
            if (_server == null || !_server.GetType().GetField("_isRunning", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_server).Equals(true))
            {
                await Task.Run(() => StartServer()); // Chạy StartServer trên luồng nền bằng Task.Run
            }
            else
            {
                StopServer();
            }
        }

        private void StartServer()
        {
            string serverIP = txtServerIP.Text.Trim();
            int serverPort;
            if (!int.TryParse(txtServerPort.Text, out serverPort))
            {
                MessageBox.Show("Cổng không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AppendToChatLog("Khởi động server...");
            try
            {
                _server.Start(serverIP, serverPort);
                UpdateServerStatus(true);
                Debug.WriteLine("[SERVER UI] UpdateServerStatus(true) đã được gọi."); // **THÊM DÒNG DEBUG NÀY**
                AppendToChatLog($"Server đã khởi động trên {serverIP}:{serverPort}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi động server: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendToChatLog($"Lỗi khởi động server: {ex.Message}");
                StopServer(); // Đảm bảo server dừng nếu có lỗi
            }
        }

        private void StopServer()
        {
            AppendToChatLog("Dừng server...");
            _server.Stop();
            UpdateServerStatus(false);
            AppendToChatLog("Server đã dừng.");
        }

        private void UpdateServerStatus(bool isRunning)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<bool>(UpdateServerStatus), isRunning);
                return;
            }
            try // **THÊM try-catch VÀO ĐÂY**
            {
                // **ĐOẠN CODE CẬP NHẬT UI**
                if (isRunning)
                {
                    btnStartServer.Text = "Dừng Server";
                    lblStatus.Text = "Trạng thái: Đang chạy";
                    txtServerPort.Enabled = false;
                    txtServerIP.Enabled = false;
                }
                else
                {
                    btnStartServer.Text = "Khởi động Server";
                    lblStatus.Text = "Trạng thái: Chưa khởi động";
                    txtServerPort.Enabled = true;
                    txtServerIP.Enabled = true;
                }
            }
            catch (Exception ex) // **BẮT EXCEPTION VÀ LOG**
            {
                Debug.WriteLine($"[SERVER UI] Lỗi trong UpdateServerStatus: {ex}");
            }
        }


        private delegate void AppendToChatLogDelegate(string text);
        private void AppendToChatLog(string text)
        {
            if (rtbChatLog.InvokeRequired)
            {
                if (!rtbChatLog.IsDisposed)
                {
                    rtbChatLog.Invoke(new AppendToChatLogDelegate(AppendToChatLog), text);
                }
            }
            else
            {
                if (!rtbChatLog.IsDisposed)
                {
                    rtbChatLog.AppendText(text + Environment.NewLine);
                    rtbChatLog.ScrollToCaret();
                }
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMessageToAll();
        }

        private void SendMessageToAll()
        {
            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            if (_server != null && _server.GetType().GetField("_isRunning", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(_server).Equals(true)) // Check if server is running
            {
                _server.BroadcastMessage($"Server: {message}"); // Send message to all clients
                AppendToChatLog($"Server (You): {message}"); // Display server's own message
                txtMessage.Clear();
            }
            else
            {
                MessageBox.Show("Server chưa khởi động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void frmChatServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopServer(); // Stop server when form closes
        }

        
    }
}