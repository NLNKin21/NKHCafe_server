using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Models
{
    public class HoaDonDoAnRequest
    {
        public int IDHoaDon { get; set; }
        public DateTime ThoiGianGui { get; set; }
        public int IdTaiKhoan { get; set; }
        public int IdMay { get; set; }
        public List<ChiTietMon> ChiTiet { get; set; } = new List<ChiTietMon>();
    }
}
