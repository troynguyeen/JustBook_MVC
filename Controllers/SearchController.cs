using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustBook.Models;

namespace JustBook.Controllers
{
    public class SearchController : Controller
    {
        // GET: Search
        DB_CT25Team23Entities db =  new DB_CT25Team23Entities();
        public ActionResult Index(string searching)
        {
            return View(db.SanPhams.Where(x => x.TenSP.Contains(searching) || searching == null).ToList());
        }

    }
}