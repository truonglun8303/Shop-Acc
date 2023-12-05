using PagedList;
using ShopAcc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models.EF;
namespace ShopAcc.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Order
        public ActionResult Index(int ? page)
        {
            var items = db.Orders.OrderByDescending(x => x.CreatedDate).ToList();
            if(page == null)
            {
                page = 1;
            }
            var pageNumber = page ?? 1;
            var pageSize = 10;  
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items.ToPagedList(pageNumber,pageSize));
        }
    }
}