using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class DonHangViewModel
    {
        public int MaDH { get; set; }
        public int MaKH { get; set; }
        public int MaGH { get; set; }
        public string DiaChiGiaoHang { get; set; }
        public System.DateTime ThoiGianTao { get; set; }
        public string NguoiTao { get; set; }
        public double TongTien { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public string TenNguoiNhan { get; set; }
        public int PhoneNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }
    }
}