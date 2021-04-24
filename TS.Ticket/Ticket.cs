using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.Tickets
{
    public class Ticket
    {
        #region Columns

        public int Tid { get; set; }
        public int cid { get; set; }
        public int uid { get; set; }
        public long ticket_type { get; set; }
        public int ticket_priority { get; set; }
        public string ticket_status { get; set; }
        public string ticket_subject { get; set; }
        public int? created_via { get; set; }
        public DateTime ticket_date { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public string custom_field_1 { get; set; }
        public string custom_field_2 { get; set; }
        public string custom_field_3 { get; set; }
        public string custom_field_4 { get; set; }
        public string custom_field_5 { get; set; }
        public string custom_field_6 { get; set; }
        public string custom_field_7 { get; set; }
        public string custom_field_8 { get; set; }
        public string custom_field_9 { get; set; }
        public string custom_field_10 { get; set; }
        public string custom_field_11 { get; set; }
        public string custom_field_12 { get; set; }
        public string custom_field_13 { get; set; }
        public string custom_field_14 { get; set; }
        public string custom_field_15 { get; set; }
        public string custom_field_16 { get; set; }
        public string custom_field_17 { get; set; }
        public string custom_field_18 { get; set; }
        public string custom_field_19 { get; set; }
        public string custom_field_20 { get; set; }

        #endregion

        public Ticket() { }

        public Ticket(int Tid, int cid, int uid, long ticket_type, byte ticket_priority, string ticket_status, string ticket_subject, int created_via, DateTime ticket_date, DateTime date_created, DateTime date_modified, string custom_field_1, string custom_field_2, string custom_field_3, string custom_field_4, string custom_field_5, string custom_field_6, string custom_field_7, string custom_field_8, string custom_field_9, string custom_field_10, string custom_field_11, string custom_field_12, string custom_field_13, string custom_field_14, string custom_field_15, string custom_field_16, string custom_field_17, string custom_field_18, string custom_field_19, string custom_field_20)
        {
            this.Tid = Tid;
            this.cid = cid;
            this.uid = uid;
            this.ticket_type = ticket_type;
            this.ticket_priority = ticket_priority;
            this.ticket_status = ticket_status;
            this.ticket_subject = ticket_subject;
            this.created_via = created_via;
            this.ticket_date = ticket_date;
            this.date_created = date_created;
            this.date_modified = date_modified;
            this.custom_field_1 = custom_field_1;
            this.custom_field_2 = custom_field_2;
            this.custom_field_3 = custom_field_3;
            this.custom_field_4 = custom_field_4;
            this.custom_field_5 = custom_field_5;
            this.custom_field_6 = custom_field_6;
            this.custom_field_7 = custom_field_7;
            this.custom_field_8 = custom_field_8;
            this.custom_field_9 = custom_field_9;
            this.custom_field_10 = custom_field_10;
            this.custom_field_11 = custom_field_11;
            this.custom_field_12 = custom_field_12;
            this.custom_field_13 = custom_field_13;
            this.custom_field_14 = custom_field_14;
            this.custom_field_15 = custom_field_15;
            this.custom_field_16 = custom_field_16;
            this.custom_field_17 = custom_field_17;
            this.custom_field_18 = custom_field_18;
            this.custom_field_19 = custom_field_19;
            this.custom_field_20 = custom_field_20;
        }

        public Ticket(DataRow DR)
        {
            this.Tid = Convert.ToInt32(DR["Tid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.uid = Convert.ToInt32(DR["uid"]);
            this.ticket_type = Convert.ToInt64(DR["ticket_type"]);
            this.ticket_priority = Convert.ToInt32(DR["ticket_priority"]);
            this.ticket_status = Convert.ToString(DR["ticket_status"]);
            this.ticket_subject = Convert.ToString(DR["ticket_subject"]);
            this.created_via = Convert.ToInt32(DR["created_via"]);
            this.ticket_date = Convert.ToDateTime(DR["ticket_date"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
            this.custom_field_1 = Convert.ToString(DR["custom_field_1"]);
            this.custom_field_2 = Convert.ToString(DR["custom_field_2"]);
            this.custom_field_3 = Convert.ToString(DR["custom_field_3"]);
            this.custom_field_4 = Convert.ToString(DR["custom_field_4"]);
            this.custom_field_5 = Convert.ToString(DR["custom_field_5"]);
            this.custom_field_6 = Convert.ToString(DR["custom_field_6"]);
            this.custom_field_7 = Convert.ToString(DR["custom_field_7"]);
            this.custom_field_8 = Convert.ToString(DR["custom_field_8"]);
            this.custom_field_9 = Convert.ToString(DR["custom_field_9"]);
            this.custom_field_10 = Convert.ToString(DR["custom_field_10"]);
            this.custom_field_11 = Convert.ToString(DR["custom_field_11"]);
            this.custom_field_12 = Convert.ToString(DR["custom_field_12"]);
            this.custom_field_13 = Convert.ToString(DR["custom_field_13"]);
            this.custom_field_14 = Convert.ToString(DR["custom_field_14"]);
            this.custom_field_15 = Convert.ToString(DR["custom_field_15"]);
            this.custom_field_16 = Convert.ToString(DR["custom_field_16"]);
            this.custom_field_17 = Convert.ToString(DR["custom_field_17"]);
            this.custom_field_18 = Convert.ToString(DR["custom_field_18"]);
            this.custom_field_19 = Convert.ToString(DR["custom_field_19"]);
            this.custom_field_20 = Convert.ToString(DR["custom_field_20"]);

        }

        public Ticket CreateTicket(Ticket ticket)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (ticket.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 += "cid,";
                query2 += "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, ticket.cid);
            }
            if (ticket.uid == 0)
            {
                throw new Exception("uid is required");
            }
            else
            {
                query1 += "uid,";
                query2 += "@uid,";
                cmd.Parameters.Add("@uid", DBType.Int, ticket.uid);
            }
            if (ticket.ticket_type == 0)
            {
                throw new Exception("ticket type is required");
            }
            else
            {
                query1 += "ticket_type,";
                query2 += "@ticket_type,";
                cmd.Parameters.Add("@ticket_type", DBType.Int, ticket.ticket_type);
            }
            if (ticket.ticket_priority == 0)
            {
                throw new Exception("ticket priority is required");
            }
            else
            {
                query1 += "ticket_priority,";
                query2 += "@ticket_priority,";
                cmd.Parameters.Add("@ticket_priority", DBType.Int, ticket.ticket_priority);
            }
            if (string.IsNullOrEmpty(ticket.ticket_status))

            {
                throw new Exception("ticket status is required");
            }
            else
            {
                query1 += "ticket_status,";
                query2 += "@ticket_status,";
                cmd.Parameters.Add("@ticket_status", DBType.VarChar, ticket.ticket_status);
            }
            if (string.IsNullOrEmpty(ticket.ticket_subject))

            {
                throw new Exception("ticket subject is required");
            }
            else
            {
                query1 += "ticket_subject,";
                query2 += "@ticket_subject,";
                cmd.Parameters.Add("@ticket_subject", DBType.VarChar, ticket.ticket_subject);
            }
            query1 += "created_via,";
            query2 += "@created_via,";
            cmd.Parameters.Add("@created_via", DBType.Int, ticket.created_via);

            if (ticket.ticket_date == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("ticket date is required");
            }
            else
            {
                query1 += "ticket_date,";
                query2 += "@ticket_date,";
                cmd.Parameters.Add("@ticket_date", DBType.DateTime, ticket.ticket_date);
            }
            if (ticket.date_created == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date created is required");
            }
            else
            {
                query1 += "date_created,";
                query2 += "@date_created,";
                cmd.Parameters.Add("@date_created", DBType.DateTime, ticket.date_created);
            }
            if (ticket.date_modified == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date modified is required");
            }
            else
            {
                query1 += "date_modified,";
                query2 += "@date_modified,";
                cmd.Parameters.Add("@date_modified", DBType.DateTime, ticket.date_modified);
            }
            if (!string.IsNullOrEmpty(ticket.custom_field_1))
            {
                query1 += "custom_field_1,";
                query2 += "@custom_field_1,";
                cmd.Parameters.Add("@custom_field_1", DBType.VarChar, ticket.custom_field_1);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_2))
            {
                query1 += "custom_field_2,";
                query2 += "@custom_field_2,";
                cmd.Parameters.Add("@custom_field_2", DBType.VarChar, ticket.custom_field_2);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_3))
            {
                query1 += "custom_field_3,";
                query2 += "@custom_field_3,";
                cmd.Parameters.Add("@custom_field_3", DBType.VarChar, ticket.custom_field_3);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_4))
            {
                query1 += "custom_field_4,";
                query2 += "@custom_field_4,";
                cmd.Parameters.Add("@custom_field_4", DBType.VarChar, ticket.custom_field_4);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_5))
            {
                query1 += "custom_field_5,";
                query2 += "@custom_field_5,";
                cmd.Parameters.Add("@custom_field_5", DBType.VarChar, ticket.custom_field_5);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_6))
            {
                query1 += "custom_field_6,";
                query2 += "@custom_field_6,";
                cmd.Parameters.Add("@custom_field_6", DBType.VarChar, ticket.custom_field_6);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_7))
            {
                query1 += "custom_field_7,";
                query2 += "@custom_field_7,";
                cmd.Parameters.Add("@custom_field_7", DBType.VarChar, ticket.custom_field_7);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_8))
            {
                query1 += "custom_field_8,";
                query2 += "@custom_field_8,";
                cmd.Parameters.Add("@custom_field_8", DBType.VarChar, ticket.custom_field_8);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_9))
            {
                query1 += "custom_field_9,";
                query2 += "@custom_field_9,";
                cmd.Parameters.Add("@custom_field_9", DBType.VarChar, ticket.custom_field_9);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_10))
            {
                query1 += "custom_field_10,";
                query2 += "@custom_field_10,";
                cmd.Parameters.Add("@custom_field_10", DBType.VarChar, ticket.custom_field_10);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_11))
            {
                query1 += "custom_field_11,";
                query2 += "@custom_field_11,";
                cmd.Parameters.Add("@custom_field_11", DBType.VarChar, ticket.custom_field_11);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_12))
            {
                query1 += "custom_field_12,";
                query2 += "@custom_field_12,";
                cmd.Parameters.Add("@custom_field_12", DBType.VarChar, ticket.custom_field_12);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_13))
            {
                query1 += "custom_field_13,";
                query2 += "@custom_field_13,";
                cmd.Parameters.Add("@custom_field_13", DBType.VarChar, ticket.custom_field_13);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_14))
            {
                query1 += "custom_field_14,";
                query2 += "@custom_field_14,";
                cmd.Parameters.Add("@custom_field_14", DBType.VarChar, ticket.custom_field_14);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_15))
            {
                query1 += "custom_field_15,";
                query2 += "@custom_field_15,";
                cmd.Parameters.Add("@custom_field_15", DBType.VarChar, ticket.custom_field_15);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_16))
            {
                query1 += "custom_field_16,";
                query2 += "@custom_field_16,";
                cmd.Parameters.Add("@custom_field_16", DBType.VarChar, ticket.custom_field_16);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_17))
            {
                query1 += "custom_field_17,";
                query2 += "@custom_field_17,";
                cmd.Parameters.Add("@custom_field_17", DBType.VarChar, ticket.custom_field_17);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_18))
            {
                query1 += "custom_field_18,";
                query2 += "@custom_field_18,";
                cmd.Parameters.Add("@custom_field_18", DBType.VarChar, ticket.custom_field_18);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_19))
            {
                query1 += "custom_field_19,";
                query2 += "@custom_field_19,";
                cmd.Parameters.Add("@custom_field_19", DBType.VarChar, ticket.custom_field_19);
            }

            if (!string.IsNullOrEmpty(ticket.custom_field_20))
            {
                query1 += "custom_field_20,";
                query2 += "@custom_field_20,";
                cmd.Parameters.Add("@custom_field_20", DBType.VarChar, ticket.custom_field_20);
            }

            string Query = "INSERT INTO Ticket(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            ticket.Tid = Convert.ToInt32(DT.Rows[0][0]);
            return ticket;


        }

        //public static List<Ticket> GetBycid(int cid)
        //{
        //    Connection con = new Connection();
        //    Command cmd = new Command();

        //    string Query = "SELECT * FROM Ticket WHERE cid=@cid";

        //    cmd.CommandText = Query;
        //    cmd.Parameters.AddWithValue("@cid", DBType.Int, cid);
        //    cmd.CommandType = DBConnection.CommandType.Text;
        //    con.cmd = cmd;

        //    DataTable DT = con.getDataTable();

        //    List<Ticket> obj = new List<Ticket>();

        //    foreach (DataRow DR in DT.Rows)
        //    {
        //        obj.Add(new Ticket(DR));
        //    }

        //    return obj;

        //}

        public static List<Ticket> GetBycid(int cid, string status = "")
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM Ticket WHERE cid=@cid " + (!String.IsNullOrEmpty(status) ? "AND ticket_status=@ticket_status" : "");

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@cid", DBType.Int, cid);
            if (!String.IsNullOrEmpty(status))
                cmd.Parameters.AddWithValue("@ticket_status", DBType.VarChar, status);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<Ticket> obj = new List<Ticket>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new Ticket(DR));
            }

            return obj;

        }

    }


}
