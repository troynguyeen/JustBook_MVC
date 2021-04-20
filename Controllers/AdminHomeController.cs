using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JustBook.Controllers
{
    public class AdminHomeController : Controller
    {
        // GET: AdminHome
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminAccount()
        {
            return View();
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

        public ActionResult AddProduct()
        {
            return View();
        }

    }
}