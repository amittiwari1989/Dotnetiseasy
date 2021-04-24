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
    public class ViewCommentController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());

        // GET: ViewComment
        public ActionResult Index()
        {
            int id = ViewBag.topic_id;
            ArticalComments c = new ArticalComments();
            List<ArticalComments> lst = new List<ArticalComments>();
            lst = c.GetByTopicId(id, con);

            return View(lst.ToList());
        }
    }
}