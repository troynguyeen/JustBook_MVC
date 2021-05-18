using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private List<OrderDetailViewModel> listOfDetail;
        public AdminHomeController()
        {
            db = new DB_CT25Team23Entities();
            listOfDetail = new List<OrderDetailViewModel>();
        }

        // GET: AdminHome
        public ActionResult Index()
        {
            if (Session["MaQT"] == null)
            {
                return RedirectToAction("AdminLogin", "Login");
            }

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

            return View(listOfDonHang);
        }

        public ActionResult OrderManagement()
        {
            IEnumerable<OrderManagementModel> listOfDonHang = (from trangthai in 
                (from trangthai in db.TrangThaiDonHangs
                    orderby trangthai.MaTrangThaiDH descending 
                    group trangthai by trangthai.MaDH into grp
                    select grp.OrderByDescending(x => x.MaTrangThaiDH).FirstOrDefault())
                    join dh in db.DonHangs on trangthai.MaDH equals dh.MaDH
                select new OrderManagementModel()
                {
                    MaDH = dh.MaDH,
                    MaKH = dh.MaKH,
                    TenNguoiNhan = dh.TenNguoiNhan,
                    PhoneNguoiNhan = dh.PhoneNguoiNhan,
                    DiaChiNguoiNhan = dh.DiaChiNguoiNhan,
                    ThoiGianTao = dh.ThoiGianTao,
                    PhuongThucThanhToan = dh.PhuongThucThanhToan,
                    TongGiaTriDonHang = dh.TongGiaTriDonHang,
                    TrangThaiDonHang = trangthai.TrangThai
                }
            ).ToList();
            return View(listOfDonHang);
        }

        public ActionResult OrderDetail()
        {
            var currentId_Url = Url.RequestContext.RouteData.Values["id"];

            OrderManagementModel dh_model_url = new OrderManagementModel();
            DonHang dh = db.DonHangs.SingleOrDefault(model => model.MaDH.ToString() == currentId_Url.ToString());
            TrangThaiDonHang trangthai = db.TrangThaiDonHangs.OrderByDescending(x => x.MaTrangThaiDH).FirstOrDefault(model => model.MaDH == dh.MaDH);

            var ListOfChiTietDH = db.ChiTietDonHangs.Where(model => model.MaDonHang == dh.MaDH).ToList();
            foreach (var chitiet in ListOfChiTietDH)
            {
                OrderDetailViewModel detail = new OrderDetailViewModel();
                SanPham sanpham = db.SanPhams.FirstOrDefault(x => x.MaSP == chitiet.MaSP);

                detail.MaChiTietDH = chitiet.MaChiTietDH;
                detail.MaDonHang = dh.MaDH;
                detail.MaSP = chitiet.MaSP;
                detail.SoLuong = chitiet.SoLuong;
                detail.SoLuongConLai = sanpham.SoLuong;
                detail.DonGia = chitiet.DonGia;
                detail.ChietKhau = chitiet.ChietKhau;
                detail.TongTien = chitiet.TongTien;
                detail.TenSP = sanpham.TenSP;
                detail.LoaiSanPham = db.LoaiSanPhams.FirstOrDefault(x => x.MaLoaiSP == sanpham.MaLoaiSP).TenLoaiSP;
                detail.ImagePath = sanpham.ImagePath;

                listOfDetail.Add(detail);
            }

            dh_model_url.MaDH = dh.MaDH;
            dh_model_url.MaKH = dh.MaKH;
            dh_model_url.TenNguoiNhan = dh.TenNguoiNhan;
            dh_model_url.PhoneNguoiNhan = dh.PhoneNguoiNhan;
            dh_model_url.DiaChiNguoiNhan = dh.DiaChiNguoiNhan;
            dh_model_url.ThoiGianTao = dh.ThoiGianTao;
            dh_model_url.PhuongThucThanhToan = dh.PhuongThucThanhToan;
            dh_model_url.TongGiaTriDonHang = dh.TongGiaTriDonHang;
            dh_model_url.TrangThaiDonHang = trangthai.TrangThai;
            dh_model_url.ChiTietDonHang = listOfDetail;

            return View(dh_model_url);
        }

        [HttpGet]
        public JsonResult getInfo(string SelectedID)
        {
            SanPham sp = db.SanPhams.Single(model => model.MaSP.ToString() == SelectedID);
            int SoLuong = sp.SoLuong;
            double DonGia = sp.DonGia;

            return Json(new { Success = true, SoLuong = SoLuong, DonGia = DonGia}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult OrderSaveChanges(int MaDH, string MaSP, string SoLuongMua, string TongTienMonHang, double TongCong, string RemoveId, string TrangThaiDonHang)
        {
            string[] listOfMaSP = MaSP.Split(",".ToCharArray());
            string[] listOfSoLuongMua = SoLuongMua.Split(",".ToCharArray());
            string[] listOfTongTienMonHang = TongTienMonHang.Split(",".ToCharArray());
            string[] listOfRemoveId = RemoveId.Split(",".ToCharArray());

            DonHang dh = db.DonHangs.SingleOrDefault(x => x.MaDH == MaDH);
            dh.TongGiaTriDonHang = TongCong;
            db.SaveChanges();

            //Cập nhật lại số lượng và trạng thái sau khi Lưu thay đổi đơn hàng
            for (int i = 0; i < listOfMaSP.Length; i++)
            {
                var MaSP_tmp = listOfMaSP[i];
                var SoLuongMua_tmp = listOfSoLuongMua[i];
                var TongTienMonHang_tmp = listOfTongTienMonHang[i];

                ChiTietDonHang chitietDH = db.ChiTietDonHangs.FirstOrDefault(x => x.MaDonHang == dh.MaDH && x.MaSP == MaSP_tmp);
                chitietDH.SoLuong = Int32.Parse(SoLuongMua_tmp);
                chitietDH.TongTien = Int32.Parse(TongTienMonHang_tmp);

                db.SaveChanges();
            }

            //Xóa trạng thái bị trùng
            if(db.TrangThaiDonHangs.Any(x => x.MaDH == dh.MaDH && x.TrangThai == TrangThaiDonHang))
            {
                TrangThaiDonHang trangthai_remove = db.TrangThaiDonHangs.FirstOrDefault(x => x.MaDH == dh.MaDH && x.TrangThai == TrangThaiDonHang);
                db.TrangThaiDonHangs.Remove(trangthai_remove);
                db.SaveChanges();
            }
            
            //Cập nhật trạng thái
            TrangThaiDonHang trangthai = new TrangThaiDonHang();
            trangthai.MaDH = dh.MaDH;
            trangthai.TrangThai = TrangThaiDonHang;
            trangthai.ThoiGian = DateTime.Now;

            db.TrangThaiDonHangs.Add(trangthai);
            db.SaveChanges();

            //Xóa sản phẩm trong chi tiết đơn hàng ko tồn tại sau khi Lưu thay đổi đơn hàng
            if (listOfRemoveId.Length > 0 && RemoveId != "null")
            {
                for (int i = 0; i < listOfRemoveId.Length; i++)
                {
                    var RemoveId_tmp = listOfRemoveId[i];

                    ChiTietDonHang chitietDH_remove = db.ChiTietDonHangs.FirstOrDefault(x => x.MaDonHang == dh.MaDH && x.MaSP == RemoveId_tmp);
                    db.ChiTietDonHangs.Remove(chitietDH_remove);
                    db.SaveChanges();
                }
            }

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelOrder(int MaDH, string TrangThaiDonHang)
        {
            DonHang dh = db.DonHangs.SingleOrDefault(x => x.MaDH == MaDH);
            TrangThaiDonHang trangthai = new TrangThaiDonHang();

            trangthai.MaDH = dh.MaDH;
            trangthai.TrangThai = TrangThaiDonHang;
            trangthai.ThoiGian = DateTime.Now;

            db.TrangThaiDonHangs.Add(trangthai);
            db.SaveChanges();

            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteOrder(int MaDH)
        {
            DonHang dh = db.DonHangs.SingleOrDefault(x => x.MaDH == MaDH);
            var listOfDetailOrder = db.ChiTietDonHangs.Where(x => x.MaDonHang == MaDH).ToList();
            var listOfStateOrder = db.TrangThaiDonHangs.Where(x => x.MaDH == MaDH).ToList();

            foreach (var chitet in listOfDetailOrder)
            {
                db.ChiTietDonHangs.Remove(chitet);
                db.SaveChanges();
            }

            foreach (var trangthai in listOfStateOrder)
            {
                db.TrangThaiDonHangs.Remove(trangthai);
                db.SaveChanges();
            }

            db.DonHangs.Remove(dh);
            db.SaveChanges();

            return Json(new { Success = true, Message = "Xóa đơn hàng #" + MaDH + " thành công." }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductManagement()
        {
            IEnumerable<SanPhamViewModel> listOfSanPham = (from sanpham in db.SanPhams
                    join loai_sp in db.LoaiSanPhams on sanpham.MaLoaiSP equals loai_sp.MaLoaiSP
                    select new SanPhamViewModel()
                    {
                        MaSP = sanpham.MaSP,
                        TenSP = sanpham.TenSP,
                        DanhMuc = loai_sp.TenLoaiSP,
                        TacGia = sanpham.TacGia,
                        DonGia = sanpham.DonGia,
                        SoLuong = sanpham.SoLuong,
                        TrangThai = sanpham.TrangThai
                    }
            ).ToList();
            return View(listOfSanPham);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            SanPhamViewModel sp_viewmodel = new SanPhamViewModel();
            sp_viewmodel.CategorySelectListItem = 
                (from loai_sp in db.LoaiSanPhams
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
                string NewImage = sp_viewmodel.MaSP + "_" + DateTime.Now.ToFileTime() + Path.GetExtension(sp_viewmodel.ImagePath.FileName);
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

                if(sp_viewmodel.SoLuong <= 0)
                {
                    sp.TrangThai = "Hết hàng";
                }
                else
                {
                    sp.TrangThai = "Còn hàng";
                }
                
                db.SanPhams.Add(sp);
                db.SaveChanges();
            }
            return Json(new { Success = true, Message = "Sản phẩm đã được thêm mới thành công." }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditProduct (string id)
        {
            var MaSP = id;

            SanPham sanpham = db.SanPhams.FirstOrDefault(x => x.MaSP == MaSP);
            SanPhamViewModel sp_viewmodel = new SanPhamViewModel();

            sp_viewmodel.MaSP = sanpham.MaSP;
            sp_viewmodel.TenSP = sanpham.TenSP;
            sp_viewmodel.TacGia = sanpham.TacGia;
            sp_viewmodel.NXB = sanpham.NXB;
            sp_viewmodel.DonGia = sanpham.DonGia;
            sp_viewmodel.LoaiBia = sanpham.LoaiBia;
            sp_viewmodel.SoLuong = sanpham.SoLuong;
            sp_viewmodel.SoTrang = sanpham.SoTrang;
            sp_viewmodel.KichThuoc = sanpham.KichThuoc;
            sp_viewmodel.TrongLuong = sanpham.TrongLuong;
            sp_viewmodel.ImageLink = sanpham.ImagePath;
            sp_viewmodel.MoTa = sanpham.MoTa;
            sp_viewmodel.MaLoaiSP = sanpham.MaLoaiSP;

            sp_viewmodel.CategorySelectListItem =
                (from loai_sp in db.LoaiSanPhams
                 select new SelectListItem()
                 {
                     Text = loai_sp.TenLoaiSP,
                     Value = loai_sp.MaLoaiSP.ToString(),
                     Selected = true
                 });
            

            return View(sp_viewmodel);
        }

        [HttpPost]
        public JsonResult EditProduct(SanPhamViewModel sp_viewmodel)
        {
            if ((sp_viewmodel.TenSP == null) ||
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
                string NewImage = "";

                if (sp_viewmodel.ImagePath != null)
                {
                    //Xóa image cũ của sp trong folder ImageProduct
                    string path = Server.MapPath("~/ImageProduct/");
                    string[] filePaths = Directory.GetFiles(path);

                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        if(filePaths[i].Contains(sp_viewmodel.MaSP))
                        {
                            FileInfo file = new FileInfo(filePaths[i]);
                            file.Delete();
                        }
                    }

                    //Save as image mới vào folder ImageProduct
                    NewImage = sp_viewmodel.MaSP + "_" + DateTime.Now.ToFileTime() + Path.GetExtension(sp_viewmodel.ImagePath.FileName);
                    sp_viewmodel.ImagePath.SaveAs(Server.MapPath("~/ImageProduct/" + NewImage));
                }
                
                SanPham sp = db.SanPhams.FirstOrDefault(x => x.MaSP == sp_viewmodel.MaSP);

                sp.MaLoaiSP = sp_viewmodel.MaLoaiSP;
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

                if (NewImage != "")
                {
                    sp.ImagePath = "~/ImageProduct/" + NewImage;
                }

                if (sp_viewmodel.SoLuong <= 0)
                {
                    sp.TrangThai = "Hết hàng";
                }
                else
                {
                    sp.TrangThai = "Còn hàng";
                }

                db.SaveChanges();
            }
            return Json(new { Success = true, Message = "Cập nhật sản phẩm thành công." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteProduct (string MaSP)
        {
            SanPham sanpham = db.SanPhams.FirstOrDefault(x => x.MaSP == MaSP);
            db.SanPhams.Remove(sanpham);
            db.SaveChanges();

            //Xóa image của sp trong folder ImageProduct
            string path = Server.MapPath("~/ImageProduct/");
            string[] filePaths = Directory.GetFiles(path);

            for (int i = 0; i < filePaths.Length; i++)
            {
                if (filePaths[i].Contains(MaSP))
                {
                    FileInfo file = new FileInfo(filePaths[i]);
                    file.Delete();
                }
            }

            return Json(new { Success = true, Message = "Xóa sản phẩm #" + MaSP + " thành công." }, JsonRequestBehavior.AllowGet);
        }
    }
}