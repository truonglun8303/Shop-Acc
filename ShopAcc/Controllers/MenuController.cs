using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models.EF;
using ShopAcc.Models;
namespace ShopAcc.Controllers
{
    public class MenuController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult menucategory()
        {
            var item = db.ProductCategories;
            return PartialView(item);
        }
    }
}