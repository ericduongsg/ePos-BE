using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WhiteCoat.API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "DefaultWebApi",
            //    url: "v1/{controller}/{action}/{id}",
            //    defaults: new { controller = "PaymentGateway", action = "AddCard", id = UrlParameter.Optional }
            //);



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "PaymentGateway", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}