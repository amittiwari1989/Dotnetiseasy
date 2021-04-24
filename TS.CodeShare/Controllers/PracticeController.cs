using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TS.CodeShare.App_Start;
using TS.CodeShare.Models;
using TS.DBConnection;
using TS.ManageContents;

namespace TS.CodeShare.Controllers
{
    public class PracticeController : Controller
    {
        Connection con = new Connection(ConnectionStrings.getConnectionStrings(), ConnectionStrings.getServerType());

        // GET: Practice
        public ActionResult Index()
        {
            QuestionGroup qg = new QuestionGroup();
            //return View(qg.GetQuestionSet(con));
            return View(qg.GetQuestionSetDs(con));
        }

        [AllowAnonymous]
        public ActionResult ViewQuestion(int QGid =0)
        {

            if (Session["uid"] == null)
            {
                return Redirect("../Account/Login?ReturnUrl=/Practice/ViewQuestion?QGid=" + QGid);
            }

            if (QGid == 0)
            { return Redirect("Practice"); }

            QuestionMaster Qm = new QuestionMaster();
            QuestionGroup Qg = new QuestionGroup();

            List<QuestionMaster> Ql = new List<QuestionMaster>();

            Qg = Qg.GetById(con, QGid);

            Ql = Qm.GetByGroup(QGid, con); ;
            Session["Qlist"] = Ql;
            if (Ql.Count > 1)
                ViewBag.Last = false;
            else
                ViewBag.Last = true;

            ViewBag.QuestionGroupName = Qg.QuestionGroupName;

            ViewBag.EndTime = DateTime.Now.AddSeconds(Ql.Count * Qg.TimePerQuestion);
            Session["EndTime"] = DateTime.Now.AddSeconds(Ql.Count * Qg.TimePerQuestion).ToString("MMM dd, yyyy HH:mm:ss");
            Session["StartTime"] = DateTime.Now;
            Session["TimeLeft"] = Ql.Count * Qg.TimePerQuestion;
            Ql[0].TotalQuestion = Ql.Count;
            return View(Ql[0]);

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult ViewQuestion(int QGid, QuestionMaster model)
        {


            ViewBag.Last = false;
            ModelState.Remove("QuestionGroup");
            ModelState.Remove("SrNo");
            ModelState.Remove("Answer");


            QuestionMaster Qm = new QuestionMaster();
            List<QuestionMaster> Ql1 = new List<QuestionMaster>();

            List<QuestionMaster> Ql = new List<QuestionMaster>();

            if (Session["Qlist"] == null)
            {
                Ql = Qm.GetByGroup(QGid, con);
                Ql[0].TotalQuestion = Ql.Count;
                Session["Qlist"] = Ql;
            }
            if (model == null)
                return View(Ql[0]);
            else
            {
                Ql = (List<QuestionMaster>)Session["Qlist"];


                if (!ModelState.IsValid || model.Answer == null)
                {
                    ModelState.AddModelError("", "Please Select 1 option.");
                    Ql1 = (Ql.FindAll(w => w.SrNo == model.SrNo));
                    Ql1[0].TotalQuestion = Ql.Count;
                    return View(Ql1[0]);
                }
                DateTime time = Convert.ToDateTime(Session["EndTime"]);
                Session["TimeLeft"] = (time -DateTime.Now).TotalSeconds;


                for (int i = 0; i < Ql.Count; i++)
                {
                    if (Ql[i].SrNo == model.SrNo)
                    {
                        Ql[i].Answer = model.Answer;
                        Session["Qlist"] = Ql;
                        break;
                    }
                }

                if (Ql.Count == model.SrNo || (time <= DateTime.Now))
                {
                    SaveResult(Ql);
                    return RedirectToAction("ViewResult");
                }


                Ql1 = (Ql.FindAll(w => w.SrNo == model.SrNo));

                foreach (QuestionOptions item in Ql1[0].Options)
                {
                    ModelState.Remove(item.Qid.ToString());
                }


                Ql1 = (Ql.FindAll(w => w.SrNo == model.SrNo + 1));
                if (Ql.Count > 0)
                {
                    if (Ql1[0].SrNo == Ql.Count)
                    {
                        ViewBag.Last = true;
                    }
                    return View(Ql1[0]);
                }
                else
                    return View();

            }

        }


        [AllowAnonymous]
        public ActionResult ViewResult(long TestId = 0)
        {
            if (TestId > 0)
                return ViewResult2(TestId);

            List<QuestionMaster> Ql = new List<QuestionMaster>();

            if (Session["Qlist"] != null)
            {
                Ql = (List<QuestionMaster>)Session["Qlist"];
                return View(Ql);
            }
            else
            {
                Redirect("Index");
            }

            return View(Ql);
        }

        [AllowAnonymous]
        public ActionResult ViewResult2(long TestId)
        {
            TestResults tr = new TestResults();
            TestList tl = new TestList();

            List<TestResults> lsttr = new List<TestResults>();

            tl = tl.GetById(TestId, con);
            lsttr = tr.GetByTestId(TestId, con);
            QuestionMaster qm = new QuestionMaster(); 
            List<QuestionMaster> Ql = new List<QuestionMaster>();

            Ql = qm.GetByGroup(tl.QuestionGroup, con);

            foreach (QuestionMaster item in Ql)
            {
                List<TestResults> lsttr1 = new List<TestResults>();
                item.Answer = lsttr.FindAll(m => m.Qid == item.Qid)[0].Answer;
            }

            Session["Qlist"] = Ql;
            //Ql = (List<QuestionMaster>)Session["Qlist"];
            return View(Ql);
            
        }

        [AllowAnonymous]
        public ActionResult ViewQuestionAll(int QGid)
        {
            QuestionMaster Qm = new QuestionMaster();
            QuestionGroup Qg = new QuestionGroup();

            QuestionTestModel model = new QuestionTestModel();

            model.Questions = Qm.GetByGroup(QGid, con);
            model.Name = Qg.GetById(con, QGid).QuestionGroupName;
            return View(model);

        }
        
        [AllowAnonymous]
        public ActionResult AddNewTest()
        {
            if (Session["uid"] == null)
            {
                return Redirect("../Account/Login?ReturnUrl=/Practice/AddNewTest");
            }

            QuestionGroup model = new QuestionGroup();
            return View(model);
        }

        [HttpPost]
        public ActionResult AddNewTest(QuestionGroup model)
        {
            if (Session["uid"] == null)
            {
                return Redirect("../Account/Login?ReturnUrl=/Practice/AddNewTest");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            QuestionGroup qg = new QuestionGroup();

            model.uid = Convert.ToInt32(Session["uid"]);
            model.CreatedDate = DateTime.Now;
            model.Active = 1;

            qg =qg.Create(model, con);

            return Redirect("../Practice/AddMcqQuestions?QgrupId="+qg.QGid);

        }

        [AllowAnonymous]
        public ActionResult MyQuestionSet()
        {
            if (Session["uid"] == null)
            {
                return Redirect("../Account/Login?ReturnUrl=/Practice/AddNewTest");
            }

            QuestionGroup obj = new QuestionGroup();

            //List<QuestionGroup> model = new List<QuestionGroup>();

            DataSet ds = obj.GetByUserDs(con,Convert.ToInt32(Session["uid"]));

            return View(ds);
        }

        [AllowAnonymous]
        public ActionResult AddMcqQuestions(int QgrupId = 1)
        {
            if (Session["uid"] == null)
            {
                return Redirect("../Account/Login?ReturnUrl=/Practice/AddMcqQuestions?QgrupId=" + QgrupId);
            }

            ViewBag.QgrupId = QgrupId;

            AddQuestionModel model = new AddQuestionModel();
            QuestionGroup grp = new QuestionGroup();
            grp = grp.GetById(con, QgrupId);
            model.QgrupId = QgrupId;
            model.Qgrup = grp.QuestionGroupName;
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddMcqQuestions(AddQuestionModel model)
        {
            ViewBag.Values = model.DynamicTextBox;


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            QuestionGroup qg = new QuestionGroup();
            QuestionMaster qm = new QuestionMaster();

            qg = qg.GetById(con, model.QgrupId);

            List<QuestionMaster> lst = new List<QuestionMaster>();
            lst = qm.GetByGroup(model.QgrupId, con);
            qm.GetByGroup(model.QgrupId, con);

            qm.Question = System.Web.HttpUtility.HtmlEncode(model.Question);
            qm.QuestionGroup = model.QgrupId;
            qm.SrNo = lst.Count + 1;
            qm.QLanguage = qg.QLanguage;
            qm.Active = 1;
            qm.QuestionType = "1";
            qm.CurrectAnswer = model.CurrectAnswer;
            qm.AnswerDetails = model.AnswerDescription;
            qm.CreatedDate = DateTime.Now;
            qm.CreatedBy = 1;
            List<QuestionOptions> lstOp = new List<QuestionOptions>();
            int srNo = 65;

            foreach (var item in model.DynamicTextBox)
            {
                char character = (char)srNo;
                string text = character.ToString();

                lstOp.Add(new QuestionOptions(0, 0, text, item, 1, DateTime.Now, 1));
                srNo += 1;
            }
            qm.Options = lstOp;
            qm.Create(qm, con);
            
            return Redirect("AddMcqQuestions?QgrupId="+model.QgrupId);
        }


        public void SaveResult(List<QuestionMaster> Ql)
        {
            DateTime time = Convert.ToDateTime(Session["EndTime"]);
            DateTime stime = Convert.ToDateTime(Session["StartTime"]);
            int currectAnsCount = 0;
            foreach (QuestionMaster item in Ql)
            {
                if (item.CurrectAnswer == item.Answer)
                    currectAnsCount += 1;
            }

            TestList tl = new TestList();
            tl.CreatedBy = Convert.ToInt32(Session["uid"]);
            tl.CreatedDate = DateTime.Now;
            tl.QuestionGroup = Ql[0].QuestionGroup;
            tl.TimeTaken = (int)(time- stime).TotalSeconds;
            tl.TotalQuestion = Ql.Count;
            tl.TestDate = DateTime.Now;
            tl.Attempt = Ql.Count;
            tl.Currect = currectAnsCount;
            tl.Uid = Convert.ToInt32(Session["uid"]);

            tl = tl.Create(tl, con);


            foreach (QuestionMaster item in Ql)
            {
                TestResults tr = new TestResults();
                tr.Tlid = tl.Tlid;
                tr.Qid = item.Qid;
                tr.Answer = item.Answer;
                tr.CreatedBy = Convert.ToInt32(Session["uid"]);
                tr.CreatedDate = DateTime.Now;

                tr.Create(tr, con);
            }

        }


        public ActionResult MyTestHistory()
        {
            TestList tl = new TestList();
            DataSet DS = tl.GetUserTestList(Convert.ToInt32(Session["uid"]), con);
            return View(DS);
        }

        //[HttpPost]
        //public ActionResult AddMcqQuestions(string QgrupId,string Question,string CurrectAnswer,string AnswerDescription, string[] DynamicTextBox)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }
        //    return View();
        //}




    }
}