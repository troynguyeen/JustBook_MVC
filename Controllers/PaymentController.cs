using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class PaymentController : Controller
    {
        private DB_CT25Team23Entities db;
        private List<ShoppingCartModel> listOfshoppingCartModels;
        public PaymentController()
        {
            db = new DB_CT25Team23Entities();
            listOfshoppingCartModels = new List<ShoppingCartModel>();
        }
        // GET: Payment
        public ActionResult Index()
        {
            if(Session["MaKH"] != null)
            {
                listOfshoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
                return View(listOfshoppingCartModels);
            }
            else
            {
                Session["Message"] = "Bạn cần đăng nhập trước khi thực hiện thanh toán.";
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpPost]
        public JsonResult CreateOrder(string TenNguoiNhan, int PhoneNguoiNhan, string DiaChiNguoiNhan, string PhuongThucThanhToan)
        {
            int MaDH = 0;
            int MaKH = Int32.Parse(Session["MaKH"].ToString());
            GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);

            DonHang donhang = new DonHang();
            donhang.MaKH = MaKH;
            donhang.TenNguoiNhan = TenNguoiNhan;
            donhang.PhoneNguoiNhan = PhoneNguoiNhan;
            donhang.DiaChiNguoiNhan = DiaChiNguoiNhan;
            donhang.ThoiGianTao = DateTime.Now;
            donhang.PhuongThucThanhToan = PhuongThucThanhToan;
            donhang.TongGiaTriDonHang = giohang.TongTien;

            db.DonHangs.Add(donhang);
            db.SaveChanges();
            MaDH = donhang.MaDH;

            foreach (var sp in listOfshoppingCartModels)
            {
                ChiTietDonHang ChiTietDH = new ChiTietDonHang();
                ChiTietDH.MaDonHang = MaDH;
                ChiTietDH.MaSP = sp.MaSP;
                ChiTietDH.SoLuong = sp.SoLuongMua;
                ChiTietDH.DonGia = sp.DonGia;
                ChiTietDH.ChietKhau = 15;
                ChiTietDH.TongTien = sp.TongCong;

                db.ChiTietDonHangs.Add(ChiTietDH);
                db.SaveChanges();
            }

            return Json(new { Success = true, Message = "Tạo Đơn hàng thành công."}, JsonRequestBehavior.AllowGet);
        }
    }
}