using System;
using System.Collections.Generic;
using System.Data;
using TS.DBConnection;

namespace TS.ManageContents
{
    public class TestResults
    {
        #region Columns  
        public long Rid { get; set; }
        public long Tlid { get; set; }
        public long Qid { get; set; }
        public string Answer { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        #endregion

        public TestResults(DataRow DR)
        {
            this.Rid = Convert.ToInt64(DR["Rid"]);
            this.Tlid = Convert.ToInt64(DR["Tlid"]);
            this.Qid = Convert.ToInt64(DR["Qid"]);
            this.Answer = Convert.ToString(DR["Answer"]);
            this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
            this.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
        }

        public TestResults() { }

        public TestResults(long Rid, long Tlid, long Qid, string Answer, DateTime CreatedDate, long CreatedBy)
        {
            this.Rid = Rid;
            this.Tlid = Tlid;
            this.Qid = Qid;
            this.Answer = Answer;
            this.CreatedDate = CreatedDate;
            this.CreatedBy = CreatedBy;
        }

        public TestResults Create(TestResults tr, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (tr.Tlid == 0)
            {
                throw new Exception("Uid is required");
            }
            else
            {
                query1 += "Tlid,";
                query2 += "@Tlid,";
                cmd.Parameters.Add("@Tlid", DBType.Int, tr.Tlid);
            }
            if (tr.Qid == 0)
            {
                throw new Exception("Qid is required");
            }
            else
            {
                query1 += "Qid,";
                query2 += "@Qid,";
                cmd.Parameters.Add("@Qid", DBType.Int, tr.Qid);
            }
            if (string.IsNullOrEmpty(tr.Answer))

            {
                throw new Exception("Answer is required");
            }
            else
            {
                query1 += "Answer,";
                query2 += "@Answer,";
                cmd.Parameters.Add("@Answer", DBType.VarChar, tr.Answer);
            }
            if (tr.CreatedDate == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("CreatedDate is required");
            }
            else
            {
                query1 += "CreatedDate,";
                query2 += "@CreatedDate,";
                cmd.Parameters.Add("@CreatedDate", DBType.DateTime, tr.CreatedDate);
            }
            query1 += "CreatedBy,";
            query2 += "@CreatedBy,";
            cmd.Parameters.Add("@CreatedBy", DBType.Int, tr.CreatedBy);


            string Query = "INSERT INTO TestResults(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            tr.Rid = Convert.ToInt32(DT.Rows[0][0]);
            return tr;
        }

        public void Create(List<TestResults> tr, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            

            foreach (TestResults item in tr)
            {
                Create(item, con);
            }
            
       
           
        }

        public List<TestResults> GetByTestId(long Tlid, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();

            string Query = @"SELECT * FROM TestResults WHERE Tlid=@Tlid;";
            cmd.Parameters.Add("@Tlid", DBType.Int, Tlid);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataSet DS = con.getDataSet();

            List<TestResults> obj = new List<TestResults>();

            foreach (DataRow DR in DS.Tables[0].Rows)
            {
                obj.Add(new TestResults(DR));
            }

            return obj;
        }

    }

}
