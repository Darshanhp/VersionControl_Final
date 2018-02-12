using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;

namespace CodeRep.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "ActionApi",
               routeTemplate: "api/{controller}/{action}/{id}",
               defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "userproject",
               routeTemplate: "api/user/{action}/{name}",
               defaults: new { controller = "user", name = RouteParameter.Optional },
               constraints: new { name = @"^[a-z]+$" }
            );

            config.Routes.MapHttpRoute(
               name: "Projects",
               routeTemplate: "api/projects/{Projectid}/files/{Fileid}",
               defaults: new { controller = "projects", Fileid = RouteParameter.Optional }
           );




            // config.Routes.MapHttpRoute(
            //    name: "ProjectsAction",
            //    routeTemplate: "api/{controller}/{action}"
            //);
            // config.Routes.MapHttpRoute(
            //    name: "ProjectsActionname",
            //    routeTemplate: "api/projects/{name}/{id}",
            //    defaults: new { controller = "projects", name = RouteParameter.Optional },
            // constraints: new { name = @"^[a-z]+$" }
            //);

            // config.Routes.MapHttpRoute(
            //     name: "Files",
            //     routeTemplate: "api/files/{id}",
            //     defaults: new { controller = "files", id = RouteParameter.Optional }
            // );
        }
    }
}
