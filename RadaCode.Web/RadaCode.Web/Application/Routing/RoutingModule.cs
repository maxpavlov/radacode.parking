using System.Web.Mvc;
using System.Web.Routing;
using Ninject.Modules;

namespace RadaCode.Web.Application.Routing
{
    public class RoutingModule: NinjectModule
    {
        private readonly RouteCollection _routes;

        public RoutingModule(RouteCollection routesCollection)
        {
            _routes = routesCollection;
        }

        public override void Load()
        {
            _routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            _routes.IgnoreRoute("Scripts/{*pathInfo}");

            _routes.MapRoute(
                "Default",                                               // Route name
                "{controller}/{action}/{id}",                            // URL with parameters
                new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
                new string[] { "RadaCode.Web.Controllers" }              // Namespance
            );
        }
    }
}