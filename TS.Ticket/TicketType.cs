using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.Tickets
{
    public class TicketType
    {
        #region Columns  
        public long TTid { get; set; }
        public int cid { get; set; }
        public string TypeName { get; set; }
        public int uid { get; set; }
        public int Active { get; set; }
        public DateTime? created_date { get; set; }
        #endregion

        public TicketType(DataRow DR)
        {
            this.TTid = Convert.ToInt64(DR["TTid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.TypeName = Convert.ToString(DR["TypeName"]);
            this.uid = Convert.ToInt32(DR["uid"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.created_date = Convert.ToDateTime(DR["created_date"]);
        }

        public TicketType() { }
        public TicketType(long TTid, int cid, string TypeName, int uid, int Active, DateTime created_date)
        {
            this.TTid = TTid;
            this.cid = cid;
            this.TypeName = TypeName;
            this.uid = uid;
            this.Active = Active;
            this.created_date = created_date;
        }

        public TicketType Create(TicketType tickettype)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (tickettype.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 += "cid,";
                query2 += "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, tickettype.cid);
            }
            if (string.IsNullOrEmpty(tickettype.TypeName))

            {
                throw new Exception("TypeName is required");
            }
            else
            {
                query1 += "TypeName,";
                query2 += "@TypeName,";
                cmd.Parameters.Add("@TypeName", DBType.VarChar, tickettype.TypeName);
            }
            if (tickettype.uid == 0)
            {
                throw new Exception("uid is required");
            }
            else
            {
                query1 += "uid,";
                query2 += "@uid,";
                cmd.Parameters.Add("@uid", DBType.Int, tickettype.uid);
            }
            if (tickettype.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 += "Active,";
                query2 += "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, tickettype.Active);
            }
            // Write Here for missing column created_date


            string Query = "INSERT INTO TicketType(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            tickettype.TTid = Convert.ToInt32(DT.Rows[0][0]);
            return tickettype;
        }

        public static List<TicketType> GetByClient(int cid)
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM TicketType WHERE Active=1 and (cid=1 OR cid=@cid)";

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@cid", DBType.Int, cid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<TicketType> _tictype = new List<TicketType>();

            foreach (DataRow DR in DT.Rows)
            {
                _tictype.Add(new TicketType(DR));
            }

            return _tictype;

        }
    }

}
