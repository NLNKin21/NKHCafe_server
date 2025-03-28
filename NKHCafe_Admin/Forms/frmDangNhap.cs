using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using NKHCafe_Admin.DAO;
using NKHCafe_Admin.DTO;

namespace NKHCafe_Admin.Forms
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text.Trim();

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                XtraMessageBox.Show("Vui lòng nhập tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nếu là admin hardcode
            if (tenDangNhap == "admin" && matKhau == "123")
            {
                XtraMessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenUserForm(-1, tenDangNhap, "Admin"); // -1 ID vì không lấy từ DB
                return;
            }

            TaiKhoan tk = TaiKhoanDAO.KiemTraDangNhap(tenDangNhap, matKhau);
            if (tk != null)
            {
                XtraMessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                OpenUserForm(tk.ID, tk.TenDangNhap, tk.LoaiTaiKhoan);
            }
            else
            {
                XtraMessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhau.Clear();
                txtMatKhau.Focus();
            }
        }

        private void OpenUserForm(int idTaiKhoan, string tenDangNhap, string loaiTaiKhoan)
        {
            this.Hide();

            if (loaiTaiKhoan == "Admin")
            {
                frmMain frm = new frmMain(idTaiKhoan, tenDangNhap, loaiTaiKhoan);
                frm.Show();
            }
            else
            {
                XtraMessageBox.Show($"Chức năng cho loại tài khoản '{loaiTaiKhoan}' đang phát triển!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Show();
            }
            FormClosed += (s, args) => this.Close(); // Đảm bảo đóng form DangNhap khi form Main đóng.
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // Không có code trong sự kiện Load
        }

        private void frmDangNhap_Load_1(object sender, EventArgs e)
        {

        }
    }
}