namespace NKHCafe_Admin.DTO
{
    public class TaiKhoan
    {
        public int ID { get; set; }
        public string TenDangNhap { get; set; }
        public string LoaiTaiKhoan { get; set; }
        // Không cần lưu mật khẩu ở đây (DTO)
    }
}