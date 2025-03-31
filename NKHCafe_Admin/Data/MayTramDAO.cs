using NKHCafe_Admin.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NKHCafe_Admin.DAO
{
    public static class MayTramDAO
    {
        public static List<MayTram> LayDanhSachMayTram()
        {
            List<MayTram> danhSachMayTram = new List<MayTram>();
            // Sử dụng LEFT JOIN để lấy thông tin cả khi không có tài khoản đăng nhập
            string query = "SELECT mt.*, tk.TenDangNhap FROM MayTram mt LEFT JOIN TaiKhoan tk ON mt.IDTaiKhoan = tk.IDTaiKhoan";

            try
            {
                using (DataTable data = KetNoiCSDL.ExecuteQuery(query)) // Sử dụng using để đảm bảo giải phóng tài nguyên
                {
                    if (data != null)
                    {
                        foreach (DataRow row in data.Rows)
                        {
                            MayTram mayTram = new MayTram
                            {
                                IdMay = Convert.ToInt32(row["IDMay"]),
                                TenMay = row["TenMay"].ToString(),
                                TrangThai = row["TrangThai"].ToString(),
                                // Kiểm tra giá trị DBNull.Value trước khi chuyển đổi
                                IdTaiKhoan = row["IDTaiKhoan"] != DBNull.Value ? Convert.ToInt32(row["IDTaiKhoan"]) : (int?)null,
                                TenTaiKhoan = row.Table.Columns.Contains("TenDangNhap") && row["TenDangNhap"] != DBNull.Value ? row["TenDangNhap"].ToString() : null,
                                ThoiGianBatDau = row["ThoiGianBatDau"] != DBNull.Value ? Convert.ToDateTime(row["ThoiGianBatDau"]) : (DateTime?)null

                            };
                            danhSachMayTram.Add(mayTram);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi, ghi log, hoặc hiển thị thông báo
                MessageBox.Show("Lỗi khi lấy danh sách máy trạm: " + ex.Message);
                // Hoặc: throw;  // Ném lại ngoại lệ để form gọi xử lý
                return null; // Hoặc trả về null tùy theo cách bạn muốn xử lý
            }
            return danhSachMayTram;
        }

        public static bool MoMay(int idMay, int idTaiKhoan)
        {
            // Chỉ mở máy nếu máy đang ở trạng thái 'Không hoạt động'
            string query = "UPDATE MayTram SET TrangThai = N'Trong', IDTaiKhoan = @IDTaiKhoan, ThoiGianBatDau = GETDATE() WHERE IDMay = @IDMay AND (TrangThai = N'Trong')";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IDTaiKhoan", idTaiKhoan),
                new SqlParameter("@IDMay", idMay)
            };
            try
            {
                int rowsAffected = KetNoiCSDL.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0; // Trả về true nếu có ít nhất 1 dòng bị ảnh hưởng

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi mở máy: " + ex.Message);
                return false;
            }
        }

        public static bool TatMay(int idMay)
        {
            // Chỉ tắt máy nếu máy đang ở trạng thái 'Đang sử dụng'
            string query = "UPDATE MayTram SET TrangThai = N'Trong', IDTaiKhoan = NULL, ThoiGianBatDau = NULL WHERE IDMay = @IDMay AND TrangThai = N'Ban'";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@IDMay", idMay)
            };

            try
            {
                int rowsAffected = KetNoiCSDL.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tắt máy: " + ex.Message);
                return false;
            }
        }

        public static MayTram LayThongTinMayTram(int idMay)
        {
            string query = "SELECT mt.*, tk.TenDangNhap FROM MayTram mt LEFT JOIN TaiKhoan tk ON mt.IDTaiKhoan = tk.IDTaiKhoan WHERE mt.IDMay = @IDMay"; // Lấy thông tin từ cả 2 bảng
            SqlParameter[] parameters = new SqlParameter[]
           {
                new SqlParameter("@IDMay", idMay)
           };

            try
            {
                using (DataTable data = KetNoiCSDL.ExecuteQuery(query, parameters))
                {
                    if (data != null && data.Rows.Count > 0)
                    {
                        DataRow row = data.Rows[0];
                        return new MayTram
                        {
                            IdMay = Convert.ToInt32(row["IDMay"]),
                            TenMay = row["TenMay"].ToString(),
                            TrangThai = row["TrangThai"].ToString(),
                            IdTaiKhoan = row["IDTaiKhoan"] != DBNull.Value ? Convert.ToInt32(row["IDTaiKhoan"]) : (int?)null,
                            TenTaiKhoan = row.Table.Columns.Contains("TenDangNhap") && row["TenDangNhap"] != DBNull.Value ? row["TenDangNhap"].ToString() : null,
                            ThoiGianBatDau = row["ThoiGianBatDau"] != DBNull.Value ? Convert.ToDateTime(row["ThoiGianBatDau"]) : (DateTime?)null
                        };
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy thông tin máy trạm: " + ex.Message);
                return null;
            }
            return null;
        }


        public static bool ThemMay(MayTram mayTram)
        {
            string query = "INSERT INTO MayTram (TenMay, TrangThai) VALUES (@TenMay, @TrangThai)";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@TenMay", mayTram.TenMay),
                    new SqlParameter("@TrangThai", mayTram.TrangThai)
            };

            try
            {
                int rowsAffected = KetNoiCSDL.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi, log lỗi, ...
                MessageBox.Show("Lỗi thêm máy: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool CapNhatMayTram(MayTram mayTram)
        {
            string query = "UPDATE MayTram SET TenMay = @TenMay, TrangThai = @TrangThai WHERE IDMay = @IDMay";
            SqlParameter[] parameters = new SqlParameter[]
            {
                    new SqlParameter("@TenMay", mayTram.TenMay),
                    new SqlParameter("@TrangThai", mayTram.TrangThai),
                    new SqlParameter("@IDMay", mayTram.IdMay)
            };

            try
            {
                int rowsAffected = KetNoiCSDL.ExecuteNonQuery(query, parameters);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật máy: " + ex.Message);
                return false;
            }
        }

        public static bool XoaMay(int idMay)
        {
            string query = "DELETE FROM MayTram WHERE IDMay = @IDMay";
            SqlParameter[] parameters = { new SqlParameter("@IDMay", idMay) };

            try
            {
                return KetNoiCSDL.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa máy: " + ex.Message);
                return false;
            }
        }
    }
}