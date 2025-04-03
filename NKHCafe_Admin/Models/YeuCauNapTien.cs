using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Models
{
    public class YeuCauNapTien
    {
        public int IdTaiKhoan { get; set; }
        public int IdMay { get; set; }
        public decimal SoTien { get; set; }
        public DateTime ThoiGianYeuCau { get; set; } = DateTime.Now;
        public string TrangThai { get; set; } = "Chờ duyệt";
    }
}
