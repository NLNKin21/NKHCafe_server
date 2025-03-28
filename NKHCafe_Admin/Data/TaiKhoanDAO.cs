using NKHCafe_Admin.DTO;
using System;
using System.Data;
using System.Data.SqlClient;

namespace NKHCafe_Admin.DAO
{
    public static class TaiKhoanDAO
    {
        public static TaiKhoan KiemTraDangNhap(string tenDangNhap, string matKhau)
        {
            string query = "SELECT IDTaiKhoan, TenDangNhap, LoaiTaiKhoan FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap AND MatKhau = @MatKhau";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau)
            };

            DataTable data = KetNoiCSDL.ExecuteQuery(query, parameters);

            if (data != null && data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new TaiKhoan
                {
                    ID = Convert.ToInt32(row["IDTaiKhoan"]),
                    TenDangNhap = row["TenDangNhap"].ToString(),
                    LoaiTaiKhoan = row["LoaiTaiKhoan"].ToString()
                };
            }
            return null; // Không tìm thấy tài khoản
        }
        public static DataTable LayDanhSachTaiKhoan()
        {
            string query = "SELECT IDTaiKhoan, TenDangNhap, MatKhau, LoaiTaiKhoan, SoDu, TrangThai FROM TaiKhoan";
            return KetNoiCSDL.ExecuteQuery(query);
        }

        public static int LayMaxID()
        {
            string query = "SELECT MAX(IDTaiKhoan) FROM TaiKhoan";
            object result = KetNoiCSDL.ExecuteScalar(query);
            return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
        }

        public static bool KiemTraTrungTenDangNhap(string tenDangNhap)
        {
            string query = "SELECT COUNT(*) FROM TaiKhoan WHERE TenDangNhap = @TenDangNhap";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TenDangNhap", tenDangNhap)
            };
            int count = (int)KetNoiCSDL.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static bool ThemTaiKhoan(string tenDangNhap, string matKhau, string loaiTaiKhoan, decimal soDu, bool trangThai)
        {
            string query = "INSERT INTO TaiKhoan (TenDangNhap, MatKhau, LoaiTaiKhoan, SoDu, TrangThai) VALUES (@TenDangNhap, @MatKhau, @LoaiTaiKhoan, @SoDu, @TrangThai)";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@LoaiTaiKhoan", loaiTaiKhoan),
                new SqlParameter("@SoDu", soDu),
                new SqlParameter("@TrangThai", trangThai)
            };
            return KetNoiCSDL.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool SuaTaiKhoan(int idTaiKhoan, string tenDangNhap, string matKhau, string loaiTaiKhoan, decimal soDu, bool trangThai)
        {
            string query = "UPDATE TaiKhoan SET TenDangNhap = @TenDangNhap, MatKhau = @MatKhau, LoaiTaiKhoan = @LoaiTaiKhoan, SoDu = @SoDu, TrangThai = @TrangThai WHERE IDTaiKhoan = @IDTaiKhoan";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@IDTaiKhoan", idTaiKhoan),
                new SqlParameter("@TenDangNhap", tenDangNhap),
                new SqlParameter("@MatKhau", matKhau),
                new SqlParameter("@LoaiTaiKhoan", loaiTaiKhoan),
                new SqlParameter("@SoDu", soDu),
                new SqlParameter("@TrangThai", trangThai)
            };
            return KetNoiCSDL.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool XoaTaiKhoan(int idTaiKhoan)
        {
            string query = "DELETE FROM TaiKhoan WHERE IDTaiKhoan = @IDTaiKhoan";
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@IDTaiKhoan", idTaiKhoan)
            };
            return KetNoiCSDL.ExecuteNonQuery(query, parameters) > 0;
        }

        public static bool KiemTraRangBuocTaiKhoan(int idTaiKhoan)
        {
            // Kiểm tra trong bảng MayTram
            string queryMayTram = "SELECT COUNT(*) FROM MayTram WHERE IDTaiKhoan = @IDTaiKhoan";
            SqlParameter[] parametersMayTram = new SqlParameter[] { new SqlParameter("@IDTaiKhoan", idTaiKhoan) };
            int countMayTram = (int)KetNoiCSDL.ExecuteScalar(queryMayTram, parametersMayTram);

            if (countMayTram > 0)
            {
                return true; // Có ràng buộc
            }

            // Kiểm tra trong bảng HoaDon (thêm nếu có bảng HoaDon)
            string queryHoaDon = "SELECT COUNT(*) FROM HoaDon WHERE IDTaiKhoan = @IDTaiKhoan";
            SqlParameter[] parametersHoaDon = new SqlParameter[] { new SqlParameter("@IDTaiKhoan", idTaiKhoan) };
            int countHoaDon = (int)KetNoiCSDL.ExecuteScalar(queryHoaDon, parametersHoaDon);
            if (countHoaDon > 0)
            {
                return true; // Có ràng buộc
            }


            // Kiểm tra trong bảng LichSuGiaoDich
            string queryLichSuGiaoDich = "SELECT COUNT(*) FROM LichSuGiaoDich WHERE IDTaiKhoan = @IDTaiKhoan";
            SqlParameter[] parametersLichSuGiaoDich = new SqlParameter[] { new SqlParameter("@IDTaiKhoan", idTaiKhoan) };
            int countLichSuGiaoDich = (int)KetNoiCSDL.ExecuteScalar(queryLichSuGiaoDich, parametersLichSuGiaoDich);

            if (countLichSuGiaoDich > 0)
            {
                return true;
            }
            return false; // Không có ràng buộc
        }
    }
}