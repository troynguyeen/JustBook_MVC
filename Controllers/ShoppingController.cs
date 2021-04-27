using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class ShoppingController : Controller
    {
        private DB_CT25Team23Entities db;
        private ShoppingViewModel sp_model;
        public ShoppingController()
        {
            db = new DB_CT25Team23Entities();
            sp_model = new ShoppingViewModel();
        }
        // GET: Shopping
        public ActionResult Index()
        {
            IEnumerable<ShoppingViewModel> shoppingViewModels = (from sp in db.SanPhams
                join
                    loai_sp in db.LoaiSanPhams
                    on sp.MaLoaiSP equals loai_sp.MaLoaiSP
                select new ShoppingViewModel()
                {
                    ImagePath = sp.ImagePath,
                    MaSP = sp.MaSP,
                    TenSP = sp.TenSP,
                    TacGia = sp.TacGia,
                    NXB = sp.NXB,
                    DonGia = sp.DonGia,
                    MoTa = sp.MoTa,
                    SoLuong = sp.SoLuong,
                    SoTrang = sp.SoTrang,
                    TrongLuong = sp.TrongLuong,
                    KichThuoc = sp.KichThuoc,
                    LoaiBia = sp.LoaiBia,
                    TrangThai = sp.TrangThai,
                    LoaiSanPham = loai_sp.TenLoaiSP
                }
            ).ToList();
            return View(shoppingViewModels);
        }

        public ActionResult Detail()
        {
            var currentId_Url = Url.RequestContext.RouteData.Values["id"];

            ShoppingViewModel sp_model_url = new ShoppingViewModel();
            SanPham sp = db.SanPhams.Single(model => model.MaSP.ToString() == currentId_Url.ToString());
            LoaiSanPham loai_sp = db.LoaiSanPhams.Single(model => model.MaLoaiSP == sp.MaLoaiSP);

            sp_model_url.MaSP = currentId_Url.ToString();
            sp_model_url.TenSP = sp.TenSP;
            sp_model_url.LoaiSanPham = loai_sp.TenLoaiSP;
            sp_model_url.TacGia = sp.TacGia;
            sp_model_url.NXB = sp.NXB;
            sp_model_url.DonGia = sp.DonGia;
            sp_model_url.MoTa = sp.MoTa;
            sp_model_url.SoLuong = sp.SoLuong;
            sp_model_url.SoTrang = sp.SoTrang;
            sp_model_url.TrongLuong = sp.TrongLuong;
            sp_model_url.KichThuoc = sp.KichThuoc;
            sp_model_url.LoaiBia = sp.LoaiBia;
            sp_model_url.TrangThai = sp.TrangThai;
            sp_model_url.ImagePath = sp.ImagePath;

            return View(sp_model_url);
        }
    }
}