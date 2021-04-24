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

	public class QuestionGroup
	{
		#region Columns  
		public long QGid { get; set; }
		[Required]
		[Display(Name = "Question Group Name")]
		public string QuestionGroupName { get; set; }
		[Required]
		[Display(Name = "Category")]
		public string QLanguage { get; set; }
		[Display(Name = "Time Per Question(second)")]
		public int TimePerQuestion { get; set; }
		public long uid { get; set; }
		public int isPublic { get; set; }
		public int Active { get; set; }
		public DateTime CreatedDate { get; set; }
		#endregion

		public QuestionGroup(DataRow DR)
		{
			this.QGid = Convert.ToInt64(DR["QGid"]);
			this.QuestionGroupName = Convert.ToString(DR["QuestionGroupName"]);
			this.QLanguage = Convert.ToString(DR["QLanguage"]);
			this.TimePerQuestion = Convert.ToInt32(DR["TimePerQuestion"]);
			this.uid = Convert.ToInt64(DR["uid"]);
			this.isPublic = Convert.ToInt32(DR["isPublic"]);
			this.Active = Convert.ToInt32(DR["Active"]);
			this.CreatedDate = Convert.ToDateTime(DR["CreatedDate"]);
		}

		public QuestionGroup() { }

		public QuestionGroup(long QGid, string QuestionGroupName, string QLanguage, int TimePerQuestion, long uid, int isPublic, int Active, DateTime CreatedDate)
		{
			this.QGid = QGid;
			this.QuestionGroupName = QuestionGroupName;
			this.QLanguage = QLanguage;
			this.TimePerQuestion = TimePerQuestion;
			this.uid = uid;
			this.isPublic = isPublic;
			this.Active = Active;
			this.CreatedDate = CreatedDate;
		}

		public QuestionGroup Create(QuestionGroup QG, Connection con)
		{
			if (con == null)
				con = new Connection();
			Command cmd = new Command();
			string query1 = "";
			string query2 = "";

			if (string.IsNullOrEmpty(QG.QuestionGroupName))

			{
				throw new Exception("QuestionGroupName is required");
			}
			else
			{
				query1 += "QuestionGroupName,";
				query2 += "@QuestionGroupName,";
				cmd.Parameters.Add("@QuestionGroupName", DBType.VarChar, QG.QuestionGroupName);
			}
			if (string.IsNullOrEmpty(QG.QLanguage))

			{
				throw new Exception("QLanguage is required");
			}
			else
			{
				query1 += "QLanguage,";
				query2 += "@QLanguage,";
				cmd.Parameters.Add("@QLanguage", DBType.VarChar, QG.QLanguage);
			}
			if (QG.TimePerQuestion == 0)
			{
				QG.TimePerQuestion = 30;
			}

			query1 += "TimePerQuestion,";
			query2 += "@TimePerQuestion,";
			cmd.Parameters.Add("@TimePerQuestion", DBType.Int, QG.TimePerQuestion);


			if (QG.uid == 0)
			{
				throw new Exception("uid is required");
			}
			else
			{
				query1 += "uid,";
				query2 += "@uid,";
				cmd.Parameters.Add("@uid", DBType.Int, QG.uid);
			}

			query1 += "isPublic,";
			query2 += "@isPublic,";
			cmd.Parameters.Add("@isPublic", DBType.Int, QG.isPublic);

			if (QG.Active == 0)
			{
				throw new Exception("Active is required");
			}
			else
			{
				query1 += "Active,";
				query2 += "@Active,";
				cmd.Parameters.Add("@Active", DBType.Int, QG.Active);
			}
			if (QG.CreatedDate == Convert.ToDateTime("1900-01-01"))
			{
				throw new Exception("CreatedDate is required");
			}
			else
			{
				query1 += "CreatedDate,";
				query2 += "@CreatedDate,";
				cmd.Parameters.Add("@CreatedDate", DBType.DateTime, QG.CreatedDate);
			}

			string Query = "INSERT INTO Question_Group(" + query1.TrimEnd(',') + ") VALUES (" + query2.TrimEnd(',') + ") ;" + (con.ServerType == DBServerType.MYSQL ? " SELECT last_insert_id(); " : "SELECT @@IDENTITY");
			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataTable DT = con.getDataTable();
			QG.QGid = Convert.ToInt32(DT.Rows[0][0]);
			return QG;
		}

		public List<QuestionGroup> GetQuestionSet(Connection con, string Language = "")
		{
			Command cmd = new Command();

			string Query = @"SELECT * FROM Question_Group WHERE Active=1";
			if (!string.IsNullOrEmpty(Language))
				cmd.Parameters.Add("@QuestionGroup", DBType.VarChar, Language);

			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataSet DS = con.getDataSet();

			List<QuestionGroup> obj = new List<QuestionGroup>();

			foreach (DataRow DR in DS.Tables[0].Rows)
			{
				obj.Add(new QuestionGroup(DR));
			}

			return obj;
		}

        public DataSet GetQuestionSetDs(Connection con, string Language = "")
        {
            Command cmd = new Command();

            string Query = @"SELECT 
	G.QGid,G.QuestionGroupName,G.QLanguage,G.TimePerQuestion,COUNT(DISTINCT Q.Qid) TotalQuestions ,COUNT(DISTINCT T.Tlid) TestTaken
FROM 
	Question_Group G
	INNER JOIN Question_Master Q
		ON G.QGid=Q.QuestionGroup
		AND Q.Active=1
	LEFT OUTER JOIN TestList T
		ON G.QGid=T.QuestionGroup
WHERE 
	G.Active=1
	AND G.isPublic=1
GROUP BY
	G.QGid,G.QuestionGroupName,G.QLanguage,G.TimePerQuestion";
            if (!string.IsNullOrEmpty(Language))
                cmd.Parameters.Add("@QuestionGroup", DBType.VarChar, Language);

            cmd.CommandText = Query;
            cmd.CommandType = DBConnection.CommandType.Text;
            con.cmd = cmd;

            return con.getDataSet();
           
        }


        public QuestionGroup GetById(Connection con, int id)
		{
			Command cmd = new Command();

			string Query = @"SELECT * FROM Question_Group WHERE Active=1 and QGid=@QGid";
			cmd.Parameters.Add("@QGid", DBType.Int, id);

			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataSet DS = con.getDataSet();

			return new QuestionGroup(DS.Tables[0].Rows[0]);
		}

		public List<QuestionGroup> GetByUser(Connection con, long uid)
		{
			Command cmd = new Command();

			string Query = @"SELECT * FROM Question_Group WHERE Active=1 and uid=@uid";
			cmd.Parameters.Add("@uid", DBType.Int, uid);

			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataSet DS = con.getDataSet();

			List<QuestionGroup> lst = new List<QuestionGroup>();

			foreach (DataRow DR in DS.Tables[0].Rows)
			{
				lst.Add(new QuestionGroup(DR));
			}

			return lst;
		}

		public DataSet GetByUserDs(Connection con, long uid)
		{
			Command cmd = new Command();

			string Query = @"SELECT 
	qg.QGid,qg.QuestionGroupName,qg.QLanguage,qg.uid,qg.isPublic,CAST(qg.CreatedDate AS DATE) CreatedDate,COUNT(qm.Qid) TotalQuestion
FROM 
	Question_Group qg
	INNER JOIN Question_Master qm
		ON qg.QGid=qm.QuestionGroup
		and qm.Active=1
WHERE
	qg.Active=1
	and qg.uid=@uid
GROUP BY
	qg.QGid,qg.QuestionGroupName,qg.QLanguage,qg.uid,qg.isPublic,CAST(qg.CreatedDate AS DATE) ";
			cmd.Parameters.Add("@uid", DBType.Int, uid);

			cmd.CommandText = Query;
			cmd.CommandType = DBConnection.CommandType.Text;
			con.cmd = cmd;

			DataSet DS = con.getDataSet();

			return DS;
		}

	}

}
