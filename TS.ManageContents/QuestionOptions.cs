using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.ManageContents
{
    public class QuestionOptions
    {
        #region Columns  
        public long Oid { get; set; }
        public long Qid { get; set; }
        public string SrNo { get; set; }
        public string Answer { get; set; }
        public int Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        #endregion

        public QuestionOptions(DataRow DR)
        {
            this.Oid = Convert.ToInt64(DR["Oid"]);
            this.Qid = Convert.ToInt64(DR["Qid"]);
            this.SrNo = Convert.ToString(DR["SrNo"]);
            this.Answer = Convert.ToString(DR["Answer"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
            this.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
        }

        public List<QuestionOptions> GetData(DataTable DT)
        {
            List<QuestionOptions> qo = new List<QuestionOptions>();
            foreach (DataRow DR in DT.Rows)
            {
                QuestionOptions _qo = new QuestionOptions();
                _qo.Oid = Convert.ToInt64(DR["Oid"]);
                _qo.Qid = Convert.ToInt64(DR["Qid"]);
                _qo.SrNo = Convert.ToString(DR["SrNo"]);
                _qo.Answer = Convert.ToString(DR["Answer"]);
                _qo.Active = Convert.ToInt32(DR["Active"]);
                _qo.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
                _qo.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
                qo.Add(_qo);
            }

            return qo;

        }
        
        public QuestionOptions() { }

        public QuestionOptions(long Oid, long Qid, string SrNo, string Answer, int Active, DateTime CreatedDate, long CreatedBy)
        {
            this.Oid = Oid;
            this.Qid = Qid;
            this.SrNo = SrNo;
            this.Answer = Answer;
            this.Active = Active;
            this.CreatedDate = CreatedDate;
            this.CreatedBy = CreatedBy;
        }

        public QuestionOptions Create(QuestionOptions QO, Connection con)
        {
            if (con == null)
                con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (QO.Qid == 0)
            {
                throw new Exception("Qid is required");
            }
            else
            {
                query1 += "Qid,";
                query2 += "@Qid,";
                cmd.Parameters.Add("@Qid", DBType.Int, QO.Qid);
            }
            if (string.IsNullOrEmpty(QO.SrNo))

            {
                throw new Exception("SrNo is required");
            }
            else
            {
                query1 += "SrNo,";
                query2 += "@SrNo,";
                cmd.Parameters.Add("@SrNo", DBType.VarChar, QO.SrNo);
            }
            if (string.IsNullOrEmpty(QO.Answer))

            {
                throw new Exception("Answer is required");
            }
            else
            {
                query1 += "Answer,";
                query2 += "@Answer,";
                cmd.Parameters.Add("@Answer", DBType.VarChar, QO.Answer);
            }
            if (QO.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 += "Active,";
                query2 += "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, QO.Active);
            }
            if (QO.CreatedDate == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("CreatedDate is required");
            }
            else
            {
                query1 += "CreatedDate,";
                query2 += "@CreatedDate,";
                cmd.Parameters.Add("@CreatedDate", DBType.DateTime, QO.CreatedDate);
            }
            query1 += "CreatedBy,";
            query2 += "@CreatedBy,";
            cmd.Parameters.Add("@CreatedBy", DBType.Int, QO.CreatedBy);


            string Query = "INSERT INTO Question_Options(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            QO.Oid = Convert.ToInt32(DT.Rows[0][0]);
            return QO;
        }

        public void Create(List<QuestionOptions> QO, Connection con)
        {
            if (con == null)
                con = new Connection();
            foreach (QuestionOptions op in QO)
            {
                Create(op, con);
            }

        }
        public void Create(List<QuestionOptions> QO,long Qid, Connection con)
        {
            if (con == null)
                con = new Connection();
            foreach (QuestionOptions op in QO)
            {
                op.Qid = Qid;
                Create(op, con);
            }

        }


    }

}
