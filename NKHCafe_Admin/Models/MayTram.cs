namespace NKHCafe_Admin.DTO
{
    public class MayTram
    {
        public int IdMay { get; set; }
        public string TenMay { get; set; }
        public string TrangThai { get; set; }
        public int? IdTaiKhoan { get; set; }
        public string TenTaiKhoan { get; set; }
        public System.DateTime? ThoiGianBatDau { get; set; } // Thêm thuộc tính Thời gian bắt đầu
    }
}