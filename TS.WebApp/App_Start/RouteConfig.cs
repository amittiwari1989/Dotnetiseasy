﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TS.WebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Ticket", "{status}",
           defaults: new { controller = "Ticket", action = "ViewTicket" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{status}",
                defaults: new { controller = "Account", action = "Login", status= UrlParameter.Optional }
            );
            //routes.MapRoute(
            //    name: "TicketView",
            //    url: "{controller}/{action}/{status}",
            //    defaults : new { controller = "Ticket", action = "ViewTicket", status = UrlParameter.Optional}
            //);
        }
    }
}
