using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TS.Enums;
using TS.Tickets;
using TS.WebApp.Models;

namespace TS.WebApp.Controllers
{
    public class TicketController : Controller
    {
        // GET: Ticket
        [AllowAnonymous]
        public ActionResult CreateTicket()
        {
            if (Session["cid"] == null)
                return RedirectToAction("Login", "Account");

            int cid = Convert.ToInt32(Session["cid"]);

            TicketVewModel ticket = new TicketVewModel();
            ticket.Status = PopulateStatus(cid);
            ticket.Type = PopulateType(cid);

            var enumData = from TicketPriority e in Enum.GetValues(typeof(TicketPriority))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };

            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");


            ticket.CustomField = PopulateCustomField(cid);

            return View(ticket);
        }

        [HttpPost]
        public ActionResult CreateTicket(TicketVewModel model)
        {

            int cid = Convert.ToInt32(Session["cid"]);

            model.Status = PopulateStatus(cid);
            var selectedItem = model.Status.Find(p => p.Value == model.StatusName.ToString());
            model.Type = PopulateType(cid);
            var selectedItem_t = model.Type.Find(p => p.Value == model.ticket_type.ToString());

            model.Priority = (TicketPriority)model.priority_type;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

        

            Ticket ticket = new Ticket();
            ticket.ticket_subject = model.Subject;
            ticket.ticket_type = Convert.ToInt64(model.ticket_type);
            ticket.ticket_priority = (int)model.priority_type;
            ticket.ticket_status= selectedItem.Text.ToString();
            ticket.ticket_date = DateTime.UtcNow;
            ticket.date_created = DateTime.UtcNow;
            ticket.date_modified = DateTime.UtcNow;
            ticket.uid = Convert.ToInt32(Session["uid"]);
            ticket.cid = Convert.ToInt32(Session["cid"]);
            ticket.custom_field_1 = model.custom_field_1;
            ticket.created_via =1;
            ticket  =ticket.CreateTicket(ticket);

            TicketMessage msg = new TicketMessage();

            msg.Tid = ticket.Tid;
            msg.uid = Convert.ToInt32(Session["uid"]);
            msg.Message = model.Description;
            msg.date_created = DateTime.UtcNow;
            msg.date_modified = DateTime.UtcNow;
            msg = msg.Create(msg);


            var enumData = from TicketPriority e in Enum.GetValues(typeof(TicketPriority))
                           select new
                           {
                               ID = (int)e,
                               Name = e.ToString()
                           };

            ViewBag.EnumList = new SelectList(enumData, "ID", "Name");



            return View(model);
        }

        private static List<SelectListItem> PopulateStatus(int cid)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            List<TicketStatus> lststatus = TicketStatus.GetByClient(cid);

            foreach (TicketStatus item in lststatus)
            {
                items.Add(new SelectListItem
                {
                    Text = item.StatusName.ToString(),
                    Value = item.TSid.ToString()
                });
            }

            
            return items;
        }


        private static List<SelectListItem> PopulateType(int cid)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            List<TicketType> lststatus = TicketType.GetByClient(cid);

            foreach (TicketType item in lststatus)
            {
                items.Add(new SelectListItem
                {
                    Text = item.TypeName.ToString(),
                    Value = item.TTid.ToString()
                });
            }


            return items;

        }

        private static List<CustomFieldMaster> PopulateCustomField(int cid)
        {   
            List<CustomFieldMaster> lst = CustomFieldMaster.GetByClient(cid);
            
            return lst;

        }

        private List<TicketVewModel> _ticket;



        public ActionResult ViewTicket(string status="")
        {
            if (Session["cid"] == null)
                return RedirectToAction("Login", "Account");

            int cid = Convert.ToInt32(Session["cid"]);

            List<Ticket> _ticket = Ticket.GetBycid(cid, status);
            List<TicketVewModel> tables =  new List<TicketVewModel>();
            foreach (Ticket t in _ticket)
            {
                TicketVewModel tm = new TicketVewModel();
                tm.Subject = t.ticket_subject;
                tm.StatusName = t.ticket_status;
                tables.Add(tm);
            }

            return View(tables.ToList());
        }


        [HttpPost]
        public ActionResult ViewTicket(TicketVewModel frm)
        {
            try
            {
                var tickets = GetTickets(frm.StatusName);
                return Json(tickets, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ViewTicket1(FormCollection frm)
        {
            try
            {
                var tickets = GetTickets(frm["s"]);
                return Json(tickets, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<TicketVewModel> GetTickets(string status="")
        {
            int cid = Convert.ToInt32(Session["cid"]);

            List<Ticket> _ticket = Ticket.GetBycid(cid, status);
            List<TicketVewModel> tables = new List<TicketVewModel>();
            foreach (Ticket t in _ticket)
            {
                TicketVewModel tm = new TicketVewModel();
                tm.Subject = t.ticket_subject;
                tm.StatusName = t.ticket_status;
                tm.Priority = (TicketPriority)t.ticket_priority;
                tm.ticket_type = ((Enums.TicketTypePre)t.ticket_type).ToString();
                tables.Add(tm);
            }

            return tables;
        }

        //        public static System.Web.Mvc.SelectList ToSelectList<TEnum>(this TEnum obj)
        //where TEnum : struct, IComparable, IFormattable, IConvertible
        //        {
        //            return new SelectList(TicketPriority.GetValues(typeof(TEnum))
        //            .OfType<TicketPriority>()
        //            .Select(x => new SelectListItem
        //            {
        //                Text = TicketPriority.GetName(typeof(TEnum), x),
        //                Value = (Convert.ToInt32(x))
        //                .ToString()
        //            }), "Value", "Text");
        //        }


        public JsonResult GetFields()
        {
            //string[] str;
            //List<string[]> s = new List<string[]>();
            //s.Add(new string[] { "name", "Subject" });
            //s.Add(new string[] { "name", "StatusName" });

            string s = @"[{ ""name"": ""Subject"" }, { ""name"": ""Status"" }]";

            int uid = Convert.ToInt32(Session["uid"]);

            //List<FieldOrder> fl = FieldOrder.GetByuid(uid);
            //s = "[";
            //foreach (FieldOrder f in fl)
            //{
            //    s += @"{ ""name"": """+ f.FieldName +@""" },";
            //}
            //s += s.TrimEnd(',') + "]";
            //s = @"[{ ""name"": ""Subject"" }, { ""name"": ""Status"" }]";

            s = FieldOrder.GetFields(uid);

            return Json( s, JsonRequestBehavior.AllowGet);
        }

    }


}