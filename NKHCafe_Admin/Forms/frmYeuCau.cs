using NKHCafe_Admin.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
        }

        private void frmYeuCau_Load(object sender, EventArgs e)
        {
            LoadLoaiYeuCau();
            LoadDanhSachYeuCau();
        }

        private void LoadLoaiYeuCau()
        {
            cbLoaiYeuCau.Items.Clear();
            cbLoaiYeuCau.Items.Add("Tất cả");
            cbLoaiYeuCau.Items.Add("Nạp tiền");
            cbLoaiYeuCau.Items.Add("Thêm giờ");
            cbLoaiYeuCau.Items.Add("Hóa đơn đồ ăn");
            cbLoaiYeuCau.SelectedIndex = 0;
        }

        private void LoadDanhSachYeuCau()
        {
            try
            {
                string loai = cbLoaiYeuCau.SelectedItem.ToString();
                string query = "SELECT ID, MaKhachHang, LoaiYeuCau, NoiDung, ThoiGian, TrangThai FROM YeuCau WHERE TrangThai = 'ChoDuyet'";

                if (loai != "Tất cả")
                {
                    string dbLoai = "";
                    if (loai == "Nạp tiền") dbLoai = "NapTien";
                    else if (loai == "Thêm giờ") dbLoai = "ThemGio";
                    else if (loai == "Hóa đơn đồ ăn") dbLoai = "HoaDonDoAn";

                    query += $" AND LoaiYeuCau = '{dbLoai}'";
                }

                _yeuCauTable = KetNoiCSDL.ExecuteQuery(query, null);
                dgvYeuCau.DataSource = _yeuCauTable;

                dgvYeuCau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvYeuCau.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách yêu cầu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbLoaiYeuCau_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDanhSachYeuCau();
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadDanhSachYeuCau();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (dgvYeuCau.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một yêu cầu để xử lý.");
                return;
            }

            string loaiYeuCau = dgvYeuCau.SelectedRows[0].Cells["LoaiYeuCau"].Value.ToString();
            int idYeuCau = Convert.ToInt32(dgvYeuCau.SelectedRows[0].Cells["ID"].Value);

            if (loaiYeuCau == "NapTien")
            {
                XuLyYeuCauNapTien(idYeuCau);
            }
            else
            {
                CapNhatTrangThaiYeuCau(idYeuCau, "DaXuLy");
                MessageBox.Show("Yêu cầu đã được xác nhận.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

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

        private void XuLyYeuCauNapTien(int idYeuCau)
        {
            try
            {
                // Lấy thông tin yêu cầu
                DataRow row = _yeuCauTable.AsEnumerable().FirstOrDefault(r => r.Field<int>("ID") == idYeuCau);
                if (row == null) return;

                int maKhachHang = Convert.ToInt32(row["MaKhachHang"]);
                string noiDung = row["NoiDung"].ToString(); // VD: "100000"
                decimal soTienNap;

                if (!Decimal.TryParse(noiDung, out soTienNap))
                {
                    MessageBox.Show("Không thể đọc số tiền nạp từ nội dung yêu cầu.");
                    return;
                }

                // Quy đổi tiền → phút (VD: 1000 đ = 6 phút)
                int soPhutThem = (int)(soTienNap / 1000) * 6;

                // Update tài khoản khách
                string updateKH = @"
                    UPDATE KhachHang
                    SET SoTien = ISNULL(SoTien, 0) + @SoTien,
                        ThoiGianConLai = ISNULL(ThoiGianConLai, 0) + @SoPhut
                    WHERE MaKhachHang = @MaKH";

                SqlParameter[] prms = new SqlParameter[]
                {
                    new SqlParameter("@SoTien", soTienNap),
                    new SqlParameter("@SoPhut", soPhutThem),
                    new SqlParameter("@MaKH", maKhachHang)
                };

                int rows = KetNoiCSDL.ExecuteNonQuery(updateKH, prms);

                if (rows > 0)
                {
                    // Cập nhật trạng thái yêu cầu
                    CapNhatTrangThaiYeuCau(idYeuCau, "DaXuLy");

                    MessageBox.Show($"✅ Đã nạp {soTienNap:N0} VNĐ cho khách {maKhachHang}. Thêm {soPhutThem} phút.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("❌ Không thể cập nhật tài khoản khách.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý nạp tiền: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvYeuCau_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }

}