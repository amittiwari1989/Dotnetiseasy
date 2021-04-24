using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.CodeShare.App_Start;
using TS.CodeShare.Models;
using TS.DBConnection;
using TS.ManageContents;

namespace TS.CodeShare.Controllers
{
    public class ArticalController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());

        // GET: Artical

        public ActionResult Index(string Artials, int id = 0)
        {
            if (Artials == "DotNet")
                return DotNet(id);
            else if (Artials == "SQL")
                return SQL(id);
            else if (Artials == "HTML")
                return HTML(id);
            else
                return Search(Artials);
        }

        public ActionResult Search(string search_tags, string Language = "")
        {
            List<code_topics> lst = new List<code_topics>();
            return View(code_topics.Search(con, search_tags, Language).ToList());


        }

        public ActionResult DotNet(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);
            if (ct == null)
            {
                return Search("DotNet");
            }
            if (System.IO.File.Exists(Server.MapPath("/Files/" + ct.fileName)))
            {
                ct.ArticalBody = System.IO.File.ReadAllText(Server.MapPath("/Files/" + ct.fileName));
            }
            return View(ct);
        }

        public ActionResult SQL(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);
            if (ct == null)
            {
                return Search("SQL");
            }
            if (System.IO.File.Exists(Server.MapPath("/Files/" + ct.fileName)))
            {
                ct.ArticalBody = System.IO.File.ReadAllText(Server.MapPath("/Files/" + ct.fileName));
            }
            return View(ct);
        }
        public ActionResult HTML(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);
            if (ct == null)
            {
                return Search("HTML");
            }
            if (System.IO.File.Exists(Server.MapPath("/Files/" + ct.fileName)))
            {
                ct.ArticalBody = System.IO.File.ReadAllText(Server.MapPath("/Files/" + ct.fileName));
            }
            return View(ct);
        }


        public ActionResult PostArtical()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult PostArtical(PostArticalModel model)
        {

            if (Session["uid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (Convert.ToInt32(Session["uid"]) == 0)
            { return RedirectToAction("Login", "Account"); }
            code_topics ct = new code_topics();
            ct.userId = Convert.ToInt32(Session["uid"]);
            ct.Language = model.Language;
            ct.search_tags = model.Subject;
            ct.topic_details = model.topic_details;
            ct.topic_subject = model.Subject;
            ct.Active = 1;
            ct.visible = 1;
            ct.Allow_Comment = 1;
            ct.Language = model.Language;
            ct.created_date = DateTime.Now;
            ct.code_topicscol = model.topic_details;

            string content = model.Content;
            if (!string.IsNullOrEmpty(content))
            {
                Random rnd = new Random();
                string strRnd = Convert.ToString(rnd.Next());

                string FilePath = Server.MapPath("~/Views/HTML/HtmlFile/");
                string fileName = DateTime.Now.ToString("yyyyMMddHHmmssms") + strRnd + ".html";
                if (!Directory.Exists(Server.MapPath("~/Views/HTML/HtmlFile/")))
                    Directory.CreateDirectory(Server.MapPath("~/Views/HTML/HtmlFile/"));

                System.IO.File.WriteAllText(FilePath + fileName, model.Content);
                ct.fileName = "HtmlFile/" + fileName;
            }
            else
            if (!string.IsNullOrEmpty(model.FileName))
            {
                ct.fileName = model.FileName;
            }
            ct.Create(ct, con);

            return PostArtical();
        }


        [AllowAnonymous]
        public ActionResult EditPost(int id)
        {
            code_topics ct = code_topics.GetByid(con, id);

            PostArticalModel model = new PostArticalModel();
            model.topicId = ct.topic_id;
            model.Subject= ct.topic_subject;
            model.Language = ct.Language;
            model.topic_details = ct.topic_details;
            if(!String.IsNullOrEmpty(ct.fileName))
            {
                string FilePath = Server.MapPath("~/Views/HTML/");
                if (System.IO.File.Exists(FilePath+ct.fileName))
                {
                    model.Content = System.IO.File.ReadAllText(FilePath + ct.fileName);
                }
            }



            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditPost(PostArticalModel model)
        {
            
            code_topics ct = new code_topics();
            ct.topic_id = model.topicId;
            ct.Language = model.Language;
            ct.search_tags = model.Subject;
            ct.topic_details = model.topic_details;
            ct.topic_subject = model.Subject;
            ct.Language = model.Language;
            ct.code_topicscol = model.topic_details;

            ct.Update(ct, con);

            return View(model);
        }


        public ActionResult AddComment()
        {
            return View();
        }

        [HttpPost]
        public string AddComment(int topic_id, string Comment)
        {
            ArticalComments comments = new ArticalComments();
            comments.Active = 1;
            comments.commentId = topic_id;
            comments.topic_id = topic_id;
            comments.Comments = Comment;
            comments.created_date = DateTime.Now;
            comments.userId = Convert.ToInt32(Session["uid"]);
            comments.Create(comments, con);

            return "OK";
        }

        public string ViewComment(int topic_id)
        {
            ArticalComments c = new ArticalComments();
            List<ArticalComments> lst = new List<ArticalComments>();
            lst = c.GetByTopicId(topic_id, con);

            return JsonConvert.SerializeObject(lst);

        }

        public ActionResult MyArticals(string Artials, int id = 0)
        {
            return View(code_topics.SearchByUser(con, Convert.ToInt32(Session["uid"])));
        }


    }
}