﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopAcc.Controllers
{
    public class BannerController : Controller
    {
        // GET: Banner
        public ActionResult Index()
        {
            return PartialView();
        }
    }
}