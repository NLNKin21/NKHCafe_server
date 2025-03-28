using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NKHCafe_Admin.Forms
{
    public partial class frmTinhTien : Form
    {
        private int _idMay;
        private int? _idTaiKhoan;
        private DateTime? _thoiGianBatDau;

        public frmTinhTien(int idMay, int? idTaiKhoan, DateTime? thoiGianBatDau)
        {
            InitializeComponent();
            _idMay = idMay;
            _idTaiKhoan = idTaiKhoan;
            _thoiGianBatDau = thoiGianBatDau;
            LoadData();
        }

        private void LoadData()
        {
            // Hiển thị thông tin
            lblMaySo.Text = $"Máy số: {_idMay}";
            lblThoiGianBatDau.Text = $"Thời gian bắt đầu: {_thoiGianBatDau:HH:mm:ss dd/MM/yyyy}";
            lblThoiGianKetThuc.Text = $"Thời gian kết thúc: {DateTime.Now:HH:mm:ss dd/MM/yyyy}";

            // Tính tổng thời gian (ví dụ)
            TimeSpan tongThoiGian = DateTime.Now - (_thoiGianBatDau ?? DateTime.Now); // Dùng ?? để tránh lỗi nếu _thoiGianBatDau null
            lblTongThoiGian.Text = $"Tổng thời gian: {tongThoiGian.TotalHours:F2} giờ";


            // Tính thành tiền (ví dụ)
            double donGia = 10000; // 10.000 VNĐ/giờ
            double thanhTien = tongThoiGian.TotalHours * donGia;
            lblThanhTien.Text = $"Thành tiền: {thanhTien:N0} VNĐ"; // Định dạng số tiền

        }

        private void btnDongY_Click(object sender, EventArgs e)
        {
            try
            {
                // Cập nhật trạng thái máy, xóa thông tin tài khoản, thời gian
                DAO.MayTramDAO.TatMay(_idMay);

                // TODO:  Thêm logic ghi hóa đơn, trừ tiền tài khoản (nếu cần)

                this.DialogResult = DialogResult.OK; // Báo hiệu đã thanh toán
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmTinhTien_Load(object sender, EventArgs e)
        {

        }
    }
}