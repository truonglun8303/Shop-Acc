using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopAcc.Models.EF;
using ShopAcc.Models;

namespace ShopAcc.Controllers
{
    public class AccountUserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: AccountUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(AccountUser model)
        {
           
            if(ModelState.IsValid)
            {
                var user = db.AccountUsers.FirstOrDefault(x => x.Username == model.Username);
               
                if(user == null)
                {
                    db.AccountUsers.Add(model);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Tên đăng nhập này đã tồn tại !";
                    return View();
                }
                
            }
            return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string user, string pass)
        {
            if (ModelState.IsValid)
            {
                var data = db.AccountUsers.Where(x => x.Username.Equals(user) && x.Password.Equals(pass));
                if (data == null)
                {
                    ViewBag.error = "Đăng nhập không thành công";
                }
                else
                {
                    ViewBag.username = data.FirstOrDefault().Username;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }


    }
}