using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RadaCode.Web.Controllers
{
    [Authorize]
    public class AccessController : Controller
    {
        //
        // GET: /Access/

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

    }
}
