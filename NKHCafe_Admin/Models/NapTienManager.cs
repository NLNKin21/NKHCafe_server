using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Models
{
    public static class NapTienManager
    {
        // Danh sách tạm thời (có thể dùng ConcurrentBag nếu đa luồng)
        public static BindingList<YeuCauNapTien> DanhSachYeuCau = new BindingList<YeuCauNapTien>();

        // Sự kiện khi có yêu cầu mới
        public static event Action<YeuCauNapTien> OnYeuCauMoi;

        public static void ThemYeuCauMoi(YeuCauNapTien yc)
        {
            DanhSachYeuCau.Add(yc);
            OnYeuCauMoi?.Invoke(yc);
        }
    }
}
