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
    public partial class frmNapTien : Form
    {
        public frmNapTien()
        {
            InitializeComponent();
        }

        private void btnNapTien_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu nhập
            if (string.IsNullOrWhiteSpace(txtTaiKhoanNap.Text) || string.IsNullOrWhiteSpace(txtSoTienNap.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string taiKhoan = txtTaiKhoanNap.Text;
            decimal soTien;

            if (!decimal.TryParse(txtSoTienNap.Text, out soTien) || soTien <= 0)
            {
                MessageBox.Show("Số tiền nạp không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Kết nối và cập nhật số dư tài khoản trong database
            //  Thay "your_connection_string" bằng chuỗi kết nối thực tế của bạn.
            string connectionString = @"Data Source=LAPTOP-5V6TA3CH\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Kiểm tra tài khoản có tồn tại không
                    string checkAccountQuery = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TaiKhoan";
                    using (SqlCommand checkAccountCmd = new SqlCommand(checkAccountQuery, connection))
                    {
                        checkAccountCmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                        int accountExists = (int)checkAccountCmd.ExecuteScalar();
                        if (accountExists == 0)
                        {
                            MessageBox.Show("Tài khoản không tồn tại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Cập nhật số dư
                    string updateBalanceQuery = "UPDATE TaiKhoan SET SoDu = SoDu + @SoTienNap WHERE TenDangNhap = @TaiKhoan";
                    using (SqlCommand updateCmd = new SqlCommand(updateBalanceQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@SoTienNap", soTien);
                        updateCmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);

                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show($"Nạp tiền thành công cho tài khoản {taiKhoan}.\nSố tiền đã nạp: {soTien:C}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Xóa trắng các trường sau khi nạp thành công
                            txtTaiKhoanNap.Text = "";
                            txtSoTienNap.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("Nạp tiền không thành công. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (SqlException ex) // Catch specific SQL exceptions for better error handling
            {
                MessageBox.Show($"Lỗi kết nối cơ sở dữ liệu: {ex.Message}\nError Number: {ex.Number}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Log the error for debugging purposes (optional)
                //Console.WriteLine($"SQL Error: {ex.Message}, Number: {ex.Number}, State: {ex.State}, Class: {ex.Class}, Server: {ex.Server}, Procedure: {ex.Procedure}, LineNumber: {ex.LineNumber}");
            }
            catch (Exception ex) // Catch any other unexpected exceptions
            {
                MessageBox.Show("Lỗi không xác định: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmNapTien_Load(object sender, EventArgs e)
        {

        }
    }
}