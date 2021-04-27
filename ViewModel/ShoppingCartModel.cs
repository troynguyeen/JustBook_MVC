using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class ShoppingCartModel
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string TacGia { get; set; }
        public int SoLuongMua { get; set; }
        public int SoLuong { get; set; }
        public double DonGia { get; set; }
        public double TongCong { get; set; }
        public string ImagePath { get; set; }
    }
}