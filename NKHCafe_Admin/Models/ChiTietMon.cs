using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Models
{
    public class ChiTietMon
    {
        public int IDMon { get; set; }
        public string TenMon { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public decimal ThanhTien => SoLuong * DonGia;

        public ChiTietMon() { }

        public ChiTietMon(int id, string ten, int sl, decimal gia)
        {
            IDMon = id;
            TenMon = ten;
            SoLuong = sl;
            DonGia = gia;
        }
    }
}
