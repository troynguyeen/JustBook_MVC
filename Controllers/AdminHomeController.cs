using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class AdminHomeController : Controller
    {
        private DB_CT25Team23Entities db;
        public AdminHomeController()
        {
            db = new DB_CT25Team23Entities();
        }

        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminAccount()
        {
            if (Session["MaQT"] == null)
            {
                return RedirectToAction("AdminLogin", "Login");
            }
            int maQt = Int32.Parse(Session["MaQT"].ToString());
            var admin = db.TaiKhoanQTs.FirstOrDefault(ad => ad.MaQT == maQt);

            return View(admin);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminEditInformation(TaiKhoanQT admin)
        {
            if(admin == null)
            {
                return RedirectToAction("Index");
            }
            var adminE = db.TaiKhoanQTs.FirstOrDefault(ad => ad.MaQT == admin.MaQT);
            if (adminE == null)
            {
                return RedirectToAction("Index");
            }
            adminE.GioiTinh = admin.GioiTinh;
            adminE.TenQT = admin.TenQT;
            adminE.Email = admin.Email;
            adminE.NgaySinh = admin.NgaySinh;
            adminE.Phone = admin.Phone;
            db.SaveChanges();
            Session["TenQT"] = admin.TenQT;
            return RedirectToAction("AdminAccount");
        }
        public ActionResult AdminNotification()
        {
            return View();
        }

        public ActionResult OrderManagement()
        {
            return View();
        }

        public ActionResult ProductManagement()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            SanPhamViewModel sp_viewmodel = new SanPhamViewModel();
            sp_viewmodel.CategorySelectListItem = (from loai_sp in db.LoaiSanPhams
                                                   select new SelectListItem()
                                                   {
                                                       Text = loai_sp.TenLoaiSP,
                                                       Value = loai_sp.MaLoaiSP.ToString(),
                                                       Selected = true
                                                   });

            return View(sp_viewmodel);
        }

        [HttpPost]
        public JsonResult AddProduct(SanPhamViewModel sp_viewmodel)
        {
            if (db.SanPhams.Any(sanpham => sanpham.MaSP == sp_viewmodel.MaSP))
            {
                return Json(new { Success = false, Message = "Mã sản phẩm đã tồn tại" }, JsonRequestBehavior.AllowGet);
            }

            if ((sp_viewmodel.ImagePath == null) ||
                    (sp_viewmodel.MaSP == null) ||
                    (sp_viewmodel.TenSP == null) ||
                    (sp_viewmodel.TacGia == null) ||
                    (sp_viewmodel.NXB == null) ||
                    (sp_viewmodel.DonGia == null) ||
                    (sp_viewmodel.MoTa == null) ||
                    (sp_viewmodel.SoLuong == null) ||
                    (sp_viewmodel.SoTrang == null) ||
                    (sp_viewmodel.TrongLuong == null) ||
                    (sp_viewmodel.KichThuoc == null))
            {
                return Json(new { Success = false, Message = "Vui lòng nhập đầy đủ thông tin." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string NewImage = sp_viewmodel.MaSP + Path.GetExtension(sp_viewmodel.ImagePath.FileName);
                sp_viewmodel.ImagePath.SaveAs(Server.MapPath("~/ImageProduct/" + NewImage));

                SanPham sp = new SanPham();
                sp.ImagePath = "~/ImageProduct/" + NewImage;
                sp.MaLoaiSP = sp_viewmodel.MaLoaiSP;
                sp.MaSP = sp_viewmodel.MaSP;
                sp.TenSP = sp_viewmodel.TenSP;
                sp.TacGia = sp_viewmodel.TacGia;
                sp.NXB = sp_viewmodel.NXB;
                sp.DonGia = sp_viewmodel.DonGia;
                sp.MoTa = sp_viewmodel.MoTa;
                sp.SoLuong = sp_viewmodel.SoLuong;
                sp.SoTrang = sp_viewmodel.SoTrang;
                sp.TrongLuong = sp_viewmodel.TrongLuong;
                sp.KichThuoc = sp_viewmodel.KichThuoc;
                sp.LoaiBia = sp_viewmodel.LoaiBia;
                sp.TrangThai = sp_viewmodel.TrangThai;
                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
            return Json(new { Success = true, Message = "Sản phẩm đã được thêm mới thành công." }, JsonRequestBehavior.AllowGet);
        }

    }
}