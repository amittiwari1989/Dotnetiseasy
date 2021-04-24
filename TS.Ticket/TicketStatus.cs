using System;
using System.Collections.Generic;
using System.Data;
using TS.DBConnection;

namespace TS.Tickets
{
    public class TicketStatus
    {
        #region Columns
        public long TSid { get; set; }
        public int cid { get; set; }
        public string StatusName { get; set; }
        public int FinalStatus { get; set; }
        public int uid { get; set; }
        public int Active { get; set; }
        public DateTime? created_date { get; set; }
        #endregion

        public TicketStatus() { }

        public TicketStatus(DataRow DR)
        {
            this.TSid = Convert.ToInt64(DR["TSid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.StatusName = Convert.ToString(DR["StatusName"]);
            this.FinalStatus = Convert.ToInt32(DR["FinalStatus"]);
            this.uid = Convert.ToInt32(DR["uid"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.created_date = Convert.ToDateTime(DR["created_date"]);
        }

        public TicketStatus(long TSid, int cid, string StatusName, int FinalStatus, int uid, int Active, DateTime created_date)
        {
            this.TSid = TSid;
            this.cid = cid;
            this.StatusName = StatusName;
            this.FinalStatus = FinalStatus;
            this.uid = uid;
            this.Active = Active;
            this.created_date = created_date;
        }

        public TicketStatus Create(TicketStatus status)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (status.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 = "cid,";
                query2 = "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, status.cid);
            }
            if (string.IsNullOrEmpty(status.StatusName))

            {
                throw new Exception("StatusName is required");
            }
            else
            {
                query1 = "StatusName,";
                query2 = "@StatusName,";
                cmd.Parameters.Add("@StatusName", DBType.VarChar, status.StatusName);
            }
            if (status.FinalStatus == 0)
            {
                throw new Exception("FinalStatus is required");
            }
            else
            {
                query1 = "FinalStatus,";
                query2 = "@FinalStatus,";
                cmd.Parameters.Add("@FinalStatus", DBType.Int, status.FinalStatus);
            }
            if (status.uid == 0)
            {
                throw new Exception("uid is required");
            }
            else
            {
                query1 = "uid,";
                query2 = "@uid,";
                cmd.Parameters.Add("@uid", DBType.Int, status.uid);
            }
            if (status.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 = "Active,";
                query2 = "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, status.Active);
            }
            // Write Here for missing column


            string Query = "INSERT INTO TicketStatus(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            status.TSid = Convert.ToInt32(DT.Rows[0][0]);
            return status;

        }

        public static List<TicketStatus> GetByClient(int cid)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            
            string Query = "SELECT * FROM TicketStatus WHERE Active=1 and (cid=1 OR cid=@cid)";
            
            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@cid", DBType.Int, cid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<TicketStatus> status = new List<TicketStatus>();

            foreach (DataRow DR in DT.Rows)
            {
                status.Add(new TicketStatus(DR));
            }

            return status;

        }
    }
}
