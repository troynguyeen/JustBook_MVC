using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class HomeController : Controller
    {
        private DB_CT25Team23Entities db;
        private List<ShoppingCartModel> listOfshoppingCartModels;
        public HomeController()
        {
            db = new DB_CT25Team23Entities();
            listOfshoppingCartModels = new List<ShoppingCartModel>();
        }
        // GET: Home

        public ActionResult Index()
        {
            if(Session["MaKH"] != null)
            {
                int MaKH = Int32.Parse(Session["MaKH"].ToString());
                int MaKhachHang = db.GioHangs.Where(x => x.MaKH == MaKH).Select(y => y.MaKH).FirstOrDefault();

                //Lấy tổng đơn hàng của KH set vào thanh Header
                IEnumerable<OrderManagementModel> listOfDonHang = (from trangthai in
                   (from trangthai in db.TrangThaiDonHangs
                    group trangthai by trangthai.MaDH into grp
                    select grp.OrderByDescending(x => x.MaTrangThaiDH).FirstOrDefault())
                        join dh in db.DonHangs on trangthai.MaDH equals dh.MaDH
                        join chitiet in db.ChiTietDonHangs on dh.MaDH equals chitiet.MaDonHang
                        join sp in db.SanPhams on chitiet.MaSP equals sp.MaSP
                        where dh.MaKH == MaKH
                        orderby dh.MaDH descending
                        select new OrderManagementModel()
                        {
                            MaDH = dh.MaDH,
                            ThoiGianTao = dh.ThoiGianTao
                        }
                   ).GroupBy(x => x.MaDH).Select(i => i.FirstOrDefault()).ToList();

                Session["TotalNotification"] = listOfDonHang.Count();

                //Sau khi KH đăng nhập lấy giỏ hàng từ database
                if (Session["CartItem"] == null && MaKhachHang == MaKH)
                {
                    GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);

                    int TongSoLuongMua = 0;
                    if (db.ChiTietGioHangs.Any(model => model.MaGioHang == giohang.MaGH))
                    {
                        var ListOfChiTietGH = db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH).ToList();
                        foreach (var chitiet in ListOfChiTietGH)
                        {
                            ShoppingCartModel cart_model = new ShoppingCartModel();
                            cart_model.MaSP = chitiet.MaSP;
                            cart_model.TenSP = chitiet.TenSP;
                            cart_model.TacGia = chitiet.TacGia;
                            cart_model.SoLuongMua = chitiet.SoLuongMua;
                            cart_model.SoLuong = chitiet.SoLuong;
                            cart_model.DonGia = chitiet.DonGia;
                            cart_model.TongCong = chitiet.TongCong;
                            cart_model.ImagePath = chitiet.ImagePath;

                            TongSoLuongMua += chitiet.SoLuongMua;

                            listOfshoppingCartModels.Add(cart_model);
                        }
                    }

                    Session["CartItem"] = listOfshoppingCartModels;
                    Session["CartCounter"] = TongSoLuongMua;

                    return View();
                }
                return View();
            }

            return View();
        }
    }
}