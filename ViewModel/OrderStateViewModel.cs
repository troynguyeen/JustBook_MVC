using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JustBook.ViewModel
{
    public class OrderStateViewModel
    {
        public int MaTrangThaiDH { get; set; }
        public int MaDH { get; set; }
        public System.DateTime ThoiGian { get; set; }
        public string TrangThai { get; set; }
    }
}