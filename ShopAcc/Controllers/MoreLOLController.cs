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
    public class MoreLOLController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: MoreLOL
        public ActionResult Index(int ? page)
        {
            var pageSize = 16;
            if(page == null)
            {
                page = 1;
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var item = db.Products.ToList();
            return View(item.ToPagedList(pageIndex,pageSize));
        }
    }
}