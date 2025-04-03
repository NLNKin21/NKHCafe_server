using Newtonsoft.Json;
using NKHCafe_Admin.DAO;
using NKHCafe_Admin.DTO;
using NKHCafe_Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKHCafe_Admin.Forms
{
    public partial class frmYeuCau : Form
    {
        private DataTable _yeuCauTable;

        public frmYeuCau()
        {
            InitializeComponent();
            LoadDanhSachYeuCau();
            ThemCotNutLenDataGridView();
            ThemCotNutLenDataGridView2();
        }

        private void LoadDanhSachYeuCau()
        {
            var danhSachHienThi = new List<YeuCauHienThi>();

            lock (HoaDonManager.DanhSachHoaDon)
            {
                foreach (var hoaDon in HoaDonManager.DanhSachHoaDon)
                {
                    foreach (var mon in hoaDon.ChiTiet)
                    {
                        danhSachHienThi.Add(new YeuCauHienThi
                        {
                            ThoiGian = hoaDon.ThoiGianGui.ToString("HH:mm:ss dd/MM/yyyy"),
                            TaiKhoan = hoaDon.IdTaiKhoan,
                            May = hoaDon.IdMay,
                            IDMon = mon.IDMon,
                            TenMon = mon.TenMon,
                            SoLuong = mon.SoLuong,
                            DonGia = mon.DonGia,
                            ThanhTien = mon.ThanhTien
                        });
                    }
                }
            }

            dgvYeuCau.DataSource = danhSachHienThi;
        }
        private void cbLoaiYeuCau_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachYeuCau();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachYeuCau();
        }

        /*private void btnXacNhan_Click(object sender, EventArgs e)
        {
            int idYeuCau = Convert.ToInt32(dgvNapTien.SelectedRows[0].Cells["IdTaiKhoan"].Value);
            int SoTien= Convert.ToInt32(dgvNapTien)
            XuLyYeuCauNapTien(idYeuCau);
        }*/

        private void btnTuChoi_Click(object sender, EventArgs e)
        {
            if (dgvYeuCau.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu để từ chối.");
                return;
            }

            int idYeuCau = Convert.ToInt32(dgvYeuCau.SelectedRows[0].Cells["ID"].Value);
            CapNhatTrangThaiYeuCau(idYeuCau, "TuChoi");
            MessageBox.Show("Yêu cầu đã bị từ chối.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CapNhatTrangThaiYeuCau(int id, string trangThai)
        {
            string query = "UPDATE YeuCau SET TrangThai = @TrangThai WHERE ID = @ID";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TrangThai", trangThai),
                new SqlParameter("@ID", id)
            };

            KetNoiCSDL.ExecuteNonQuery(query, parameters);
            LoadDanhSachYeuCau();
        }

        private void XuLyYeuCauNapTien(int idYeuCau, decimal SoTien)
        {
            try
            {
                // 1. Lấy thông tin tài khoản từ DB
                string query = "SELECT IDTaiKhoan, SoDu FROM TaiKhoan WHERE IDTaiKhoan = @IDTaiKhoan";
                SqlParameter[] prms = new SqlParameter[]
                {
            new SqlParameter("@IDTaiKhoan", idYeuCau)
                };

                DataTable dt = KetNoiCSDL.ExecuteQuery(query, prms);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("❌ Không tìm thấy tài khoản với ID: " + idYeuCau);
                    return;
                }

                DataRow row = dt.Rows[0];
                decimal soDuHienTai = Convert.ToDecimal(row["SoDu"]);

                // 2. Cộng tiền vào tài khoản
                string updateTaiKhoan = @"
            UPDATE TaiKhoan
            SET SoDu = ISNULL(SoDu, 0) + @SoTienNap
            WHERE IDTaiKhoan = @IDTaiKhoan";

                SqlParameter[] prmsUpdate = new SqlParameter[]
                {
            new SqlParameter("@SoTienNap", SoTien),
            new SqlParameter("@IDTaiKhoan", idYeuCau)
                };

                int rows = KetNoiCSDL.ExecuteNonQuery(updateTaiKhoan, prmsUpdate);
                if (rows == 0)
                {
                    MessageBox.Show("❌ Không thể cập nhật số dư tài khoản.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3. Hiển thị thông báo thành công
                MessageBox.Show($"✅ Đã nạp {SoTien:N0} VNĐ vào tài khoản ID {idYeuCau}.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 4. Gợi ý: Làm mới danh sách nếu có
                // LamMoiDanhSachTaiKhoan();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý nạp tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmYeuCau_Load(object sender, EventArgs e)
        {
            // Liên kết BindingList với DataGridView
            dgvNapTien.DataSource = NapTienManager.DanhSachYeuCau;

            // Đăng ký sự kiện nếu muốn xử lý khi có yêu cầu mới
            NapTienManager.OnYeuCauMoi += (yc) =>
            {
                // Cập nhật UI nếu cần (Invoke nếu gọi từ thread khác)
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => dgvNapTien.Refresh()));
                }
                else
                {
                    dgvNapTien.Refresh(); // Cập nhật lại lưới
                }
            };
        }

        private void dgvNapTien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvNapTien.Columns[e.ColumnIndex].Name == "btnXacNhan")
            {
                // Lấy ID yêu cầu từ dòng được nhấn
                int idYeuCau = Convert.ToInt32(dgvNapTien.Rows[e.RowIndex].Cells["IdTaiKhoan"].Value);
                decimal SoTien = Convert.ToDecimal(dgvNapTien.Rows[e.RowIndex].Cells["SoTien"].Value);
                // Gọi hàm xử lý nạp tiền
                XuLyYeuCauNapTien(idYeuCau, SoTien);

                // Sau khi xử lý xong có thể làm mới lại lưới
                LamMoiDanhSachNapTien();
            }
        }
        private void ThemCotNutLenDataGridView()
        {
            if (!dgvNapTien.Columns.Contains("btnXacNhan"))
            {
                DataGridViewButtonColumn btnXacNhan = new DataGridViewButtonColumn();
                btnXacNhan.Name = "btnXacNhan";
                btnXacNhan.HeaderText = "Xác nhận";
                btnXacNhan.Text = "✔ Xác nhận";
                btnXacNhan.UseColumnTextForButtonValue = true;
                dgvNapTien.Columns.Add(btnXacNhan);
            }
        }
        private void LamMoiDanhSachNapTien()
        {
            // Tùy bạn lấy dữ liệu từ đâu: database hay danh sách tạm
            dgvNapTien.DataSource = null;
            dgvNapTien.DataSource = NapTienManager.DanhSachYeuCau; // Hoặc DataTable từ DB
        }
        private void ThemCotNutLenDataGridView2()
        {
            if (!dgvYeuCau.Columns.Contains("btnXacNhan"))
            {
                DataGridViewButtonColumn btnXacNhan = new DataGridViewButtonColumn();
                btnXacNhan.Name = "btnXacNhan";
                btnXacNhan.HeaderText = "Xác nhận";
                btnXacNhan.Text = "✔ Xác nhận";
                btnXacNhan.UseColumnTextForButtonValue = true;
                dgvYeuCau.Columns.Add(btnXacNhan);
            }
        }

        private void dgvYeuCau_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra nếu người dùng nhấn vào cột "Xác nhận"
            if (e.RowIndex >= 0 && dgvYeuCau.Columns[e.ColumnIndex].Name == "btnXacNhan")
            {
                var selectedRow = dgvYeuCau.Rows[e.RowIndex];

                try
                {
                    // Lấy dữ liệu từ dòng được chọn
                    int idMon = Convert.ToInt32(selectedRow.Cells["IDMon"].Value);
                    int idMay = Convert.ToInt32(selectedRow.Cells["May"].Value);
                    int taiKhoan = Convert.ToInt32(selectedRow.Cells["TaiKhoan"].Value);

                    lock (HoaDonManager.DanhSachHoaDon)
                    {
                        foreach (var hoaDon in HoaDonManager.DanhSachHoaDon)
                        {
                            if (hoaDon.IdMay == idMay && hoaDon.IdTaiKhoan == taiKhoan)
                            {
                                var chiTiet = hoaDon.ChiTiet.FirstOrDefault(m => m.IDMon == idMon);
                                if (chiTiet != null)
                                {
                                    // Ghi dữ liệu vào database trước khi xóa
                                    LuuChiTietVaoDatabase(hoaDon.IDHoaDon, chiTiet);

                                    // Xóa khỏi danh sách trong bộ nhớ
                                    hoaDon.ChiTiet.Remove(chiTiet);
                                    break; // Thoát khỏi vòng lặp sau khi xử lý
                                }
                            }
                        }
                    }

                    // Làm mới lại danh sách hiển thị
                    LoadDanhSachYeuCau();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xác nhận yêu cầu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void LuuChiTietVaoDatabase(int idHoaDon, ChiTietMon chiTiet)
        {
            string connectionString = @"Data Source=LAPTOP-5V6TA3CH\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO ChiTietHoaDon (IDHoaDon, IDMon, SoLuong, DonGia, ThanhTien)
                             VALUES (@IDHoaDon, @IDMon, @SoLuong, @DonGia, @ThanhTien)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Tránh lỗi SQL Injection và đảm bảo đúng kiểu dữ liệu
                        cmd.Parameters.AddWithValue("@IDHoaDon", idHoaDon);
                        cmd.Parameters.AddWithValue("@IDMon", chiTiet.IDMon);
                        cmd.Parameters.AddWithValue("@SoLuong", chiTiet.SoLuong);
                        cmd.Parameters.AddWithValue("@DonGia", chiTiet.DonGia);
                        cmd.Parameters.AddWithValue("@ThanhTien", chiTiet.ThanhTien);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu vào CSDL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}