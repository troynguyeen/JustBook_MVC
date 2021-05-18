using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JustBook
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "EditProduct",
                url: "AdminHome/ProductManagement/EditProduct/{id}",
                defaults: new { controller = "AdminHome", action = "EditProduct", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AddProduct",
                url: "AdminHome/ProductManagement/AddProduct",
                defaults: new { controller = "AdminHome", action = "AddProduct" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
