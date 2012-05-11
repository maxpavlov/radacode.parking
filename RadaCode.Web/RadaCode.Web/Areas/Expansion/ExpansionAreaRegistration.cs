using System.Web.Mvc;

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
            context.MapRoute(
                "Expansion_default",
                "Expansion/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
