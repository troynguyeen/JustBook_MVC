using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class LoginController : Controller
    {
        DB_CT25Team23Entities db;
        public LoginController()
        {
            db = new DB_CT25Team23Entities();
        }
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminVerify(JustBook.Models.TaiKhoanQT accountAdminModel)
        {
            var accountAdminDetail = db.TaiKhoanQTs.Where(acc => acc.Email == accountAdminModel.Email && acc.MatKhau == accountAdminModel.MatKhau).FirstOrDefault();

            if (accountAdminDetail == null)
            {
                Session["AdminMessage"] = "Wrong email or password.";
                return View("AdminLogin", accountAdminModel);
            }

            Session["MaQT"] = accountAdminDetail.MaQT;
            Session["TenQT"] = accountAdminDetail.TenQT;

            //Lấy tổng đơn hàng của người QT quản lý set vào thanh Header
            IEnumerable<OrderManagementModel> listOfDonHang = (from trangthai in
            (from trangthai in db.TrangThaiDonHangs
                orderby trangthai.MaTrangThaiDH descending
                group trangthai by trangthai.MaDH into grp
                select grp.OrderByDescending(x => x.MaTrangThaiDH).FirstOrDefault())
                join dh in db.DonHangs on trangthai.MaDH equals dh.MaDH
                orderby dh.MaDH descending
                select new OrderManagementModel()
                {
                    MaDH = dh.MaDH,
                    ThoiGianTao = dh.ThoiGianTao
                }
            ).ToList();

            Session["TotalAdminNotification"] = listOfDonHang.Count();
            return RedirectToAction("Index", "AdminHome");
        }

        [HttpPost]
        public ActionResult Verify(JustBook.Models.TaiKhoanKH accountModel)
        {
            using (DB_CT25Team23Entities db = new DB_CT25Team23Entities())
            {
                var accountDetail = db.TaiKhoanKHs.Where(acc => acc.Email == accountModel.Email && acc.MatKhau == accountModel.MatKhau).FirstOrDefault();
                if(accountDetail == null)
                {
                    accountModel.LoginErrorMessage = "Wrong email or password.";
                    return View("Index", accountModel);
                }
                else
                {
                    int MaKH = accountDetail.MaKH;

                    Session["MaKH"] = accountDetail.MaKH;
                    Session["TenKH"] = accountDetail.TenKH;
                    Session["Phone"] = accountDetail.Phone;
                    Session["DiaChi"] = accountDetail.DiaChi;

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

                    if (Session["CartItem"] != null)
                    {
                        if (db.GioHangs.Any(model => model.MaKH == MaKH))
                        {
                            GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);
                            db.ChiTietGioHangs.RemoveRange(db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH));
                            db.GioHangs.Remove(db.GioHangs.Find(giohang.MaGH));
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index", "Cart");
                    }

                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}