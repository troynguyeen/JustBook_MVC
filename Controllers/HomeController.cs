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

                //Sau khi đăng nhập lấy giỏ hàng từ database
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

                    return View(listOfshoppingCartModels);
                }
            }
            return View();
        }
    }
}