using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RadaCode.Web.Application.ActionFilters
{
    public class CoolAuthorize : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var userAuthenticated = filterContext.HttpContext.User.IsInRole("Star");

            if (!userAuthenticated)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                          {
                                              {"controller", "Access"}, 
                                              {"action", "Login"},
                                              {"sourceAction", filterContext.ActionDescriptor}
                                          });
            
        }


    }
}