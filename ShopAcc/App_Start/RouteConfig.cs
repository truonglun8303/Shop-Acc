using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopAcc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           
            routes.MapRoute(
              name: "detailProduct",
              url: "chi-tiet/{id}",
              defaults: new { controller = "Products", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "ShopAcc.Controllers" }
            );
            routes.MapRoute(
             name: "Shoppingcart",
             url: "gio-hang",
             defaults: new { controller = "Shoppingcart", action = "Index", id = UrlParameter.Optional },
             namespaces: new[] { "ShopAcc.Controllers" }
           );
            routes.MapRoute(
             name: "CheckOut",
             url: "thanh-toan",
             defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
             namespaces: new[] { "ShopAcc.Controllers" }
         );
            routes.MapRoute(
            name: "vnpay_return",
            url: "vnpay_return",
            defaults: new { controller = "ShoppingCart", action = "VnpayReturn", alias = UrlParameter.Optional },
            namespaces: new[] { "ShopAcc.Controllers" }
          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "ShopAcc.Controllers" }
            );
           
        }
    }
}
