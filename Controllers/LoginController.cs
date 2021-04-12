using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;

namespace JustBook.Controllers
{
    public class LoginController : Controller
    {

        // GET: Login
        public ActionResult Index()
        {
            return View();
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
                    Session["MaKH"] = accountDetail.MaKH;
                    Session["TenKH"] = accountDetail.TenKH;
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