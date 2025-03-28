using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKHCafe_Client
{
    public partial class frmOrderMon : Form
    {
        private List<ChiTietOrder> _gioHang = new List<ChiTietOrder>();

        public frmOrderMon()
        {
            InitializeComponent();
        }
        // Constructor có tham số IDTaiKhoan (nếu cần truyền từ frmClientMain)
        // public frmOrderMon(int idTaiKhoan)
        // {
        //     InitializeComponent();
        //     _idTaiKhoan = idTaiKhoan; // Lưu IDTaiKhoan
        // }

        private void frmOrderMon_Load(object sender, EventArgs e)
        {
            LoadThucDon();
        }

        private void LoadThucDon()
        {
            try
            {
                string query = "SELECT IDMon, TenMon, DonGia FROM ThucDon";
                DataTable dt = KetNoiCSDL.ExecuteQuery(query);

                dgvThucDon.DataSource = dt;
                dgvThucDon.Columns["IDMon"].Visible = false;
                dgvThucDon.Columns["TenMon"].HeaderText = "Tên Món";
                dgvThucDon.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvThucDon.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải thực đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            if (dgvThucDon.SelectedRows.Count > 0)
            {
                int idMon = Convert.ToInt32(dgvThucDon.SelectedRows[0].Cells["IDMon"].Value);
                string tenMon = dgvThucDon.SelectedRows[0].Cells["TenMon"].Value.ToString();
                decimal donGia = Convert.ToDecimal(dgvThucDon.SelectedRows[0].Cells["DonGia"].Value);
                int soLuong = (int)nudSoLuong.Value;

                if (soLuong <= 0)
                {
                    MessageBox.Show("Vui lòng chọn số lượng lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                ChiTietOrder existingItem = _gioHang.FirstOrDefault(item => item.IDMon == idMon);
                if (existingItem != null)
                {
                    existingItem.SoLuong += soLuong;
                }
                else
                {
                    ChiTietOrder newItem = new ChiTietOrder(idMon, tenMon, soLuong, donGia);
                    _gioHang.Add(newItem);
                }

                HienThiGioHang();
            }
        }

        private void HienThiGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = _gioHang;
            dgvGioHang.Columns["IDMon"].Visible = false;
            dgvGioHang.Columns["DonGia"].DefaultCellStyle.Format = "N0";
            dgvGioHang.Columns["ThanhTien"].DefaultCellStyle.Format = "N0";
        }
        private void btnXacNhanOrder_Click(object sender, EventArgs e)
        {
            if (_gioHang.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống. Vui lòng chọn món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Lấy IDHoaDon (tạo mới nếu chưa có) -  sử dụng lại hàm GetOrCreateHoaDon từ frmClientMain
                int idHoaDon = GetOrCreateHoaDon(); //sử dụng lại hàm GetOrCreateHoaDon

                // 2. Thêm ChiTietHoaDon vào CSDL
                foreach (ChiTietOrder item in _gioHang)
                {
                    string query = "INSERT INTO ChiTietHoaDon (IDHoaDon, IDMon, SoLuong, DonGia) VALUES (@IDHoaDon, @IDMon, @SoLuong, @DonGia)";
                    object[] parameters = { idHoaDon, item.IDMon, item.SoLuong, item.DonGia };
                    KetNoiCSDL.ExecuteNonQuery(query, parameters);
                }

                // 3. Gửi thông báo đến Admin (qua bảng trung gian)
                string notificationQuery = "INSERT INTO ThongBao (NoiDung, ThoiGian, IDNguoiGui, IDNguoiNhan, DaXem) VALUES (@NoiDung, @ThoiGian, @IDNguoiGui, @IDNguoiNhan, 0)";
                string noiDung = $"Máy {((frmClientMain)this.Owner)._idMay} đã order món. ID hóa đơn: {idHoaDon}"; // Lấy _idMay từ form cha
                object[] notificationParams = { noiDung, DateTime.Now, ((frmClientMain)this.Owner)._idTaiKhoan, null }; // Lấy _idTaiKhoan
                KetNoiCSDL.ExecuteNonQuery(notificationQuery, notificationParams);

                // 4. Xóa giỏ hàng
                _gioHang.Clear();
                HienThiGioHang();

                MessageBox.Show("Order thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Đóng form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xác nhận order: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //Sử dụng lại hàm GetOrCreateHoaDon, và có chỉnh sửa 1 chút
        private int GetOrCreateHoaDon()
        {
            // Lấy IDTaiKhoan và IDMay từ form cha (frmClientMain)
            int idTaiKhoan = ((frmClientMain)this.Owner)._idTaiKhoan; // Ép kiểu this.Owner thành frmClientMain
            int idMay = ((frmClientMain)this.Owner)._idMay;
            // Kiểm tra hóa đơn đang chờ
            string query = "SELECT IDHoaDon FROM HoaDon WHERE IDTaiKhoan = @IDTaiKhoan AND IDMay = @IDMay AND TrangThai = 'DangCho'";
            object[] parameters = { idTaiKhoan, idMay };
            DataTable result = KetNoiCSDL.ExecuteQuery(query, parameters);

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["IDHoaDon"]);
            }
            else
            {
                // Tạo hóa đơn mới (nếu hết tiền, đã xử lý ở trên, hóa đơn vẫn được tạo với trạng thái nợ)
                string insertQuery = "INSERT INTO HoaDon (IDTaiKhoan, IDMay, ThoiGianBatDau, TrangThai) VALUES (@IDTaiKhoan, @IDMay, @ThoiGianBatDau, 'DangCho')";
                object[] insertParams = { idTaiKhoan, idMay, DateTime.Now };
                KetNoiCSDL.ExecuteNonQuery(insertQuery, insertParams);

                // Lấy IDHoaDon vừa tạo (cách lấy identity sau khi insert)
                string getIdQuery = "SELECT @@IDENTITY"; // Hoặc SELECT SCOPE_IDENTITY()
                return Convert.ToInt32(KetNoiCSDL.ExecuteScalar(getIdQuery));
            }
        }

        private void dgvThucDon_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
