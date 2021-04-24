using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TS.CodeShare.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        ArticalController AC = new ArticalController();
        public ActionResult Index(string Artials, int id = 0)
        {
            if (Artials == "DotNet")
                return AC.DotNet(id);
            else if (Artials == "SQL")
                return AC.SQL(id);
            else if (Artials == "HTML")
                return AC.HTML(id);
            else
                return AC.Search(Artials);
        }

        public ActionResult PrivacyPolicy() {
            return View();
        }

        public ActionResult TermsOfService() {
            return View();
        }
    }
}