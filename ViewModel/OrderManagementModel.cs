using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class OrderManagementModel
    {
        public int MaDH { get; set; }
        public int MaKH { get; set; }
        public string TenNguoiNhan { get; set; }
        public int PhoneNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }
        public System.DateTime ThoiGianTao { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public double TongGiaTriDonHang { get; set; }
        public string TrangThaiDonHang { get; set; }
        public virtual ICollection<OrderDetailViewModel> ChiTietDonHang { get; set; }
    }
}