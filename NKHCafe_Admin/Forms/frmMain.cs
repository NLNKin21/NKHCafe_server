using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.SqlClient;
using NKHCafe_Admin.DAO;
using NKHCafe_Admin.DTO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.IO; // Thêm cho logging

namespace NKHCafe_Admin.Forms
{
    public partial class frmMain : Form
    {
        private int _idTaiKhoan;
        private string _tenDangNhap;
        private string _loaiTaiKhoan;
        private TcpListener _server;
        private bool _serverRunning;
        private const int ServerPort = 12345; // Đặt cổng cố định
        private const string ServerIP = "127.0.0.1"; // Địa chỉ IP localhost
        private readonly string _logFilePath = "server_log.txt"; // Đường dẫn tệp log


        public frmMain(int idTaiKhoan, string tenDangNhap, string loaiTaiKhoan)
        {
            InitializeComponent();
            _idTaiKhoan = idTaiKhoan;
            _tenDangNhap = tenDangNhap;
            _loaiTaiKhoan = loaiTaiKhoan;

            LoadMayTram();
            this.Text = $"Quản lý quán Net - Xin chào, {_tenDangNhap} ({_loaiTaiKhoan})";

            StartServer();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Có thể thêm các tác vụ khởi tạo ở đây (nếu cần)
        }

