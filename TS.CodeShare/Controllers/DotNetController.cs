using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.CodeShare.App_Start;
using TS.DBConnection;
using TS.ManageContents;


namespace TS.CodeShare.Controllers
{
    public class DotNetController : Controller
    {
        // GET: DotNet

        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());
        ArticalController AC = new ArticalController();

        public ActionResult Index()
        {
            return AC.Search("","DotNet");
        }

        
        public ActionResult Topics(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);
            if (System.IO.File.Exists(Server.MapPath("/Files/" + ct.fileName)))
            {
                ct.ArticalBody = System.IO.File.ReadAllText(Server.MapPath("/Files/" + ct.fileName));
            }

            return View(ct);

            
        }

        public ActionResult Artical(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);
            if (System.IO.File.Exists(Server.MapPath("/Files/" + ct.fileName)))
            {
                ct.ArticalBody = System.IO.File.ReadAllText(Server.MapPath("/Files/" + ct.fileName));
            }

            return View(ct);


        }

        public ActionResult SimpleLoginAspDotNetWinform()
        {
            return View();
        }

        public ActionResult GenerateRandomString()
        {
            return View();
        }
        public string getRandomString(int length, bool includeAlphabet = true, bool includeNumbers = true,bool includespecialChar = false, bool capsOnly = false)
        {
            return Helpers.CommonStuff.GetRandomString(length, includeAlphabet , includeNumbers , includespecialChar , capsOnly);
        }
    }
}