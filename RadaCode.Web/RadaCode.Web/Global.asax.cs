using System.Web.Mvc;
using RadaCode.Web.Application.ViewEngine;

namespace RadaCode.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RadaCodeViewEngine());
        }
    }
}