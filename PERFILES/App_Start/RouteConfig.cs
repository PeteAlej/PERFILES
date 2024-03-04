using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PERFILES
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "ModificarEmpleados",
                url: "Empleados/Modificar/{id}",
                defaults: new { controller = "Empleados", action = "Modificar", id = UrlParameter.Optional }
            );
            /*
            routes.MapRoute(
                name: "GenerarReporte",
                url: "Empleados/{action}",
                defaults: new { controller = "Empleados", action = "Reporte" }
            );

            routes.MapRoute(
                name: "FiltradoReporte",
                url: "Empleados/{controller}/{action}",
                defaults: new { controller = "Reporte", action = "Filtro" }
            );
            */

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
