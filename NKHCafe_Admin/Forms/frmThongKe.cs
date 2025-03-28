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
    public partial class frmThongKe : Form
    {
        public frmThongKe()
        {
            InitializeComponent();
            // Đặt giá trị mặc định cho DateTimePicker (ví dụ: tháng hiện tại)
            dtpDenNgay.Value = DateTime.Now;
            dtpTuNgay.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date; // Lấy phần ngày, bỏ qua phần giờ
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1); // Lấy đến 23:59:59 của ngày được chọn

            // TODO:  Kết nối và truy vấn dữ liệu từ database
            string connectionString = "your_connection_string"; // Thay bằng chuỗi kết nối
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Truy vấn dữ liệu thống kê
                    string query = @"
                SELECT 
                    ngay_giao_dich,
                    SUM(tong_tien) AS TongTien
                FROM 
                    GiaoDich -- Thay GiaoDich bằng tên bảng chứa thông tin giao dịch của bạn
                WHERE 
                    ngay_giao_dich >= @TuNgay AND ngay_giao_dich <= @DenNgay
                GROUP BY
                    ngay_giao_dich
                ORDER BY
                    ngay_giao_dich";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TuNgay", tuNgay);
                        command.Parameters.AddWithValue("@DenNgay", denNgay);

                        DataTable dtThongKe = new DataTable();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dtThongKe);
                        }

                        dgvThongKe.DataSource = dtThongKe;
                        // Đặt tên cột (nếu cần)
                        dgvThongKe.Columns["ngay_giao_dich"].HeaderText = "Ngày Giao Dịch";
                        dgvThongKe.Columns["TongTien"].HeaderText = "Tổng Tiền";

                        // Tính tổng doanh thu
                        decimal tongDoanhThu = 0;
                        foreach (DataRow row in dtThongKe.Rows)
                        {
                            tongDoanhThu += (decimal)row["TongTien"];
                        }
                        lblTongDoanhThu.Text = "Tổng Doanh Thu: " + tongDoanhThu.ToString("C"); // Định dạng tiền tệ
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvThongKe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTongDoanhThu_Click(object sender, EventArgs e)
        {

        }
    }
}