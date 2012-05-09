using System.Web.Mvc;
using System.Web.Routing;
using RadaCode.Web.Application.Routing;
using RadaCode.Web.Core.Setttings;

namespace RadaCode.Web.Areas.shipyard
{
    public class ShipyardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "shipyard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var currentHost = DependencyResolver.Current.GetService<IRadaCodeWebSettings>().CurrentHost;

            var domainParams = "shipyard." + currentHost;

            var dataTokens = new RouteValueDictionary();
            var ns = new string[] { "RadaCode.Web.Areas.Shipyard.Controllers" };

            dataTokens["Namespaces"] = ns;

            context.Routes.Insert(1, new DomainRoute(
                                    domainParams,                                                           // Domain with parameters
                                    "",                                                                     // URL with parameters 
                                    new { area = this.AreaName, controller = "Home", action = "Index" },    // Parameter defaults 
                                    dataTokens                                                              // Custom data tokens
                                    ));
        }
    }
}
