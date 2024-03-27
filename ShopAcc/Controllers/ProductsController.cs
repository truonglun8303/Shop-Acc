using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using ShopAcc.Models;
using ShopAcc.Models.EF;

namespace ShopAcc.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_Product()
        {
            
            var item = db.Products.Where(x => x.IsHome).Take(8).ToList();
            return PartialView(item);
        }

        public ActionResult Detail(int id)
        {
            var item = db.Products.Find(id);
            return View(item);
        }
    }
}