using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.Tickets
{
    public class CustomFieldList
    {
        #region Columns  
        public long CFLid { get; set; }
        public int cid { get; set; }
        public long CFMid { get; set; }
        public string DataField { get; set; }
        public string TextField { get; set; }
        public int? active { get; set; }
        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        #endregion

        public CustomFieldList(DataRow DR)
        {
            this.CFLid = Convert.ToInt64(DR["CFLid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.CFMid = Convert.ToInt64(DR["CFMid"]);
            this.DataField = Convert.ToString(DR["DataField"]);
            this.TextField = Convert.ToString(DR["TextField"]);
            this.active = Convert.ToInt32(DR["active"]);
            this.created_by = Convert.ToInt32(DR["created_by"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
        }

        public CustomFieldList() { }
        public CustomFieldList(long CFLid, int cid, long CFMid, string DataField, string TextField, int active, int created_by, DateTime date_created, DateTime date_modified)
        {
            this.CFLid = CFLid;
            this.cid = cid;
            this.CFMid = CFMid;
            this.DataField = DataField;
            this.TextField = TextField;
            this.active = active;
            this.created_by = created_by;
            this.date_created = date_created;
            this.date_modified = date_modified;
        }

        public CustomFieldList Create(CustomFieldList cfl)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (cfl.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 += "cid,";
                query2 += "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, cfl.cid);
            }
            if (cfl.CFMid == 0)
            {
                throw new Exception("CFMid is required");
            }
            else
            {
                query1 += "CFMid,";
                query2 += "@CFMid,";
                cmd.Parameters.Add("@CFMid", DBType.Int, cfl.CFMid);
            }
            if (string.IsNullOrEmpty(cfl.DataField))

            {
                throw new Exception("DataField is required");
            }
            else
            {
                query1 += "DataField,";
                query2 += "@DataField,";
                cmd.Parameters.Add("@DataField", DBType.VarChar, cfl.DataField);
            }
            if (string.IsNullOrEmpty(cfl.TextField))

            {
                throw new Exception("TextField is required");
            }
            else
            {
                query1 += "TextField,";
                query2 += "@TextField,";
                cmd.Parameters.Add("@TextField", DBType.VarChar, cfl.TextField);
            }
            query1 += "active,";
            query2 += "@active,";
            cmd.Parameters.Add("@active", DBType.Int, cfl.active);

            if (cfl.created_by == 0)
            {
                throw new Exception("created by is required");
            }
            else
            {
                query1 += "created_by,";
                query2 += "@created_by,";
                cmd.Parameters.Add("@created_by", DBType.Int, cfl.created_by);
            }
            if (cfl.date_created == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date created is required");
            }
            else
            {
                query1 += "date_created,";
                query2 += "@date_created,";
                cmd.Parameters.Add("@date_created", DBType.DateTime, cfl.date_created);
            }
            if (cfl.date_modified == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date modified is required");
            }
            else
            {
                query1 += "date_modified,";
                query2 += "@date_modified,";
                cmd.Parameters.Add("@date_modified", DBType.DateTime, cfl.date_modified);
            }

            string Query = "INSERT INTO CustomFieldList(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            cfl.CFLid = Convert.ToInt32(DT.Rows[0][0]);
            return cfl;
        }

        public static List<CustomFieldList> GetByFeild(long cfid)
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM CustomFieldList WHERE active=1 and CFMid=@CFMid";

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@CFMid", DBType.Int, cfid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<CustomFieldList> obj = new List<CustomFieldList>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new CustomFieldList(DR));
            }

            return obj;

        }

    }

}
