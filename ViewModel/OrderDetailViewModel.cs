using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class OrderDetailViewModel
    {
        public int MaChiTietDH { get; set; }
        public int MaDonHang { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int SoLuong { get; set; }
        public int SoLuongConLai { get; set; }
        public double DonGia { get; set; }
        public double ChietKhau { get; set; }
        public double TongTien { get; set; }
        public string LoaiSanPham { get; set; }
        public string ImagePath { get; set; }
    }
}