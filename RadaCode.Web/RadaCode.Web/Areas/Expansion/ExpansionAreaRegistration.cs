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
                "ExpansionDef",
                "Expansion/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "ExpansionMap", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
