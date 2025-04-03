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

        
        private void dgvThongKe_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblTongDoanhThu_Click(object sender, EventArgs e)
        {

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            DateTime tuNgay = dtpTuNgay.Value.Date;
            DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1); // đến cuối ngày được chọn

            string connectionString = @"Data Source=LAPTOP-5V6TA3CH\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // 1. Thống kê tổng tiền theo từng máy từ HoaDon
                    string queryTongTienMay = @"
                SELECT 
                    IDMay AS [Máy], 
                    SUM(TongTien) AS [Tổng Tiền Máy]
                FROM HoaDon
                WHERE ThoiGianBatDau BETWEEN @TuNgay AND @DenNgay
                GROUP BY IDMay
                ORDER BY IDMay";

                    DataTable dtThongKe = new DataTable();
                    using (SqlCommand cmd = new SqlCommand(queryTongTienMay, connection))
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dtThongKe);
                        }
                    }

                    dgvThongKe.DataSource = dtThongKe;

                    decimal tongTienMay = 0;
                    foreach (DataRow row in dtThongKe.Rows)
                    {
                        tongTienMay += Convert.ToDecimal(row["Tổng Tiền Máy"]);
                    }

                    // 2. Tổng tiền món
                    string queryTongTienMon = @"
    SELECT SUM(ThanhTien)
    FROM ChiTietHoaDon
    WHERE ThoiGianDatMon BETWEEN @TuNgay AND @DenNgay";

                    decimal tongTienMon = 0;
                    using (SqlCommand cmd = new SqlCommand(queryTongTienMon, connection))
                    {
                        cmd.Parameters.AddWithValue("@TuNgay", tuNgay);
                        cmd.Parameters.AddWithValue("@DenNgay", denNgay);

                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                            tongTienMon = Convert.ToDecimal(result);
                    }

                    // 3. Hiển thị tổng cộng
                    decimal tongCong = tongTienMay + tongTienMon;
                    lblTongDoanhThu.Text = $"🖥 Máy: {tongTienMay.ToString("C")} | 🍽 Món: {tongTienMon.ToString("C")} | 💰 Tổng: {tongCong.ToString("C")}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thống kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}