using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;
using JustBook.ViewModel;

namespace JustBook.Controllers
{
    public class CartController : Controller
    {
        private DB_CT25Team23Entities db;
        private List<ShoppingCartModel> listOfshoppingCartModels;
        public CartController()
        {
            db = new DB_CT25Team23Entities();
            listOfshoppingCartModels = new List<ShoppingCartModel>();
        }
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["CartItem"] != null)
            {
                listOfshoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;

                int MaKH = 0;
                if(Session["MaKH"] != null)
                {
                    MaKH = Int32.Parse(Session["MaKH"].ToString());
                }

                if (Session["MaKH"] != null && !db.GioHangs.Any(gh => gh.MaKH == MaKH))
                {
                    int MaGH = 0;
                    GioHang giohang = new GioHang();
                    giohang.MaKH = MaKH;

                    foreach (var sp in listOfshoppingCartModels)
                    {
                        giohang.TongTien += sp.TongCong;
                    }

                    db.GioHangs.Add(giohang);
                    db.SaveChanges();
                    MaGH = giohang.MaGH;

                    foreach (var sp in listOfshoppingCartModels)
                    {
                        ChiTietGioHang ChiTietGH = new ChiTietGioHang();
                        ChiTietGH.MaGioHang = MaGH;
                        ChiTietGH.MaSP = sp.MaSP;
                        ChiTietGH.TenSP = sp.TenSP;
                        ChiTietGH.TacGia = sp.TacGia;
                        ChiTietGH.SoLuongMua = sp.SoLuongMua;
                        ChiTietGH.SoLuong = sp.SoLuong;
                        ChiTietGH.DonGia = sp.DonGia;
                        ChiTietGH.TongCong = sp.TongCong;
                        ChiTietGH.ImagePath = sp.ImagePath;

                        db.ChiTietGioHangs.Add(ChiTietGH);
                        db.SaveChanges();
                    }

                    int TongSoLuongMua = 0;
                    double TongCong = 0;
                    for (int i = 0; i < listOfshoppingCartModels.Count; i++)
                    {
                        TongSoLuongMua += listOfshoppingCartModels[i].SoLuongMua;
                        TongCong += listOfshoppingCartModels[i].TongCong;
                        Session["TongSoLuongMua"] = TongSoLuongMua;
                        Session["TongCong_temp"] = string.Format("{0:#,##0 VND}", TongCong);
                        Session["TongCong"] = string.Format("{0:#,##0 VND}", TongCong - TongCong * 0.15);
                    }

                    int CartCounter = TongSoLuongMua;
                    string CartTotal = Session["TongCong"].ToString();
                    string CartTotal_temp = Session["TongCong_temp"].ToString();
                    Session["CartCounter"] = CartCounter;
                    Session["CartItem"] = listOfshoppingCartModels;
                    return View(listOfshoppingCartModels);
                }
                else
                {
                    int TongSoLuongMua = 0;
                    double TongCong = 0;
                    for (int i = 0; i < listOfshoppingCartModels.Count; i++)
                    {
                        TongSoLuongMua += listOfshoppingCartModels[i].SoLuongMua;
                        TongCong += listOfshoppingCartModels[i].TongCong;
                        Session["TongSoLuongMua"] = TongSoLuongMua;
                        Session["TongCong_temp"] = string.Format("{0:#,##0 VND}", TongCong);
                        Session["TongCong"] = string.Format("{0:#,##0 VND}", TongCong - TongCong * 0.15);
                    }

                    int CartCounter = TongSoLuongMua;
                    Session["CartCounter"] = TongSoLuongMua;
                    Session["CartItem"] = listOfshoppingCartModels;

                    return View(listOfshoppingCartModels);
                }
            }
            return View();
        }

        [HttpPost]
        public JsonResult Index(string SelectedID, int QuantityBuying)
        {
            ShoppingCartModel cart_model = new ShoppingCartModel();
            SanPham sp = db.SanPhams.Single(model => model.MaSP.ToString() == SelectedID);
            LoaiSanPham loai_sp = db.LoaiSanPhams.Single(model => model.MaLoaiSP == sp.MaLoaiSP);

            string message = "";

            if (Session["CartCounter"] != null)
            {
                listOfshoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }

            if(listOfshoppingCartModels.Any(model => model.MaSP == SelectedID))
            {
                cart_model = listOfshoppingCartModels.Single(model => model.MaSP == SelectedID);

                if (QuantityBuying <= sp.SoLuong)
                {
                    cart_model.SoLuongMua = cart_model.SoLuongMua + QuantityBuying;

                    if (cart_model.SoLuongMua > cart_model.SoLuong)
                    {
                        cart_model.SoLuongMua = cart_model.SoLuong;
                        message = "Bạn chỉ có thể mua tối đa " + cart_model.SoLuong + " sản phẩm.";
                    }

                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                }
                
            }
            else
            {
                cart_model.MaSP = SelectedID;
                cart_model.ImagePath = sp.ImagePath;
                cart_model.TenSP = sp.TenSP;
                cart_model.TacGia = sp.TacGia;
                cart_model.SoLuong = sp.SoLuong;
                cart_model.DonGia = sp.DonGia; 

                if(QuantityBuying <= sp.SoLuong)
                {
                    cart_model.SoLuongMua = QuantityBuying;
                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                }
                else
                {
                    cart_model.SoLuongMua = sp.SoLuong;
                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                    message = "Bạn chỉ có thể mua tối đa " + cart_model.SoLuong + " sản phẩm.";
                }

                listOfshoppingCartModels.Add(cart_model);
            }

            int TongSoLuongMua = 0;
            double TongCong = 0;
            for (int i = 0; i < listOfshoppingCartModels.Count; i++)
            {
                TongSoLuongMua += listOfshoppingCartModels[i].SoLuongMua;
                TongCong += listOfshoppingCartModels[i].TongCong;
                Session["TongSoLuongMua"] = TongSoLuongMua;
                Session["TongCong_temp"] = string.Format("{0:#,##0 VND}", TongCong);
                Session["TongCong"] = string.Format("{0:#,##0 VND}", TongCong - TongCong * 0.15);
            }

            int CartCounter = Int32.Parse(Session["TongSoLuongMua"].ToString());
            string CartTotal = Session["TongCong"].ToString();
            string CartTotal_temp = Session["TongCong_temp"].ToString();
            Session["CartCounter"] = CartCounter;
            Session["CartItem"] = listOfshoppingCartModels;

            //Update giỏ hàng trong Database
            int MaKH = 0;
            if (Session["MaKH"] != null)
            {
                MaKH = Int32.Parse(Session["MaKH"].ToString());
                //Xóa giỏ hàng cũ trong Database
                if (db.GioHangs.Any(model => model.MaKH == MaKH))
                {
                    GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);
                    var ListOfChiTietGH = db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH).ToList();

                    foreach (var chitiet in ListOfChiTietGH)
                    {
                        db.ChiTietGioHangs.Remove(chitiet);
                        db.SaveChanges();
                    }
                    db.GioHangs.Remove(giohang);
                    db.SaveChanges();
                }

                //Thay thế giỏ hàng cũ bằng cách thêm giỏ hàng mới
                int MaGH = 0;
                GioHang update_giohang = new GioHang();
                update_giohang.MaKH = MaKH;

                foreach (var sp_giohang in listOfshoppingCartModels)
                {
                    update_giohang.TongTien += sp_giohang.TongCong;
                }

                db.GioHangs.Add(update_giohang);
                db.SaveChanges();
                MaGH = update_giohang.MaGH;

                foreach (var so_chitietGH in listOfshoppingCartModels)
                {
                    ChiTietGioHang ChiTietGH = new ChiTietGioHang();
                    ChiTietGH.MaGioHang = MaGH;
                    ChiTietGH.MaSP = so_chitietGH.MaSP;
                    ChiTietGH.TenSP = so_chitietGH.TenSP;
                    ChiTietGH.TacGia = so_chitietGH.TacGia;
                    ChiTietGH.SoLuongMua = so_chitietGH.SoLuongMua;
                    ChiTietGH.SoLuong = so_chitietGH.SoLuong;
                    ChiTietGH.DonGia = so_chitietGH.DonGia;
                    ChiTietGH.TongCong = so_chitietGH.TongCong;
                    ChiTietGH.ImagePath = so_chitietGH.ImagePath;

                    db.ChiTietGioHangs.Add(ChiTietGH);
                    db.SaveChanges();
                }
            }

            return Json(new { Success = true, Counter = CartCounter, TotalPrice = CartTotal, TotalPrice_temp = CartTotal_temp, Message = message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult getSoLuong (string SelectedID)
        {
            SanPham sp = db.SanPhams.Single(model => model.MaSP.ToString() == SelectedID);
            return Json(sp.SoLuong, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateQuantityBuying (string SelectedID, int QuantityBuying)
        {
            ShoppingCartModel cart_model = new ShoppingCartModel();
            SanPham sp = db.SanPhams.Single(model => model.MaSP.ToString() == SelectedID);
            LoaiSanPham loai_sp = db.LoaiSanPhams.Single(model => model.MaLoaiSP == sp.MaLoaiSP);

            string message = "";

            if (Session["CartCounter"] != null)
            {
                listOfshoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;
            }

            if (listOfshoppingCartModels.Any(model => model.MaSP == SelectedID))
            {
                cart_model = listOfshoppingCartModels.Single(model => model.MaSP == SelectedID);

                if (QuantityBuying <= sp.SoLuong)
                {
                    cart_model.SoLuongMua = QuantityBuying;
                    
                    if (cart_model.SoLuongMua > cart_model.SoLuong)
                    {
                        cart_model.SoLuongMua = cart_model.SoLuong;
                        message = "Bạn chỉ có thể mua tối đa " + cart_model.SoLuong + " sản phẩm.";
                    }

                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                }

            }
            else
            {
                cart_model.MaSP = SelectedID;
                cart_model.ImagePath = sp.ImagePath;
                cart_model.TenSP = sp.TenSP;
                cart_model.TacGia = sp.TacGia;
                cart_model.SoLuong = sp.SoLuong;
                cart_model.DonGia = sp.DonGia;

                if (QuantityBuying <= sp.SoLuong)
                {
                    cart_model.SoLuongMua = QuantityBuying;
                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                }
                else
                {
                    cart_model.SoLuongMua = sp.SoLuong;
                    cart_model.TongCong = cart_model.SoLuongMua * cart_model.DonGia;
                    message = "Bạn chỉ có thể mua tối đa " + cart_model.SoLuong + " sản phẩm.";
                }

                listOfshoppingCartModels.Add(cart_model);
            }

            int TongSoLuongMua = 0;
            double TongCong = 0;
            for (int i = 0; i < listOfshoppingCartModels.Count; i++)
            {
                TongSoLuongMua += listOfshoppingCartModels[i].SoLuongMua;
                TongCong += listOfshoppingCartModels[i].TongCong;
                Session["TongSoLuongMua"] = TongSoLuongMua;
                Session["TongCong_temp"] = string.Format("{0:#,##0 VND}", TongCong);
                Session["TongCong"] = string.Format("{0:#,##0 VND}", TongCong - TongCong * 0.15);
            }

            int CartCounter = Int32.Parse(Session["TongSoLuongMua"].ToString());
            string CartTotal = Session["TongCong"].ToString();
            string CartTotal_temp = Session["TongCong_temp"].ToString();
            Session["CartCounter"] = CartCounter;
            Session["CartItem"] = listOfshoppingCartModels;

            //Update giỏ hàng trong Database
            int MaKH = 0;
            if (Session["MaKH"] != null)
            {
                MaKH = Int32.Parse(Session["MaKH"].ToString());
                //Xóa giỏ hàng cũ trong Database
                if (db.GioHangs.Any(model => model.MaKH == MaKH))
                {
                    GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);
                    var ListOfChiTietGH = db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH).ToList();

                    foreach (var chitiet in ListOfChiTietGH)
                    {
                        db.ChiTietGioHangs.Remove(chitiet);
                        db.SaveChanges();
                    }
                    db.GioHangs.Remove(giohang);
                    db.SaveChanges();
                }

                //Thay thế giỏ hàng cũ bằng cách thêm giỏ hàng mới
                int MaGH = 0;
                GioHang update_giohang = new GioHang();
                update_giohang.MaKH = MaKH;

                foreach (var sp_giohang in listOfshoppingCartModels)
                {
                    update_giohang.TongTien += sp_giohang.TongCong;
                }

                db.GioHangs.Add(update_giohang);
                db.SaveChanges();
                MaGH = update_giohang.MaGH;

                foreach (var so_chitietGH in listOfshoppingCartModels)
                {
                    ChiTietGioHang ChiTietGH = new ChiTietGioHang();
                    ChiTietGH.MaGioHang = MaGH;
                    ChiTietGH.MaSP = so_chitietGH.MaSP;
                    ChiTietGH.TenSP = so_chitietGH.TenSP;
                    ChiTietGH.TacGia = so_chitietGH.TacGia;
                    ChiTietGH.SoLuongMua = so_chitietGH.SoLuongMua;
                    ChiTietGH.SoLuong = so_chitietGH.SoLuong;
                    ChiTietGH.DonGia = so_chitietGH.DonGia;
                    ChiTietGH.TongCong = so_chitietGH.TongCong;
                    ChiTietGH.ImagePath = so_chitietGH.ImagePath;

                    db.ChiTietGioHangs.Add(ChiTietGH);
                    db.SaveChanges();
                }
            }

            return Json(new { Success = true, Counter = CartCounter, TotalPrice = CartTotal, TotalPrice_temp = CartTotal_temp, Message = message }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveItemFromCart(string SelectedID)
        {
            ShoppingCartModel cart_model = new ShoppingCartModel();
            listOfshoppingCartModels = Session["CartItem"] as List<ShoppingCartModel>;

            if(listOfshoppingCartModels.Any(model => model.MaSP == SelectedID))
            {
                cart_model = listOfshoppingCartModels.Single(model => model.MaSP == SelectedID);
            }

            bool removeAll = false;
            int CartCounter = Int32.Parse(Session["TongSoLuongMua"].ToString());

            double TongCong = 0;
            for (int i = 0; i < listOfshoppingCartModels.Count; i++)
            {
                TongCong += listOfshoppingCartModels[i].TongCong;
                Session["TongCong_temp"] = string.Format("{0:#,##0 VND}", TongCong - cart_model.TongCong);
                Session["TongCong"] = string.Format("{0:#,##0 VND}", (TongCong - cart_model.TongCong) - (TongCong - cart_model.TongCong) * 0.15);
            }

            CartCounter -= cart_model.SoLuongMua;
            listOfshoppingCartModels.Remove(cart_model);

            int MaKH = 0;

            if(Session["MaKH"] != null)
            {
                MaKH = Int32.Parse(Session["MaKH"].ToString());
            }

            if(CartCounter != 0)
            {
                Session["TongSoLuongMua"] = CartCounter;

                //Update giỏ hàng trong Database
                //Xóa giỏ hàng cũ trong Database
                if (db.GioHangs.Any(model => model.MaKH == MaKH))
                {
                    GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);
                    var ListOfChiTietGH = db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH).ToList();

                    foreach (var chitiet in ListOfChiTietGH)
                    {
                        db.ChiTietGioHangs.Remove(chitiet);
                        db.SaveChanges();
                    }
                    db.GioHangs.Remove(giohang);
                    db.SaveChanges();
                }

                //Thay thế giỏ hàng cũ bằng cách thêm giỏ hàng mới
                int MaGH = 0;
                GioHang update_giohang = new GioHang();
                update_giohang.MaKH = MaKH;

                foreach (var sp_giohang in listOfshoppingCartModels)
                {
                    update_giohang.TongTien += sp_giohang.TongCong;
                }

                db.GioHangs.Add(update_giohang);
                db.SaveChanges();
                MaGH = update_giohang.MaGH;

                foreach (var so_chitietGH in listOfshoppingCartModels)
                {
                    ChiTietGioHang ChiTietGH = new ChiTietGioHang();
                    ChiTietGH.MaGioHang = MaGH;
                    ChiTietGH.MaSP = so_chitietGH.MaSP;
                    ChiTietGH.TenSP = so_chitietGH.TenSP;
                    ChiTietGH.TacGia = so_chitietGH.TacGia;
                    ChiTietGH.SoLuongMua = so_chitietGH.SoLuongMua;
                    ChiTietGH.SoLuong = so_chitietGH.SoLuong;
                    ChiTietGH.DonGia = so_chitietGH.DonGia;
                    ChiTietGH.TongCong = so_chitietGH.TongCong;
                    ChiTietGH.ImagePath = so_chitietGH.ImagePath;

                    db.ChiTietGioHangs.Add(ChiTietGH);
                    db.SaveChanges();
                }
            }
            else
            {
                Session["TongSoLuongMua"] = null;
                removeAll = true;

                //Xóa giỏ hàng cũ trong Database
                if (db.GioHangs.Any(model => model.MaKH == MaKH))
                {
                    GioHang giohang = db.GioHangs.FirstOrDefault(model => model.MaKH == MaKH);
                    var ListOfChiTietGH = db.ChiTietGioHangs.Where(model => model.MaGioHang == giohang.MaGH).ToList();

                    foreach (var chitiet in ListOfChiTietGH)
                    {
                        db.ChiTietGioHangs.Remove(chitiet);
                        db.SaveChanges();
                    }
                    db.GioHangs.Remove(giohang);
                    db.SaveChanges();
                }
            }

            string CartTotal = Session["TongCong"].ToString();
            string CartTotal_temp = Session["TongCong_temp"].ToString();
            Session["CartCounter"] = CartCounter;
            Session["CartItem"] = listOfshoppingCartModels;

            return Json(new { Success = true, Counter = CartCounter, TotalPrice = CartTotal, TotalPrice_temp = CartTotal_temp, RemoveAll = removeAll }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveInfo(string TenKH, string Phone, string DiaChi)
        {
            Session["TenKH_GiaoHang"] = TenKH;
            Session["Phone_GiaoHang"] = Phone;
            Session["DiaChi_GiaoHang"] = DiaChi;

            return Json(new { Success = true, TenKH = TenKH, Phone = Phone, DiaChi = DiaChi }, JsonRequestBehavior.AllowGet);
        }
    }
}