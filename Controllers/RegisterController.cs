using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;

namespace JustBook.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register

        [HttpGet]
        public ActionResult Index(int id=0)
        {
            TaiKhoanKH accountModel = new TaiKhoanKH();
            return View(accountModel);
        }

        [HttpPost]
        public ActionResult Index(TaiKhoanKH accountModel)
        {
            using (DB_CT25Team23Entities db = new DB_CT25Team23Entities())
            {
                if(db.TaiKhoanKHs.Any(acc => acc.Email == accountModel.Email))
                {
                    ViewBag.DuplicateMessage = "Email đã tồn tại.";
                    return View("Index", accountModel);
                }

                db.TaiKhoanKHs.Add(accountModel);
                db.SaveChanges();
            }
            ModelState.Clear();
            Session["MaKH"] = accountModel.MaKH;
            Session["TenKH"] = accountModel.TenKH;
            return RedirectToAction("Index", "Home");
        }
    }
}