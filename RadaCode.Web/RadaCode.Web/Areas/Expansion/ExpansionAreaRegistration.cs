using System.Web.Mvc;
using System.Web.Routing;

namespace RadaCode.Web.Areas.Expansion
{
    public class ExpansionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Expansion";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var dataTokens = new RouteValueDictionary();
            var ns = new string[] { "RadaCode.Web.Areas.Expansion.Controllers" };

            dataTokens["Namespaces"] = ns;
            dataTokens["Area"] = this.AreaName;

            var areaRoute = new Route(
                                    "Expansion/{controller}/{action}/{id}",                                                           // Route URL
                                    new RouteValueDictionary{
                                            { "area" , this.AreaName}, 
                                            { "controller" , "Expansion"}, 
                                            { "action" , "Index"}, 
                                            { "id" , UrlParameter.Optional }
                                        },
                                    null,
                                    dataTokens,
                                    new MvcRouteHandler()                                              
                                    );

            context.Routes.Insert(2, areaRoute);
        }
    }
}
