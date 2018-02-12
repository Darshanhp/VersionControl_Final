using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CodeRepository.UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "action",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action=RouteParameter.Optional, id = RouteParameter.Optional }
            );
        }
    }
}
