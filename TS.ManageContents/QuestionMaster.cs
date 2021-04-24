using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS.DBConnection;

namespace TS.ManageContents
{
    public class QuestionMaster
    {
        #region Columns  

        public long Qid { get; set; }
        public string Question { get; set; }
        public int QuestionGroup { get; set; }
        public int? SrNo { get; set; }
        public int Active { get; set; }
        public string QLanguage { get; set; }
        public string QuestionType { get; set; }
        public string CurrectAnswer { get; set; }
        [Required]
        public string Answer { get; set; }
        public string AnswerDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        // Additional 
        public List<QuestionOptions> Options { get;set;}
        public string QuestionGroupName  { get; set; }
        public int TotalQuestion { get; set; }


        #endregion

        public QuestionMaster(DataRow DR)
        {
            this.Qid = Convert.ToInt64(DR["Qid"]);
            this.Question = Convert.ToString(DR["Question"]);
            this.QuestionGroup = Convert.ToInt32(DR["QuestionGroup"]);
            this.SrNo = Convert.ToInt32(DR["SrNo"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.QLanguage = Convert.ToString(DR["QLanguage"]);
            this.QuestionType = Convert.ToString(DR["QuestionType"]);
            this.CurrectAnswer = Convert.ToString(DR["CurrectAnswer"]);
            this.AnswerDetails = Convert.ToString(DR["AnswerDetails"]);
            this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
            this.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
        }

        public QuestionMaster(DataRow DR,List<QuestionOptions> qop)
        {
            this.Qid = Convert.ToInt64(DR["Qid"]);
            this.Question = Convert.ToString(DR["Question"]);
            this.QuestionGroup = Convert.ToInt32(DR["QuestionGroup"]);
            this.SrNo = Convert.ToInt32(DR["SrNo"]);
            this.Active = Convert.ToInt32(DR["Active"]);
            this.QLanguage = Convert.ToString(DR["QLanguage"]);
            this.QuestionType = Convert.ToString(DR["QuestionType"]);
            this.CurrectAnswer = Convert.ToString(DR["CurrectAnswer"]);
            this.AnswerDetails = Convert.ToString(DR["AnswerDetails"]);
            this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
            this.CreatedBy = Convert.ToInt64(DR["CreatedBy"]);
            this.Options = qop;
        }


        public QuestionMaster() { }

        public QuestionMaster(long Qid, string Question, int QuestionGroup, int SrNo, int Active, string QLanguage, string QuestionType, string CurrectAnswer, string AnswerDetails, DateTime CreatedDate, long CreatedBy)
        {
            this.Qid = Qid;
            this.Question = Question;
            this.QuestionGroup = QuestionGroup;
            this.SrNo = SrNo;
            this.Active = Active;
            this.QLanguage = QLanguage;
            this.QuestionType = QuestionType;
            this.CurrectAnswer = CurrectAnswer;
            this.AnswerDetails = AnswerDetails;
            this.CreatedDate = CreatedDate;
            this.CreatedBy = CreatedBy;
        }

        public QuestionMaster Create(QuestionMaster QM, Connection con)
        {
            if(con==null)
                con = new Connection();
            Command cmd = new Command();
            string query1 = "";
            string query2 = "";

            if (string.IsNullOrEmpty(QM.Question))

            {
                throw new Exception("Question is required");
            }
            else
            {
                query1 += "Question,";
                query2 += "@Question,";
                cmd.Parameters.Add("@Question", DBType.VarChar, QM.Question);
            }
            if (QM.QuestionGroup == 0)
            {
                throw new Exception("QuestionGroup is required");
            }
            else
            {
                query1 += "QuestionGroup,";
                query2 += "@QuestionGroup,";
                cmd.Parameters.Add("@QuestionGroup", DBType.Int, QM.QuestionGroup);
            }
            query1 += "SrNo,";
            query2 += "@SrNo,";
            cmd.Parameters.Add("@SrNo", DBType.Int, QM.SrNo);

            if (QM.Active == 0)
            {
                throw new Exception("Active is required");
            }
            else
            {
                query1 += "Active,";
                query2 += "@Active,";
                cmd.Parameters.Add("@Active", DBType.Int, QM.Active);
            }
            if (string.IsNullOrEmpty(QM.QLanguage))

            {
                throw new Exception("QLanguage is required");
            }
            else
            {
                query1 += "QLanguage,";
                query2 += "@QLanguage,";
                cmd.Parameters.Add("@QLanguage", DBType.VarChar, QM.QLanguage);
            }
            if (!string.IsNullOrEmpty(QM.QuestionType))
            {
                query1 += "QuestionType,";
                query2 += "@QuestionType,";
                cmd.Parameters.Add("@QuestionType", DBType.VarChar, QM.QuestionType);
            }

            if (string.IsNullOrEmpty(QM.CurrectAnswer))

            {
                throw new Exception("CurrectAnswer is required");
            }
            else
            {
                query1 += "CurrectAnswer,";
                query2 += "@CurrectAnswer,";
                cmd.Parameters.Add("@CurrectAnswer", DBType.VarChar, QM.CurrectAnswer);
            }
            if (string.IsNullOrEmpty(QM.AnswerDetails))
            {
                //throw new Exception("AnswerDetails is required");
            }
            else
            {
                query1 += "AnswerDetails,";
                query2 += "@AnswerDetails,";
                cmd.Parameters.Add("@AnswerDetails", DBType.VarChar, QM.AnswerDetails);
            }
            if (QM.CreatedDate == Convert.ToDateTime("1900-01-01"))
            {
                throw new Exception("CreatedDate is required");
            }
            else
            {
                query1 += "CreatedDate,";
                query2 += "@CreatedDate,";
                cmd.Parameters.Add("@CreatedDate", DBType.DateTime, QM.CreatedDate);
            }
            query1 += "CreatedBy,";
            query2 += "@CreatedBy,";
            cmd.Parameters.Add("@CreatedBy", DBType.Int, QM.CreatedBy);


            string Query = "INSERT INTO Question_Master(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataTable DT = con.getDataTable();
            QM.Qid = Convert.ToInt32(DT.Rows[0][0]);
            
            QuestionOptions qo = new QuestionOptions();
            qo.Create(QM.Options, QM.Qid, con);

            return QM;
        }

        public List<QuestionMaster> GetByGroup(int GroupId, Connection con)
        {
            Command cmd = new Command();

            string Query = @"SELECT * FROM Question_Master WHERE QuestionGroup=@QuestionGroup AND Active=1;
                             SELECT * FROM Question_Options QO WHERE QO.Qid IN(SELECT Q.Qid FROM Question_Master Q WHERE QuestionGroup=@QuestionGroup and Q.Active=1) and QO.Active=1";
            cmd.Parameters.Add("@QuestionGroup", DBType.Int, GroupId);
            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            DataSet DS = con.getDataSet();

            List<QuestionMaster> obj = new List<QuestionMaster>();

            QuestionOptions q = new QuestionOptions();
            List<QuestionOptions> op = q.GetData(DS.Tables[1]);

            foreach (DataRow DR in DS.Tables[0].Rows)
            {
                obj.Add(new QuestionMaster(DR,op.FindAll(r=> r.Qid==(long)DR["Qid"])));
            }

            return obj;
        }

    }

}
