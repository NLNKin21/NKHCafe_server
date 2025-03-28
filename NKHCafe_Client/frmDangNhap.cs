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
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text; // Không trim mật khẩu
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboMayTram.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn máy trạm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int idMay = ((DataRowView)cboMayTram.SelectedItem)["IDMay"] is DBNull ? 0 : Convert.ToInt32(((DataRowView)cboMayTram.SelectedItem)["IDMay"]);
            if (idMay == 0)
            {
                MessageBox.Show("Máy này chưa được thiết lập, vui lòng chọn máy khác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // Kiểm tra đăng nhập
                string query = "SELECT IDTaiKhoan, TenDangNhap, SoDu FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau AND LoaiTaiKhoan = 'Client'";
                object[] parameters = { tenDangNhap, matKhau }; // Không mã hóa, chỉ demo
                DataTable result = KetNoiCSDL.ExecuteQuery(query, parameters);


                if (result.Rows.Count > 0)
                {
                    // Đăng nhập thành công
                    int idTaiKhoan = Convert.ToInt32(result.Rows[0]["IDTaiKhoan"]);
                    string tenDN = result.Rows[0]["TenDangNhap"].ToString();
                    decimal soDu = Convert.ToDecimal(result.Rows[0]["SoDu"]);

                    // Cập nhật trạng thái máy
                    string updateMayQuery = "UPDATE MayTram SET TrangThai = 'Ban', IDTaiKhoan = @IDTaiKhoan WHERE IDMay = @IDMay AND TrangThai = 'Trong'";
                    object[] updateMayParams = { idTaiKhoan, idMay };

                    int rowsAffected = KetNoiCSDL.ExecuteNonQuery(updateMayQuery, updateMayParams);
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Máy đã có người sử dụng, vui lòng chọn máy khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        LoadMayTram(); // load lai danh sach may
                        return;
                    }


                    // Mở form chính
                    this.Hide();
                    frmClientMain frmMain = new frmClientMain(idTaiKhoan, tenDN, soDu, idMay);
                    frmMain.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbHienThiMK_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = !cbHienThiMK.Checked;
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            LoadMayTram();
        }
        private void LoadMayTram()
        {
            try
            {
                // Lấy danh sách máy trạm còn trống
                string query = "SELECT IDMay, TenMay FROM MayTram WHERE TrangThai = 'Trong'";
                DataTable dt = KetNoiCSDL.ExecuteQuery(query);
                // Thêm dòng "Chọn máy" (tùy chọn)
                // DataRow firstRow = dt.NewRow();
                // firstRow["IDMay"] = DBNull.Value;  // Or  firstRow["IDMay"] = 0;
                // firstRow["TenMay"] = "-- Chọn máy --";
                // dt.Rows.InsertAt(firstRow, 0);

                cboMayTram.DataSource = dt;
                cboMayTram.DisplayMember = "TenMay";
                cboMayTram.ValueMember = "IDMay";
                // cboMayTram.SelectedIndex = 0; // chọn dòng "Chọn máy" (nếu có)
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách máy trạm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}