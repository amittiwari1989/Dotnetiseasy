using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.Tickets
{
    public class FieldOrder
    {
        #region Columns  
        public int FOid { get; set; }
        public int cid { get; set; }
        public int uid { get; set; }
        public string FieldName { get; set; }
        public int visible { get; set; }
        public int SeqNo { get; set; }
        public int Active { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        #endregion

        public FieldOrder(DataRow DR)
        {
            this.FOid = Convert.ToInt32(DR["FOid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.uid = Convert.ToInt32(DR["uid"]);
            this.FieldName = Convert.ToString(DR["FieldName"]);
            this.visible = Convert.ToInt32(DR["visible"]);
            this.SeqNo = Convert.ToInt32(DR["SeqNo"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
        }

        public FieldOrder() { }
        public FieldOrder(int FOid, int cid, int uid, string FieldName, int visible, int SeqNo, int Active, DateTime date_created, DateTime date_modified)
        {
            this.FOid = FOid;
            this.cid = cid;
            this.uid = uid;
            this.FieldName = FieldName;
            this.visible = visible;
            this.SeqNo = SeqNo;
            this.Active = Active;
            this.date_created = date_created;
            this.date_modified = date_modified;
        }

        public FieldOrder Create(FieldOrder obj)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (obj.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 += "cid,";
                query2 += "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, obj.cid);
            }
            if (obj.uid == 0)
            {
                throw new Exception("uid is required");
            }
            else
            {
                query1 += "uid,";
                query2 += "@uid,";
                cmd.Parameters.Add("@uid", DBType.Int, obj.uid);
            }
            if (string.IsNullOrEmpty(obj.FieldName))

            {
                throw new Exception("FieldName is required");
            }
            else
            {
                query1 += "FieldName,";
                query2 += "@FieldName,";
                cmd.Parameters.Add("@FieldName", DBType.VarChar, obj.FieldName);
            }
            if (obj.visible == 0)
            {
                throw new Exception("visible is required");
            }
            else
            {
                query1 += "visible,";
                query2 += "@visible,";
                cmd.Parameters.Add("@visible", DBType.Int, obj.visible);
            }
            if (obj.SeqNo == 0)
            {
                throw new Exception("SeqNo is required");
            }
            else
            {
                query1 += "SeqNo,";
                query2 += "@SeqNo,";
                cmd.Parameters.Add("@SeqNo", DBType.Int, obj.SeqNo);
            }
            if (obj.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 += "Active,";
                query2 += "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, obj.Active);
            }
            if (obj.date_created == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date created is required");
            }
            else
            {
                query1 += "date_created,";
                query2 += "@date_created,";
                cmd.Parameters.Add("@date_created", DBType.DateTime, obj.date_created);
            }
            if (obj.date_modified == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date modified is required");
            }
            else
            {
                query1 += "date_modified,";
                query2 += "@date_modified,";
                cmd.Parameters.Add("@date_modified", DBType.DateTime, obj.date_modified);
            }

            string Query = "INSERT INTO FieldOrder(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            obj.FOid = Convert.ToInt32(DT.Rows[0][0]);
            return obj;
        }

        public static List<FieldOrder> GetByuid(int uid)
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM FieldOrder WHERE Active=1 and visible=1 and uid=@uid";

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@uid", DBType.Int, uid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<FieldOrder> obj = new List<FieldOrder>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new FieldOrder(DR));
            }

            return obj;

        }

        public static string GetFields(int uid)
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = @"SELECT SUBSTRING( 
( 
     SELECT ',{""name"" : ""' + FieldName + '""}' AS 'data()' 
         FROM FieldOrder WHERE visible = 1 and uid=@uid and Active=1 order by SeqNo FOR XML PATH('')
), 2 , 9999) As Fields
";

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@uid", DBType.Int, uid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            return "[" + DT.Rows[0]["Fields"].ToString() + "]";
            

        }
    }
}
