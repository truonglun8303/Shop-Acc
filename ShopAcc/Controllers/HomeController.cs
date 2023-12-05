using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models.EF;
using ShopAcc.Models;

namespace ShopAcc.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var item = db.Products;
            return View(item);
        }
        

    }
} 