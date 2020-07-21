using CMS.WebApi.CustomHandler;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.ExceptionHandling;

namespace CMS.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //var cors = new EnableCorsAttribute("*","*", "*");
            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );


        }
    }
}