        private void LoadMayTram()
        {
            try
            {
                List<MayTram> danhSachMayTram = MayTramDAO.LayDanhSachMayTram();

                lvMayTram.Items.Clear();

                foreach (MayTram mayTram in danhSachMayTram)
                {
                    ListViewItem item = new ListViewItem(mayTram.IdMay.ToString());
                    item.SubItems.Add(mayTram.TenMay);
                    item.SubItems.Add(mayTram.TrangThai);
                    item.SubItems.Add(mayTram.TenTaiKhoan ?? "");

                    lvMayTram.Items.Add(item);
                }
            }
            catch (SqlException ex) // Bắt SqlException cụ thể
            {
                string errorMessage = $"Lỗi SQL khi tải danh sách máy trạm: {ex.Message}";
                Log(errorMessage);
                MessageBox.Show(errorMessage, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Lỗi khi tải danh sách máy trạm: {ex.Message}";
                Log(errorMessage);
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event Handlers
        private void menuDangNhap_Click(object sender, EventArgs e)
        {
            StopServer();
            frmDangNhap frm = new frmDangNhap();
            frm.Show();
            this.Hide();
        }

        private void menuDangXuat_Click(object sender, EventArgs e)
        {
            StopServer();
            this.Close();
            frmDangNhap frm = new frmDangNhap();
            frm.Show();
        }

        private void menuThoat_Click(object sender, EventArgs e)
        {
            StopServer();
            Application.Exit();
        }

        private void menuQuanLyTaiKhoan_Click(object sender, EventArgs e)
        {
            frmQuanLyTaiKhoan frm = new frmQuanLyTaiKhoan();
            frm.ShowDialog();
        }

        private void menuQuanLyMayTram_Click(object sender, EventArgs e)
        {
            frmQuanLyMayTram frm = new frmQuanLyMayTram();
            frm.ShowDialog();
        }

        private void menuQuanLyThucDon_Click(object sender, EventArgs e)
        {
            frmQuanLyThucDon frm = new frmQuanLyThucDon();
            frm.ShowDialog();
        }

        private void menuNapTien_Click(object sender, EventArgs e)
        {
            frmNapTien frm = new frmNapTien();
            frm.ShowDialog();
        }

        private void menuThongKe_Click(object sender, EventArgs e)
        {
            frmThongKe frm = new frmThongKe();
            frm.ShowDialog();
        }
        private void menuChat_Click(object sender, EventArgs e)
        {
            //Mở form chat
        }

        private void btnMoMay_Click(object sender, EventArgs e)
        {
            SendCommandToClient("OPEN");
        }

        private void btnTatMay_Click(object sender, EventArgs e)
        {
            SendCommandToClient("CLOSE");
        }


        private void btnTinhTien_Click(object sender, EventArgs e)
        {
            if (lvMayTram.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvMayTram.SelectedItems[0];
                int idMay = int.Parse(selectedItem.Text);

                try
                {
                    MayTram mayTram = MayTramDAO.LayThongTinMayTram(idMay);
                    if (mayTram == null || mayTram.TrangThai != "Đang sử dụng")
                    {
                        MessageBox.Show("Máy này chưa được mở hoặc đang không hoạt động.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    frmTinhTien frm = new frmTinhTien(mayTram.IdMay, mayTram.IdTaiKhoan, mayTram.ThoiGianBatDau);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        LoadMayTram();
                        // Có thể gửi thông báo đến client (nếu cần)
                    }
                }
                catch (SqlException ex)
                {
                    string errorMessage = $"Lỗi SQL khi tính tiền: {ex.Message}";
                    Log(errorMessage);
                    MessageBox.Show(errorMessage, "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"Lỗi khi tính tiền: {ex.Message}";
                    Log(errorMessage);
                    MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một máy để tính tiền.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lvMayTram_SelectedIndexChanged(object sender, EventArgs e) { } // Không làm gì


        private void SendCommandToClient(string command)
        {
            if (lvMayTram.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = lvMayTram.SelectedItems[0];
                int idMay = int.Parse(selectedItem.Text);
                SendSocketCommand(command, idMay);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một máy.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        #region Socket Server

        private void StartServer()
        {
            _serverRunning = true;
            Thread serverThread = new Thread(ServerThread);
            serverThread.Start();
        }

        private void StopServer()
        {
            _serverRunning = false;
            if (_server != null)
            {
                _server.Stop();
            }
        }

        private void ServerThread()
        {
            try
            {
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, ServerPort);
                _server = new TcpListener(ipEndPoint);
                _server.Start();

                Log($"Server started. Listening on port {ServerPort}...");

                while (_serverRunning)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    Log("Client connected.");

                    Thread clientThread = new Thread(HandleClient);
                    clientThread.Start(client);
                }
            }
            catch (SocketException ex)
            {
                string errorMessage = $"Socket exception: {ex.Message}";
                Log(errorMessage);
                MessageBox.Show(errorMessage, "Lỗi Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_server != null)
                {
                    _server.Stop();
                }
            }
        }
        private void HandleClient(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Log($"Received from client: {dataReceived}");

                    string[] parts = dataReceived.Split(':');
                    if (parts.Length == 2)
                    {
                        string command = parts[0].ToUpper(); // Chuyển về chữ hoa để so sánh không phân biệt hoa/thường
                        if (int.TryParse(parts[1], out int idMay))
                        {
                            switch (command)
                            {
                                case "OPEN":
                                    if (MayTramDAO.MoMay(_idTaiKhoan, idMay)) // Kiểm tra kết quả trả về
                                    {
                                        this.Invoke((MethodInvoker)delegate { LoadMayTram(); });
                                        SendResponse(stream, "OK");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "ERROR_OPENING"); // Phản hồi lỗi cụ thể
                                    }
                                    break;
                                case "CLOSE":
                                    if (MayTramDAO.TatMay(idMay))
                                    {
                                        this.Invoke((MethodInvoker)delegate { LoadMayTram(); });
                                        SendResponse(stream, "OK");
                                    }
                                    else
                                    {
                                        SendResponse(stream, "ERROR_CLOSING");
                                    }
                                    break;
                                default:
                                    SendResponse(stream, "INVALID_COMMAND");
                                    break;
                            }
                        }
                        else
                        {
                            SendResponse(stream, "INVALID_ID");
                        }
                    }
                    else
                    {
                        SendResponse(stream, "INVALID_FORMAT");
                    }
                }
            }
            catch (IOException ex) // Bắt IOException cụ thể cho kết nối
            {
                Log($"Client disconnected unexpectedly: {ex.Message}");
                // Có thể xử lý thêm (ví dụ: cập nhật trạng thái máy về "Không hoạt động")
            }
            catch (Exception ex)
            {
                Log($"Client handling exception: {ex.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
                Log("Client disconnected.");
            }
        }


        private void SendResponse(NetworkStream stream, string response)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(response);
                stream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Log($"Error sending response: {ex.Message}");
                // Xử lý lỗi gửi phản hồi (ví dụ: client đã ngắt kết nối)
            }
        }

        private void SendSocketCommand(string command, int idMay)
        {
            try
            {
                using (TcpClient client = new TcpClient(ServerIP, ServerPort))
                using (NetworkStream stream = client.GetStream())
                {
                    string message = $"{command}:{idMay}";
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Xử lý phản hồi
                    if (response != "OK")
                    {
                        Log($"Server returned an error for command {command}:{idMay}: {response}");
                        MessageBox.Show($"Lỗi từ server: {response}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (SocketException ex)
            {
                string errorMessage = $"Socket error sending command: {ex.Message}";
                Log(errorMessage);
                MessageBox.Show(errorMessage, "Lỗi Socket", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error sending command: {ex.Message}";
                Log(errorMessage);
                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Log(string message)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now}: {message}");
                }
            }
            catch (Exception ex)
            {
                // Không thể ghi log, có thể hiển thị thông báo lỗi (nhưng không nên làm ứng dụng dừng lại)
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }

        #endregion

        private void menuHeThong_Click(object sender, EventArgs e)
        {

        }
    }
}
