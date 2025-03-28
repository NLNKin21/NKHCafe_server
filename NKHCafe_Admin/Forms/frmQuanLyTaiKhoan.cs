using NKHCafe_Admin.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Thêm using này

namespace NKHCafe_Admin.Forms
{
    public partial class frmQuanLyTaiKhoan : Form
    {
        public frmQuanLyTaiKhoan()
        {
            InitializeComponent();
        }

        private void frmQuanLyTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            // Sử dụng TaiKhoanDAO
            dgvTaiKhoan.DataSource = TaiKhoanDAO.LayDanhSachTaiKhoan();
            dgvTaiKhoan.Columns["MatKhau"].Visible = false;

            ClearInputs();
            SetControlState(false);
        }

        private void ClearInputs()
        {
            // Gom chung việc reset txtIDTaiKhoan
            txtIDTaiKhoan.Text = "";
            txtIDTaiKhoan.Enabled = false; // Luôn tắt
            txtIDTaiKhoan.ReadOnly = true; // Nên đặt trong Designer

            txtTenDangNhapTK.Text = "";
            txtMatKhauTK.Text = "";
            txtSoDuTK.Text = "";
            cboLoaiTaiKhoan.SelectedIndex = -1;
            chkTrangThai.Checked = true;
        }

        private void SetControlState(bool isEditing)
        {
            btnThem.Enabled = !isEditing;
            btnSua.Enabled = !isEditing;
            btnXoa.Enabled = !isEditing;
            btnLuu.Enabled = isEditing;
            btnHuy.Enabled = isEditing;

            txtTenDangNhapTK.Enabled = isEditing;
            txtMatKhauTK.Enabled = isEditing;
            txtSoDuTK.Enabled = isEditing; // Nên cho sửa số dư
            cboLoaiTaiKhoan.Enabled = isEditing;
            chkTrangThai.Enabled = isEditing;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearInputs();
            SetControlState(true);
            txtTenDangNhapTK.Focus();

            // Tạo ID tự tăng (sử dụng TaiKhoanDAO)
            txtIDTaiKhoan.Text = (TaiKhoanDAO.LayMaxID() + 1).ToString();

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.SelectedRows.Count > 0)
            {
                SetControlState(true);
                txtTenDangNhapTK.Focus();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.SelectedRows.Count > 0)
            {
                DialogResult confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa tài khoản này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    int idTaiKhoan = Convert.ToInt32(dgvTaiKhoan.SelectedRows[0].Cells["IDTaiKhoan"].Value);

                    if (TaiKhoanDAO.KiemTraRangBuocTaiKhoan(idTaiKhoan)) // Sử dụng TaiKhoanDAO
                    {
                        MessageBox.Show("Không thể xóa tài khoản này vì có ràng buộc dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (TaiKhoanDAO.XoaTaiKhoan(idTaiKhoan)) // Sử dụng TaiKhoanDAO
                    {
                        LoadData();
                        MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xóa tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một tài khoản để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!KiemTraDuLieuHopLe())
            {
                return;
            }

            int idTaiKhoan = Convert.ToInt32(txtIDTaiKhoan.Text);
            string tenDangNhap = txtTenDangNhapTK.Text;
            string matKhau = txtMatKhauTK.Text;
            string loaiTaiKhoan = cboLoaiTaiKhoan.SelectedItem.ToString();
            decimal soDu = Convert.ToDecimal(txtSoDuTK.Text);
            bool trangThai = chkTrangThai.Checked;

            if (btnThem.Enabled == false) // Thêm mới
            {
                if (TaiKhoanDAO.KiemTraTrungTenDangNhap(tenDangNhap)) // Sử dụng TaiKhoanDAO
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (TaiKhoanDAO.ThemTaiKhoan(tenDangNhap, matKhau, loaiTaiKhoan, soDu, trangThai)) // Sử dụng TaiKhoanDAO
                {
                    LoadData();
                    MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Thêm tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Sửa
            {
                if (TaiKhoanDAO.SuaTaiKhoan(idTaiKhoan, tenDangNhap, matKhau, loaiTaiKhoan, soDu, trangThai)) // Sử dụng TaiKhoanDAO
                {
                    LoadData();
                    MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật tài khoản thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvTaiKhoan.Rows[e.RowIndex];

                txtIDTaiKhoan.Text = row.Cells["IDTaiKhoan"].Value.ToString();
                txtTenDangNhapTK.Text = row.Cells["TenDangNhap"].Value.ToString();
                txtMatKhauTK.Text = row.Cells["MatKhau"].Value.ToString();
                txtSoDuTK.Text = row.Cells["SoDu"].Value.ToString();
                cboLoaiTaiKhoan.SelectedItem = row.Cells["LoaiTaiKhoan"].Value.ToString();
                chkTrangThai.Checked = Convert.ToBoolean(row.Cells["TrangThai"].Value);

                SetControlState(false);
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
        }

        private bool KiemTraDuLieuHopLe()
        {
            if (string.IsNullOrWhiteSpace(txtTenDangNhapTK.Text))
            {
                MessageBox.Show("Tên đăng nhập không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDangNhapTK.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMatKhauTK.Text))
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMatKhauTK.Focus(); // Nên focus vào txtMatKhauTK
                return false;
            }

            decimal soDu;
            if (!decimal.TryParse(txtSoDuTK.Text, out soDu))
            {
                MessageBox.Show("Số dư phải là số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoDuTK.Focus();
                return false;
            }

            if (cboLoaiTaiKhoan.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn loại tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboLoaiTaiKhoan.Focus();
                return false;
            }

            return true;
        }
    }
}