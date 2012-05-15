using System.Web.Mvc;
using RadaCode.Web.Application.ActionFilters;

namespace RadaCode.Web.Areas.Expansion.Controllers
{
    public class ExpansionMapController : Controller
    {
        //
        // GET: /Expansion/ExpansionMap/
        [CoolAuthorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
