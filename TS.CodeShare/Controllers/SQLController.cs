using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.CodeShare.App_Start;
using TS.DBConnection;
using TS.ManageContents;

namespace TS.CodeShare.Controllers
{
    public class SQLController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());
        ArticalController AC = new ArticalController();

        // GET: SQL
        public ActionResult Index()
        {
            return AC.Search("","SQL");
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

    }
}