using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using TS.DBConnection;

namespace TS.Tickets
{
    [Table("TicketMessage")]
    public class TicketMessage
    {
        #region Columns
        [Key]
        public int Mid { get; set; }
        public int Tid { get; set; }
        public int uid { get; set; }
        public string Message { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }



        #endregion

        public TicketMessage() { }

        public TicketMessage(int Mid, int Tid, int uid, string Message, DateTime date_created, DateTime date_modified)
        {
            this.Mid = Mid;
            this.Tid = Tid;
            this.uid = uid;
            this.Message = Message;
            this.date_created = date_created;
            this.date_modified = date_modified;
        }

        public TicketMessage(DataRow DR)
        {
            this.Mid = Convert.ToInt32(DR["Mid"]);
            this.Tid = Convert.ToInt32(DR["Tid"]);
            this.uid = Convert.ToInt32(DR["uid"]);
            this.Message = Convert.ToString(DR["Message"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
        }

        public TicketMessage Create(TicketMessage ticketMsg)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (ticketMsg.Tid == 0)
            {
                throw new Exception("Tid is required");
            }
            else
            {
                query1 += "Tid,";
                query2 += "@Tid,";
                cmd.Parameters.Add("@Tid", DBType.Int, ticketMsg.Tid);
            }
            if (ticketMsg.uid == 0)
            {
                throw new Exception("uid is required");
            }
            else
            {
                query1 += "uid,";
                query2 += "@uid,";
                cmd.Parameters.Add("@uid", DBType.Int, ticketMsg.uid);
            }
            if (string.IsNullOrEmpty(ticketMsg.Message))

            {
                throw new Exception("Message is required");
            }
            else
            {
                query1 += "Message,";
                query2 += "@Message,";
                cmd.Parameters.Add("@Message", DBType.VarChar, ticketMsg.Message);
            }
            if (ticketMsg.date_created == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date created is required");
            }
            else
            {
                query1 += "date_created,";
                query2 += "@date_created,";
                cmd.Parameters.Add("@date_created", DBType.DateTime, ticketMsg.date_created);
            }
            if (ticketMsg.date_modified == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date modified is required");
            }
            else
            {
                query1 += "date_modified,";
                query2 += "@date_modified,";
                cmd.Parameters.Add("@date_modified", DBType.DateTime, ticketMsg.date_modified);
            }

            string Query = "INSERT INTO TicketMessage(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            ticketMsg.Mid = Convert.ToInt32(DT.Rows[0][0]);
            return ticketMsg;

        }

    }
    
}
