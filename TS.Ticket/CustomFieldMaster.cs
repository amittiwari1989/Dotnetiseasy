using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.Tickets
{
    public class CustomFieldMaster
    {
        #region Columns  
        public long CFMid { get; set; }
        public int cid { get; set; }
        public string Label { get; set; }
        public string FieldName { get; set; }
        public int DataType { get; set; }
        public int compulsory { get; set; }
        public int? active { get; set; }
        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_modified { get; set; }
        public List<CustomFieldList> customFieldList { get; set; }
        #endregion

        public CustomFieldMaster() { }

        public CustomFieldMaster(DataRow DR)
        {
            this.CFMid = Convert.ToInt64(DR["CFMid"]);
            this.cid = Convert.ToInt32(DR["cid"]);
            this.Label = Convert.ToString(DR["Label"]);
            this.FieldName = Convert.ToString(DR["FieldName"]);
            this.DataType = Convert.ToInt32(DR["DataType"]);
            this.compulsory = Convert.ToInt32(DR["compulsory"]);
            this.active = Convert.ToInt32(DR["active"]);
            this.created_by = Convert.ToInt32(DR["created_by"]);
            this.date_created = Convert.ToDateTime(DR["date_created"]);
            this.date_modified = Convert.ToDateTime(DR["date_modified"]);
            this.customFieldList = CustomFieldList.GetByFeild(this.CFMid);
        }

        public CustomFieldMaster(long CFMid, int cid, string Label, string FieldName, int DataType, int compulsory, int active, int created_by, DateTime date_created, DateTime date_modified)
        {
            this.CFMid = CFMid;
            this.cid = cid;
            this.Label = Label;
            this.FieldName = FieldName;
            this.DataType = DataType;
            this.compulsory = compulsory;
            this.active = active;
            this.created_by = created_by;
            this.date_created = date_created;
            this.date_modified = date_modified;
        }

        public CustomFieldMaster Create(CustomFieldMaster cfm)
        {
            Connection con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (cfm.cid == 0)
            {
                throw new Exception("cid is required");
            }
            else
            {
                query1 += "cid,";
                query2 += "@cid,";
                cmd.Parameters.Add("@cid", DBType.Int, cfm.cid);
            }
            if (string.IsNullOrEmpty(cfm.Label))

            {
                throw new Exception("Label is required");
            }
            else
            {
                query1 += "Label,";
                query2 += "@Label,";
                cmd.Parameters.Add("@Label", DBType.VarChar, cfm.Label);
            }
            if (string.IsNullOrEmpty(cfm.FieldName))

            {
                throw new Exception("FieldName is required");
            }
            else
            {
                query1 += "FieldName,";
                query2 += "@FieldName,";
                cmd.Parameters.Add("@FieldName", DBType.VarChar, cfm.FieldName);
            }
            if (cfm.DataType == 0)
            {
                throw new Exception("DataType is required");
            }
            else
            {
                query1 += "DataType,";
                query2 += "@DataType,";
                cmd.Parameters.Add("@DataType", DBType.Int, cfm.DataType);
            }
            if (cfm.compulsory == 0)
            {
                throw new Exception("compulsory is required");
            }
            else
            {
                query1 += "compulsory,";
                query2 += "@compulsory,";
                cmd.Parameters.Add("@compulsory", DBType.Int, cfm.compulsory);
            }
            query1 += "active,";
            query2 += "@active,";
            cmd.Parameters.Add("@active", DBType.Int, cfm.active);

            if (cfm.created_by == 0)
            {
                throw new Exception("created by is required");
            }
            else
            {
                query1 += "created_by,";
                query2 += "@created_by,";
                cmd.Parameters.Add("@created_by", DBType.Int, cfm.created_by);
            }
            if (cfm.date_created == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date created is required");
            }
            else
            {
                query1 += "date_created,";
                query2 += "@date_created,";
                cmd.Parameters.Add("@date_created", DBType.DateTime, cfm.date_created);
            }
            if (cfm.date_modified == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("date modified is required");
            }
            else
            {
                query1 += "date_modified,";
                query2 += "@date_modified,";
                cmd.Parameters.Add("@date_modified", DBType.DateTime, cfm.date_modified);
            }

            string Query = "INSERT INTO CustomFieldMaster(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") SELECT @@IDENTITY";
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            cfm.CFMid = Convert.ToInt32(DT.Rows[0][0]);
            return cfm;
        }
        
        public static List<CustomFieldMaster> GetByClient(int cid)
        {
            Connection con = new Connection();
            Command cmd = new Command();

            string Query = "SELECT * FROM CustomFieldMaster WHERE active=1 and cid=@cid";

            cmd.CommandText = Query;
            cmd.Parameters.AddWithValue("@cid", DBType.Int, cid);
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();

            List<CustomFieldMaster> obj = new List<CustomFieldMaster>();

            foreach (DataRow DR in DT.Rows)
            {
                obj.Add(new CustomFieldMaster(DR));
            }

            return obj;

        }

    }

}
