using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class ShoppingViewModel
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TacGia { get; set; }
        public string NXB { get; set; }
        public double DonGia { get; set; }
        public string MoTa { get; set; }
        public int SoLuong { get; set; }
        public int SoTrang { get; set; }
        public string TrongLuong { get; set; }
        public string KichThuoc { get; set; }
        public string LoaiBia { get; set; }
        public string TrangThai { get; set; }
        public string ImagePath { get; set; }
        public string LoaiSanPham { get; set; }
    }
}